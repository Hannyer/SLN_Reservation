
using EntityLayer;
using Repository.Repository;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SLN_Reservation
{
    public class LoginController : Controller
    {
        IUserService _UserService;
        IConfigurationService _configurationService;
        IRoleService _roleService;
        IIdentificationTypeService _IdentificationTypeService;
        public LoginController(IUserService service, IConfigurationService configurationService, IRoleService roleService, IIdentificationTypeService identificationTypeService)
        {
            _UserService = service;
            _configurationService = configurationService;
            _roleService = roleService;
            _IdentificationTypeService = identificationTypeService;
        }
        public ActionResult Index()
        {
            Session["User"] = null;
            Session["List_Menu"] = null;
            FillDropDownListIdentificationType();
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserE model)
        {
            if (ModelState.IsValid)
            {
                var user = _UserService.GetList(model).Where(x => x.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();

                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        if (user.Status)
                        {
                            Session["User"] = user;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "El usuario se encuentra inactivo");
                        }


                    }
                    else
                    {
                        ModelState.AddModelError("", "Credenciales incorrectas. Por favor, verifica tu usuario y contraseña.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Credenciales incorrectas. Por favor, verifica tu usuario y contraseña.");
                }
            }
            FillDropDownListIdentificationType();
            return View("Index", model);
        }


        [HttpPost]
        public JsonResult RegisterNewUser(UserE userRequest) 
        {
            
            if (string.IsNullOrEmpty(userRequest.Password) ||
                string.IsNullOrEmpty(userRequest.Name) ||
                string.IsNullOrEmpty(userRequest.Email) ||         
                string.IsNullOrEmpty(userRequest.PhoneNumber) ||    
                string.IsNullOrEmpty(userRequest.DocumentID) ||
                userRequest.IdentificationType_Description == "0")

            {
                return Json(new { success = false, message = "Por favor, complete todos los campos obligatorios." });
            }

            
            if (!IsValidEmail(userRequest.Email))
            {
                return Json(new { success = false, message = "El formato del correo electrónico es inválido." });
            }

            if (!IsValidPhoneNumber(userRequest.PhoneNumber))
            {
                return Json(new { success = false, message = "El número de teléfono debe ser de 8 dígitos numéricos." });
            }

            if (!IsValidDocumentID(userRequest.DocumentID, userRequest.IdentificationType_Description.ToString())) 
            {
                return Json(new { success = false, message = "El formato del documento de identidad no coincide con el tipo de identificación seleccionado." });
            }


            List<UserE> existingUsers =  _UserService.GetList(new UserE() { Opcion = 0, Email = userRequest.Email}); 


            bool emailExists = existingUsers.Any(u => u.Email != null && u.Email.Equals(userRequest.Email, StringComparison.OrdinalIgnoreCase));
            if (emailExists)
            {
                return Json(new { success = false, message = "El correo electrónico ya está registrado." });
            }

            bool documentIDExists = existingUsers.Any(u => u.DocumentID != null && u.DocumentID.Equals(userRequest.DocumentID, StringComparison.OrdinalIgnoreCase));
            if (documentIDExists)
            {
                return Json(new { success = false, message = "El número de documento ya está registrado." });
            }

            List<RoleE> roles = _roleService.GetList(new RoleE() { Opcion = 0, });

            RoleE rol = roles.FirstOrDefault(r => r.Description != null && r.Description.IndexOf("Cliente", StringComparison.OrdinalIgnoreCase) >= 0);

            var configEmail = _configurationService.GetList(new ConfigurationE()
            {
                Opcion = 0,
                KEY01 = "PARAMETRO",
                KEY02 = "FUNCIONALIDAD",
                KEY03 = "MRB",
                KEY04 = "ROL",
                KEY05 = "CLIENTE",
                KEY06 = "ROLCLIENTE"
            });

            userRequest.Opcion = 0;
            userRequest.Id_Role = int.Parse(configEmail.Where(x => x.KEY06 == "ROLCLIENTE").FirstOrDefault().VALUE);
            userRequest.Status = true; 
            userRequest.ResetPassword = false;

            try
            {

                var userService = new UserService(new UserRepository());


                bool result =  _UserService.Maintenance(userRequest);

                if (result)
                {
                    return Json(new { success = true, message = "¡Cuenta creada exitosamente! Ya puedes iniciar sesión." });
                }
                else
                {
                    return Json(new { success = false, message = "Error al crear la cuenta. Verifica los datos e intenta de nuevo." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error interno al crear la cuenta. Por favor, intente más tarde." });
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                return Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            return Regex.IsMatch(phone, @"^[0-9]{8}$");
        }

        private bool IsValidDocumentID(string docId, string identificationTypeID)
        {
            if (string.IsNullOrWhiteSpace(docId)) return false;

            // Permite 1-XXXX-XXXX o XXXXXXXXX
            string cedulaFisicaRegex = @"^(\d-\d{4}-\d{4}|\d{9})$";

            string cedulaJuridicaRegex = @"^\d-\d{3}-\d{6}$";
            string dimexPasaporteRegex = @"^[A-Z0-9]{6,15}$";
            string niteRegex = @"^\d{10}$";

            switch (identificationTypeID)
            {
                case "1": // Cédula Física
                    return Regex.IsMatch(docId, cedulaFisicaRegex);
                case "2": // Cédula Jurídica
                    return Regex.IsMatch(docId, cedulaJuridicaRegex);
                case "3": // DIMEX / Pasaporte
                    return Regex.IsMatch(docId, dimexPasaporteRegex, RegexOptions.IgnoreCase);
                case "4": // NITE
                    return Regex.IsMatch(docId, niteRegex);
                default:
                    return false;
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost]
        public JsonResult SendPasswordResetLink(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "El correo electrónico es requerido." });
            }

            if (!IsValidEmail(email))
            {
                return Json(new { success = false, message = "El formato del correo electrónico es inválido." });
            }

            try
            {
                var userFilter = new UserE() { Email = email, Opcion = 0 }; 
                var userFound = _UserService.GetList(userFilter)
                                         .FirstOrDefault(u => u.Email != null && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (userFound == null)
                {
                    return Json(new { success = true, message = "Si el correo electrónico está registrado, recibirás un mensaje con la nueva contraseña." });
                }

                string newPlainTextPassword = GenerateRandomPassword(8);

                string newEncryptedPassword = UtilitarioE.EncriptarString(newPlainTextPassword);

                userFound.Password = newEncryptedPassword;
                userFound.Opcion = 2; 
                userFound.ResetPassword = true;

                bool updateResult = _UserService.Maintenance(userFound);

                if (updateResult)
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

                    var emailConfig = new EmailConfigurationE()
                    {
                        Email = configEmail.Where(x => x.KEY06 == "CORREO").FirstOrDefault().VALUE,
                        Password = configEmail.Where(x => x.KEY06 == "PASSWORD").FirstOrDefault().VALUE,
                        Host = configEmail.Where(x => x.KEY06 == "HOST").FirstOrDefault().VALUE,
                        Port = Convert.ToInt32(configEmail.Where(x => x.KEY06 == "PORT").FirstOrDefault().VALUE),

                    };

                    string emailSubject = "Restablecimiento de Contraseña - Hotel Malibú";
                    string emailBody = $"Hola {userFound.Name},<br><br>Tu nueva contraseña para iniciar sesión en Hotel Malibú es: <strong>{newPlainTextPassword}</strong><br><br>Por favor, inicia sesión con esta nueva contraseña y cámbiala lo antes posible por una que puedas recordar.<br><br>Saludos cordiales,<br>El equipo de Hotel Malibú";

                    try
                    {
                        UtilitarioE.SendEmail(emailConfig, userFound.Email, emailSubject, emailBody);
                    }
                    catch (Exception emailEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al enviar correo de restablecimiento: {emailEx.Message}");
                        return Json(new { success = true, message = "Contraseña restablecida, pero hubo un problema al enviar el correo. Por favor, contacte a soporte si no la recibe." });
                    }

                    return Json(new { success = true, message = "Se ha enviado una nueva contraseña a tu correo electrónico. Por favor, revisa tu bandeja de entrada y spam." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo restablecer la contraseña en este momento. Intente de nuevo." });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en SendPasswordResetLink: {ex.Message}");
                return Json(new { success = false, message = "Ocurrió un error inesperado al procesar su solicitud. Por favor, intente más tarde." });
            }
        }

    }
}