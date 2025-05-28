using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repository
{
    public  class Connection
    {


        public static string GetConnectionString()
        {
            try
            {

                string XMLFile = ConfigurationManager.AppSettings["StringConnetion"];

                if (string.IsNullOrEmpty(XMLFile))
                {
                    throw new Exception("La ruta del archivo XML no está configurada en web.config.");
                }

                XDocument xmlDoc = XDocument.Load(XMLFile);

                string server = xmlDoc.Element("CONFIGURATION").Element("SERVER").Value;
                string database = xmlDoc.Element("CONFIGURATION").Element("DATABASE").Value;
                string user = xmlDoc.Element("CONFIGURATION").Element("USER").Value;
                string password = xmlDoc.Element("CONFIGURATION").Element("PASSWORD").Value;


                string cadenaConexion = $"Server={server};Database={database};User={user};Password={password};";
                return cadenaConexion;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener la cadena de conexión: {ex.Message}");
                return null;
            }
        }
        //public static string GetConnectionString()
        //{
        //    try
        //    {

        //        string connetion = ConfigurationManager.AppSettings["StringConnetion2"];

        //        return connetion;
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine($"Error al obtener la cadena de conexión: {ex.Message}");
        //        return null;
        //    }
        //}


    }
}
