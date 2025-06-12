
using Datos;
using EntityLayer;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class DailyJobsRepository : IDailyJobsRepository
    {
        public DailyJobsRepository() { }
        public List<DailyJobsE> GetList(DailyJobsE dailyJobsE)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_CON_MBR_TBL_DAILYJOBS";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", dailyJobsE.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID", dailyJobsE.Id);
                        cmd.Parameters.AddWithValue("@P_TYPE", dailyJobsE.Type);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<DailyJobsE> List = new List<DailyJobsE>();
                            while (reader.Read())
                            {
                                List.Add(new DailyJobsE()
                                {
                                    Id = Convert.ToInt32(reader["Id"].ToString()),
                                    Type = reader["Type"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Date = Convert.ToDateTime(reader["Date"].ToString()),
                                    Status =reader["Status"].ToString(),
                                    Collaborator = reader["Collaborator"].ToString(),
                                    Frequency = reader["Frequency"].ToString(),
                                    Dias_Sin_Realizar = UtilitySQL.ObtieneInt(reader, "Dias_Sin_Realizar"),
                                    Name=UtilitySQL.ObtieneString(reader, "Name"),
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

        public int GetListCount(DailyJobsE dailyJobsE)
        {
            using (var connection = new SqlConnection(Connection.GetConnectionString()))
            using (var cmd = new SqlCommand("dbo.PA_CON_MBR_TBL_DAILYJOBS", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_OPCION", dailyJobsE.Opcion);
                cmd.Parameters.AddWithValue("@P_ID", dailyJobsE.Id);
                cmd.Parameters.AddWithValue("@P_TYPE", dailyJobsE.Type ?? string.Empty);
                connection.Open();
                object resultado = cmd.ExecuteScalar();
                return resultado == null || resultado == DBNull.Value
                    ? 0
                    : Convert.ToInt32(resultado);
            }
        }



        public bool Maintenance(DailyJobsE dailyJob)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    string script = "dbo.PA_MAN_MBR_TBL_DAILYJOBS";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_OPCION", dailyJob.Opcion);
                        cmd.Parameters.AddWithValue("@P_ID", dailyJob.Id);
                        cmd.Parameters.AddWithValue("@P_TYPE", dailyJob.Type);
                        cmd.Parameters.AddWithValue("@P_DESCRIPTION", dailyJob.Description);
                        cmd.Parameters.AddWithValue("@P_DATE", dailyJob.Date);
                        cmd.Parameters.AddWithValue("@P_STATUS", dailyJob.Status);
                        cmd.Parameters.AddWithValue("@P_COLLABORATOR", dailyJob.Collaborator);
                        cmd.Parameters.AddWithValue("@P_FREQUENCY", dailyJob.Frequency);

                        int AffectedRows = cmd.ExecuteNonQuery();

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
