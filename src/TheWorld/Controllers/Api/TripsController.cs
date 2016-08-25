using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private ILogger<TripsController> _logger;
        private IWorldRespository _repository;

        public TripsController(IWorldRespository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        //public JsonResult Get()
        public IActionResult Get()
        {
            //return Json(new Trip { Name = "My Trip" });
            //return Ok(new Trip { Name = "My Trip" });
            //return Ok(_repository.GetAllTrips()); // <- Now going to return a collection of TripViewModels' instead of Trips
            try
            {
                //var results = _repository.GetAllTrips();
                var results = _repository.GetUserTripsWithStops(User.Identity.Name);

                return Ok(Mapper.Map<IEnumerable<Trip/*ViewModel*/>>(results));
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Failed to get all trips: {ex}");
                return BadRequest("Error Occurred!");
            }
         }

        [HttpPost("")]
        //public IActionResult Post([FromBody]Trip theTrip)
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save Trip not TripViewModel to database!
                // LET'S USE AUTOMAPPER INSTEAD!!! (NuGet package defined in project.json)
                //var newTrip = new Trip()
                //{
                //    Name = theTrip.Name,
                //    DateCreated = theTrip.Created
                //};
                var newTrip = Mapper.Map<Trip>(theTrip);
                newTrip.UserName = User.Identity.Name;

                // Save new trip to database
                _repository.AddTrip(newTrip);
                if (await _repository.SaveChangesAsync())
                {
                    //return Created($"api/trips/{theTrip.Name}", newTrip); // <- using newTrip (Trip) instead of theTrip (TripViewModel)
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip)); // <- Returning TripViewModel instead of exposing Trip
                }
            }

            //return Ok(true);
            //return BadRequest("Bad data!");
            return BadRequest("Failed to save the trip!");
        }
    }
}
