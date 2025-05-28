using EntityLayer;
using System;
using System.Collections.Generic;


namespace Service.IService
{
    public interface IReservationReportService
    {
        List<ReservationReportE> GetList(ReservationReportE reservationReportE);
        List<TotalReportE> GetListTotalReport(TotalReportE totalReportE);

    }
}
