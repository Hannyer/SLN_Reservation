using EntityLayer;
using Newtonsoft.Json;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SLN_Reservation.Controllers.Mantenimientos
{
    public class ClientController : Controller
    {
        IClientService _ClientService;
        IIdentificationTypeService _IdentificationTypeService;
        IRateTypeService _RateTypeService;
        IHotelRoomService hotelRoomService;
        ConfigurationService _configurationService;
        IRateService _operateService;
        IReservationService _reservation;
        IUserService _userService;
        public ClientController(IClientService service, IIdentificationTypeService identificationTypeService, IRateTypeService rateTypeService, IHotelRoomService hotelRoomService, ConfigurationService configurationService, IRateService operateService, IReservationService reservation, IUserService userService)
        {
            this._ClientService = service;
            _IdentificationTypeService = identificationTypeService;
            _RateTypeService = rateTypeService;
            this.hotelRoomService = hotelRoomService;
            _configurationService = configurationService;
            _operateService = operateService;
            _reservation = reservation;
            _userService = userService;
        }
        public ActionResult Index()
        {
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            FillDropDownListIdentificationType();
            FillDropDownListRateType();
            var list = _ClientService.GetList(new ClientE());
            return View(list);
        }
        public ActionResult IndexS()
        {
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            FillDropDownListIdentificationType();
            FillDropDownListRateType();
            var list = _ClientService.GetList(new ClientE());
            return View(list);
        }

        public ActionResult IndexReservation()
        {
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            FillDropDownListIdentificationType();
            FillDropDownListRateType();
            var list = _ClientService.GetList(new ClientE());
            return View(list);
        }
        public string NewClient(int Opcion, string Full_Name, string IdCard, string Phone_number1, string Phone_number2, string Mail, string Detail, int Id_Identificationtype,int RateType_Id)
        {

            string answer = "";
            bool tmpAnswer = _ClientService.Maintenance(new ClientE() {Opcion=Opcion,Full_Name=Full_Name,IdCard=IdCard, Phone_number1= Phone_number1, Phone_number2= Phone_number2, Mail= Mail, Detail= Detail, IdentificationType_Id= Id_Identificationtype,RateType_Id= RateType_Id });
            if (tmpAnswer)
            {
                answer = "Cliente Agregado exitosamente";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }

        public string ModifyClient(int Opcion, string Full_Name, string IdCard, string Phone_number1, string Phone_number2, string Mail, string Detail, int Id_Identificationtype, int RateType_Id)
        {
            string answer = "";
            bool tmpAnswer = _ClientService.Maintenance(new ClientE() { Opcion = Opcion, Full_Name = Full_Name, IdCard = IdCard, Phone_number1 = Phone_number1, Phone_number2 = Phone_number2, Mail = Mail, Detail = Detail, IdentificationType_Id = Id_Identificationtype,RateType_Id= RateType_Id });
            if (tmpAnswer)
            {
                answer = "Cliente Modificado con éxito";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }

        [HttpPost]
        public string DeletClient(ClientE ClientRequest)
        {
          
            try
            {
                string answer = "";
                bool result = _ClientService.Maintenance(ClientRequest);
                if (result)
                {
                    answer = "¡Usuario eliminado exitosamente!";
                    RedirectToAction("Index");

                }
                else
                {
                    answer = "Ha ocurrido un error al agregar el usuario";
                }
                return answer;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void FillDropDownListIdentificationType()
        {
            var Identification = _IdentificationTypeService.GetList(new IdentificationTypeE() { Opcion = 0 });


            var IdentificationList = Identification.Select(IdentificationL => new SelectListItem
            {
                Value = IdentificationL.ID.ToString(),
                Text = IdentificationL.Description
            });


            ViewBag.IdentificationList = IdentificationList;

        }
        public void FillDropDownListRateType()
        {
            var ratetype = _RateTypeService.GetList(new RateTypeE () { Opcion = 0 });


            var RateTypeList = ratetype.Select(RateTypeL => new SelectListItem
            {
                Value = RateTypeL.ID.ToString(),
                Text = RateTypeL.Description
            });


            ViewBag.RateTypeList = RateTypeList;

        }


        public void pruebas() { 
            BaseE basee=new BaseE();

            RoleE role = new RoleE();
          
            int resultado = role.Suma(1,2);

        }

        [HttpPost]
        public string SearchReservation(DateTime checkInDate, DateTime checkOutDate, int numberOfGuests)
        {
            var rooms = hotelRoomService.GetList(new Hotel_RoomE
            {
                Opcion = 1,
                StardDate = checkInDate,
                EndDate = checkOutDate,
                Capacity = numberOfGuests
            });

            var html = new System.Text.StringBuilder();

            foreach (var room in rooms)
            {
                html.Append($@"
                    <div class='col'>
                        <div class='card h-100 shadow-sm'>
                            <div class='card-body d-flex flex-column'>
                                <div class='text-center mb-3'>
                                    <i class='fas fa-bed fa-3x text-primary'></i>
                                </div>
                                <h5 class='card-title'>{room.Name}</h5>
                                <p class='card-text mb-1'><strong>Capacidad:</strong> {room.Capacity} {(room.Capacity == 1 ? "persona" : "personas")}</p>
                                <p class='card-text mb-1'><strong>Descripción:</strong> {room.Description}</p> 
                                <p class='card-text mb-1'>
                                    <strong>Precio:</strong> ₡{room.Price:N0}<br />
                                    <strong>Precio Dolares:</strong> ${room.DolarPrice:N0}<br />
                                </p>
                                <div class='mt-auto'>
                                    <button type='button'
                                            class='btn btn-success w-100 btn-book'
                                            data-id='{room.ID}'
                                            data-price='{room.Price}' 
                                            data-name='{room.Name}' 
                                            data-description='{room.Description}'> 
                                        Reservar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>");
                        }

            return html.ToString();
        }


        [HttpPost]
        public async Task<string> BookRoom(int idRoom, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                var roomDetails = hotelRoomService.GetList(new Hotel_RoomE { ID = idRoom, Opcion = 0 }).FirstOrDefault();

                if (roomDetails == null)
                {
                    return "Error: La habitación seleccionada no fue encontrada.";
                }

                var user = Session["User"] as UserE;
                var currentUser = _userService.GetList(new UserE() { Opcion = 0, Email = user.Email }).FirstOrDefault();
                if (currentUser == null)
                {
                    return "Error: Usuario no autenticado. Por favor, inicie sesión de nuevo.";
                }

                var configIVA = _configurationService.GetList(new ConfigurationE()
                {
                    Opcion = 0,
                    KEY01 = "PARAMETRO",
                    KEY02 = "FUNCIONALIDAD",
                    KEY03 = "MRB",
                    KEY04 = "IMPUESTO",
                    KEY05 = "IVA"
                }).FirstOrDefault();

                if (configIVA == null)
                {
                    return "Error: Configuración de impuestos (IVA) no encontrada.";
                }

                double ivaRate = Convert.ToDouble(configIVA.VALUE) / 100;

                TimeSpan duration = checkOut.Date - checkIn.Date;
                int numberOfNights = (int)duration.TotalDays;

                if (numberOfNights <= 0)
                {
                    return "Error: La fecha de salida debe ser posterior a la de entrada.";
                }

                double totalAmount = (double)(numberOfNights * roomDetails.Price); 

                double subtotalWithoutTax = Math.Round(totalAmount /  ivaRate, 2);
                double taxAmount = Math.Round(totalAmount - subtotalWithoutTax, 2);

                ReservationE newReservation = new ReservationE
                {
                    ID_ROOM = idRoom,
                    CheckIn = new DateTime(checkIn.Year, checkIn.Month, checkIn.Day, 15, 0, 0),
                    CheckOut = new DateTime(checkOut.Year, checkOut.Month, checkOut.Day, 12, 0, 0),
                    Days = numberOfNights,
                    Status = "1",
                    Reservation_Description = $"Reservación: {roomDetails.Name} {roomDetails.Description}",
                    Price = roomDetails.Price,
                    create_by_external=true,



                    ID_USER = currentUser.ID,
                    IdCard_Client = currentUser.DocumentID,
                    Full_Name = currentUser.Name,
                    Client_Mail = currentUser.Email,

                    SubtotalWithoutTax = subtotalWithoutTax,
                    TaxAmount = taxAmount,
                    TotalAmount = totalAmount,

                    Opcion = 0,
                    START_DATE = DateTime.Now,
                    END_DATE = DateTime.Now,
                    creation_date = DateTime.Now
                };


                int IdGenerate = _reservation.Maintenance(newReservation);

                string answer = "";

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
                    UtilitarioE.SendEmail(email, user.Email, "Confirmación de reservación", GenerateReservationConfirmationEmail(getGenerateReservation, null));
                }
                else
                {
                    answer = "Ha ocurrido un error";
                }
                return answer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en BookRoom: {ex.Message} - StackTrace: {ex.StackTrace}");
                return "Ha ocurrido un error inesperado al procesar su reservación. Por favor, contacte a soporte.";
            }
        }

        public string GenerateReservationConfirmationEmail(ReservationE reservation, DollarDataE dollarData)
        {
            var emailContentBuilder = new StringBuilder();

            emailContentBuilder.AppendLine("<html>");
            emailContentBuilder.AppendLine("<head>");
            emailContentBuilder.AppendLine("<meta charset=\"UTF-8\">");
            emailContentBuilder.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            emailContentBuilder.AppendLine("<title>Confirmación de Reserva - Hotel CTP</title>");
            emailContentBuilder.AppendLine("<style>");
            emailContentBuilder.AppendLine("    body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f4f4f4; }");
            emailContentBuilder.AppendLine("    .email-container { max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 8px rgba(0,0,0,0.1); }");
            emailContentBuilder.AppendLine("    .header { background-color: #2C3E50; color: #ffffff; padding: 25px 20px; text-align: center; border-top-left-radius: 8px; border-top-right-radius: 8px; }");
            emailContentBuilder.AppendLine("    .header h1 { margin: 0; font-size: 28px; }");
            emailContentBuilder.AppendLine("    .content { padding: 30px; }");
            emailContentBuilder.AppendLine("    .content p { margin-bottom: 15px; font-size: 15px; }");
            emailContentBuilder.AppendLine("    .details-table { width: 100%; border-collapse: collapse; margin-top: 25px; margin-bottom: 25px; border: 1px solid #e0e0e0; }");
            emailContentBuilder.AppendLine("    .details-table th, .details-table td { padding: 12px 15px; text-align: left; border-bottom: 1px solid #e0e0e0; }");
            emailContentBuilder.AppendLine("    .details-table th { background-color: #f9f9f9; color: #555; font-weight: 600; }");
            emailContentBuilder.AppendLine("    .details-table tr:last-child td { border-bottom: none; }");
            emailContentBuilder.AppendLine("    .total-row { background-color: #eaf7ff; font-weight: bold; }");
            emailContentBuilder.AppendLine("    .footer { background-color: #f0f0f0; color: #777; text-align: center; padding: 20px; font-size: 13px; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px; }");
            emailContentBuilder.AppendLine("    .button-link { display: inline-block; background-color: #3498db; color: #ffffff !important; padding: 10px 20px; text-decoration: none; border-radius: 5px; margin-top: 20px; font-weight: bold; }");
            emailContentBuilder.AppendLine("    .button-link:hover { background-color: #2980b9; }");
            emailContentBuilder.AppendLine("</style>");
            emailContentBuilder.AppendLine("</head>");
            emailContentBuilder.AppendLine("<body>");
            emailContentBuilder.AppendLine("    <div class=\"email-container\">");
            emailContentBuilder.AppendLine("        <div class=\"header\">");
            emailContentBuilder.AppendLine("            <h1>Hotel CTP</h1>");
            emailContentBuilder.AppendLine("            <p style=\"font-size: 16px;\">¡Su reserva ha sido confirmada!</p>");
            emailContentBuilder.AppendLine("        </div>");

            emailContentBuilder.AppendLine("        <div class=\"content\">");
            emailContentBuilder.AppendLine($"            <p>Estimado(a) <strong>{reservation.Full_Name}</strong>,</p>");
            emailContentBuilder.AppendLine("            <p>¡Nos complace informarle que su reserva en Hotel CTP ha sido confirmada exitosamente! Estamos ansiosos por darle la bienvenida y asegurarnos de que disfrute de una estancia inolvidable.</p>");

            emailContentBuilder.AppendLine("            <h3 style=\"color: #2C3E50; margin-top: 30px; border-bottom: 1px solid #eee; padding-bottom: 10px;\">Detalles de su Reserva</h3>");

            emailContentBuilder.AppendLine("            <table class=\"details-table\">");
            emailContentBuilder.AppendLine("                <tbody>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Habitación #</strong></td><td>{reservation.ID_ROOM.ToString()}</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Descripción de la Reserva</strong></td><td>{reservation.Reservation_Description}</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Ingreso<strong></td><td>{reservation.CheckIn.ToString("dddd, dd MMMM  yyyy", CultureInfo.CreateSpecificCulture("es-CR"))} (15:00 HRS)</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Salida</strong></td><td>{reservation.CheckOut.ToString("dddd, dd MMMM  yyyy", CultureInfo.CreateSpecificCulture("es-CR"))} (12:00 HRS)</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td><strong>Noches</strong></td><td>{reservation.Days}</td></tr>");
            emailContentBuilder.AppendLine("                </tbody>");
            emailContentBuilder.AppendLine("            </table>");

            emailContentBuilder.AppendLine("            <h3 style=\"color: #2C3E50; margin-top: 30px; border-bottom: 1px solid #eee; padding-bottom: 10px;\">Resumen de Pago</h3>");
            emailContentBuilder.AppendLine("            <table class=\"details-table\" style=\"width: 80%; margin-left: auto; margin-right: auto;\">"); 
            emailContentBuilder.AppendLine("                <tbody>");

            emailContentBuilder.AppendLine($"                    <tr><td>Precio por Noche:</td><td style=\"text-align: right;\">{Math.Round(reservation.Price, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR"))} (IVA incluido)</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td>Subtotal (sin IVA):</td><td style=\"text-align: right;\">{Math.Round(reservation.SubtotalWithoutTax, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR"))}</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr><td>IVA:</td><td style=\"text-align: right;\">{Math.Round(reservation.TaxAmount, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR"))}</td></tr>");
            emailContentBuilder.AppendLine($"                    <tr class=\"total-row\"><td>Total a Pagar:</td><td style=\"text-align: right;\">{Math.Round(reservation.TotalAmount, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR"))}</td></tr>");

            emailContentBuilder.AppendLine("                </tbody>");
            emailContentBuilder.AppendLine("            </table>");

            emailContentBuilder.AppendLine("            <p style=\"margin-top: 30px;\">Estamos a su disposición para cualquier consulta o solicitud especial. No dude en contactarnos si necesita ayuda con su reserva.</p>");
            emailContentBuilder.AppendLine("        </div>");

            emailContentBuilder.AppendLine("        <div class=\"footer\">");
            emailContentBuilder.AppendLine("            <p>Gracias por elegir Hotel CTP.</p>");
            emailContentBuilder.AppendLine("            <p>¡Esperamos verle pronto!</p>");
            emailContentBuilder.AppendLine("            <p>Este mensaje ha sido generado automáticamente. Por favor, no responda a este correo electrónico.</p>");
            emailContentBuilder.AppendLine("            <p>&copy; " + DateTime.Now.Year + " Hotel CTP. Todos los derechos reservados.</p>");
            emailContentBuilder.AppendLine("        </div>");
            emailContentBuilder.AppendLine("    </div>");
            emailContentBuilder.AppendLine("</body>");
            emailContentBuilder.AppendLine("</html>");

            return emailContentBuilder.ToString();
        }

    }
}