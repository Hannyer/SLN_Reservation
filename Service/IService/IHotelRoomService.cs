using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IHotelRoomService
    {
        List<Hotel_RoomE> GetList(Hotel_RoomE hotel_Room);
        bool Maintenance(Hotel_RoomE hotel_Room);
    }
}
