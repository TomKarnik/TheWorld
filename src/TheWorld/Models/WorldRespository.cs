using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRespository : IWorldRespository
    {
        private WorldContext _context;
        private ILogger<WorldRespository> _logger;

        public WorldRespository(WorldContext context, ILogger<WorldRespository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetTripByName(tripName, username);

            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all trips from database.");

            return _context.Trips.ToList();
        }

        public Trip GetTripByName(string tripName, string username)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName && t.UserName == username)
                .FirstOrDefault();
        }

        public IEnumerable<Trip>GetUserTripsWithStops(string name)
        {
            try
            {
                //return _context.Trips.ToList();
                //return _context.Trips.Include(t => t.Stops).OrderBy(t => t.Name).ToList();
                return _context.Trips.Include(t => t.Stops).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get user trips with stops from database", ex);
                return null;
            }
            
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await (_context.SaveChangesAsync()) > 0;
        }
    }
}
