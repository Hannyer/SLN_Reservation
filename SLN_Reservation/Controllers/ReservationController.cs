
using EntityLayer;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Xml;
using System.Net.Mail;
using System.Net;
using SLN_Reservation.DTO;
using SLN_Reservation.model.DTo;
using Service.Service;

namespace SLN_Reservation.Controllers
{
    public class ReservationController : Controller
    {

        IReservationService _reservation;
        IClientService _clientService;
        IRateService _operateService;
        IRateTypeService _operateTypeService;
        IConfigurationService _configurationService;
        IAboutService _aboutService;
        IHotelRoomService _hotelRoomService;
        IUserService _userService;
        ReservationDto reservationDto;

        int IDRolClient;

        public ReservationController(IReservationService reservationService, IClientService clientService, IRateService operateService, IRateTypeService operateTypeService, IConfigurationService configurationService, IAboutService aboutService, IHotelRoomService hotelRoomService,
            IUserService service)
        {
            this._reservation = reservationService;
            _clientService = clientService;
            _operateService = operateService;
            _operateTypeService = operateTypeService;
            _configurationService = configurationService;
            _aboutService = aboutService;
            _hotelRoomService = hotelRoomService;
            _userService = service;
            reservationDto =new ReservationDto();

            IDRolClient = Convert.ToInt32(_configurationService.GetList(new ConfigurationE()
            {
                Opcion = 0,
                KEY01 = "PARAMETRO",
                KEY02 = "FUNCIONALIDAD",
                KEY03 = "MRB",
                KEY04 = "ROL",
                KEY05 = "CLIENTE"
            }).FirstOrDefault().VALUE);
        }
        // GET: Reservation
        public async Task<ActionResult> Index()
        {
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            reservationDto.reservationList = _reservation.GetList(new ReservationE() { Opcion = 1, START_DATE = DateTime.Now.AddDays(-5), END_DATE = DateTime.Now.AddDays(5) });

            FillDropDownListSeachClient();
            await GetDollarValue();
           
            return View(reservationDto);
        }
        [HttpGet]
        public ActionResult SeachReservationByStatus(string reservationStatus, DateTime StartDate, DateTime EndDate)
        {

            FillDropDownListSeachClient();
            List<ReservationE> list = new List<ReservationE>();
            if (!reservationStatus.Equals("4"))
            {
                list = _reservation.GetList(new ReservationE() { Opcion = 0, Status = reservationStatus, START_DATE = StartDate, END_DATE = EndDate });

            }
            else
            {
                list = _reservation.GetList(new ReservationE() { Opcion = 1, START_DATE = StartDate, END_DATE = EndDate });
            }

            return PartialView("PartialViewReservation", new ReservationDto() {reservationList=list });
        }
        [HttpGet]
        public ActionResult PartialViewReservation(List<ReservationE> Lista)
        {
            

            return PartialView(Lista);
        }
        public async Task<string> NewReservation(ReservationDTO reservation)
        {
            ReservationE reservationE = new ReservationE();
            reservationE.START_DATE = DateTime.Now;
            reservationE.END_DATE = DateTime.Now;
            var config = _configurationService.GetList(new ConfigurationE()
            {
                Opcion = 0,
                KEY01 = "PARAMETRO",
                KEY02 = "FUNCIONALIDAD",
                KEY03 = "MRB",
                KEY04 = "IMPUESTO",
                KEY05 = "IVA"
            }).FirstOrDefault();
            Hotel_RoomE room=_hotelRoomService.GetList(new Hotel_RoomE() {ID=reservation.RoomId }).FirstOrDefault();
          
            int numberOfNights = reservation.Nights;
            double subtotal = Convert.ToDouble(numberOfNights * room.Price);
            double valor = Convert.ToDouble(config.VALUE) / 100;
            double subtotalWithoutTax = 0;
            double taxAmount = 0;
            DollarDataE DollarData = null;
            double totalAmount = 0;
          
                subtotalWithoutTax = Math.Round((subtotal / valor), 2);
                taxAmount = subtotal - subtotalWithoutTax;
                totalAmount = subtotal;

            reservationE.SubtotalWithoutTax = subtotalWithoutTax;
            reservationE.TaxAmount = taxAmount;
            reservationE.TotalAmount = totalAmount;
            reservationE.Reservation_Description = "Reservación: " + room.Name+" " + room.Description+" " +reservation.Description;


            reservationE.CheckIn = new DateTime(reservation.CheckIn.Year, reservation.CheckIn.Month, reservation.CheckIn.Day, 15, 0, 0);
            reservationE.CheckOut = new DateTime(reservation.CheckOut.Year, reservation.CheckOut.Month, reservation.CheckOut.Day, 12, 0, 0);
            reservationE.Days = reservation.Nights;
            reservationE.Opcion = 0;
            string answer = "";
            reservationE.START_DATE = DateTime.Now;
            reservationE.END_DATE = DateTime.Now;
            reservationE.ID_USER = (Session["User"] as UserE).ID;
            reservationE.ID_ROOM = reservation.RoomId;
            reservationE.IdCard_Client = reservation.ClientId; 
            int IdGenerate = _reservation.Maintenance(reservationE);


            var configEmail = _configurationService.GetList(new ConfigurationE()
            {
                Opcion = 0,
                KEY01 = "PARAMETRO",
                KEY02 = "FUNCIONALIDAD",
                KEY03 = "MRB",
                KEY04 = "CREDENCIALES",
                KEY05 = "CORREO"
            });

            EmailConfigurationE email = new EmailConfigurationE()
            {
                Email = configEmail.Where(x => x.KEY06 == "CORREO").FirstOrDefault().VALUE,
                Password = configEmail.Where(x => x.KEY06 == "PASSWORD").FirstOrDefault().VALUE,
                Host = configEmail.Where(x => x.KEY06 == "HOST").FirstOrDefault().VALUE,
                Port = Convert.ToInt32(configEmail.Where(x => x.KEY06 == "PORT").FirstOrDefault().VALUE),

            };

            if (IdGenerate > 0)
            {
                answer = "Reservación Agregada con exitosamente!";
                var getGenerateReservation = _reservation.GetList(new ReservationE() { Opcion = 2, Id = IdGenerate, START_DATE = DateTime.Now, END_DATE = DateTime.Now }).FirstOrDefault();
                UtilitarioE.SendEmail(email, getGenerateReservation.Client_Mail, "Confirmación de reservación", GenerateReservationConfirmationEmail(getGenerateReservation, DollarData));
                //RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }
        public string ModifyReservation(ReservationE reservation)
        {
            reservation.CheckIn = new DateTime(reservation.CheckIn.Year, reservation.CheckIn.Month, reservation.CheckIn.Day, 15, 0, 0);
            reservation.CheckOut = new DateTime(reservation.CheckOut.Year, reservation.CheckOut.Month, reservation.CheckOut.Day, 12, 0, 0);
            reservation.Opcion = 2;
            string answer = "";
            int IdGenerate = _reservation.Maintenance(reservation);
            if (IdGenerate > 0)
            {
                answer = "Reservación Modificada con exitosamente";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }
        public string DeleteReservation(ReservationE Reservation)
        {
            string answer = "";
            try
            {
                Reservation.CheckIn = DateTime.Now;
                Reservation.CheckOut = DateTime.Now;
                Reservation.Opcion = 1;

                int IdGenerate = _reservation.Maintenance(Reservation);

                answer = "Reservación Eliminada exitosamente";
                RedirectToAction("Index");


                return answer;
            }
            catch (Exception ex)
            {

                return answer = "Ha ocurrido un error"; ;
            }





        }
        [HttpPost]
        public string GenerateInvoceReservation(ReservationE reservation)
        {
            string answer = "";
            try
            {
                var config = _configurationService.GetList(new ConfigurationE()
                {
                    Opcion = 0,
                    KEY01 = "PARAMETRO",
                    KEY02 = "FUNCIONALIDAD",
                    KEY03 = "MRB",
                    KEY04 = "IMPUESTO",
                    KEY05 = "IVA"
                }).FirstOrDefault();
                var configEmail = _configurationService.GetList(new ConfigurationE()
                {
                    Opcion = 0,
                    KEY01 = "PARAMETRO",
                    KEY02 = "FUNCIONALIDAD",
                    KEY03 = "MRB",
                    KEY04 = "CREDENCIALES",
                    KEY05 = "CORREO"
                });

                EmailConfigurationE email = new EmailConfigurationE()
                {
                    Email = configEmail.Where(x => x.KEY06 == "CORREO").FirstOrDefault().VALUE,
                    Password = configEmail.Where(x => x.KEY06 == "PASSWORD").FirstOrDefault().VALUE,
                    Host = configEmail.Where(x => x.KEY06 == "HOST").FirstOrDefault().VALUE,
                    Port = Convert.ToInt32(configEmail.Where(x => x.KEY06 == "PORT").FirstOrDefault().VALUE),

                };
                double IVA = Convert.ToDouble(config.VALUE) / 100;

                var about = _aboutService.GetList(new AboutE() { Opcion = 0, ID = 1 }).FirstOrDefault();
                var client = _userService.GetList(new UserE() { DocumentID = reservation.IdCard_Client }).FirstOrDefault();
                ElectronicInvoiceE factura = new ElectronicInvoiceE();
                factura.Key = UtilitarioE.GenerateElectronicInvoiceKey(506, DateTime.Now, about.ID_Hotel, reservation.Id, 3, about.SecurityCode);
                factura.ActivityCode = about.ActivityCode;
                factura.ConsecutiveNumber = factura.Key.Substring(21, 20);
                factura.EmissionDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");

                factura.Issuer = new IssuerE()
                {
                    CommercialName = about.Name,
                    Name = about.Name,
                    Identification = new IdentificationE()
                    {
                        Number = about.ID_Hotel,
                        Type = about.ID_Type
                    },
                    Email = about.Email,
                    Phone = new PhoneE()
                    {
                        CountryCode = about.Phone.Split(' ').ToArray()[0],
                        PhoneNumber = about.Phone.Split(' ').ToArray()[1],
                    },
                    Location = new LocationE()
                    {
                        Province = about.Province,
                        Canton = about.Canton,
                        District = about.Distric,
                        OtherSigns = about.OtherSigns
                    }
                };
                factura.Receiver = new ReceiverE()
                {
                    Identification = new IdentificationE()
                    {
                        Type = "0" + client.IdIdentificationType.ToString(),
                        Number = client.DocumentID
                    },
                    Email = client.Email,
                    Phone = new PhoneE()
                    {
                        CountryCode = SepararCodigoYNumeroTelefono(client.PhoneNumber)[0],
                        PhoneNumber = SepararCodigoYNumeroTelefono(client.PhoneNumber)[1]
                    },
                    Location = new LocationE()
                    {
                        Province = about.Province,
                        Canton = about.Canton,
                        District = about.Distric,
                        OtherSigns = about.OtherSigns
                    },
                    Name = client.Name
                };
                factura.SaleCondition = "01"; //efectivo
                factura.Halfpayment = "04"; //Medio de pago Transferencia
                factura.ServiceDetail = new ServiceDetailE()
                {
                    LineDetails = new List<LineDetailE>() { new LineDetailE(){
                    LineNumber="1",
                    Quantity=reservation.Days,
                    UnitOfMeasure="Unid",
                    Detail=reservation.DisplayName_Rate,
                    UnitPrice=Math.Round(reservation.Price/(decimal)IVA,2),
                    TotalAmount=(decimal)reservation.SubtotalWithoutTax,
                    SubTotal=(decimal)reservation.SubtotalWithoutTax,
                    Tax=new TaxE(){ //impuesto
                    Code="01",
                    TariffCode="08",
                     Rate=Convert.ToInt32(config.VALUE.Substring(1)),
                     Amount=(decimal)reservation.TaxAmount
                    },
                    NetTax=(decimal)reservation.TaxAmount,
                    TotalLineAmount=(decimal) reservation.TotalAmount,
                    Code=about.Cabys_service
                    }
                    }
                };
                factura.InvoiceSummary = new InvoiceSummaryE()
                {
                    CurrencyCode = new CurrencyCodeE() { Currency = "CRC", ExchangeRate = 1 },
                    TotalSale = (decimal)reservation.SubtotalWithoutTax,
                    NetTotalSale = (decimal)reservation.SubtotalWithoutTax,
                    TotalTax = (decimal)reservation.TaxAmount,
                    TotalVoucher = (decimal)reservation.TotalAmount,
                    Price = reservation.Price,
                };
                factura.Others = new OthersE() { OtherText = reservation.Reservation_Description };



                reservation.Status = "2";
                reservation.Opcion = 2;
                string InvoceXMLPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Documentos", "XML");
                string InvocePdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Documentos", "PDF");

                if (!Directory.Exists(InvocePdfPath))
                    Directory.CreateDirectory(InvocePdfPath);
                if (!Directory.Exists(InvoceXMLPath))
                    Directory.CreateDirectory(InvoceXMLPath);
                XmlDocument xml = GenerateElectronicInvoiceXML(factura);
                string FullXMLPath = InvoceXMLPath + "\\" + factura.Key + ".xml";
                string FullPDFPath = InvocePdfPath + "\\" + factura.Key + ".pdf";

                xml.Save(FullXMLPath);
                GenerateInvoicePdf(factura, FullPDFPath);

                SendElectronicInvoice(factura.Receiver.Email, "Factura electrónica", "Factura electronica", FullPDFPath, FullXMLPath, email);

                System.IO.File.Delete(FullXMLPath);
                System.IO.File.Delete(FullPDFPath);
                answer = "Se ha generado la factura exitosamente.";

                _reservation.Maintenance(reservation);

                RedirectToAction("Index");

                return answer;
            }
            catch (Exception ex)
            {

                return answer = "Ha ocurrido un error";
            }




        }
        public string EndProccess(ReservationE reservation)
        {
           
            string answer = "";
            try
            {
                if (reservation.CheckOut > DateTime.Now)
                {
                    return answer="La reservación no puede ser finalizada, la fecha de salida es menor a la fecha actual.";
                }
                reservation.Status = "3";
                reservation.Opcion = 2;


                answer = "La reservación ha sido finalizada exitosamente.";

                _reservation.Maintenance(reservation);

                RedirectToAction("Index");

                return answer;
            }
            catch (Exception ex)
            {

                return answer = "Ha ocurrido un error";
            }




        }
        public void FillDropDownListSeachClient()
        {
            var Clients = _clientService.GetList(new ClientE() { Opcion = 1 });


            var clientList = Clients.Select(ClientL => new SelectListItem
            {
                Value = ClientL.IdCard.ToString(),
                Text = ClientL.Full_Name,

            });


            ViewBag.ClientList = clientList;

        }
        public JsonResult GetRateListByClientRateType(string Id_ClientSelected)
        {
            var ClientSelected = _clientService.GetList(new ClientE() { Opcion = 0, IdCard = Id_ClientSelected }).FirstOrDefault();
            var rate = _operateService.GetList(new RateE() { Opcion = 0, ID_RateTýpe = ClientSelected.RateType_Id });

            var rateList = rate.Select(RateL => new SelectListItem
            {
                Value = RateL.ID.ToString(),
                Text = RateL.Currency.ToUpper() == "USD" ? "$ " + RateL.Price.ToString("###,###,###,##") + " - " + RateL.Description : "₡ " + RateL.Price.ToString("###,###,###,##") + " - " + RateL.Description
            });

            return Json(rateList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHotelRoomListByCapacity( DateTime StartDate, DateTime EndDate, int GuestCount)
        {
            int Id_RateSelected = 1;
            var rate = _operateService.GetList(new RateE() { Opcion = 0, ID = Id_RateSelected }).FirstOrDefault();
            var hotelRoom = _hotelRoomService.GetList(new Hotel_RoomE() { Opcion = 1, Capacity = GuestCount, StardDate = StartDate, EndDate = EndDate });
            var hotelRoomList = hotelRoom.Select(hotelL => new Hotel_RoomE
            {
                ID = hotelL.ID,
                Description = hotelL.Description + " - Capacidad: " + hotelL.Capacity,
                Price = hotelL.Price,
                DolarPrice = hotelL.DolarPrice,

            });

            return Json(hotelRoomList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRateTypebyId(int Id_Rate, int numberOfNights)
        {
            var Rate = _operateService.GetList(new RateE() { Opcion = 0, ID = Id_Rate }).FirstOrDefault();
            var config = _configurationService.GetList(new ConfigurationE()
            {
                Opcion = 0,
                KEY01 = "PARAMETRO",
                KEY02 = "FUNCIONALIDAD",
                KEY03 = "MRB",
                KEY04 = "IMPUESTO",
                KEY05 = "IVA"
            }).FirstOrDefault();

            double subtotal = Convert.ToDouble(numberOfNights * Rate.Price);
            double valor = Convert.ToDouble(config.VALUE) / 100;



            double subtotalWithoutTax = Math.Round((subtotal / valor), 2);
            double taxAmount = subtotal - subtotalWithoutTax;
            double totalAmount = subtotal;



            return Json(Rate, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Expense_Details(int numberOfNights, int roomId)
        {
         
            var hotelRoom = _hotelRoomService.GetList(new Hotel_RoomE() { Opcion = 0, ID = roomId }).FirstOrDefault();
            var config = _configurationService.GetList(new ConfigurationE()
            {
                Opcion = 0,
                KEY01 = "PARAMETRO",
                KEY02 = "FUNCIONALIDAD",
                KEY03 = "MRB",
                KEY04 = "IMPUESTO",
                KEY05 = "IVA"
            }).FirstOrDefault();

            double subtotal = Convert.ToDouble(numberOfNights * hotelRoom.Price);
            double valor = Convert.ToDouble(config.VALUE) / 100;

            double subtotalWithoutTax = Math.Round((subtotal / valor), 2);
            double taxAmount = subtotal - subtotalWithoutTax;
            double totalAmount = subtotal;
            string expenseDetail = "";
 

            var detalle = new ExpenseDetailDto
            {
                Nights = numberOfNights,
                PricePerNight = hotelRoom.Price,
                SubTotal = subtotalWithoutTax,
                Tax = taxAmount,
                Total = totalAmount
            };

            // FillDropDownListSeachClient();
            return Json(detalle, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetDollarValue()
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

                        DateTime fecha = DateTime.ParseExact(sale.DollarBuyE.Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        ViewBag.BuyDate = fecha.ToString("dd/MM/yyyy");
                        fecha = DateTime.ParseExact(sale.DollarSaleE.Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        ViewBag.SaleDate = fecha.ToString("dd/MM/yyyy");

                        ViewBag.SaleValue = sale.DollarSaleE.Value;
                        ViewBag.BuyValue = sale.DollarBuyE.Value;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error while consuming the API.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
            }

            return View();
        }
        public string GenerateReservationConfirmationEmail(ReservationE reservation, DollarDataE dollarData)
        {
            var email = new StringBuilder();

            email.AppendLine("<!DOCTYPE html>");
            email.AppendLine("<html lang=\"es\">");
            email.AppendLine("<head>");
            email.AppendLine("  <meta charset=\"UTF-8\">");
            email.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            email.AppendLine("  <title>Confirmación de Reserva - Hotel CTP</title>");
            email.AppendLine("  <style>");
            email.AppendLine("    body {");
            email.AppendLine("      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;");
            email.AppendLine("      background-color: #f4f4f4;");
            email.AppendLine("      margin: 0; padding: 0; color: #333; line-height: 1.6;");
            email.AppendLine("    }");
            email.AppendLine("    .container {");
            email.AppendLine("      max-width: 600px; margin: 20px auto; background: #fff;");
            email.AppendLine("      border-radius: 8px; overflow: hidden;");
            email.AppendLine("      box-shadow: 0 4px 8px rgba(0,0,0,0.1);");
            email.AppendLine("    }");
            email.AppendLine("    .header {");
            email.AppendLine("      background: #2C3E50; color: #fff;");
            email.AppendLine("      padding: 25px 20px; text-align: center;");
            email.AppendLine("    }");
            email.AppendLine("    .header h1 { margin:0; font-size:28px; }");
            email.AppendLine("    .content { padding: 30px; }");
            email.AppendLine("    .content p { margin: 0 0 15px; font-size:15px; }");
            email.AppendLine("    .details-table {");
            email.AppendLine("      width:100%; border-collapse:collapse;");
            email.AppendLine("      margin:20px 0; border:1px solid #e0e0e0;");
            email.AppendLine("    }");
            email.AppendLine("    .details-table th, .details-table td {");
            email.AppendLine("      padding:12px 15px; border-bottom:1px solid #e0e0e0; text-align:left;");
            email.AppendLine("    }");
            email.AppendLine("    .details-table th { background:#f9f9f9; font-weight:600; }");
            email.AppendLine("    .total-row { background:#eaf7ff; font-weight:bold; }");
            email.AppendLine("    .footer {");
            email.AppendLine("      background:#f0f0f0; color:#777;");
            email.AppendLine("      text-align:center; padding:20px; font-size:13px;");
            email.AppendLine("    }");
            email.AppendLine("  </style>");
            email.AppendLine("</head>");
            email.AppendLine("<body>");
            email.AppendLine("  <div class=\"container\">");

            email.AppendLine("    <div class=\"header\">");
            email.AppendLine("      <h1>Hotel CTP</h1>");
            email.AppendLine("      <p style=\"font-size:16px; margin-top:5px;\">¡Su reserva ha sido confirmada!</p>");
            email.AppendLine("    </div>");

        
            email.AppendLine("    <div class=\"content\">");
            email.AppendLine($"      <p>Estimado(a) <strong>{reservation.Full_Name}</strong>,</p>");
            email.AppendLine("      <p>Nos complace confirmar su reserva en Hotel CTP con los siguientes detalles:</p>");

           
            email.AppendLine("      <h3 style=\"color:#2C3E50; margin-top:30px; border-bottom:1px solid #eee; padding-bottom:10px;\">Detalles de su Reserva</h3>");
            email.AppendLine("      <table class=\"details-table\">");
            email.AppendLine("        <tr><th>Habitación #</th><td>" + reservation.DESCRIPTION_HOTELROOM_NAME + "</td></tr>");
            email.AppendLine("        <tr><th>Descripción</th><td>" + reservation.Reservation_Description + "</td></tr>");
            email.AppendLine("        <tr><th>Ingreso</th><td>" + reservation.CheckIn.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-CR")) + " (15:00 HRS)" + "</td></tr>");
            email.AppendLine("        <tr><th>Salida</th><td>" + reservation.CheckOut.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-CR")) + " (12:00 HRS)" + "</td></tr>");
            email.AppendLine("        <tr><th>Noches</th><td>" + reservation.Days + "</td></tr>");
            email.AppendLine("      </table>");

            email.AppendLine("      <h3 style=\"color:#2C3E50; margin-top:30px; border-bottom:1px solid #eee; padding-bottom:10px;\">Resumen de Pago</h3>");
            email.AppendLine("      <table class=\"details-table\">");
            email.AppendLine("        <tr><td>Precio por Noche</td><td style=\"text-align:right;\">" +
                Math.Round(reservation.Price, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR")) +
                " (IVA incl.)</td></tr>");
            email.AppendLine("        <tr><td>Subtotal (sin IVA)</td><td style=\"text-align:right;\">" +
                Math.Round(reservation.SubtotalWithoutTax, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR")) +
                "</td></tr>");
            email.AppendLine("        <tr><td>IVA</td><td style=\"text-align:right;\">" +
                Math.Round(reservation.TaxAmount, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR")) +
                "</td></tr>");
            email.AppendLine("        <tr class=\"total-row\"><td>Total a Pagar</td><td style=\"text-align:right;\">" +
                Math.Round(reservation.TotalAmount, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR")) +
                "</td></tr>");
            email.AppendLine("      </table>");

            email.AppendLine("      <p>Si tiene alguna duda o desea modificar su reserva, contáctenos sin problema.</p>");
            email.AppendLine("    </div>");

       
            email.AppendLine("    <div class=\"footer\">");
            email.AppendLine("      <p>Gracias por elegir Hotel CTP.</p>");
            email.AppendLine("      <p>¡Esperamos verle pronto!</p>");
            email.AppendLine("      <p>Este mensaje fue generado automáticamente. Por favor, no responda.</p>");
            email.AppendLine("      <p>&copy; " + DateTime.Now.Year + " Hotel CTP. Todos los derechos reservados.</p>");
            email.AppendLine("    </div>");

            email.AppendLine("  </div>");
            email.AppendLine("</body>");
            email.AppendLine("</html>");

            return email.ToString();
        }

        public string[] SepararCodigoYNumeroTelefono(string numeroTelefonoCompleto)
        {

            string numeroTelefono = numeroTelefonoCompleto.StartsWith("+")
                                    ? numeroTelefonoCompleto.Substring(1)
                                    : numeroTelefonoCompleto;


            string codigoPais = "";
            string numeroLocal = "";

            for (int i = 1; i <= 3 && i < numeroTelefono.Length; i++)
            {

                if (numeroTelefono.Length - i >= 7)
                {
                    codigoPais = numeroTelefono.Substring(0, i);
                    numeroLocal = numeroTelefono.Substring(i);

                }
            }

            return new string[] { codigoPais, numeroLocal };
        }
        public static XmlDocument GenerateElectronicInvoiceXML(ElectronicInvoiceE factura)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ElectronicInvoiceE));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            XmlDocument xmlDoc = new XmlDocument();

            using (MemoryStream stream = new MemoryStream())
            {

                XmlWriterSettings settings = new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = true };
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, factura, namespaces);
                }


                stream.Position = 0;


                xmlDoc.Load(stream);
                return xmlDoc;
            }
        }
        public void GenerateInvoicePdf(ElectronicInvoiceE invoice, string filePath)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();
            CultureInfo cultureInfo = new CultureInfo("es-CR");
            cultureInfo.NumberFormat.CurrencySymbol = "₡";
            int horizontalAlign = Element.ALIGN_CENTER;
            int verticalAlign = Element.ALIGN_MIDDLE;

            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font subtitleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font normalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);


            iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("Factura Electrónica", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            document.Add(new iTextSharp.text.Paragraph($"Factura Electrónica N°: {invoice.ConsecutiveNumber}", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"Versión: 4.3", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"Clave Numérica: {invoice.Key}", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"Fecha emisión: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}", subtitleFont));
            document.Add(new Chunk(new LineSeparator(0.5f, 100.0f, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));
            document.Add(new iTextSharp.text.Paragraph($"Emisor: {invoice.Issuer.Name}", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"Identificación: {invoice.Issuer.Identification.Number}", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"teléfono: {invoice.Issuer.Phone.CountryCode + " " + invoice.Issuer.Phone.PhoneNumber}", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"Correo: {invoice.Issuer.Email}", subtitleFont));


            document.Add(new Chunk(new LineSeparator(0.5f, 100.0f, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));
            document.Add(new iTextSharp.text.Paragraph($"Receptor: {invoice.Receiver.Name}", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"Identificación: {invoice.Receiver.Identification.Number}", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"Teléfono: {"+" + invoice.Receiver.Phone.CountryCode + " " + invoice.Receiver.Phone.PhoneNumber}", subtitleFont));
            document.Add(new iTextSharp.text.Paragraph($"Correo: {invoice.Receiver.Email}", subtitleFont));
            document.Add(new Chunk(new LineSeparator(0.5f, 100.0f, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));



            document.Add(new iTextSharp.text.Paragraph("\n"));



            PdfPTable detailsTable = new PdfPTable(new float[] { 1, 3, 1, 1, 1, 1 });
            detailsTable.WidthPercentage = 100;

            detailsTable.AddCell(new PdfPCell(new Phrase("Código", subtitleFont)));
            detailsTable.AddCell(new PdfPCell(new Phrase("Descripción", subtitleFont)));
            detailsTable.AddCell(new PdfPCell(new Phrase("Días", subtitleFont)));
            detailsTable.AddCell(new PdfPCell(new Phrase("Precio Unitario", subtitleFont)));
            detailsTable.AddCell(new PdfPCell(new Phrase("Monto Total", subtitleFont)));
            detailsTable.AddCell(new PdfPCell(new Phrase("Impuesto", subtitleFont)));

            // Añadir las líneas de detalle
            foreach (var line in invoice.ServiceDetail.LineDetails)
            {
                detailsTable.AddCell(new PdfPCell(new Phrase(line.Code, normalFont)));
                detailsTable.AddCell(new PdfPCell(new Phrase(line.Detail, normalFont)));
                detailsTable.AddCell(new PdfPCell(new Phrase(line.Quantity.ToString(), normalFont)));
                detailsTable.AddCell(new PdfPCell(new Phrase("₡ " + line.UnitPrice.ToString(), normalFont)));
                detailsTable.AddCell(new PdfPCell(new Phrase("₡ " + line.TotalAmount.ToString("C", cultureInfo), normalFont)));
                detailsTable.AddCell(new PdfPCell(new Phrase("₡ " + line.Tax.Amount.ToString("C", cultureInfo), normalFont)));
            }
            document.Add(detailsTable);

            document.Add(new iTextSharp.text.Paragraph("\n"));

            document.Add(new iTextSharp.text.Paragraph("\n"));

            document.Add(new iTextSharp.text.Paragraph("\n"));

            document.Add(new iTextSharp.text.Paragraph("\n"));

            PdfPTable totalsTable = new PdfPTable(2);
            totalsTable.TotalWidth = 100f;
            totalsTable.WidthPercentage = 50;

            totalsTable.SetWidths(new float[] { 1, 1 });

            totalsTable.AddCell(new Phrase("Cantidad de días:", subtitleFont));
            totalsTable.AddCell(new Phrase(invoice.ServiceDetail.LineDetails[0].Quantity.ToString()));



            totalsTable.AddCell(new Phrase("Precio unitario:", subtitleFont));
            totalsTable.AddCell(new Phrase(invoice.InvoiceSummary.Price.ToString("C", cultureInfo), subtitleFont));


            totalsTable.AddCell(new Phrase("Total Venta:", subtitleFont));
            totalsTable.AddCell(new Phrase(invoice.InvoiceSummary.TotalSale.ToString("C", cultureInfo), subtitleFont));

            totalsTable.AddCell(new Phrase("Total Impuesto:", subtitleFont));
            totalsTable.AddCell(new Phrase(invoice.InvoiceSummary.TotalTax.ToString("C", cultureInfo), subtitleFont));

            totalsTable.AddCell(new Phrase("Total Comprobante:", subtitleFont));
            totalsTable.AddCell(new Phrase(invoice.InvoiceSummary.TotalVoucher.ToString("C", cultureInfo), subtitleFont));

            totalsTable.HorizontalAlignment = horizontalAlign;

            document.Add(totalsTable);

            document.Add(new iTextSharp.text.Paragraph("\n"));

            document.Add(new iTextSharp.text.Paragraph("\n"));

            document.Add(new Chunk(new LineSeparator(0.5f, 100.0f, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));
            document.Add(new iTextSharp.text.Paragraph("\n"));

            document.Add(new iTextSharp.text.Paragraph("\n"));

            if (!string.IsNullOrEmpty(invoice.Others.OtherText))
            {
                document.Add(new iTextSharp.text.Paragraph("Nota: " + invoice.Others.OtherText, normalFont));
            }


            document.Close();
        }
        public void SendElectronicInvoice(string correoDestinatario, string asunto, string cuerpo, string rutaAdjunto1, string rutaAdjunto2, EmailConfigurationE email)
        {
            try
            {

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(email.Email);
                    mail.To.Add(correoDestinatario);
                    mail.Subject = asunto;
                    mail.Body = cuerpo;
                    mail.IsBodyHtml = true;



                    mail.Attachments.Add(new Attachment(rutaAdjunto1));
                    mail.Attachments.Add(new Attachment(rutaAdjunto2));

                    using (SmtpClient smtp = new SmtpClient(email.Host, email.Port))
                    {
                        smtp.Credentials = new NetworkCredential(email.Email, email.Password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo enviar el correo. Error: " + ex.Message);
            }
        }
        public async Task<string> AddDeposit(ReservationE reservation, decimal Ammount)
        {
            try
            {
                DollarDataE DollarData = await UtilitarioE.GetDollarValue();
                string answer = "";


                if (reservation.Currency.Equals("CRC"))
                {
                    if ((reservation.Deposit + Ammount) > Convert.ToDecimal(reservation.TotalAmount))
                    {
                        answer = "El deposito no puede ser mayor al monto total de la resevación";
                        return answer;
                    }
                }
                else
                {
                    reservation.Deposit += (Ammount * Convert.ToDecimal(DollarData.DollarBuyE.Value));
                    if ((reservation.Deposit) > Convert.ToDecimal(reservation.TotalAmount))
                    {
                        answer = "El deposito no puede ser mayor al monto total de la resevación";
                        return answer;
                    }
                }


                if (reservation.Deposit == null || reservation.Deposit == 0)
                {
                    if (reservation.Currency.Equals("CRC"))
                    {
                        reservation.Deposit = Ammount;
                    }
                    else
                    {
                        reservation.Deposit = (Ammount * Convert.ToDecimal(DollarData.DollarBuyE.Value));
                    }
                }
                else
                {
                    if (reservation.Currency.Equals("CRC"))
                    {
                        reservation.Deposit += Ammount;
                    }
                    else
                    {
                        reservation.Deposit += (Ammount * Convert.ToDecimal(DollarData.DollarBuyE.Value));
                    }
                }
                reservation.Opcion = 3;
                _reservation.Maintenance(reservation);
                answer = "Se ha agregado el deposito exitosamente a la reservación.";

                RedirectToAction("Index");

                return answer;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return ex.Message + "\n" + ex.StackTrace;
            }
        }

        [HttpGet]
        public JsonResult BuscarClientes(string term)
       {
            var clientes = _userService.GetList(new UserE() { Opcion = 1,Id_Role= IDRolClient })
                .Where(c => c.Name.ToLower().Contains(term.ToLower()) || c.DocumentID.Contains(term) )
                .Select(c => new {
                    id = c.DocumentID,
                    nombre = c.Name,
                    cedula = c.DocumentID,
                    correo = c.Email
                })
                .Take(20)
                .ToList();


            return Json(clientes, JsonRequestBehavior.AllowGet);
        }



    }

}
