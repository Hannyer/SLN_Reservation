using EntityLayer;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLN_Reservation.Controllers
{
    public class ReservationClientController : Controller
    {
        IRateTypeService _RateTypeService;
        IIdentificationTypeService _IdentificationTypeService;
        public ReservationClientController(IRateTypeService rates, IIdentificationTypeService identificationTypeService = null)
        {
            _RateTypeService = rates;
            _IdentificationTypeService = identificationTypeService; 
        }

        // GET: ReservationClient
        public ActionResult Index()
        {
            
            FillDropDownListRateType();
            FillDropDownListIdentificationType();
            return View();
        }

        public void FillDropDownListRateType()
        {
            var ratetype = _RateTypeService.GetList(new RateTypeE() { Opcion = 0 });


            var RateTypeList = ratetype.Select(RateTypeL => new SelectListItem
            {
                Value = RateTypeL.ID.ToString(),
                Text = RateTypeL.Description
            });


            ViewBag.RateTypeList = RateTypeList;

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
    }
}