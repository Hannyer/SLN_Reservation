
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
    public class IdentificationTypeRepository : IIdentificationTypeRepository
    {
        public List<IdentificationTypeE> GetList(IdentificationTypeE IdentificationTypeE)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.[PA_CON_MBR_TBL_IDENTIFICATIONTYPE]";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", IdentificationTypeE.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID", IdentificationTypeE.ID);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<IdentificationTypeE> List = new List<IdentificationTypeE>();
                            while (reader.Read())
                            {
                                List.Add(new IdentificationTypeE()
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

        public bool Maintenance(IdentificationTypeE identificationType)
        {
            throw new NotImplementedException();
        }
    }
}
