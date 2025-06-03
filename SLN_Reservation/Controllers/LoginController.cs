
using EntityLayer;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SLN_Reservation
{
    public class LoginController : Controller
    {
        IUserService _UserService;
        IConfigurationService _configurationService;
        public LoginController(IUserService service, IConfigurationService configurationService)
        {
            _UserService = service;
            _configurationService = configurationService;
        }
        public ActionResult Index()
        {
            Session["User"] = null;
            Session["List_Menu"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserE model)
        {
            if (ModelState.IsValid)
            {
                var user = _UserService.GetList(model).Where(x => x.User.ToLower() == model.User.ToLower()).FirstOrDefault();

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
            return View("Index", model);
        }


        [HttpPost]
        public JsonResult RegisterNewUser(UserE userRequest) // userRequest ahora contendrá Email, PhoneNumber, DocumentID
        {
            // VALIDACIÓN BÁSICA (puedes expandirla con DataAnnotations en UserE)
            if (string.IsNullOrEmpty(userRequest.User) ||
                string.IsNullOrEmpty(userRequest.Password) ||
                string.IsNullOrEmpty(userRequest.Name) ||
                string.IsNullOrEmpty(userRequest.Email) ||         // Validar nuevo campo
                string.IsNullOrEmpty(userRequest.PhoneNumber) ||    // Validar nuevo campo
                string.IsNullOrEmpty(userRequest.DocumentID))      // Validar nuevo campo
            {
                return Json(new { success = false, message = "Por favor, complete todos los campos obligatorios." });
            }

            // Validaciones adicionales en el backend (redundantes con JS pero más seguras)
            if (!IsValidEmail(userRequest.Email))
            {
                return Json(new { success = false, message = "El formato del correo electrónico es inválido." });
            }

            if (!IsValidPhoneNumber(userRequest.PhoneNumber))
            {
                return Json(new { success = false, message = "El número de teléfono debe ser de 8 dígitos numéricos." });
            }

            if (!IsValidDocumentID(userRequest.DocumentID))
            {
                return Json(new { success = false, message = "El formato del documento de identidad (cédula o pasaporte) es inválido." });
            }


            // Obtener la lista de usuarios existentes para validar el nombre de usuario
            List<UserE> existingUsers = _UserService.GetList(new UserE() { Opcion = 0 });

            // Validar si ya existe un usuario con el mismo nombre (ignorando mayúsculas/minúsculas)
            bool userExists = existingUsers.Any(u => u.User.Equals(userRequest.User, StringComparison.OrdinalIgnoreCase));

            if (userExists)
            {
                return Json(new { success = false, message = "El nombre de usuario ya está en uso. Por favor, elija otro." });
            }

            // Validar si ya existe un usuario con el mismo correo electrónico
            bool emailExists = existingUsers.Any(u => u.Email != null && u.Email.Equals(userRequest.Email, StringComparison.OrdinalIgnoreCase));
            if (emailExists)
            {
                return Json(new { success = false, message = "El correo electrónico ya está registrado." });
            }

            // Validar si ya existe un usuario con el mismo número de documento
            bool documentIDExists = existingUsers.Any(u => u.DocumentID != null && u.DocumentID.Equals(userRequest.DocumentID, StringComparison.OrdinalIgnoreCase));
            if (documentIDExists)
            {
                return Json(new { success = false, message = "El número de documento ya está registrado." });
            }


            // ***** AQUÍ SE "QUEMA" EL ROL Y EL ESTADO POR DEFECTO *****
            userRequest.Opcion = 0; // Opcion para insertar
            userRequest.Id_Role = 24; // Rol por defecto (ajusta según tus necesidades)
            userRequest.Status = true; // Asigna el estado activo por defecto
            userRequest.ResetPassword = false;

            try
            {
                // Llama a tu servicio para agregar el usuario con el rol y estado quemados
                bool result = _UserService.Maintenance(userRequest);

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
                // Aquí deberías loggear la excepción 'ex' para depuración
                return Json(new { success = false, message = "Ocurrió un error interno al crear la cuenta. Por favor, intente más tarde." });
            }
        }

        // Métodos de validación para el backend (iguales a los de JS pero en C#)
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                // Expresión regular para validar formato de correo electrónico
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
            // Valida que el número de teléfono sean exactamente 8 dígitos numéricos
            return Regex.IsMatch(phone, @"^[0-9]{8}$");
        }

        private bool IsValidDocumentID(string docId)
        {
            if (string.IsNullOrWhiteSpace(docId)) return false;
            // Cédula costarricense: 1-1234-5678 (ej: 1-0123-0456, 1-1234-5678, etc.)
            // Podría ser 1-9XXX-YYYY, 2-XXX-YYYY (extranjero residente), etc.
            // Para simplicidad, se valida el formato N-NNNN-NNNN
            bool isCedula = Regex.IsMatch(docId, @"^\d-\d{4}-\d{4}$");

            // Pasaporte: puede ser alfanumérico, ej: ABC123456, 123456789. Longitud flexible.
            // Se asume un rango de 6 a 15 caracteres alfanuméricos.
            bool isPassport = Regex.IsMatch(docId, @"^[A-Z0-9]{6,15}$", RegexOptions.IgnoreCase);

            return isCedula || isPassport;
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
            // 1. Validar que el correo electrónico no esté vacío y tenga formato válido
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
                // 2. Buscar al usuario por correo electrónico en la base de datos
                var userFilter = new UserE() { Email = email, Opcion = 0 }; // Opcion 0 para consultar
                var userFound = _UserService.GetList(userFilter)
                                         .FirstOrDefault(u => u.Email != null && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (userFound == null)
                {
                    // Por razones de seguridad, es mejor no confirmar si el correo existe o no.
                    return Json(new { success = true, message = "Si el correo electrónico está registrado, recibirás un mensaje con la nueva contraseña." });
                }

                // 3. Generar una nueva contraseña aleatoria (texto plano)
                string newPlainTextPassword = GenerateRandomPassword(8); // Esta es la contraseña que se enviará por correo.

                // 4. Encriptar la nueva contraseña antes de guardarla en la base de datos
                string newEncryptedPassword = UtilitarioE.EncriptarString(newPlainTextPassword);

                // 5. Actualizar la contraseña del usuario en la base de datos
                userFound.Password = newEncryptedPassword; // Guardamos la contraseña encriptada
                userFound.Opcion = 2; // Opcion 2 para actualizar
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
                    // El cuerpo del correo ahora usa la contraseña en texto plano (newPlainTextPassword)
                    string emailBody = $"Hola {userFound.Name},<br><br>Tu nueva contraseña para iniciar sesión en Hotel Malibú es: <strong>{newPlainTextPassword}</strong><br><br>Por favor, inicia sesión con esta nueva contraseña y cámbiala lo antes posible por una que puedas recordar.<br><br>Saludos cordiales,<br>El equipo de Hotel Malibú";

                    try
                    {
                        UtilitarioE.SendEmail(emailConfig, userFound.Email, emailSubject, emailBody);
                    }
                    catch (Exception emailEx)
                    {
                        // Si el correo falla, loggearlo, pero aún así reportar éxito al usuario.
                        // El restablecimiento de contraseña fue exitoso en la BD, el problema es solo el envío.
                        System.Diagnostics.Debug.WriteLine($"Error al enviar correo de restablecimiento: {emailEx.Message}");
                        return Json(new { success = true, message = "Contraseña restablecida, pero hubo un problema al enviar el correo. Por favor, contacte a soporte si no la recibe." });
                    }

                    // 7. Mensaje de éxito para el usuario
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