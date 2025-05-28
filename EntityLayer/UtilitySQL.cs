using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class UtilitySQL
    {
        public static string ObtieneString(IDataReader reader, string columnName)
        {
            try
            {
                return reader[columnName].ToString().Trim();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string ObtieneNumeroString(IDataReader reader, string columnName)
        {
            try
            {
                return reader[columnName].ToString();
            }
            catch (Exception)
            {
                return "0";
            }
        }
        public static int ObtieneInt(IDataReader reader, string columnName)
        {
            try
            {
                return Convert.ToInt32(reader[columnName].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool ObtieneBool(IDataReader reader, string columnName)
        {

            try
            {
                return Convert.ToBoolean(Convert.ToInt32(reader[columnName].ToString()));
            }
            catch (Exception)
            {
                try
                {
                    return Convert.ToBoolean(reader[columnName].ToString());
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public static decimal ObtieneDecimal(IDataReader reader, string columnName)
        {
            try
            {
                NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
                numberFormatInfo.NumberDecimalSeparator = ",";
                numberFormatInfo.CurrencyDecimalSeparator = ",";
                numberFormatInfo.NumberGroupSeparator = ".";
                numberFormatInfo.CurrencyGroupSeparator = ".";

                string numero = reader[columnName].ToString().Replace('.', ',');
                return Convert.ToDecimal(numero, numberFormatInfo);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static double ObtieneDouble(IDataReader reader, string columnName)
        {
            try
            {
                NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
                numberFormatInfo.NumberDecimalSeparator = ",";
                numberFormatInfo.CurrencyDecimalSeparator = ",";
                numberFormatInfo.NumberGroupSeparator = ".";
                numberFormatInfo.CurrencyGroupSeparator = ".";

                string numero = reader[columnName].ToString().Replace('.', ',');
                return Convert.ToDouble(numero, numberFormatInfo);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static DateTime ObtieneDateTime(IDataReader reader, string columnName)
        {
            try
            {
                return Convert.ToDateTime(reader[columnName].ToString());
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }


    }
}
