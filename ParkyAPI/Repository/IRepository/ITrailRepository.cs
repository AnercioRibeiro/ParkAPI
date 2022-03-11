using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Repository.IRepository
{
    public interface ITrailRepository : IRepository<Trail>
    {
        ICollection<Trail> GetTrails();
        ICollection<Trail> GetTrailInNationalPark(int nationalParkId);
        Trail GetTrail(int trailId);
        bool TrailExists(string name);
        bool TrailExists(int id);
        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(Trail trail);
        bool Save();


    }
}
