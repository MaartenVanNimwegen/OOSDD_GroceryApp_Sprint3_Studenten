using Grocery.Core.Exceptions;
using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Xml.Linq;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;
        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }
        public Client? Login(string email, string password)
        {
            Client? client = _clientService.Get(email);
            if (client == null) return null;
            if (PasswordHelper.VerifyPassword(password, client.Password)) return client;
            return null;
        }

        public Client Register(string email, string password, string name)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password)) throw new ArgumentException();

            if (_clientService.Get(email.Trim()) != null) throw new UsedEmailException();

            if (!EmailHelper.IsValidEmail(email)) throw new InvalidEmailException();

            if (!PasswordHelper.IsStrongPassword(password)) throw new InvalidPasswordException();

            Client c = _clientService.Create(email.Trim(), PasswordHelper.HashPassword(password), name.Trim());

            return c;
        }
    }
}
