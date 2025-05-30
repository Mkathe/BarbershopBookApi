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
        public async Task<HairdresserModel?> GetHairdresserById(Guid id)
        {
            var hairdresser = await _context.Hairdressers.FindAsync(id);
            if (hairdresser is null)
                return null!;
            return hairdresser;
        }
        public async Task<HairdresserModel?> GetHairdresserByLastName(string lastName)
        {
            var hairdresser = await _context.Hairdressers.FirstOrDefaultAsync(x => x.LastName == lastName);
            if (hairdresser is null)
                return null!;
            return hairdresser;
        }

        public async Task<bool> IsHairdresserFreeAtTheChoosingDate(string lastName, DateTime date)
        {
            var hairdresser = await GetHairdresserByLastName(lastName);
            if (hairdresser is null)
                return false;
            return hairdresser.FreeDateTime.Contains(date);
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
                Email = hairdresser.Email,
                FreeDateTime = hairdresser.FreeDateTime
                    .Select(dt =>
                    {
                        var result = dt.Kind == DateTimeKind.Utc ? dt : dt.ToUniversalTime();
                        _logger.LogWarning("DateTime kind: {kind}", dt.Kind);
                        return result;
                    })
                    .ToList(),
                IsBooked = hairdresser.IsBooked
            };
            _logger.LogWarning(0,"FreeDate dates: {dates}", hairdresser.FreeDateTime);
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
            existingHairdresser.Email = hairdresser.Email;
            existingHairdresser.Address = hairdresser.Address;
            existingHairdresser.FreeDateTime = hairdresser.FreeDateTime
                .Select(dt =>
                {
                    
                    var result = dt.Kind == DateTimeKind.Utc ? dt : dt.ToUniversalTime();
                    _logger.LogWarning("DateTime kind: {kind}", dt.Kind);
                    return result;
                })
                .ToList();
            _logger.LogWarning("The existing dates: {date} and hairdresserDtos' dates {dtoDate}", existingHairdresser.FreeDateTime, hairdresser.FreeDateTime );
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
        public async Task<HairdresserModel?> ToBook(string lastName, DateTime date)
        {
            var hairdresser = await GetHairdresserByLastName(lastName);
            if (hairdresser is null || 
                hairdresser.IsBooked || 
                !hairdresser.FreeDateTime.Contains(date) || 
                date <= DateTime.Now)
            {
                _logger.LogError("Hairdresser.Date: {hairdresser.Date}, Booking.Date: {date}",hairdresser.FreeDateTime, date);
                return null;
            }
            hairdresser.FreeDateTime.Remove(date);
            hairdresser.IsBooked = true;
            await _context.SaveChangesAsync();
            return hairdresser;
        }

        public async Task<HairdresserModel?> ToUnBook(string lastName)
        {
            var hairdresser = await _context.Hairdressers.FirstOrDefaultAsync(x => x.LastName == lastName);
            if (hairdresser is null)
                return null;
            hairdresser.IsBooked = false;
            await _context.SaveChangesAsync();
            return hairdresser;
        }
    }