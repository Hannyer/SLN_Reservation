using EntityLayer;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class RoleRepository : IRoleRepository
    {
        public List<RoleE> GetList(RoleE RoleE)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_CON_MBR_TBL_Role";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", RoleE.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID_ROLE", RoleE.ID_Role);
                        int a = 0;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<RoleE> List = new List<RoleE>();
                            while (reader.Read())
                            {
                                List.Add(new RoleE()
                                {
                                    ID_Role = Convert.ToInt32(reader["ID_ROLE"].ToString()),
                                    Description = reader["DESCRIPCION"].ToString(),
                                    Status = reader["ESTADO"].ToString().Equals("1")?true:false,
                                  
                                });
                            }
                            return List;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int Maintenance(RoleE RoleE)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.[PA_MAN_MBR_TBL_ROLE]";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", RoleE.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID_ROLE", RoleE.ID_Role);
                        cmd.Parameters.AddWithValue("@P_DESCRIPTION", RoleE.Description);
                        cmd.Parameters.AddWithValue("@P_STATUS", RoleE.Status);


                      int rowAffected= cmd.ExecuteNonQuery();

                        return rowAffected;

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
