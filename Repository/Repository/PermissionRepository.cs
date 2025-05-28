using EntityLayer;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        public List<PermissionE> GetList(PermissionE permission)
        {
            throw new NotImplementedException();
        }

        public void Maintenance(PermissionE permission)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.[PA_MAN_MBR_TBL_PERMISSIONS]";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION ", permission.Opcion);
                       
                        cmd.Parameters.AddWithValue("@P_PK_TBL_MTRA_SEG_PERMISOS", permission.ID);
                        cmd.Parameters.AddWithValue("@P_FK_TBL_MTRA_SEG_PERFIL", permission.FK_Role);
                        cmd.Parameters.AddWithValue("@P_FK_TBL_MTRA_SEG_MENU", permission.FK_Menu);
                        cmd.Parameters.AddWithValue("@P_DESCRIPCION", permission.Description);
                        cmd.Parameters.AddWithValue("@P_ESTADO", permission.Status);
                        cmd.Parameters.AddWithValue("@P_LISTA_MENU", permission.ListMenu);
                        cmd.Parameters.AddWithValue("@P_LISTA_PERMISOS", permission.ListPermmison);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
              
            }
        }
    }
}
