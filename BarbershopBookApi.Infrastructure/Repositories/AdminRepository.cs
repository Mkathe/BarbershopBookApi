using System.Security.Cryptography;
using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace BarbershopBookApi.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _context;
    public AdminRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AdminModel>> GetAdmins()
    {
        var admins = await _context.Admins.ToListAsync();
        return admins;
    }
    public async Task<AdminModel> GetAdmin(Guid id)
    {
        var admin = await _context.Admins.FindAsync(id);
        if (admin is null)
            return null!;
        return admin;
    }
    public async Task<AdminModel> AddAdmin(AdminModel admin)
    {
        await _context.Admins.AddAsync(admin);
        await _context.SaveChangesAsync();
        return admin;
    }

    public async Task<bool> IsAdminExist(AdminDto admin)
    {
        return await _context.Admins.AnyAsync(x => x.UserName == admin.UserName);
    }

    public async Task<AdminModel?> FindAdminByName(AdminDto admin)
    {
        return await _context.Admins.FirstOrDefaultAsync(x => x.UserName == admin.UserName);

    }
    public async Task<AdminModel?> FindAdminById(Guid Id)
    {
        return await _context.Admins.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<string> GenerateAndSaveRefreshToken(AdminModel adminModel)
    {
        var refreshToken = GenerateRefreshToken();
        adminModel.RefreshToken = refreshToken;
        adminModel.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();
        return refreshToken;
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public async Task<AdminModel> DeleteAdmin(AdminModel admin)
    {
        var existingAdmin = await _context.Admins.FindAsync(admin.Id);
        if (existingAdmin is null)
            return null!;
        _context.Admins.Remove(existingAdmin);
        await _context.SaveChangesAsync();
        return existingAdmin;
    }

}