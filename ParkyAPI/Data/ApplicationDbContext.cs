
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Models;

namespace ParkyAPI.Data
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public Microsoft.EntityFrameworkCore.DbSet<NationalPark> NationalParks { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Trail> Trails { get; set; }
    }
}
