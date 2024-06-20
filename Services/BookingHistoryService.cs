using BusinessObjects;
using DataAccessLayer.DTO;
using DataAccessObjects.DTO.Request;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class BookingHistoryService : IBookingHistoryService
    {
        private readonly IBookingHistoryRepository _repo;

        public BookingHistoryService()
        {
            _repo = new BookingHistoryRepository();
        }

        public async Task<BookingReservation?> GetBookingById(int id) => await _repo.GetBookingById(id);

        public async Task<List<BookingHistoryDTO>> GetBookingByCusId(int id) => await _repo.GetBookingByCusId(id);

        public BookingReservation CreateBooking(BookingDTO booking) => _repo.CreateBooking(booking);

        public async Task UpdateBooking(BookingHistoryDTO booking) => await _repo.UpdateBooking(booking);

        public int CountBookings() => _repo.CountBookings();

        public decimal? CalcRevenue() => _repo.CalcRevenue();
    }
}
