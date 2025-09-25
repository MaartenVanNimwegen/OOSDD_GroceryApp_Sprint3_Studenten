using Grocery.Core.Exceptions;
using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Services;
using Grocery.UnitTests.Mocks;
using Xunit;

namespace TestCore;

public class AuthServiceTests
{
    private IAuthService _authService;

    private const string _validUsedEmail = "maarten@gmail.com";
    private const string _validUnusedEmail = "lotje@gmail.com";
    private const string _validPassword = "Test123!";
    private const string _validName = "Maarten van Nimwegen";

    [SetUp]
    public void Setup()
    {
        IClientRepository clientRepository = new MockClientRepository();
        IClientService clientService = new ClientService(clientRepository);
        _authService = new AuthService(clientService);
    }

    [Test]
    public void Login_ExistingCredentials_ReturnsClient()
    {
        var client = _authService.Login(_validUsedEmail, _validPassword);
        Assert.NotNull(client);
        Assert.AreEqual(_validUsedEmail, client?.EmailAddress);
    }

    [Test]
    public void Login_NonExistingEmail_ReturnsNull()
    {
        var client = _authService.Login(_validUnusedEmail, _validPassword);
        Assert.Null(client);
    }

    [Test]
    public void Login_WrongPassword_ReturnsNull()
    {
        var client = _authService.Login(_validUsedEmail, "WrongPassword1!");
        Assert.Null(client);
    }

    [Test]
    public void Register_NewClient_ReturnsClient()
    {
        var client = _authService.Register(_validUnusedEmail, _validPassword, _validName);
        Assert.NotNull(client);
        Assert.AreEqual(_validUnusedEmail, client?.EmailAddress);
        Assert.AreEqual(_validName, client?.name);
    }

    [Test]
    public void Register_ExistingEmail_ReturnsNull()
    {
        var ex = Record.Exception(() => _authService.Register(_validUsedEmail, _validPassword, _validName));
        Assert.True(ex is UsedEmailException);
    }

    [Test]
    public void Register_WeakPassword_ReturnsNull()
    {
        var ex = Record.Exception(() => _authService.Register(_validUnusedEmail, "weak", _validName));
        Assert.True(ex is InvalidPasswordException);
    }

    [Test]
    public void PasswordHelper_HashAndVerifyPassword_WorksCorrectly()
    {
        var password = "StrongPass1!";
        var hashed = PasswordHelper.HashPassword(password);
        Assert.IsTrue(PasswordHelper.VerifyPassword(password, hashed));
        Assert.IsFalse(PasswordHelper.VerifyPassword("WrongPass1!", hashed));
    }

    [Test]
    public void PasswordHelper_IsStrongPassword_WorksCorrectly()
    {
        Assert.IsTrue(PasswordHelper.IsStrongPassword("StrongPass1!"));
        Assert.IsFalse(PasswordHelper.IsStrongPassword("weak"));
        Assert.IsFalse(PasswordHelper.IsStrongPassword("NoDigits!"));
        Assert.IsFalse(PasswordHelper.IsStrongPassword("nouppercase1!"));
        Assert.IsFalse(PasswordHelper.IsStrongPassword("NOLOWERCASE1!"));
        Assert.IsFalse(PasswordHelper.IsStrongPassword("NoSpecialChar1"));
    }

    [Test]
    public void Register_InvalidEmail_ThrowsException()
    {
        var ex = Record.Exception(() => _authService.Register("invalidemail", _validPassword, _validName));
        Assert.True(ex is InvalidEmailException);
    }
}
