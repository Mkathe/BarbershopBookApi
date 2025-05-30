using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Domain;

namespace BarbershopBookApi.Application.Interfaces;

public interface IHairdresserRepository
{
    Task<IEnumerable<HairdresserModel>> GetHairdressers();
    Task<HairdresserModel?> GetHairdresserById(Guid id);
    Task<HairdresserModel?> GetHairdresserByLastName(string lastName);
    Task<bool> IsHairdresserFreeAtTheChoosingDate(string lastName, DateTime date);
    Task<HairdresserModel> AddHairdresser(HairdresserDto hairdresser);
    Task<HairdresserModel?> UpdateHairdresser(Guid id, HairdresserDto hairdresser);
    Task<HairdresserModel?> DeleteHairdresser(Guid id);
    Task<HairdresserModel?> ToBook(string lastName, DateTime date);
    Task<HairdresserModel?> ToUnBook(string lastName);
    Task<HairdresserModel?>? ToAddFreeDate(AddingDatesToHairdresserDto dateDto);
}