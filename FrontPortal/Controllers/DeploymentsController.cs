using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Gitloy.Services.FrontPortal.BusinessLogic.Core;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Handlers;
using Gitloy.Services.FrontPortal.BusinessLogic.Core.Model;
using Gitloy.Services.FrontPortal.ViewModels;
using Gitloy.Services.FrontPortal.ViewModels.Deployment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Identity;

namespace Gitloy.Services.FrontPortal.Controllers
{
    [Controller]
    public class DeploymentsController : Controller
    {
        private readonly IDeploymentHandler _handler;

        public DeploymentsController(IDeploymentHandler handler)
        {
            _handler = handler;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            ViewData.Model = _handler.ListAll(User);
            return View();
        }
        
        [HttpGet]
        public IActionResult Details(Guid guid)
        {
            try
            {
                ViewData.Model = _handler.GetDetails(guid);
                return View();
            }
            catch (Exception ex)
            {
//                throw;
                return View("Error", new ErrorViewModel() {Exception = ex});
            }
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DeploymentCreateViewModel deployment)
        {
            if (!ModelState.IsValid)
                return View();
            
            var newDeployment = _handler.CreateDeployment(deployment, User);
            return CreateRedirect(newDeployment);
        }

        private IActionResult CreateRedirect(Deployment deployment)
        {
            return View("CreateRedirect", _handler.GenerateWebhookParams(deployment));
        }
        
        [HttpGet]
        public IActionResult Edit(Guid guid)
        {
            try
            {
                ViewData.Model = _handler.ViewDeployment(guid);
                return View();
            }
            catch (Exception ex)
            {
//                throw;
                return View("Error", new ErrorViewModel() {Exception = ex});
            }
        }

        [HttpPost]
        public IActionResult Edit(DeploymentViewModel deployment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                
                _handler.UpdateDeployment(deployment);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
//                throw;
                return View("Error", new ErrorViewModel() {Exception = ex});
            }
        }

        [HttpGet]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                _handler.DeleteDeployment(guid);
                ViewBag.AlertSuccess = $"Deployment with guid: {guid} deleted";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
//                throw;
                return View("Error", new ErrorViewModel() {Exception = ex});
            }
        }
    }
}