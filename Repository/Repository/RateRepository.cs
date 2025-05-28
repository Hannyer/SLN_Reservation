
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
    public class RateRepository : IRateRepository
    {
        public RateRepository() { }
        public List<RateE> GetList(RateE Rate)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.[PA_CON_MBR_TBL_RATE]";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", Rate.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID", Rate.ID);
                        cmd.Parameters.AddWithValue("@P_ID_RATETYPE", Rate.ID_RateTýpe);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<RateE> List = new List<RateE>();
                            while (reader.Read())
                            {
                                List.Add(new RateE()
                                {
                                    ID = Convert.ToInt32(reader["ID"].ToString()),
                                    DisplayName = reader["DISPLAYNAME"].ToString(),
                                    Description = reader["DESCRIPTION"].ToString(),
                                    Price = UtilitySQL.ObtieneDecimal(reader, "PRICE"),
                                    Currency = UtilitySQL.ObtieneString(reader, "CURRENCY"),
                                    ID_RateTýpe = UtilitySQL.ObtieneInt(reader, "ID_RATETYPE"),
                                    Description_RateType = UtilitySQL.ObtieneString(reader, "DESCRIPTION_RATETYPE"),
                                    Number_People = UtilitySQL.ObtieneInt(reader,"NUMBER_PEOPLE")
                                }) ;
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

        public bool Maintenance(RateE Rate)
        {
            throw new NotImplementedException();
        }
    }
}
