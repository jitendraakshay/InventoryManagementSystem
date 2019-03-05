using DomainEntities;
using InventoryManagementSystem.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DomainInterface;

namespace InventoryManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UnitsController : Controller
    {
        
        private readonly IUnitsRepo unitsRepo;        
        private readonly ILoginUser loginUser;
       
       

        #region Constructor
        public UnitsController(IUnitsRepo unitsRepo, ILoginUser LoginUser)
        {
            this.unitsRepo = unitsRepo;        
            this.loginUser = LoginUser;

        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ReturnType saveUnit(Units units)
        {

            try
            {
                units.EntryBy = loginUser.GetCurrentUser();
                ReturnType type = unitsRepo.saveUnit(units);
                return type;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public object getAllUnits()
        {
            JsonResponse response = new JsonResponse();
            List<Units> model = unitsRepo.getAllUnit();
            response.ResponseData = model;
            return JsonConvert.SerializeObject(response);

        }
    }
}