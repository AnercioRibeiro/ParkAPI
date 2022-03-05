﻿using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(p => p.NationalPark).FirstOrDefault(a => a.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(p => p.NationalPark).OrderBy(a=>a.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(a=>a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(a=>a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return _db.Trails.Include(p=>p.NationalPark).Where(p=>p.NationalParkId == nationalParkId).ToList();
        }
    }
}
