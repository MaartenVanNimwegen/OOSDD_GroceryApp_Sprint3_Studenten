using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Grocery.UnitTests.Mocks
{
    internal class MockClientRepository : IClientRepository
    {
        private readonly List<Client> clientList;
        public MockClientRepository()
        {
            clientList = [
                new Client(1, "M. van Nimwegen", "maarten@gmail.com", "/EaRI9eg2d3NBSFvF1CsMw==.z3E66MoaYkUA1ZEYxtNLOMiH9cTOGicZVOieRBU20Bw="), // Password is "Test123!"
                new Client(2, "L.M. Van Koningsveld", "lotte@gmail.com", "/EaRI9eg2d3NBSFvF1CsMw==.z3E66MoaYkUA1ZEYxtNLOMiH9cTOGicZVOieRBU20Bw="), // Password is "Test123!"
            ];
        }
        public Client? Get(string email)
        {
            Client? client = clientList.FirstOrDefault(c => c.EmailAddress.Equals(email));
            return client;
        }
        public Client? Get(int id)
        {
            Client? client = clientList.FirstOrDefault(c => c.Id == id);
            return client;
        }

        public void Add(Client client)
        {
            clientList.Add(client);
        }

        public List<Client> GetAll()
        {
            return clientList;
        }
    }
}