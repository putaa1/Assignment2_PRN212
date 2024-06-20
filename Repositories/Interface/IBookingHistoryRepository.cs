using BusinessObjects;
using DataAccessLayer.DTO;
using DataAccessObjects.DTO.Request;

namespace Repositories.Interface
{
    public interface IBookingHistoryRepository
    {
        Task<BookingReservation?> GetBookingById(int id);
        Task<List<BookingHistoryDTO>> GetBookingByCusId(int id);
        BookingReservation CreateBooking(BookingDTO booking);
        Task UpdateBooking(BookingHistoryDTO booking);
        Task UpdateBooking(BookingReservation booking);
        int CountBookings();
        decimal? CalcRevenue();
    }
}
