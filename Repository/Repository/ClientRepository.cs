using Repository.IRepository;

using EntityLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Repository.Repository
{
    public class ClientRepository : IClientRepository
    {
        public ClientRepository() { }
        public List<ClientE> GetList(ClientE cient)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_CON_MBR_TBL_Client";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", cient.Opcion);
                      
                        cmd.Parameters.AddWithValue("@P_IdCard", cient.IdCard);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<ClientE> List = new List<ClientE>();
                            while (reader.Read())
                            {
                                List.Add(new ClientE()
                                {
                                   
                                    IdCard = reader["IdCard"].ToString(),
                                    Full_Name = reader["Full_Name"].ToString(),
                                    Phone_number1 = reader["Phone_number1"].ToString(),
                                    Phone_number2 = reader["Phone_number2"].ToString(),
                                    Mail = reader["Mail"].ToString(),
                                    Detail = reader["Detail"].ToString(),
                                    IdentificationType_Id = UtilitySQL.ObtieneInt(reader, "ID_IDENTIFICATIONTYPE"),
                                    IdentificationType_Description = UtilitySQL.ObtieneString(reader, "IDENTIFICATIONTYPE_DESCRIPTION"),
                                    RateType_Id = UtilitySQL.ObtieneInt(reader, "ID_RATETYPE"),
                                    RateType_Description = UtilitySQL.ObtieneString(reader, "RATETYPE_DESCRIPTION"),
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


        public bool Maintenance(ClientE client)
        {

            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_MAN_MBR_TBL_Client";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", client.Opcion);
                        cmd.Parameters.AddWithValue("@P_FULL_NAME", client.Full_Name);
                        cmd.Parameters.AddWithValue("@P_IDCARD", client.IdCard);
                        cmd.Parameters.AddWithValue("@P_PHONE_NUMER1", client.Phone_number1);
                        cmd.Parameters.AddWithValue("@P_PHONE_NUMER2", client.Phone_number2);
                        cmd.Parameters.AddWithValue("@P_MAIL", client.Mail);
                        cmd.Parameters.AddWithValue("@P_DETAIL", client.Detail);
                        cmd.Parameters.AddWithValue("@P_IDENTIFYCATIONTYPE", client.IdentificationType_Id);
                        cmd.Parameters.AddWithValue("@P_RATETYPE_ID", client.RateType_Id);

                        int AffectedRows = cmd.ExecuteNonQuery();

                        return AffectedRows > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
