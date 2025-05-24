using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Domain;

namespace BarbershopBookApi.Application.Interfaces;

public interface IHairdresserRepository
{
    Task<IEnumerable<HairdresserModel>> GetHairdressers();
    Task<HairdresserModel?> GetHairdresser(Guid id);
    Task<HairdresserModel> AddHairdresser(HairdresserModel hairdresser);
    Task<HairdresserModel?> UpdateHairdresser(Guid id, HairdresserDto hairdresser);
    Task<HairdresserModel?> DeleteHairdresser(HairdresserModel hairdresser);
}