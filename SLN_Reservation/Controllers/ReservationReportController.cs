
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using EntityLayer;
using iTextSharp.text.pdf.codec.wmf;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace SLN_Reservation.Controllers
{
    public class ReservationReportController : Controller
    {
        static List<ReservationReportE> tmplist = new List<ReservationReportE>();
        static List<TotalReportE> tmplistTotalReport = new List<TotalReportE>();
        static List<ReservationE> tmplistAvalaibilityReport = new List<ReservationE>();
        IReservationReportService _ReservationReportService;
        IReservationService _reservation;
        IConfigurationService _configurationService;
        // GET: ReservationReport
        public ReservationReportController(IReservationReportService reservationReportService, IReservationService reservation, IConfigurationService configurationService)
        {
            this._ReservationReportService = reservationReportService;
            _reservation = reservation;
            _configurationService = configurationService;
        }
        public ActionResult ReservationReport()
        {
            List<ReservationReportE> list = new List<ReservationReportE>();
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            return View(list);
        }
        public ActionResult TotalReport()
        {
            List<TotalReportE> list = new List<TotalReportE>();
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            return View(list);
        }

        public PartialViewResult ExportData(string checkIn, string tmpCheckOut)
        {
            var user = Session["User"] as UserE;
            int Opcion = 0;
            int userId = 0;

            var rolClient = _configurationService.GetList(new ConfigurationE()
            {
                Opcion = 0,
                KEY01 = "PARAMETRO",
                KEY02 = "FUNCIONALIDAD",
                KEY03 = "MRB",
                KEY04 = "ROL",
                KEY05 = "CLIENTE",
                KEY06 = "ROLCLIENTE"
            });

            if (user.Id_Role.ToString() == rolClient.First().VALUE.ToString())
            {
                userId = user.ID;
            }

            string answer = "";
            DateTime parsedCheckIn;
            DateTime parsedCheckOut;

            DateTime.TryParse(checkIn, out parsedCheckIn);
            DateTime.TryParse(tmpCheckOut, out parsedCheckOut);

            List<ReservationReportE> list = new List<ReservationReportE>();
            list = _ReservationReportService.GetList(new ReservationReportE()
            {
                Opcion = Opcion,
                Identification = "",
                Days = "",
                checkIn = parsedCheckIn,
                checkOut = parsedCheckOut,
                SubTotalWithOutTax = 0,
                TaxAmount = 0,
                TotalAmount = 0,
                UserId = userId
            });
            tmplist = list;
            return PartialView("ViewExportData", list);
        }



        public async Task<ContentResult> CancelReservation(int reservationId)
        {
            try
            {
                ReservationE reservationToCancel = _reservation.GetList(new ReservationE { Opcion = 2, Id = reservationId, START_DATE = DateTime.Now, END_DATE = DateTime.Now }).FirstOrDefault();

                if (reservationToCancel == null)
                {
                    return Content("error: La reserva no fue encontrada.");
                }

                ReservationE reservation = new ReservationE
                {
                    Opcion = 1, 
                    Id = reservationId,
                    CheckIn = DateTime.Now, 
                    CheckOut = DateTime.Now
                };

                int result = _reservation.Maintenance(reservation); 

                if (result > 0) 
                {
                    var configEmail = _configurationService.GetList(new ConfigurationE()
                    {
                        Opcion = 0,
                        KEY01 = "PARAMETRO",
                        KEY02 = "FUNCIONALIDAD",
                        KEY03 = "MRB",
                        KEY04 = "CREDENCIALES",
                        KEY05 = "CORREO"
                    });

                    EmailConfigurationE emailConfig = new EmailConfigurationE()
                    {
                        Email = configEmail.Where(x => x.KEY06 == "CORREO").FirstOrDefault().VALUE,
                        Password = configEmail.Where(x => x.KEY06 == "PASSWORD").FirstOrDefault().VALUE,
                        Host = configEmail.Where(x => x.KEY06 == "HOST").FirstOrDefault().VALUE,
                        Port = Convert.ToInt32(configEmail.Where(x => x.KEY06 == "PORT").FirstOrDefault().VALUE),
                    };

                    string cancellationEmailBody = GenerateReservationCancellationEmail(reservationToCancel);

                    UtilitarioE.SendEmail(emailConfig, reservationToCancel.Client_Mail, "Confirmación de Cancelación de Reserva", cancellationEmailBody);

                    return Content("ok");
                }
                else
                {
                    return Content("error: No se pudo cancelar la reserva.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cancelar reserva: {ex.Message} - StackTrace: {ex.StackTrace}");
                return Content("error: Ha ocurrido un error inesperado al cancelar la reserva.");
            }
        }

        public string GenerateReservationCancellationEmail(ReservationE reservation)
        {
            var emailContentBuilder = new StringBuilder();

            emailContentBuilder.AppendLine("<html>");
            emailContentBuilder.AppendLine("<head>");
            emailContentBuilder.AppendLine("<meta charset=\"UTF-8\">");
            emailContentBuilder.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            emailContentBuilder.AppendLine("<title>Cancelación de Reserva - Hotel CTP</title>");
            emailContentBuilder.AppendLine("<style>");
            emailContentBuilder.AppendLine("    body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f4f4f4; }");
            emailContentBuilder.AppendLine("    .email-container { max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 8px rgba(0,0,0,0.1); }");
            emailContentBuilder.AppendLine("    .header { background-color: #E74C3C; color: #ffffff; padding: 25px 20px; text-align: center; border-top-left-radius: 8px; border-top-right-radius: 8px; }");
            emailContentBuilder.AppendLine("    .header h1 { margin: 0; font-size: 28px; }");
            emailContentBuilder.AppendLine("    .content { padding: 30px; }");
            emailContentBuilder.AppendLine("    .content p { margin-bottom: 15px; font-size: 15px; }");
            emailContentBuilder.AppendLine("    .details-table { width: 100%; border-collapse: collapse; margin-top: 25px; margin-bottom: 25px; border: 1px solid #e0e0e0; }");
            emailContentBuilder.AppendLine("    .details-table th, .details-table td { padding: 12px 15px; text-align: left; border-bottom: 1px solid #e0e0e0; }");
            emailContentBuilder.AppendLine("    .details-table th { background-color: #f9f9f9; color: #555; font-weight: 600; }");
            emailContentBuilder.AppendLine("    .details-table tr:last-child td { border-bottom: none; }");
            emailContentBuilder.AppendLine("    .footer { background-color: #f0f0f0; color: #777; text-align: center; padding: 20px; font-size: 13px; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px; }");
            emailContentBuilder.AppendLine("</style>");
            emailContentBuilder.AppendLine("</head>");
            emailContentBuilder.AppendLine("<body>");
            emailContentBuilder.AppendLine("    <div class=\"email-container\">");
            emailContentBuilder.AppendLine("        <div class=\"header\">");
            emailContentBuilder.AppendLine("            <h1>Hotel CTP</h1>");
            emailContentBuilder.AppendLine("            <p style=\"font-size: 16px;\">Su reserva ha sido cancelada</p>");
            emailContentBuilder.AppendLine("        </div>");

            emailContentBuilder.AppendLine("        <div class=\"content\">");
            emailContentBuilder.AppendLine($"            <p>Estimado(a) <strong>{reservation.Full_Name}</strong>,</p>");
            emailContentBuilder.AppendLine("            <p>Le informamos que su reserva con los siguientes detalles en Hotel CTP ha sido **cancelada exitosamente**.</p>"); 

            emailContentBuilder.AppendLine("            <h3 style=\"color: #E74C3C; margin-top: 30px; border-bottom: 1px solid #eee; padding-bottom: 10px;\">Detalles de la Reserva Cancelada</h3>"); 

            emailContentBuilder.AppendLine("            <table class=\"details-table\">");
            emailContentBuilder.AppendLine("                <tbody>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Número de Reserva:</strong></td><td>{reservation.Id.ToString()}</td></tr>"); 
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Descripción de la Reserva</strong></td><td>{reservation.Reservation_Description}</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Fecha de Ingreso</strong></td><td>{reservation.CheckIn.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-CR"))}</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Fecha de Salida</strong></td><td>{reservation.CheckOut.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-CR"))}</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Noches Reservadas</strong></td><td>{reservation.Days}</td></tr>");
            emailContentBuilder.AppendLine("                </tbody>");
            emailContentBuilder.AppendLine("            </table>");

            emailContentBuilder.AppendLine("            <p style=\"margin-top: 30px;\">Si esta cancelación fue un error o si tiene alguna pregunta, por favor, no dude en contactarnos lo antes posible.</p>");
            emailContentBuilder.AppendLine("        </div>");

            emailContentBuilder.AppendLine("        <div class=\"footer\">");
            emailContentBuilder.AppendLine("            <p>Gracias por haber considerado Hotel CTP.</p>");
            emailContentBuilder.AppendLine("            <p>Este mensaje ha sido generado automáticamente. Por favor, no responda a este correo electrónico.</p>");
            emailContentBuilder.AppendLine("            <p>&copy; " + DateTime.Now.Year + " Hotel CTP. Todos los derechos reservados.</p>");
            emailContentBuilder.AppendLine("        </div>");
            emailContentBuilder.AppendLine("    </div>");
            emailContentBuilder.AppendLine("</body>");
            emailContentBuilder.AppendLine("</html>");

            return emailContentBuilder.ToString();
        }




        public PartialViewResult ExportDataTotalReport(string checkIn, string tmpCheckOut)
        {
            int Opcion = 0;
            string answer = "";

            List<TotalReportE> list = new List<TotalReportE>();
            list = _ReservationReportService.GetListTotalReport(new TotalReportE()
            {
                Opcion = Opcion,
                ReservationType = "",
                Descripction = "",
                checkIn = checkIn,
                checkOut = tmpCheckOut,
                SubTotalWithOutTax = 0,
                TaxAmount = 0,
                TotalAmount = 0
            });
            tmplistTotalReport = list;
            return PartialView("ViewExportDataTotalReport", list);
        }
        public PartialViewResult ExportDataAvailability(DateTime checkIn, DateTime tmpCheckOut)
        {
            int Opcion = 0;
            string answer = "";

            var list = _reservation.GetList(new ReservationE() { Opcion = 1, START_DATE = checkIn, END_DATE = tmpCheckOut });
            tmplistAvalaibilityReport = list;


            return PartialView("ViewAvailabilityReport", list);
        }
        public ActionResult ReportAvailability()
        {
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }

            var list = new List<ReservationE>();

            return View(list);
        }
        public PartialViewResult ViewExportData()
        {
            int Opcion = 0;
            string answer = "";
            List<ReservationReportE> list = new List<ReservationReportE>();
            list = _ReservationReportService.GetList(new ReservationReportE()
            {
                Opcion = Opcion,
                Identification = "",
                Days = "",
                checkIn = DateTime.Now,
                checkOut = DateTime.Now,
                SubTotalWithOutTax = 0,
                TaxAmount = 0,
                TotalAmount = 0
            });

            return PartialView(list);
        }
        public PartialViewResult ViewExportDataTotalReport()
        {
            int Opcion = 0;
            string answer = "";
            List<TotalReportE> list = new List<TotalReportE>();
            list = _ReservationReportService.GetListTotalReport(new TotalReportE()
            {
                Opcion = Opcion,
                ReservationType = "",
                Descripction = "",
                checkIn = DateTime.Now.ToString("dd-MM-yyyy"),
                checkOut = DateTime.Now.ToString("dd-MM-yyyy"),
                SubTotalWithOutTax = 0,
                TaxAmount = 0,
                TotalAmount = 0
            });

            return PartialView(list);
        }

        public DataTable ConvertToDataTable(List<ReservationReportE> reservationReportE)
        {
            DataTable dt = new DataTable("reservationReportE");


            dt.Columns.AddRange(new DataColumn[13] {
                new DataColumn("Código"),
                new DataColumn("Identificación"),
                new DataColumn("Usuario"),
                new DataColumn("Email Usuario"),
                new DataColumn("Días"),
                new DataColumn("Fecha de Entrada"),
                new DataColumn("Fecha de Salida"),
                new DataColumn("SubTotal sin Impuesto", typeof(decimal)),
                new DataColumn("Monto Impuesto", typeof(decimal)),
                new DataColumn("Total", typeof(decimal)),
                new DataColumn("Descripción Habitación"),
                new DataColumn("Nombre Habitación"),
                new DataColumn("Capacidad Habitación", typeof(int))
            });


            foreach (ReservationReportE reservation in reservationReportE)
            {
                dt.Rows.Add(
                    reservation.Id,
                    reservation.Identification,
                    reservation.UserName,
                    reservation.UserEmail,
                    reservation.Days,
                    reservation.checkIn,
                    reservation.checkOut,
                    reservation.SubTotalWithOutTax,
                    reservation.TaxAmount,
                    reservation.TotalAmount,
                    reservation.RoomDescription,
                    reservation.RoomName,
                    reservation.RoomCapacity
                );
            }

            return dt;
        }
        public DataTable ConvertToDataTable2(List<ReservationE> reservationReportE)
        {
            DataTable dt = new DataTable("ReservationE");


            dt.Columns.AddRange(new DataColumn[8] {

                    new DataColumn("Identificación"),
                    new DataColumn("Cliente"),
                    new DataColumn("Correo"),
                    new DataColumn("Descripción"),
                    new DataColumn("Entrada"),
                    new DataColumn("Salida"),
                    new DataColumn("Días"),
                    new DataColumn("Habitación")
         });


            foreach (ReservationE reservation in reservationReportE)
            {
                dt.Rows.Add(

                    reservation.IdCard_Client,
                    reservation.Full_Name,
                    reservation.Client_Mail,
                    reservation.Rate_Description,
                    reservation.FormattedCheckIn,
                    reservation.FormattedCheckOut,
                    reservation.Days,
                   reservation.DESCRIPTION_HOTELROOM
                );
            }

            return dt;
        }
        public DataTable ConvertToDataTableTotalReportE(List<TotalReportE> totalReportE)
        {
            DataTable dt = new DataTable("totalReportE");


            dt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("Detalle"),
                    new DataColumn("Tipo"),
                    new DataColumn("SubTotal", typeof(decimal)),
                    new DataColumn("Impuestos", typeof(decimal)),
                    new DataColumn("Total", typeof(decimal))
         });


            foreach (TotalReportE reservation in totalReportE)
            {
                dt.Rows.Add(
                    reservation.Descripction,
                    reservation.ReservationType,
                    reservation.SubTotalWithOutTax,
                    reservation.TaxAmount,
                    reservation.TotalAmount
                );
            }

            return dt;
        }
        public ActionResult ExportReservationsToExcel()
        {


            var reservations = tmplistTotalReport;


            var dataTable = ConvertToDataTableTotalReportE(reservations);


            return ExportToExcel(dataTable, "Reporte_Ingresos");
        }

        public ActionResult ExportReservationReportEToExcel()
        {


            var reservations = tmplist;


            var dataTable = ConvertToDataTable(reservations);


            return ExportToExcel(dataTable, "Reporte_Ingresos");
        }
        public ActionResult ExportReservationAvailabilityReportEToExcel()
        {


            var reservations = tmplistAvalaibilityReport;


            var dataTable = ConvertToDataTable2(reservations);


            return ExportToExcel(dataTable, "Reporte_Disponibilidad");
        }

        private ActionResult ExportToExcel(DataTable dataTable, string tmpNombre)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(dataTable, "Data");

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Datos.xlsx");
                }
            }
        }
    }
}