using David.DataAccess.Data;
using David.DataAccess.Repository.IRepository;
using David.Models;
using David.Models.ViewModels;
using David.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Collections.Generic;

namespace DavidProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
    
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }

        }
        [HttpPost]
public IActionResult Upsert(Company CompanyObj)
{
    if (ModelState.IsValid)
    {
   
               
        if(CompanyObj.Id ==0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
        else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
        //_unitOfWork.Company.Add(CompanyVM.Company);
        _unitOfWork.Save();
        TempData["Success"] = "Company created successfully";
        return RedirectToAction("Index");
    }
    else
    {


        return View(CompanyObj);
    }
}




           

        #region APICALLS

        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data=objCompanyList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)


        {

            var CompanyToBeDeleted = _unitOfWork.Company.Get(u=> u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

           

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            List<Company> objCompanyList = _unitOfWork.Company.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objCompanyList });
        }
        #endregion
    }
}

