using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;


namespace Service.Service
{
    public class ReservationReportService : IReservationReportService
    {
        private IReservationReportRepository _ReservationReportRepository;
        public ReservationReportService(IReservationReportRepository reservationReportRepository)
        {
            this._ReservationReportRepository = reservationReportRepository;
        }
        public List<ReservationReportE> GetList(ReservationReportE reservationReportE)
        {
            try
            {
                return _ReservationReportRepository.GetList(reservationReportE);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TotalReportE> GetListTotalReport(TotalReportE totalReportE)
        {
            try
            {
                return _ReservationReportRepository.GetListTotalReport(totalReportE);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}