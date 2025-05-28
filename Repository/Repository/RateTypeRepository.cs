using Datos;
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
    public class RateTypeRepository : IRateTypeRepository
    {
        public List<RateTypeE> GetList(RateTypeE RateType)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.[PA_CON_MBR_TBL_RATETYPE]";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", RateType.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID", RateType.ID);
                   

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<RateTypeE> List = new List<RateTypeE>();
                            while (reader.Read())
                            {
                                List.Add(new RateTypeE()
                                {
                                    ID = Convert.ToInt32(reader["ID"].ToString()),
                                    Description = reader["DESCRIPTION"].ToString()
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

        public bool Maintenance(RateTypeE RateType)
        {
            throw new NotImplementedException();
        }
    }
}
