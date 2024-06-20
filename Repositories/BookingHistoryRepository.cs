using BusinessObjects;
using DataAccessLayer;
using DataAccessLayer.DTO;
using DataAccessObjects.DTO.Request;
using Repositories.Interface;

namespace Repositories
{
    public class BookingHistoryRepository : IBookingHistoryRepository
    {
        public async Task<BookingReservation?> GetBookingById(int id) => await BookingHistoryDAO.GetBookingById(id);

        public async Task<List<BookingHistoryDTO>> GetBookingByCusId(int id) => await BookingHistoryDAO.GetBookingByCusId(id);

        public BookingReservation CreateBooking(BookingDTO booking) => BookingHistoryDAO.CreateBooking(booking);

        public async Task UpdateBooking(BookingHistoryDTO booking) => await BookingHistoryDAO.UpdateBooking(booking);

        public async Task UpdateBooking(BookingReservation booking) => await BookingHistoryDAO.UpdateBooking(booking);

        public int CountBookings() => BookingHistoryDAO.CountBookings();

        public decimal? CalcRevenue() => BookingHistoryDAO.CalcRevenue();
    }
}
