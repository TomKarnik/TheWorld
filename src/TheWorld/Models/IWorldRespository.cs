using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRespository
    {
        IEnumerable<Trip> GetAllTrips();
        void AddTrip(Trip trip);
        Task<bool> SaveChangesAsync();
        Trip GetTripByName(string tripName, string username);
        void AddStop(string tripName, Stop newStop, string username);
        IEnumerable<Trip> GetUserTripsWithStops(string name);
    }
}