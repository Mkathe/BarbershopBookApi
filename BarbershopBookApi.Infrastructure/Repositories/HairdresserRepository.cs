using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace BarbershopBookApi.Infrastructure.Repositories;

public class HairdresserRepository : IHairdresserRepository
{
    private readonly ApplicationDbContext _context;

    public HairdresserRepository(ApplicationDbContext context)
    {
        _context = context;
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
    public async Task<HairdresserModel> AddHairdresser(HairdresserModel hairdresser)
    {
        await _context.Hairdressers.AddAsync(hairdresser);
        await _context.SaveChangesAsync();
        return hairdresser;
    }

    public async Task<HairdresserModel?> UpdateHairdresser(Guid id, HairdresserDto hairdresser)
    {
        var existingHairdresser = await _context.Hairdressers.FindAsync(id);
        if (existingHairdresser is null)
            return null!;
        existingHairdresser.FirstName = hairdresser.FirstName;
        existingHairdresser.LastName = hairdresser.LastName;
        existingHairdresser.HiredIn = hairdresser.HiredIn;
        existingHairdresser.Phone = hairdresser.Phone;
        existingHairdresser.Address = hairdresser.Address;
        await _context.SaveChangesAsync();
        return existingHairdresser;
    }
    public async Task<HairdresserModel?> DeleteHairdresser(HairdresserModel hairdresser)
    {
        var existingHairdresser = await _context.Hairdressers.FindAsync(hairdresser.Id);
        if (existingHairdresser is null)
            return null!;
        _context.Hairdressers.Remove(existingHairdresser);
        await _context.SaveChangesAsync();
        return existingHairdresser;   
    }
}