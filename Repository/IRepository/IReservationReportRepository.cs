
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IReservationReportRepository
    {
        List<ReservationReportE> GetList(ReservationReportE reservationReportE);
        List<TotalReportE> GetListTotalReport(TotalReportE totalReportE);
    }
}
