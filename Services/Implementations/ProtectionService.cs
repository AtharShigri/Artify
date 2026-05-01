using System.Security.Claims;
using System.Security.Cryptography;
using SkiaSharp;
using artifi.Api.DTOs.Artist;
using artifi.Api.Models;
using artifi.Api.Repositories.Interfaces;
using artifi.Api.Services.Interfaces;

namespace artifi.Api.Services.Implementations
{
    public class ProtectionService : IProtectionService
    {
        private readonly IProtectionRepository _protectionRepo;
        private readonly IArtworkRepository _artworkRepo;
        private readonly IArtistRepository _artistRepo;
        private readonly IWebHostEnvironment _env;

        public ProtectionService(
            IProtectionRepository protectionRepo,
            IArtworkRepository artworkRepo,
            IArtistRepository artistRepo,
            IWebHostEnvironment env)
        {
            _protectionRepo = protectionRepo;
            _artworkRepo = artworkRepo;
            _artistRepo = artistRepo;
            _env = env;
        }

        // ── 1. WATERMARKING ───────────────────────────────────────────────────
        // Uses SkiaSharp to draw a visible copyright watermark over the image.
        public async Task<WatermarkResultDto> ApplyWatermarkAsync(ClaimsPrincipal user, IFormFile file)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artistUser = await _artistRepo.GetByIdAsync(artistId);
            var artistName = artistUser?.FullName ?? "artifi Artist";

            var wwwRoot = Path.Combine(_env.ContentRootPath, "wwwroot");
            
            // Ensure output directory exists
            var outputDir = Path.Combine(wwwRoot, "images", "watermarked");
            Directory.CreateDirectory(outputDir);

            // Temp dir for input
            var tempDir = Path.Combine(wwwRoot, "images", "temp");
            Directory.CreateDirectory(tempDir);

            var tempFile = Path.Combine(tempDir, $"{Guid.NewGuid()}_{file.FileName}");

            using (var tempStream = new FileStream(tempFile, FileMode.Create))
                await file.CopyToAsync(tempStream);

            // Load image with SkiaSharp
            using var inputStream = new FileStream(tempFile, FileMode.Open);
            using var originalBitmap = SKBitmap.Decode(inputStream);

            if (originalBitmap == null)
                return new WatermarkResultDto { Success = false };

            using var surface = SKSurface.Create(new SKImageInfo(originalBitmap.Width, originalBitmap.Height));
            var canvas = surface.Canvas;

            // Draw original image
            canvas.DrawBitmap(originalBitmap, 0, 0);

            // ── Draw diagonal watermark text across the whole image ───────────
            var watermarkText = $"© {artistName} | artifi";
            var fontSize = Math.Max(originalBitmap.Width / 20f, 18f);

            using var textPaint = new SKPaint
            {
                Color = SKColors.White.WithAlpha(80),   // semi-transparent white
                TextSize = fontSize,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                FakeBoldText = true,
                Typeface = SKTypeface.Default
            };

            // Tile the watermark diagonally across the image
            canvas.Save();
            canvas.Translate(originalBitmap.Width / 2f, originalBitmap.Height / 2f);
            canvas.RotateDegrees(-30);

            int stepY = (int)(fontSize * 4);
            int stepX = (int)(textPaint.MeasureText(watermarkText) * 1.5f);
            int startY = -originalBitmap.Height;
            int endY = originalBitmap.Height;
            int startX = -originalBitmap.Width;
            int endX = originalBitmap.Width;

            for (int y = startY; y < endY; y += stepY)
                for (int x = startX; x < endX; x += stepX)
                    canvas.DrawText(watermarkText, x, y, textPaint);

            canvas.Restore();

            // ── Draw solid corner watermark (more readable) ──────────────────
            using var cornerPaint = new SKPaint
            {
                Color = SKColors.Black.WithAlpha(140),
                TextSize = Math.Max(fontSize * 0.8f, 14f),
                IsAntialias = true,
                TextAlign = SKTextAlign.Right
            };

            float margin = 10f;
            canvas.DrawText(
                watermarkText,
                originalBitmap.Width - margin,
                originalBitmap.Height - margin,
                cornerPaint);

            // Save watermarked image
            var outputFileName = $"wm_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var outputPath = Path.Combine(outputDir, outputFileName);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);
            await File.WriteAllBytesAsync(outputPath, data.ToArray());

            // Cleanup temp
            File.Delete(tempFile);

            return new WatermarkResultDto
            {
                Success = true,
                WatermarkedUrl = $"/images/watermarked/{outputFileName}",
                OriginalFileName = file.FileName
            };
        }

        // ── 2. METADATA EMBEDDING ─────────────────────────────────────────────
        public async Task<MetadataResultDto> EmbedMetadataAsync(ClaimsPrincipal user, MetadataDto dto)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artwork = await _artworkRepo.GetByIdAsync(dto.ArtworkId);

            if (artwork == null || artwork.ArtistProfileId != artistId)
                return new MetadataResultDto { Success = false };

            // Upsert metadata log
            var existing = await _protectionRepo.GetMetadataAsync(dto.ArtworkId);
            if (existing != null)
            {
                // Update the existing record
                existing.ArtistName = dto.ArtistName;
                existing.CopyrightText = dto.CopyrightText;
                existing.Description = dto.Description;
                existing.CreatedAt = DateTime.UtcNow;
                await _protectionRepo.UpdateMetadataLogAsync(existing); // will use EF tracking
            }
            else
            {
                var metadataLog = new ArtworkMetadataLog
                {
                    Id = Guid.NewGuid(),
                    ArtworkId = dto.ArtworkId,
                    ArtistName = dto.ArtistName,
                    CopyrightText = dto.CopyrightText,
                    Description = dto.Description,
                    CreatedAt = DateTime.UtcNow
                };
                await _protectionRepo.AddMetadataLogAsync(metadataLog);
            }

            return new MetadataResultDto
            {
                Success = true,
                ArtworkId = dto.ArtworkId,
                ArtistName = dto.ArtistName,
                CopyrightText = dto.CopyrightText
            };
        }

        // ── 3. HASH REGISTRATION ──────────────────────────────────────────────
        // Computes SHA-256 (exact match) + pHash (perceptual similarity) for the artwork image.
        public async Task<HashResultDto> GenerateHashAsync(ClaimsPrincipal user, HashDto dto)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artwork = await _artworkRepo.GetByIdAsync(dto.ArtworkId);

            if (artwork == null || artwork.ArtistProfileId != artistId)
                return new HashResultDto { Success = false };

            // Locate the image file
            if (string.IsNullOrEmpty(artwork.ImageUrl))
                return new HashResultDto { Success = false };

            var wwwRoot = Path.Combine(_env.ContentRootPath, "wwwroot");
            var imagePath = Path.Combine(wwwRoot, artwork.ImageUrl.TrimStart('/').Replace("/", "\\"));
            if (!File.Exists(imagePath))
                return new HashResultDto { Success = false };

            var imageBytes = await File.ReadAllBytesAsync(imagePath);

            // SHA-256 — exact match fingerprint
            var sha256 = ComputeSha256(imageBytes);

            // pHash — perceptual hash (survives minor edits/compression)
            var pHash = ComputePerceptualHash(imageBytes);

            // Combined fingerprint stored on the artwork
            var combinedHash = $"sha256:{sha256}|phash:{pHash}";

            // Save to ArtworkHashes table
            var existing = await _protectionRepo.GetArtworkHashAsync(dto.ArtworkId);
            var hashRecord = existing ?? new ArtworkHash { Id = Guid.NewGuid(), ArtworkId = dto.ArtworkId };
            hashRecord.HashValue = combinedHash;
            hashRecord.CreatedAt = DateTime.UtcNow;

            if (existing == null)
                await _protectionRepo.AddHashRecordAsync(hashRecord);
            else
                await _protectionRepo.UpdateHashRecordAsync(hashRecord);

            // Also store on the artwork itself for quick plagiarism lookup
            artwork.HashValue = sha256;
            await _artworkRepo.UpdateAsync(artwork);

            return new HashResultDto
            {
                Success = true,
                ArtworkId = dto.ArtworkId,
                Sha256Hash = sha256,
                PerceptualHash = pHash,
                RegisteredAt = hashRecord.CreatedAt
            };
        }

        // ── 4. PLAGIARISM CHECK ───────────────────────────────────────────────
        // Compares an uploaded image against ALL registered artworks using pHash Hamming distance.
        // Threshold: Hamming ≤ 10 out of 64 bits → similar (≥ 84%)
        public async Task<PlagiarismResultDto> CheckPlagiarismAsync(ClaimsPrincipal user, byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return new PlagiarismResultDto { Success = false, Summary = "Failed to read image data for plagiarism check." };
            }

            var uploadedSha256 = ComputeSha256(imageBytes);
            var uploadedPHash = ComputePerceptualHash(imageBytes);

            // Fetch all registered hashes
            var allHashes = await _protectionRepo.GetAllHashesAsync();
            var matches = new List<PlagiarismMatchDto>();

            foreach (var record in allHashes)
            {
                if (string.IsNullOrEmpty(record.HashValue)) continue;

                // Parse stored hash (format: "sha256:...|phash:...")
                var parts = record.HashValue.Split('|');
                string? storedSha256 = null;
                string? storedPHash = null;

                foreach (var part in parts)
                {
                    if (part.StartsWith("sha256:")) storedSha256 = part["sha256:".Length..];
                    if (part.StartsWith("phash:")) storedPHash = part["phash:".Length..];
                }

                bool isExact = storedSha256 == uploadedSha256;
                double similarityPct = 0;

                if (storedPHash != null && storedPHash.Length == uploadedPHash.Length)
                {
                    int distance = HammingDistance(uploadedPHash, storedPHash);
                    similarityPct = Math.Round((1.0 - (double)distance / uploadedPHash.Length) * 100, 1);
                }

                if (isExact || similarityPct >= 80.0)
                {
                    var artwork = record.Artwork;
                    var artistProfile = artwork?.ArtistProfile;

                    matches.Add(new PlagiarismMatchDto
                    {
                        MatchedArtworkId = record.ArtworkId,
                        MatchedArtworkTitle = artwork?.Title ?? "Unknown",
                        MatchedArtistName = artistProfile?.User?.FullName ?? "Unknown Artist",
                        SimilarityPercent = isExact ? 100.0 : similarityPct,
                        IsExactMatch = isExact
                    });
                }
            }

            bool detected = matches.Count > 0;

            // Log in DB if plagiarism found
            if (detected)
            {
                var topMatch = matches.OrderByDescending(m => m.SimilarityPercent).First();
                var log = new PlagiarismLog
                {
                    Id = Guid.NewGuid(),
                    ArtworkId = topMatch.MatchedArtworkId,
                    SuspectedArtworkId = topMatch.MatchedArtworkId, // Same for now (no upload registration)
                    SimilarityScore = topMatch.SimilarityPercent,
                    IsReviewed = false,
                    ActionTaken = false,
                    CreatedAt = DateTime.UtcNow,
                    Notes = $"Detected via plagiarism check. SHA-256: {uploadedSha256}"
                };
                await _protectionRepo.AddPlagiarismLogAsync(log);
            }

            return new PlagiarismResultDto
            {
                Success = true,
                PlagiarismDetected = detected,
                UploadedFileHash = uploadedSha256,
                Matches = matches.OrderByDescending(m => m.SimilarityPercent).ToList(),
                Summary = detected
                    ? $"⚠️ {matches.Count} potential match(es) found. Highest similarity: {matches.Max(m => m.SimilarityPercent):F1}%."
                    : "✅ No matching artwork found in the artifi registry. Your work appears to be original."
            };
        }

        // ── 5. PROTECTION STATUS ─────────────────────────────────────────────
        public async Task<ProtectionStatusDto?> GetProtectionStatusAsync(ClaimsPrincipal user, Guid artworkId)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artwork = await _artworkRepo.GetByIdAsync(artworkId);

            if (artwork == null || artwork.ArtistProfileId != artistId) return null;

            var hash = await _protectionRepo.GetArtworkHashAsync(artworkId);
            var meta = await _protectionRepo.GetMetadataAsync(artworkId);

            return new ProtectionStatusDto
            {
                ArtworkId = artworkId,
                Title = artwork.Title,
                IsHashed = hash != null,
                HasMetadata = meta != null,
                HashValue = hash?.HashValue,
                HashRegisteredAt = hash?.CreatedAt,
                CopyrightText = meta?.CopyrightText
            };
        }

        // ── SHA-256 Hash ─────────────────────────────────────────────────────
        private static string ComputeSha256(byte[] data)
        {
            using var sha256 = SHA256.Create();
            return BitConverter.ToString(sha256.ComputeHash(data)).Replace("-", "").ToLowerInvariant();
        }

        // ── Perceptual Hash (DCT-based, 64-bit) ──────────────────────────────
        // 1. Resize to 32x32 greyscale
        // 2. Apply DCT and take top-left 8x8 = 64 values
        // 3. Compare each value to the median → 64-bit binary string
        private static string ComputePerceptualHash(byte[] imageBytes)
        {
            try
            {
                using var bitmap = SKBitmap.Decode(imageBytes);
                if (bitmap == null) return string.Empty;

                // Resize to 32x32 greyscale
                using var small = bitmap.Resize(new SKImageInfo(32, 32, SKColorType.Gray8, SKAlphaType.Opaque), SKFilterQuality.Medium);
                if (small == null) return string.Empty;

                // Convert pixels to double array
                double[,] pixels = new double[32, 32];
                for (int y = 0; y < 32; y++)
                    for (int x = 0; x < 32; x++)
                    {
                        var c = small.GetPixel(x, y);
                        pixels[y, x] = (c.Red * 0.299 + c.Green * 0.587 + c.Blue * 0.114);
                    }

                // Apply 2D DCT (simplified — row then column)
                double[,] dct = ApplyDct2D(pixels, 32);

                // Extract top-left 8x8 (excluding DC component at [0,0])
                var dctValues = new List<double>();
                for (int y = 0; y < 8; y++)
                    for (int x = 0; x < 8; x++)
                        if (!(x == 0 && y == 0))
                            dctValues.Add(dct[y, x]);

                double median = Median(dctValues);

                // Binary string: 1 if above median, 0 otherwise
                return string.Concat(dctValues.Select(v => v > median ? "1" : "0"));
            }
            catch
            {
                return string.Empty;
            }
        }

        private static double[,] ApplyDct2D(double[,] input, int n)
        {
            var temp = new double[n, n];
            var output = new double[n, n];

            // DCT on rows
            for (int y = 0; y < n; y++)
                for (int u = 0; u < n; u++)
                {
                    double sum = 0;
                    for (int x = 0; x < n; x++)
                        sum += input[y, x] * Math.Cos((2 * x + 1) * u * Math.PI / (2 * n));
                    temp[y, u] = sum * (u == 0 ? 1.0 / Math.Sqrt(n) : Math.Sqrt(2.0 / n));
                }

            // DCT on columns
            for (int u = 0; u < n; u++)
                for (int v = 0; v < n; v++)
                {
                    double sum = 0;
                    for (int y = 0; y < n; y++)
                        sum += temp[y, u] * Math.Cos((2 * y + 1) * v * Math.PI / (2 * n));
                    output[v, u] = sum * (v == 0 ? 1.0 / Math.Sqrt(n) : Math.Sqrt(2.0 / n));
                }

            return output;
        }

        private static double Median(List<double> values)
        {
            var sorted = values.OrderBy(v => v).ToList();
            int mid = sorted.Count / 2;
            return sorted.Count % 2 == 0 ? (sorted[mid - 1] + sorted[mid]) / 2.0 : sorted[mid];
        }

        // ── Hamming Distance ─────────────────────────────────────────────────
        private static int HammingDistance(string a, string b)
        {
            if (a.Length != b.Length) return int.MaxValue;
            return a.Zip(b).Count(pair => pair.First != pair.Second);
        }
    }
}
