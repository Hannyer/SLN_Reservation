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
        // GET: Cliente
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
                                <h5 class='card-title'>{room.Description}</h5>
                                <p class='card-text mb-1'><strong>Capacidad:</strong> {room.Capacity} {(room.Capacity == 1 ? "persona" : "personas")}</p>
                                <p class='card-text mb-1'>
                                    <strong>Precio:</strong> ₡{room.Price:N0}<br />
                                    <small class='text-muted'>USD ${room.DolarPrice:N2}</small>
                                </p>
                                <div class='mt-auto'>
                                    <button type='button'
                                            class='btn btn-success w-100 btn-book'
                                            data-id='{room.ID}'>
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
                // 1. Obtener los detalles de la habitación seleccionada
                var roomDetails = hotelRoomService.GetList(new Hotel_RoomE { ID = idRoom, Opcion = 0 }).FirstOrDefault();

                if (roomDetails == null)
                {
                    return "Error: La habitación seleccionada no fue encontrada.";
                }

                // 2. Obtener los datos del usuario de la sesión
                var user = Session["User"] as UserE;
                var currentUser = _userService.GetList(new UserE() { Opcion = 0, Email = user.Email }).FirstOrDefault();
                if (currentUser == null)
                {
                    return "Error: Usuario no autenticado. Por favor, inicie sesión de nuevo.";
                }

                // 3. Obtener la configuración del IVA (impuesto)
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

                // 4. Calcular el número de noches
                TimeSpan duration = checkOut.Date - checkIn.Date;
                int numberOfNights = (int)duration.TotalDays;

                if (numberOfNights <= 0)
                {
                    return "Error: La fecha de Check-out debe ser posterior a la de Check-in.";
                }

                // 5. Realizar cálculos de precios (Directamente en Colones CRC)
                double subtotal = (double)(numberOfNights * roomDetails.Price); // roomDetails.Price se asume en CRC

                double subtotalWithoutTax = 0;
                double taxAmount = 0;
                double totalAmount = 0;

                // Lógica de cálculo del IVA: Asumiendo que roomDetails.Price ya incluye el IVA
                subtotalWithoutTax = Math.Round(subtotal / (1 + ivaRate), 2);
                taxAmount = subtotal - subtotalWithoutTax;
                totalAmount = subtotal; // El total es el subtotal original ya con IVA

                // 6. Crear y poblar el objeto ReservationE
                ReservationE newReservation = new ReservationE
                {
                    ID_ROOM = idRoom,
                    CheckIn = new DateTime(checkIn.Year, checkIn.Month, checkIn.Day, 15, 0, 0),
                    CheckOut = new DateTime(checkOut.Year, checkOut.Month, checkOut.Day, 12, 0, 0),
                    Days = numberOfNights,
                    Status = "1",
                    Reservation_Description = $"Reserva para habitación {roomDetails.Description} del {checkIn.ToShortDateString()} al {checkOut.ToShortDateString()}",
                    Price = roomDetails.Price, 
                    DESCRIPTION_HOTELROOM = roomDetails.Description,

                    // Datos del cliente de la sesión
                    ID_USER = currentUser.ID,
                    IdCard_Client = currentUser.DocumentID,
                    Full_Name = currentUser.Name,
                    Client_Mail = currentUser.Email,

                    // Datos calculados
                    SubtotalWithoutTax = subtotalWithoutTax,
                    TaxAmount = taxAmount,
                    TotalAmount = totalAmount,

                    Opcion = 0,
                    START_DATE = DateTime.Now,
                    END_DATE = DateTime.Now
                };


                // 7. Guardar la reservación en la base de datos
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
                    UtilitarioE.SendEmail(email, getGenerateReservation.Client_Mail, "Confirmación de reservación", GenerateReservationConfirmationEmail(getGenerateReservation, null));
                    //RedirectToAction("Index");
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
            emailContentBuilder.AppendLine("<style>");
            emailContentBuilder.AppendLine("  /* Your CSS Styles */");
            emailContentBuilder.AppendLine("</style>");
            emailContentBuilder.AppendLine("</head>");
            emailContentBuilder.AppendLine("<body>");
            emailContentBuilder.AppendLine("<h2 style='text-align: center;'>Hotel Malibú los Sueños</h2>");
            emailContentBuilder.AppendLine($"<p>Estimado(a) {reservation.Full_Name},</p>");
            emailContentBuilder.AppendLine("<p>Le confirmamos su reserva con los siguientes detalles:</p>");
            emailContentBuilder.AppendLine($"<p>Tipo de tarifa {reservation.RateType_Description}</p>");
            emailContentBuilder.AppendLine($"<p> {reservation.Reservation_Description}</p>");
            emailContentBuilder.AppendLine($"<p>Ingreso: {reservation.CheckIn.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-CR"))}</p>");
            emailContentBuilder.AppendLine($"<p>Salida: {reservation.CheckOut.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-CR"))}</p>");
            emailContentBuilder.AppendLine($"<p>Noches: {reservation.Days}</p>");
            emailContentBuilder.AppendLine("<table>");
            //if (reservation.Currency.ToUpper().Equals("CRC"))
            //{
                emailContentBuilder.AppendLine("<tr><td>Precio por noche:</td><td>" + Math.Round(reservation.Price, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR")) + "/IVA incluido</td></tr>");
                emailContentBuilder.AppendLine("<tr><td>Subtotal:</td><td>" + Math.Round(reservation.SubtotalWithoutTax, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR")) + "</td></tr>");
                emailContentBuilder.AppendLine("<tr><td>IVA:</td><td>" + Math.Round(reservation.TaxAmount, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR")) + "</td></tr>");
                emailContentBuilder.AppendLine("<tr><td>Total:</td><td>" + Math.Round(reservation.TotalAmount, 2).ToString("C", CultureInfo.CreateSpecificCulture("es-CR")) + "</td></tr>");
            //}
            //else
            //{
            //    emailContentBuilder.AppendLine("<tr><td>Precio por noche:</td><td>" + Math.Round(reservation.Price, 2).ToString("C", CultureInfo.CreateSpecificCulture("en-US")) + " IVA incluido</td></tr>");
            //    emailContentBuilder.AppendLine("<tr><td>Subtotal:</td><td>" + Math.Round(Convert.ToDouble(reservation.SubtotalWithoutTax) / dollarData.DollarBuyE.Value, 2).ToString("C", CultureInfo.CreateSpecificCulture("en-US")) + "</td></tr>");
            //    emailContentBuilder.AppendLine("<tr><td>IVA:</td><td>" + Math.Round(Convert.ToDouble(reservation.TaxAmount) / dollarData.DollarBuyE.Value, 2).ToString("C", CultureInfo.CreateSpecificCulture("en-US")) + "</td></tr>");
            //    emailContentBuilder.AppendLine("<tr><td>Total:</td><td>" + Math.Round(Convert.ToDouble(reservation.TotalAmount) / dollarData.DollarBuyE.Value, 2).ToString("C", CultureInfo.CreateSpecificCulture("en-US")) + "</td></tr>");
            //}

            emailContentBuilder.AppendLine("</table>");
            emailContentBuilder.AppendLine("<p>Gracias por su preferencia. Esperamos que disfrute su estancia.</p>");
            emailContentBuilder.AppendLine("<p>Si tiene alguna pregunta o necesita hacer un cambio en su reserva, no dude en contactarnos.</p>");
            emailContentBuilder.AppendLine("<p>Este mensaje ha sido generado automáticamente. Por favor, no responda a este correo electrónico.</p>");
            emailContentBuilder.AppendLine("</body>");
            emailContentBuilder.AppendLine("</html>");

            return emailContentBuilder.ToString();
        }

    }
}