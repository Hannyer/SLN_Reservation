using EntityLayer;
using Newtonsoft.Json;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
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
        // GET: Cliente
        public ClientController(IClientService service, IIdentificationTypeService identificationTypeService, IRateTypeService rateTypeService)
        {
            this._ClientService = service;
            _IdentificationTypeService = identificationTypeService;
            _RateTypeService = rateTypeService;
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
    }
}