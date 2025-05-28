using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Configuration;
using System.Xml;

namespace EntityLayer
{
    public class UtilitarioE
    {

        //metodo que envia parámetro y ejecuta el metodo Desencriptar
        public static string DesencriptarString(string textoEncriptado)
        {
            return Desencriptar(textoEncriptado, "HotelMalibu", "s@lAvz", "MD5",
              1, "@1B2c3D4e5F6g7H8", 128);
        }
        //metodo que envia parámetro y ejecuta el metodo encriptar
        public static string EncriptarString(string stringEncriptado)
        {
            return Encriptar(stringEncriptado,
              "HotelMalibu", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
        }
        //metodo encargado de Encriptar nuestro texto 
        public static string Encriptar(string textoQueEncriptaremos, string passBase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,
              saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes,
              initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor,
             CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        public static string Desencriptar(string textoEncriptado, string passBase,
        string saltValue, string hashAlgorithm, int passwordIterations,
        string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,
              saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes,
              initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor,
              CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0,
              plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0,
              decryptedByteCount);
            return plainText;
        }

        public static void SendEmail(EmailConfigurationE emailConfigurationE,string toAddress, string subject, string body)
        {
            try
            {

                SmtpClient smtp = new SmtpClient
                {
                    Host = emailConfigurationE.Host,
                    Port = emailConfigurationE.Port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailConfigurationE.Email, emailConfigurationE.Password)
                };


                using (MailMessage message = new MailMessage(emailConfigurationE.Email, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {

                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            { 

                throw;
            }
          
        }
        public static async Task<DollarDataE> GetDollarValue()
        {
            try
            {
                string ApiUrl = ConfigurationManager.AppSettings["URL_API_DailyExchangeRate"];
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(ApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var sale = JsonConvert.DeserializeObject<DollarDataE>(content);
                        return sale;
                        //DateTime fecha = DateTime.ParseExact(sale.DollarBuyE.Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        //ViewBag.BuyDate = fecha.ToString("dd/MM/yyyy");
                        //fecha = DateTime.ParseExact(sale.DollarSaleE.Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        //ViewBag.SaleDate = fecha.ToString("dd/MM/yyyy");

                        //ViewBag.SaleValue = sale.DollarSaleE.Value;
                        //ViewBag.BuyValue = sale.DollarBuyE.Value;
                    }
                    else {
                        throw new Exception("Error al comunicase con el API, favor comunicarse con el departamento de TI");
                    
                    }
                   
                }
            }
            catch (Exception ex)
            {
              throw ex;
            }

           
        }

        // Método para generar la clave numérica de la factura electrónica
        public static string GenerateElectronicInvoiceKey(
            int countryCode,
            DateTime issueDate,
            string issuerTaxId,
            long consecutiveNumber,
            int situationCode,
            string securityCode)
        {
            // Construir el consecutivo como una cadena de texto
            string consecutive = $"{countryCode.ToString("D3")}" +
                                 $"{issueDate.Day.ToString("D2")}" +
                                 $"{issueDate.Month.ToString("D2")}" +
                                 $"{issueDate.Year % 100:D2}" + // Agarramos los ultimos 2 digitos del año
                                 $"{issuerTaxId.PadLeft(12, '0')}" + // Se rellena con 0 si es necesario para el consecutivo
                                 $"{consecutiveNumber.ToString("D20")}" +
                                 $"{situationCode}" +
                                 $"{securityCode}";

            return consecutive;
        }

        // Método para validar la clave numérica de la factura electrónica
        public static bool ValidateElectronicInvoiceKey(string electronicInvoiceKey)
        {
            // Validar longitud total de 50 dígitos
            if (electronicInvoiceKey.Length != 50)
            {
                return false;
            }

            // Validar el código del país (3 dígitos y debe ser 506)
            if (electronicInvoiceKey.Substring(0, 3) != "506")
            {
                return false;
            }

            // Validar la estructura de la fecha (6 dígitos)
            if (!DateTime.TryParseExact(electronicInvoiceKey.Substring(3, 6), "ddMMyy", null, System.Globalization.DateTimeStyles.None, out _))
            {
                return false;
            }

            // Validar la cédula del emisor (12 dígitos)
            if (electronicInvoiceKey.Substring(9, 12).Length != 12)
            {
                return false;
            }

            // Validar el consecutivo del comprobante electrónico (20 dígitos)
            if (electronicInvoiceKey.Substring(21, 20).Length != 20)
            {
                return false;
            }

            // Validar el código de situación (1 dígito)
            int situationCode = int.Parse(electronicInvoiceKey.Substring(41, 1));
            if (situationCode < 1 || situationCode > 3)
            {
                return false;
            }

            // Validar el código de seguridad (8 dígitos)
            if (electronicInvoiceKey.Substring(42, 8).Length != 8)
            {
                return false;
            }

            return true;
        }

        public static string CrearFacturaElectronicaXml()
        {
            XmlDocument doc = new XmlDocument();

            // Creación del elemento raíz
            XmlElement root = doc.CreateElement("FacturaElectronica");
            root.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            root.SetAttribute("xmlns", "https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.3/facturaElectronica");
            doc.AppendChild(root);

            // Agregar elementos como Clave, CodigoActividad, etc.
            root.AppendChild(CreateElement(doc, "Clave", "50613102300310161019800100024010035551646200000000"));
            root.AppendChild(CreateElement(doc, "CodigoActividad", "642005"));
            root.AppendChild(CreateElement(doc, "NumeroConsecutivo", "00100024010035551646"));
            root.AppendChild(CreateElement(doc, "FechaEmision", "2023-10-13T02:38:25-06:00"));

            // Emisor
            XmlElement emisor = doc.CreateElement("Emisor");
            emisor.AppendChild(CreateElement(doc, "Nombre", "LIBERTY TELECOMUNICACIONES DE COSTA RICA LY S.A."));
            // ... Agregar más elementos al emisor ...
            root.AppendChild(emisor);

            // Receptor
            XmlElement receptor = doc.CreateElement("Receptor");
            receptor.AppendChild(CreateElement(doc, "Nombre", "HANNYER SMYKEL PITTERSON MARTINEZ"));
            // ... Agregar más elementos al receptor ...
            root.AppendChild(receptor);

            // DetalleServicio
            XmlElement detalleServicio = doc.CreateElement("DetalleServicio");
            // Aquí agregarías cada LineaDetalle como se muestra abajo
            // Ejemplo para una LineaDetalle
            XmlElement lineaDetalle = doc.CreateElement("LineaDetalle");
            lineaDetalle.AppendChild(CreateElement(doc, "NumeroLinea", "1"));
            // ... Agregar más elementos a la LineaDetalle ...
            detalleServicio.AppendChild(lineaDetalle);
            // ... Agregar más LineaDetalle si es necesario ...
            root.AppendChild(detalleServicio);

            // OtrosCargos
            // Similar a LineaDetalle, aquí agregarías los elementos para OtrosCargos
            // ...

            // ResumenFactura
            XmlElement resumenFactura = doc.CreateElement("ResumenFactura");
            // ... Agregar los elementos al resumenFactura ...
            root.AppendChild(resumenFactura);

            // Otros
            // Similar a los anteriores, agregar los elementos para Otros
            // ...

            return BeautifyXml(doc);
        }

        private static XmlElement CreateElement(XmlDocument doc, string name, string innerText)
        {
            XmlElement element = doc.CreateElement(name);
            element.InnerText = innerText;
            return element;
        }

        private static string BeautifyXml(XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
                Encoding = new UTF8Encoding(false)
            };

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();
        }


    }
}
