
using EntityLayer;
using Repository.IRepository;
using Repository.Repository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class ReservationService : IReservationService
    {
        private IReservationRepository _reservationRepository;
        public ReservationService(IReservationRepository ireservationRepository)
        {
            this._reservationRepository = ireservationRepository;
        }
        public List<ReservationE> GetList(ReservationE reservation)
        {
            try
            {
                return _reservationRepository.GetList(reservation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int Maintenance(ReservationE reservation)
        {
            try
            {
                return _reservationRepository.Maintenance(reservation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}