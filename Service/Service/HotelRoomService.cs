using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class HotelRoomService : IHotelRoomService
    {
        IHotelRoomRepository _hotelRoomRepository;
        public HotelRoomService(IHotelRoomRepository hotelRoomService) {
            _hotelRoomRepository= hotelRoomService;
        }
        public List<Hotel_RoomE> GetList(Hotel_RoomE hotel_Room)
        {
           return _hotelRoomRepository.GetList(hotel_Room);
        }

        public bool Maintenance(Hotel_RoomE hotel_Room)
        {
            throw new NotImplementedException();
        }
    }
}