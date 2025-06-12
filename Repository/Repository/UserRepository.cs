using Datos;
using EntityLayer;
using Repository.IRepository;
using Repository.Supabase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
            
        }

        #region SQL
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
                                    DocumentID = UtilitySQL.ObtieneString(reader, "DocumentID"),
                                    ResetPassword = UtilitySQL.ObtieneBool(reader, "ResetPassword"),
                                    IdIdentificationType = Convert.ToInt32(reader["IdIdentificationType"].ToString()),
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
                        cmd.Parameters.AddWithValue("@P_IdIdentificationType", user.IdIdentificationType);
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

        #endregion

        #region SUPABASE

        public async Task<List<UserE>> GetListAsync(UserE user)
        {
            try
            {
                var rpcParams = new
                {
                    p_opcion = user.Opcion,
                    p_id = user.ID,
                    p_user = user.User,
                    p_email = user.Email
                };

                var result = await SupabaseRest.PostRpcAsync<List<UserE>>("pa_con_mbr_tbl_user", rpcParams);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> MaintenanceAsync(UserE user)
        {
            try
            {
                var rpcParams = new
                {
                    p_opcion = user.Opcion,
                    p_id = user.ID,
                    p_user = user.User,
                    p_password = UtilitarioE.EncriptarString(user.Password),
                    p_id_role = user.Id_Role,
                    p_status = user.Status,
                    p_name = user.Name,
                    p_email = user.Email,
                    p_phonenumber = user.PhoneNumber,
                    p_documentid = user.DocumentID,
                    p_resetpassword = user.ResetPassword
                };

                await SupabaseRest.PostRpcAsync<object>("pa_man_mbr_tbl_user", rpcParams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> MaintenanceAsync(int opcion, int id, string user, string password, int role)
        {
            try
            {
                var rpcParams = new
                {
                    p_opcion = opcion,
                    p_id = id,
                    p_user = user,
                    p_password = UtilitarioE.EncriptarString(password),
                    p_id_role = role
                };

                await SupabaseRest.PostRpcAsync<object>("pa_man_mbr_tbl_user", rpcParams);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #endregion
    }
}
