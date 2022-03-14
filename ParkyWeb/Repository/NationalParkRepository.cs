
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ParkyWeb.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
