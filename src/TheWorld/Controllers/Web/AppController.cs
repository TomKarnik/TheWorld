using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IWorldRespository _repository;
        private ILogger<AppController> _logger;

        //private WorldContext _context; // <- Now using World Repository

        //public AppController(IMailService mailService, IConfigurationRoot config, WorldContext context)
        public AppController(IMailService mailService, IConfigurationRoot config, IWorldRespository repository,
            ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            //_context = context;
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //try
            //{
            //    //var data = _context.Trips.ToList();
            //    var data = _repository.GetAllTrips();
            //    return View(data);
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogCritical($"Error getting all trips in index page: {ex.Message}");
            //    return Redirect("/Index");
            //}
            return View();
        }

        [Authorize]
        public IActionResult Trips()
        {
            try
            {
                //var data = _context.Trips.ToList();
                var data = _repository.GetAllTrips();
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error getting all trips in index page: {ex.Message}");
                return Redirect("/Index");
            }
        }

        public IActionResult Contact()
        {
             return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("", "We don't support AOL addresses!");
            }

            if (ModelState.IsValid)
            { 
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From theWorld", model.Message);

                // Show message sent and clear the form
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent!";
            }

            return View();
        }
  
        public IActionResult About()
        {
            return View();
        }
    }
}
