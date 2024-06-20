using BusinessObjects;
using DataAccessLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class RoomDAO
    {
        public static List<RoomDTO> GetRooms(Func<RoomInformation, bool> predicate)
        {
            using var db = new FuminiHotelManagementContext();
            return db.RoomInformations
                            .Include(r => r.RoomType)
                            .Where(predicate)
                            .Select(r => new RoomDTO
                            {
                                RoomId = r.RoomId,
                                RoomNumber = r.RoomNumber,
                                RoomDetailDescription = r.RoomDetailDescription,
                                RoomMaxCapacity = r.RoomMaxCapacity,
                                RoomStatus = r.RoomStatus,
                                RoomPricePerDay = r.RoomPricePerDay,
                                RoomType = r.RoomType.RoomTypeName,
                            })
                            .ToList();
        }

        public static int CountRooms()
        {
            using var db = new FuminiHotelManagementContext();
            return db.RoomInformations
                 .Where(r => r.RoomStatus == 1)
                 .Count();
        }

        public static void AddRoom(RoomDTO room)
        {
            using var db = new FuminiHotelManagementContext();
            var newRoom = new RoomInformation
            {
                RoomNumber = room.RoomNumber,
                RoomDetailDescription = room.RoomDetailDescription,
                RoomMaxCapacity = room.RoomMaxCapacity,
                RoomStatus = room.RoomStatus,
                RoomPricePerDay = room.RoomPricePerDay,
                RoomType = db.RoomTypes.FirstOrDefault(rt => rt.RoomTypeName == room.RoomType)
            };

            db.RoomInformations.Add(newRoom);
            db.SaveChanges();
        }

        public static void UpdateRoom(RoomDTO room)
        {
            using var db = new FuminiHotelManagementContext();
            var existingRoom = db.RoomInformations.Find(room.RoomId);
            if (existingRoom != null)
            {
                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.RoomDetailDescription = room.RoomDetailDescription;
                existingRoom.RoomMaxCapacity = room.RoomMaxCapacity;
                existingRoom.RoomStatus = room.RoomStatus;
                existingRoom.RoomPricePerDay = room.RoomPricePerDay;
                existingRoom.RoomType = db.RoomTypes.FirstOrDefault(rt => rt.RoomTypeName == room.RoomType);

                db.RoomInformations.Update(existingRoom);
                db.SaveChanges();
            }
        }

        public static void DeleteRoom(int roomId)
        {
            using var db = new FuminiHotelManagementContext();
            var room = db.RoomInformations.Find(roomId);
            if (room != null)
            {
                db.RoomInformations.Remove(room);
                db.SaveChanges();
            }
        }
    }
}
