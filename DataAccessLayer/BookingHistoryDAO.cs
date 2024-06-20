using BusinessObjects;
using DataAccessLayer.DTO;
using DataAccessObjects.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class BookingHistoryDAO
{
    public static async Task<BookingReservation?> GetBookingById(int id)
    {
        using var db = new FuminiHotelManagementContext();
        return await db.BookingReservations.FirstOrDefaultAsync(b => b.Equals(id));
    }

    public static async Task<List<BookingHistoryDTO>> GetBookingByCusId(int id)
    {
        using var db = new FuminiHotelManagementContext();
        return await db.BookingDetails
            .Include(bd => bd.BookingReservation)
                .ThenInclude(br => br.Customer)
            .Include(bd => bd.Room)
            .Where(bd => bd.BookingReservation.CustomerId == id
            && bd.BookingReservation.BookingStatus == 1)
            .Select(bd => new BookingHistoryDTO
            {
                BookingReservationId = bd.BookingReservationId,
                RoomId = bd.RoomId,
                RoomNumber = bd.Room.RoomNumber,
                StartDate = bd.StartDate,
                EndDate = bd.EndDate,
                ActualPrice = bd.ActualPrice,
                BookingDate = bd.BookingReservation.BookingDate,
                TotalPrice = bd.BookingReservation.TotalPrice,
                CustomerId = bd.BookingReservation.CustomerId,
                BookingStatus = bd.BookingReservation.BookingStatus
            })
            .ToListAsync();
    }

    public static int CountBookings()
    {
        using var db = new FuminiHotelManagementContext();
        return db.BookingReservations
            .Where(b => b.BookingStatus == 1)
            .Count();
    }

    public static decimal? CalcRevenue()
    {
        using var db = new FuminiHotelManagementContext();
        return db.BookingReservations
            .Where(b => b.BookingStatus == 1)
            .Sum(b => b.TotalPrice);
    }

    public static BookingReservation CreateBooking(BookingDTO bookingDto)
    {
        using var db = new FuminiHotelManagementContext();

        var bookingReservation = new BookingReservation
        {
            BookingReservationId = db.BookingReservations.Count() + 1,
            BookingDate = DateOnly.FromDateTime((DateTime)bookingDto.BookingDate),
            CustomerId = bookingDto.CustomerId,
            BookingStatus = bookingDto.BookingStatus,
            BookingDetails = new List<BookingDetail>(),
            TotalPrice = bookingDto.TotalPrice,
        };

        var booking = db.BookingReservations.Add(bookingReservation);

        db.SaveChanges();

        foreach (var detailDto in bookingDto.BookingDetails)
        {
            var room = db.RoomInformations.Find(detailDto.Room.RoomId);

            room.RoomStatus = 0;
            db.SaveChanges();

            var detail = new BookingDetail
            {
                BookingReservationId = booking.Entity.BookingReservationId,
                RoomId = detailDto.Room.RoomId,
                StartDate = DateOnly.FromDateTime(detailDto.StartDate),
                EndDate = DateOnly.FromDateTime(detailDto.EndDate),
                ActualPrice = detailDto.ActualPrice,
                Room = room,
            };

            //db.BookingDetails.Add(detail);
            booking.Entity.BookingDetails.Add(detail);
        }

        db.BookingReservations.Update(booking.Entity);

        db.SaveChanges();

        return bookingReservation;
    }

    public static BookingDTO? GetBookingDTOById(int id)
    {
        using var db = new FuminiHotelManagementContext();
        return db.BookingReservations
            .Where(b => b.BookingReservationId == id)
            .Include(b => b.BookingDetails)
                .ThenInclude(bd => bd.Room)
            .Select(b => new BookingDTO
            {
            }).FirstOrDefault();
    }

    public static async Task UpdateBooking(BookingHistoryDTO bookingDTO)
    {
        using var db = new FuminiHotelManagementContext();
        var bookingReservation = db.BookingReservations
            .Where(b => bookingDTO.BookingReservationId == b.BookingReservationId).FirstOrDefault();
        bookingReservation.BookingStatus = 0;
        db.BookingReservations.Update(bookingReservation);
        await db.SaveChangesAsync();
    }

    public static async Task UpdateBooking(BookingReservation booking)
    {
        using var db = new FuminiHotelManagementContext();
        booking.BookingStatus = 0;
        db.BookingReservations.Update(booking);
        await db.SaveChangesAsync();
    }
}
