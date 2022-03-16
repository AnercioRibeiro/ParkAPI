using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public User Authenticate(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public bool IsUniqueUser(string username)
        {
            throw new System.NotImplementedException();
        }

        public User Register(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
