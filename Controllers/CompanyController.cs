using MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            IEnumerable<MvcCompanyModel> companyList;
            HttpResponseMessage clientResponce = GlobalVariables.webapiClient.GetAsync("Company").Result;
            companyList = clientResponce.Content.ReadAsAsync<IEnumerable<MvcCompanyModel>>().Result;
            return View(companyList);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new MvcCompanyModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.webapiClient.GetAsync("Company/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<MvcCompanyModel>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(MvcCompanyModel company)
        {
            if (company.Id == 0)
            {
                HttpResponseMessage response = GlobalVariables.webapiClient.PostAsJsonAsync("Company", company).Result;
                TempData["SuccessMessage"] = "Saved Successfully";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.webapiClient.PutAsJsonAsync("Company/" + company.Id, company).Result;

                if (response.IsSuccessStatusCode == false)
                {
                    TempData["SuccessMessage"] = "Changes Unable to be Saved to the Database.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Updated Successfully";
                }
                //TempData["SuccessMessage"] = "Updated Successfully";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.webapiClient.DeleteAsync("Company/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }

        public ActionResult IsISIN_Available(string Isin)
        {
            if (Isin != String.Empty)
            { 
                /// call out to webapi 
                HttpResponseMessage response = GlobalVariables.webapiClient.GetAsync("Company/ISIN/" + Isin).Result;
                if (response.IsSuccessStatusCode == true)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }


    }
}