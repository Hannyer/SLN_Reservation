
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EntityLayer;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLN_Reservation.Controllers
{
    public class ReservationReportController : Controller
    {
        static List<ReservationReportE> tmplist = new List<ReservationReportE>();
        static List<TotalReportE> tmplistTotalReport = new List<TotalReportE>();
        static List<ReservationE> tmplistAvalaibilityReport = new List<ReservationE>();
        IReservationReportService _ReservationReportService;
        IReservationService _reservation;
        // GET: ReservationReport
        public ReservationReportController(IReservationReportService reservationReportService, IReservationService reservation)
        {
            this._ReservationReportService = reservationReportService;
            _reservation = reservation;
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
            int Opcion = 0;
            string answer = "";

            List<ReservationReportE> list = new List<ReservationReportE>();
            list = _ReservationReportService.GetList(new ReservationReportE() { Opcion = Opcion, Identification = "", Client = "", ReservationType = "", Days = "", Descripction = "", checkIn = checkIn, 
                checkOut = tmpCheckOut, SubTotalWithOutTax =0, TaxAmount = 0 , TotalAmount = 0});
            tmplist = list;
            return PartialView("ViewExportData", list);
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
                SubTotalWithOutTax =0,
                TaxAmount =0,
                TotalAmount= 0
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
        public ActionResult ReportAvailability() {
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }

            var list = new List<ReservationE>(); // _reservation.GetList(new ReservationE() { Opcion = 1, START_DATE = DateTime.Now, END_DATE = DateTime.Now.AddDays(1) });

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
                Client = "",
                ReservationType = "",
                Days = "",
                Descripction = "",
                checkIn = DateTime.Now.ToString("dd-MM-yyyy"),
                checkOut = DateTime.Now.ToString("dd-MM-yyyy"),
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
                TotalAmount =0
            });

            return PartialView(list);
        }

        public DataTable ConvertToDataTable(List<ReservationReportE> reservationReportE)
        {
            DataTable dt = new DataTable("reservationReportE");


            dt.Columns.AddRange(new DataColumn[8] {
                    new DataColumn("Código"),
                    new DataColumn("Identificación"),
                    new DataColumn("Cliente"),
                    new DataColumn("Tipo"),
                    new DataColumn("Días"),
                    new DataColumn("Detalle"),
                    new DataColumn("Fecha de Entrada"),
                    new DataColumn("Fecha de Salida")
         });


            foreach (ReservationReportE reservation in reservationReportE)
            {
                dt.Rows.Add(
                    reservation.Id,
                    reservation.Identification,
                    reservation.Client,
                    reservation.ReservationType,
                    reservation.Days,
                    reservation.Descripction,
                    reservation.checkIn,
                    reservation.checkOut
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


            return ExportToExcel(dataTable,"Reporte_Ingresos");
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

        private ActionResult ExportToExcel(DataTable dataTable,string tmpNombre)
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