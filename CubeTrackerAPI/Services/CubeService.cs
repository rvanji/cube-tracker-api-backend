using CubeTrackerAPI.Data;
using CubeTrackerAPI.DTOs;
using CubeTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CubeTrackerAPI.Services
{
    public class CubeService
    {
        private readonly AppDbContext _context;
        private const decimal CubeRate = 2000;

        public CubeService(AppDbContext context)
        {
            _context = context;
        }

        // Add daily return
        public async Task<DailyReturn> AddDailyReturn(DateTime date, decimal amount)
        {
            var entry = new DailyReturn
            {
                Date = date,
                Amount = amount,
                CubeRate = CubeRate
            };

            _context.DailyReturns.Add(entry);
            await _context.SaveChangesAsync();

            return entry;
        }

        // Get all returns
        public async Task<List<DailyReturn>> GetAll()
        {
            return await _context.DailyReturns
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        // Total cubes
        public async Task<decimal> GetTotalCubes()
        {
            var data = await _context.DailyReturns.ToListAsync();
            return data.Sum(x => x.Amount / CubeRate);
        }

        // Total amount
        public async Task<decimal> GetTotalAmount()
        {
            return await _context.DailyReturns.SumAsync(x => x.Amount);
        }

        // Progress to target
        public async Task<object> GetProgress(decimal target = 2250)
        {
            var totalCubes = await GetTotalCubes();

            var percent = (totalCubes / target) * 100;

            return new
            {
                totalCubes,
                target,
                percent
            };
        }

        // Daily average (important for forecasting)
        public async Task<decimal> GetDailyAverage()
        {
            var data = await _context.DailyReturns.ToListAsync();

            if (!data.Any()) return 0;

            var days = (data.Max(x => x.Date) - data.Min(x => x.Date)).Days + 1;

            var totalCubes = data.Sum(x => x.Amount / CubeRate);

            return totalCubes / days;
        }

        public async Task<DailyReturn?> UpdateReturn(int id, DateTime date, decimal amount)
        {
            var entry = await _context.DailyReturns.FirstOrDefaultAsync(x => x.Id == id);

            if (entry == null)
                return null;

            entry.Date = date;
            entry.Amount = amount;
            entry.CubeRate = 2000;

            await _context.SaveChangesAsync();

            return entry;
        }

        public async Task<bool> DeleteReturn(int id)
        {
            var entry = await _context.DailyReturns.FirstOrDefaultAsync(x => x.Id == id);

            if (entry == null)
                return false;

            _context.DailyReturns.Remove(entry);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<SummaryResponseDto> GetSummary()
        {
            var returns = await _context.DailyReturns.ToListAsync();

            decimal totalAmount = returns.Sum(x => x.Amount);

            decimal totalCubes = returns.Sum(x => x.Cubes);

            decimal target = 2250;

            decimal remaining = Math.Max(target - totalCubes, 0);

            decimal progress =
                totalCubes == 0
                    ? 0
                    : (totalCubes / target) * 100;

            decimal dailyAverage = returns.Any()
                ? totalCubes / returns.Count
                : 0;

            var today = DateTime.Today;

            var todayAmount = returns
                .Where(x => x.Date.Date == today)
                .Sum(x => x.Amount);

            var todayCubes = todayAmount / 2000;

            return new SummaryResponseDto
            {
                TotalAmount = totalAmount,
                TotalCubes = totalCubes,
                TargetCubes = target,
                RemainingCubes = remaining,
                ProgressPercentage = progress,
                DailyAverage = dailyAverage,
                TodayAmount = todayAmount,
                TodayCubes = todayCubes,
                TotalEntries = returns.Count
            };
        }
    }
}
