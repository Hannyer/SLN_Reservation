using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IClientRepository
    {
        List<ClientE> GetList(ClientE cient);

        bool Maintenance(ClientE client);
    }
}
