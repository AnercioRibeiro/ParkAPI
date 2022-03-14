using ParkyWeb.Repository.IRepository;
using System.Net.Http;

namespace ParkyWeb.Repository
{
    public class UnitOfWork
    {
        private readonly IHttpClientFactory _clientFactory;
        public UnitOfWork(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            Trail = new TrailRepository(_clientFactory);
            NationalPark = new NationalParkRepository(_clientFactory);
        }
        public ITrailRepository Trail { get; private set; }
        public INationalParkRepository NationalPark { get; private set; }

        //public void Dispose()
        //{
        //    _db.Dispose();
        //}
        //public void Save()
        //{
        //    _db.SaveChanges();
        }
    }
