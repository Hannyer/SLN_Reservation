using Repository.IRepository;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Datos;
using System.Xml.Linq;

namespace Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
            
        }
        public List<UserE> GetList(UserE user)
        {
            try
            {
                using (var connection= new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_CON_MBR_TBL_USER";
                    connection.Open();
                    using (SqlCommand cmd=new SqlCommand(script,connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", user.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID",user.ID);
                        cmd.Parameters.AddWithValue("@P_User", user.User);
                        cmd.Parameters.AddWithValue("@P_Email", user.Email);

                        using (SqlDataReader reader=cmd.ExecuteReader())
                        {
                            List <UserE> List=new List<UserE>();
                            while (reader.Read()) {
                                List.Add(new UserE()
                                {
                                    ID = Convert.ToInt32(reader["ID"].ToString()),
                                    User = reader["User"].ToString(),
                                    Password = UtilitarioE.DesencriptarString(reader["Password"].ToString()),
                                    Id_Role = Convert.ToInt32(reader["Id_Role"].ToString()),
                                    Description = reader["Descripcion"].ToString(),
                                    Status = UtilitySQL.ObtieneBool(reader, "Status"),
                                    Name = UtilitySQL.ObtieneString(reader,"Name"),
                                    Email = UtilitySQL.ObtieneString(reader, "Email"),
                                    PhoneNumber = UtilitySQL.ObtieneString(reader, "PhoneNumber"),
                                    DocumentID = UtilitySQL.ObtieneString(reader, "DocumentID")
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

        public bool Maintenance(int P_OPCION, int P_ID, string P_USER, string P_PASSWORD, int P_ROLE)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_MAN_MBR_TBL_user";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", P_OPCION);
                        cmd.Parameters.AddWithValue("@P_ID", P_ID);
                        cmd.Parameters.AddWithValue("@P_User", P_USER);
                        cmd.Parameters.AddWithValue("@P_Password", UtilitarioE.EncriptarString(P_PASSWORD));
                        cmd.Parameters.AddWithValue("@P_ID_ROLE", P_ROLE);
                       

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

        public bool Maintenance(UserE user)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_MAN_MBR_TBL_user";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", user.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID", user.ID);
                        cmd.Parameters.AddWithValue("@P_User", user.User);
                        cmd.Parameters.AddWithValue("@P_Password", UtilitarioE.EncriptarString(user.Password));
                        cmd.Parameters.AddWithValue("@P_ID_ROLE", user.Id_Role);
                        cmd.Parameters.AddWithValue("@P_Status", user.Status);
                        cmd.Parameters.AddWithValue("@P_Name", user.Name);
                        cmd.Parameters.AddWithValue("@P_Email", user.Email);
                        cmd.Parameters.AddWithValue("@P_PhoneNumber", user.PhoneNumber);
                        cmd.Parameters.AddWithValue("@P_DocumentID", user.DocumentID);
                        cmd.Parameters.AddWithValue("@P_ResetPassword", user.ResetPassword);
                        int AffectedRows = Convert.ToInt32(cmd.ExecuteNonQuery());

                        return AffectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
