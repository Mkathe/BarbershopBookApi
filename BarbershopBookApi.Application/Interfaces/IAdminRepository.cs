using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Domain;

namespace BarbershopBookApi.Application.Interfaces;

public interface IAdminRepository
{
    Task<IEnumerable<AdminModel>> GetAdmins();
    Task<AdminModel> GetAdmin(Guid id);
    Task<AdminModel> AddAdmin(AdminModel admin);
    Task<bool> IsAdminExist(AdminDto admin);
    Task<AdminModel?> FindAdminByName(AdminDto admin);
    Task<AdminModel?> FindAdminById(Guid Id);
    Task<string> GenerateAndSaveRefreshToken(AdminModel adminModel);
}