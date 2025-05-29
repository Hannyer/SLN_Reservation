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

namespace Repository.Repository
{
    public class MenuRepository : IMenuRepository
    {
        public List<MenuE> GetList(MenuE menu)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.[PA_CON_MBR_MENU]";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", menu.Opcion);
                        cmd.Parameters.AddWithValue("@P_PK_MENU", menu.ID);
                        cmd.Parameters.AddWithValue("@P_FK_ROLE", menu.IDP_ROLE);
                        cmd.Parameters.AddWithValue("@P_MENU_STATUS", menu.STATUS_Menu ? "1" : "");
                        cmd.Parameters.AddWithValue("@P_PERMISSON_STATUS", menu.Permisson_Status ? "1" : "");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<MenuE> List = new List<MenuE>();
                            while (reader.Read())
                            {

                                List.Add(new MenuE()
                                {
                                    ID = Convert.ToInt32(reader["ID_MENU"].ToString()),
                                    PARENT_CODE = reader["PARENT_CODE"].ToString(),
                                    DESCRIPTION = reader["DESCRIPTION"].ToString(),
                                    ICON = reader["ICON"].ToString(),
                                    CONTROLLER = reader["CONTROLLER"].ToString(),
                                    ACTION = reader["ACTION"].ToString(),
                                    STATUS_Menu = UtilitySQL.ObtieneBool(reader,"STATUS"),
                                    Creeate_Menu = UtilitySQL.ObtieneBool(reader, "Create"),
                                    Edit_Menu = UtilitySQL.ObtieneBool(reader, "Edit"),
                                    View_Menu = UtilitySQL.ObtieneBool(reader, "View"),
                                    Send_Menu = UtilitySQL.ObtieneBool(reader, "Send"),
                                    Permisson_Status = UtilitySQL.ObtieneBool(reader, "PERMISSON_STATUS"),
                                    Permisson_Create = UtilitySQL.ObtieneBool(reader,"PERMISSON_CREATE"),
                                    Permisson_Edit = UtilitySQL.ObtieneBool(reader,"PERMISSON_EDIT"),
                                    Permisson_View = UtilitySQL.ObtieneBool(reader,"PERMISSON_VIEW"),
                                    Permisson_Send = UtilitySQL.ObtieneBool(reader,"PERMISSON_SEND"),
                                    Order = UtilitySQL.ObtieneInt(reader, "Order"),
                                    Image=UtilitySQL.ObtieneString(reader,"Image")

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
        public async Task<List<MenuE>> GetListAsync(MenuE menu)
        {
            try
            {
                var rpcParams = new
                {
                    p_opcion = (short)menu.Opcion,
                    p_pk_menu = menu.ID,
                    p_fk_role = menu.IDP_ROLE,
                    p_menu_status = menu.STATUS_Menu ? "1" : null,
                    p_perm_status = menu.Permisson_Status ? "1" : null
                };
                var task = await SupabaseRest.PostRpcAsync<List<MenuE>>(
                    "pa_con_mbr_menu",
                    rpcParams
                );
             
                return task;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public void Maintenance(MenuE menu)
        {
            throw new NotImplementedException();
        }
    }
    
}
