using System.Globalization;
using IntelliCasePro.Web.Data;
using IntelliCasePro.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Services;

public class ReportsService
{
    private readonly AppDbContext _db;
    private readonly string[] _colors =
    {
        "#3b82f6",
        "#f97316",
        "#14b8a6",
        "#8b5cf6",
        "#f43f5e",
        "#64748b"
    };

    public ReportsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ReportsViewModel> BuildAsync()
    {
        var cases = await _db.Cases.ToListAsync();
        var invoices = await _db.Invoices.ToListAsync();

        var grouped = cases
            .GroupBy(x => x.Status)
            .OrderByDescending(g => g.Count())
            .ToList();

        var totalCases = Math.Max(1, grouped.Sum(g => g.Count()));
        var segments = new List<ReportSegmentViewModel>();
        var current = 0m;
        var gradientParts = new List<string>();

        for (int i = 0; i < grouped.Count; i++)
        {
            var value = grouped[i].Count();
            var percentage = Math.Round((decimal)value / totalCases * 100m, 1);
            var color = _colors[i % _colors.Length];
            var start = current;
            current += percentage;

            gradientParts.Add($"{color} {start.ToString(CultureInfo.InvariantCulture)}% {current.ToString(CultureInfo.InvariantCulture)}%");

            segments.Add(new ReportSegmentViewModel
            {
                Label = grouped[i].Key.ToString(),
                Value = value,
                Percentage = percentage,
                Color = color
            });
        }

        if (!gradientParts.Any())
        {
            gradientParts.Add("#3b82f6 0 100%");
        }

        var caseDistributionGradient = $"conic-gradient({string.Join(", ", gradientParts)})";

        var year = DateTime.Today.Year;
        var revenueSeries = Enumerable.Range(1, 12)
            .Select(month =>
            {
                var total = invoices
                    .Where(x => x.IssuedOn.Year == year && x.IssuedOn.Month == month)
                    .Sum(x => x.Total);

                return new RevenuePointViewModel
                {
                    Month = new DateTime(year, month, 1).ToString("MMM"),
                    Amount = total
                };
            })
            .ToList();

        var closedCount = cases.Count(x => x.Status == CaseStatus.Closed);
        var closureRate = cases.Count == 0 ? 0 : (int)Math.Round((double)closedCount / cases.Count * 100d);

        var closureGradient = $"conic-gradient(#3b82f6 0 {closureRate}%, #e5e7eb {closureRate}% 100%)";

        return new ReportsViewModel
        {
            CaseDistribution = segments,
            CaseDistributionGradient = caseDistributionGradient,
            RevenueSeries = revenueSeries,
            ClosureRate = closureRate,
            ClosureGradient = closureGradient,
            TotalRevenue = invoices.Sum(x => x.Total),
            CasesClosedThisYear = closedCount,
            AverageCaseValue = invoices.Any() ? Math.Round(invoices.Average(x => x.Total), 2) : 0m
        };
    }
}
