using DomainEntities;
using DomainInterface;
using InventoryManagementSystem.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductRepo productRepo;
        private readonly ILoginUser loginUser;
        
        public ProductsController(IProductRepo ProductRepo, ILoginUser LoginUser)
        {
            this.productRepo = ProductRepo;
            this.loginUser = LoginUser;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public int saveProduct(Products products)
        {

            try
            {
                products.EntryBy = loginUser.GetCurrentUser();
                int type = productRepo.saveProducts(products);
                return type;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ReturnType saveAttribute(Attributes attribute)
        {

            try
            {
                attribute.EntryBy = loginUser.GetCurrentUser();
                ReturnType type = productRepo.saveAttribute(attribute);
                return type;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        public int saveSKUs(List<SKUs> sku)
        {

            try
            {                
                int type = productRepo.saveSKUs(sku);
                return type;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ReturnType saveAttributeValue(AttributeValue attribute)
        {

            try
            {                
                ReturnType type = productRepo.saveAttributeValue(attribute);
                return type;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public object getAllProducts()
        {
            JsonResponse response = new JsonResponse();
            List<Products> model = productRepo.getAllProducts();
            response.ResponseData = model;
            return JsonConvert.SerializeObject(response);

        }

        [HttpGet]
        public object getAllAttributes(int? productID)
        {
            JsonResponse response = new JsonResponse();
            List<Attributes> model = productRepo.getAllAttributes(productID);
            response.ResponseData = model;
            return JsonConvert.SerializeObject(response);

        }

        [HttpGet]
        public object getAllAttributeValues(int? productID, int? optionID)
        {
            JsonResponse response = new JsonResponse();
            List<AttributeValue> model = productRepo.getAllAttributeValues(productID,optionID);
            response.ResponseData = model;
            return JsonConvert.SerializeObject(response);

        }
        public object getALLSkus(int? productID)
        {
            JsonResponse response = new JsonResponse();
            List<SKUs> model = productRepo.getALLSkus(productID);
            response.ResponseData = model;
            return JsonConvert.SerializeObject(response);

        }
    }
}