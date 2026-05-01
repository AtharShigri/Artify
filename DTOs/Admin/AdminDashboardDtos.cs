using System;
using System.Collections.Generic;

namespace artifi.Api.DTOs.Admin
{
    public class AdminDashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalArtists { get; set; }
        public int TotalBuyers { get; set; }
        public int TotalArtworks { get; set; }
        public int PendingArtworks { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveReports { get; set; }
        public int PlagiarismAlerts { get; set; }
    }

    public class RecentUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }

    public class PlagiarismReportDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = "Plagiarism";
        public string Subject { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public double SimilarityScore { get; set; }
    }
}
