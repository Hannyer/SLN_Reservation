using Datos;
using EntityLayer;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Repository.Repository
{
    public class AboutRepository : IAboutRepository
    {
        public List<AboutE> GetList(AboutE about)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_CON_MBR_TBL_About";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", about.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID", about.ID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<AboutE> List = new List<AboutE>();
                            while (reader.Read())
                            {
                                List.Add(new AboutE()
                                {

                                    ID = UtilitySQL.ObtieneInt(reader,"ID"),
                                    Name = UtilitySQL.ObtieneString(reader, "Name"),
                                    ID_Hotel = UtilitySQL.ObtieneString(reader, "ID_HOTEL"),
                                    SecurityCode = UtilitySQL.ObtieneString(reader, "SecurityCode"),
                                    Phone = UtilitySQL.ObtieneString(reader, "PHONE"),
                                    Email = UtilitySQL.ObtieneString(reader, "EMAIL"),
                                    Cabys_service=UtilitySQL.ObtieneString(reader, "CABY_SERVICE"),
                                    ID_Type = UtilitySQL.ObtieneString(reader, "ID_Type"),
                                    Province = UtilitySQL.ObtieneString(reader, "Province"),
                                    Canton = UtilitySQL.ObtieneString(reader, "Canton"),
                                    Distric = UtilitySQL.ObtieneString(reader, "Distric"),
                                    OtherSigns = UtilitySQL.ObtieneString(reader, "OtherSigns"),
                                    ActivityCode = UtilitySQL.ObtieneString(reader, "ActivityCode"),
                                    
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
    }
}
