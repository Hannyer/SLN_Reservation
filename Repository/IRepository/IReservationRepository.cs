
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IReservationRepository
    {
        List<ReservationE> GetList(ReservationE reservation);
        int Maintenance(ReservationE reservation);
    }
}
