using BusinessObjects;
using DataAccessLayer.DTO;
using DataAccessObjects.DTO.Request;

namespace Services.Interface
{
    public interface IBookingHistoryService
    {
        Task<BookingReservation?> GetBookingById(int id);
        Task<List<BookingHistoryDTO>> GetBookingByCusId(int id);
        BookingReservation CreateBooking(BookingDTO booking);
        Task UpdateBooking(BookingHistoryDTO booking);
        int CountBookings();
        decimal? CalcRevenue();
    }
}
