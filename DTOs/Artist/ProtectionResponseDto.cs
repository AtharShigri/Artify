using System;
using System.Collections.Generic;

namespace artifi.Api.DTOs.Artist
{
    // ── Watermark ──────────────────────────────────────────────────────────────
    public class WatermarkResultDto
    {
        public bool Success { get; set; }
        public string WatermarkedUrl { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
    }

    // ── Hash ───────────────────────────────────────────────────────────────────
    public class HashResultDto
    {
        public bool Success { get; set; }
        public Guid ArtworkId { get; set; }
        public string Sha256Hash { get; set; } = string.Empty;
        public string PerceptualHash { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
    }

    // ── Metadata ───────────────────────────────────────────────────────────────
    public class MetadataResultDto
    {
        public bool Success { get; set; }
        public Guid ArtworkId { get; set; }
        public string ArtistName { get; set; } = string.Empty;
        public string CopyrightText { get; set; } = string.Empty;
    }

    // ── Plagiarism ─────────────────────────────────────────────────────────────
    public class PlagiarismMatchDto
    {
        public Guid MatchedArtworkId { get; set; }
        public string MatchedArtworkTitle { get; set; } = string.Empty;
        public string MatchedArtistName { get; set; } = string.Empty;
        public double SimilarityPercent { get; set; }  // 0-100
        public bool IsExactMatch { get; set; }
    }

    public class PlagiarismResultDto
    {
        public bool Success { get; set; }
        public bool PlagiarismDetected { get; set; }
        public string UploadedFileHash { get; set; } = string.Empty;
        public List<PlagiarismMatchDto> Matches { get; set; } = new();
        public string Summary { get; set; } = string.Empty;
    }

    // ── Protection Status ──────────────────────────────────────────────────────
    public class ProtectionStatusDto
    {
        public Guid ArtworkId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsHashed { get; set; }
        public bool HasMetadata { get; set; }
        public string? HashValue { get; set; }
        public DateTime? HashRegisteredAt { get; set; }
        public string? CopyrightText { get; set; }
    }
}
