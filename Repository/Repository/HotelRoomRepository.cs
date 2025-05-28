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
    public class HotelRoomRepository : IHotelRoomRepository
    {
        public List<Hotel_RoomE> GetList(Hotel_RoomE hotel_Room)
        {

            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.[PA_CON_MBR_TBL_HOTEL_ROOM]";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", hotel_Room.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID", hotel_Room.ID);
                        cmd.Parameters.AddWithValue("@P_Capacity", hotel_Room.Capacity);
                        cmd.Parameters.AddWithValue("@P_START_DATE", hotel_Room.StardDate);
                        cmd.Parameters.AddWithValue("@P_END_DATE", hotel_Room.EndDate);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Hotel_RoomE> List = new List<Hotel_RoomE>();
                            while (reader.Read())
                            {
                                List.Add(new Hotel_RoomE()
                                {
                                    ID = Convert.ToInt32(reader["ID"].ToString()),
                                    Description = reader["DESCRIPTION"].ToString(),
                                    Capacity=UtilitySQL.ObtieneInt(reader,"CAPACITY")
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

        public bool Maintenance(Hotel_RoomE hotel_Room)
        {
            throw new NotImplementedException();
        }
    }
}
