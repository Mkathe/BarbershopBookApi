using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BarbershopBookApi.Infrastructure.Repositories;

public class HairdresserRepository : IHairdresserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HairdresserRepository> _logger;
    public HairdresserRepository(ApplicationDbContext context, ILogger<HairdresserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<HairdresserModel>> GetHairdressers()
    {
        if (await _context.Hairdressers.AnyAsync(x => x.Id == Guid.Empty))
            return null!;
        var hairdressers = await _context.Hairdressers.ToListAsync();
        return hairdressers;
    }
    public async Task<HairdresserModel?> GetHairdresser(Guid id)
    {
        var hairdresser = await _context.Hairdressers.FindAsync(id);
        if (hairdresser is null)
            return null!;
        return hairdresser;
    }
    public async Task<HairdresserModel> AddHairdresser(HairdresserDto hairdresser)
    {
        var hairdresserModel = new HairdresserModel()
        {
            Id = Guid.NewGuid(),
            FirstName = hairdresser.FirstName,
            LastName = hairdresser.LastName,
            Address = hairdresser.Address,
            HiredIn = hairdresser.HiredIn,
            Phone = hairdresser.Phone,
            Date = hairdresser.Date,
            IsBooked = hairdresser.IsBooked
        };
        await _context.Hairdressers.AddAsync(hairdresserModel);
        await _context.SaveChangesAsync();
        return hairdresserModel;
    }

    public async Task<HairdresserModel?> UpdateHairdresser(Guid id, HairdresserDto hairdresser)
    {
        var existingHairdresser = await _context.Hairdressers.FindAsync(id);
        if (existingHairdresser is null)
            return null;
        existingHairdresser.FirstName = hairdresser.FirstName;
        existingHairdresser.LastName = hairdresser.LastName;
        existingHairdresser.HiredIn = hairdresser.HiredIn;
        existingHairdresser.Phone = hairdresser.Phone;
        existingHairdresser.Address = hairdresser.Address;
        existingHairdresser.Date = hairdresser.Date;
        existingHairdresser.IsBooked = hairdresser.IsBooked;
        await _context.SaveChangesAsync();
        return existingHairdresser;
    }
    public async Task<HairdresserModel?> DeleteHairdresser(Guid id)
    {
        var existingHairdresser = await _context.Hairdressers.FindAsync(id);
        if (existingHairdresser is null)
            return null!;
        _context.Hairdressers.Remove(existingHairdresser);
        await _context.SaveChangesAsync();
        return existingHairdresser;   
    }

    public async Task<HairdresserModel?> ToBook(Guid id, DateTime date)
    {
        var hairdresser = await GetHairdresser(id);
        if (hairdresser is null || !hairdresser.IsBooked || hairdresser.Date != date ||
            hairdresser.Date <= DateTime.UtcNow)
        {
            _logger.LogError($"Hairdresser.Date: {hairdresser.Date.Date}, Booking.Date: {date.Date}");
            return null;
        }
        hairdresser.IsBooked = true;
        await _context.SaveChangesAsync();
        return hairdresser;
    }
}