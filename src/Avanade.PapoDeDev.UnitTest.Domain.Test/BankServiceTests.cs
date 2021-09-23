using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Entities;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Account.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Services;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Entities;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.Interfaces.Repositories;
using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Customer.ValueObject;
using Avanade.PapoDeDev.UnitTest.Domain.Logging;
using Avanade.PapoDeDev.UnitTest.Domain.Test.Fixture;
using Avanade.PapoDeDev.UnitTest.Infra.HttpService;
using FluentAssertions;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BankingTests
{
    public class BankServiceTests
    {
        private BankService _bankService;

        private CustomerFixture _customerFixture;

        private Mock<IBankRepository> _bankRepositoryMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<ValidateCustomerRegistry> _validateCustomerRegistryMock;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private HttpClient _httpClient;

        private Mock<IBankLogger> _iloogerMock;

        public BankServiceTests()
        {
            _customerFixture = new CustomerFixture();

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:7071")
            };

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("code", "5SHVXi/oxtVX2p4/IYReeupsko4wzrHpi1CVDi37TXu9WUSFSfgdww==");

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock.Setup(h => h.CreateClient(It.IsAny<string>())
            ).Returns(_httpClient);

            _iloogerMock = new Mock<IBankLogger>();
        }

        [Fact(DisplayName = "AddAccount_WhenIsValid_ShallRequestAddNewAccount")]
        public async Task AddAccount_WhenIsValid_ShallRequestAddNewAccount()
        {
            //Arrange
            var account = new Account(
                id: "123",
                customer: new Customer(
                    name: "Customer A",
                    document: new Document(
                        DocumentType.CPF,
                        number: "1111111111")),
            balance: 100.00m);

            var account2 = new Account(
                id: "123",
                customer: _customerFixture.GenerateValidCustomer(),
            balance: 100.00m);

            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerRepositoryMock.Setup(c => c.FindCustomerByDocumentAsync(It.IsAny<Document>()))
                .Returns(Task.FromResult((Customer)null));

            //Call the Mock
            _validateCustomerRegistryMock = new Mock<ValidateCustomerRegistry>(new[] { _httpClientFactoryMock.Object });
            _validateCustomerRegistryMock.Setup(m => m.ValidateCustomerAsync(It.Is<string>(d => d == account.Customer.Document.Number)))
                .ReturnsAsync(true);

            //Call the real http
            var _validateCustomerRegistryReal = new ValidateCustomerRegistry(_httpClientFactoryMock.Object);

            //Call the real
            var _validateCustomerRegistryMock2 = new ValidateCustomerRegistry(_httpClientFactoryMock.Object);

            _bankRepositoryMock = new Mock<IBankRepository>();
            _bankRepositoryMock.Setup(b => b.InsertAccountAsync(account))
                .Returns(Task.CompletedTask);

            //dont need create a Mock - is not usage
            _accountRepositoryMock = new Mock<IAccountRepository>();

            _bankService = new BankService(
                _bankRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _iloogerMock.Object,
                _validateCustomerRegistryMock.Object); // Change between _validateCustomerRegistryMock.Object and _validateCustomerRegistryReal

            //Act
            var actual = await _bankService.AddAccountAsync(account);

            //Assert
            actual.Should().Contain($"Customer {account.Customer.Name} created");

            _bankRepositoryMock.Verify(b => b.InsertAccountAsync(It.IsAny<Account>()),
                times: Times.Once);
        }

        [Fact(DisplayName = "AddAccount_WhenIsNotValid_ShallNotRequestAddNewAccount")]
        public async Task AddAccount_WhenIsNotValid_ShallNotRequestAddNewAccount()
        {
            //Arrange
            var account = new Account(
                id: "123",
                customer: new Customer(
                    name: "Customer A",
                    document: new Document(
                        DocumentType.CPF,
                        number: "222")),
            balance: 100.00m);

            var account2 = new Account(
                id: "123",
                customer: _customerFixture.GenerateInvalidCustomer(),
            balance: 100.00m);

            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerRepositoryMock.Setup(c => c.FindCustomerByDocumentAsync(It.IsAny<Document>()))
                .Returns(Task.FromResult(new Customer(
                    name: "Customer A",
                    document: new Document(
                        DocumentType.CPF,
                        number: "222"))));

            //Call the Mock
            _validateCustomerRegistryMock = new Mock<ValidateCustomerRegistry>(new[] { _httpClientFactoryMock.Object });
            _validateCustomerRegistryMock.Setup(m => m.ValidateCustomerAsync(It.Is<string>(d => d == account.Customer.Document.Number)))
                .ReturnsAsync(true);

            //Call the real http
            var _validateCustomerRegistryReal = new ValidateCustomerRegistry(_httpClientFactoryMock.Object);

            _bankRepositoryMock = new Mock<IBankRepository>();
            _bankRepositoryMock.Setup(b => b.InsertAccountAsync(account))
                .Returns(Task.CompletedTask);

            //dont need create a Mock - is not usage
            _accountRepositoryMock = new Mock<IAccountRepository>();

            _bankService = new BankService(
                _bankRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _iloogerMock.Object,
                _validateCustomerRegistryMock.Object); // Change between _validateCustomerRegistryMock.Object and _validateCustomerRegistryReal

            //Act
            var actual = await _bankService.AddAccountAsync(account);

            //Assert
            actual.Should().Contain($"Customer {account.Customer.Name} exists with document {account.Customer.Document.Number}");

            _bankRepositoryMock.Verify(b => b.InsertAccountAsync(It.IsAny<Account>()),
                times: Times.Never);
        }

        [Fact(DisplayName = "AddAccount_WhenDocumentIsNotValid_ShallNotRequestAddNewAccount")]
        public async Task AddAccount_WhenDocumentIsNotValid_ShallNotRequestAddNewAccount()
        {
            //Arrange
            var account = new Account(
                id: "123",
                customer: new Customer(
                    name: "Customer A",
                    document: new Document(
                        DocumentType.CPF,
                        number: "1111111112")), //last digit is 2 = invalid
            balance: 100.00m);

            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerRepositoryMock.Setup(c => c.FindCustomerByDocumentAsync(It.IsAny<Document>()))
                .ReturnsAsync((Customer)default);

            //Call the Mock
            _validateCustomerRegistryMock = new Mock<ValidateCustomerRegistry>(new[] { _httpClientFactoryMock.Object });
            _validateCustomerRegistryMock.Setup(m => m.ValidateCustomerAsync(It.Is<string>(d => d == account.Customer.Document.Number)))
                .ReturnsAsync(false);

            //Call the real http
            var _validateCustomerRegistryReal = new ValidateCustomerRegistry(_httpClientFactoryMock.Object);

            _bankRepositoryMock = new Mock<IBankRepository>();
            _bankRepositoryMock.Setup(b => b.InsertAccountAsync(account))
                .Returns(Task.CompletedTask);

            //dont need create a Setup - is not usage
            _accountRepositoryMock = new Mock<IAccountRepository>();

            _bankService = new BankService(
                _bankRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _iloogerMock.Object,
                _validateCustomerRegistryMock.Object); // Change between _validateCustomerRegistryMock.Object and _validateCustomerRegistryReal

            //Act
            var actual = await _bankService.AddAccountAsync(account);

            //Assert
            actual.Should().Contain($"Customer {account.Customer.Name} is not valid in external document validate");

            _bankRepositoryMock.Verify(b => b.InsertAccountAsync(It.IsAny<Account>()),
                times: Times.Never);
        }

        [Fact(DisplayName = "AddAccount_WhenDocumentIsNotValid_ShallNotRequestAddNewAccount")]
        public async Task AddAccount_WhenDocumentIsValid_ShallNotRequestAddNewAccount()
        {
            //Arrange
            var account = new Account(
                id: "123",
                customer: new Customer(
                    name: "Customer A",
                    document: new Document(
                        DocumentType.CPF,
                        number: "1111111111")), //last digit is 1 = Valid
            balance: 100.00m);

            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerRepositoryMock.Setup(c => c.FindCustomerByDocumentAsync(It.IsAny<Document>()))
                .ReturnsAsync(default(Customer));

            //Call the Mock
            _validateCustomerRegistryMock = new Mock<ValidateCustomerRegistry>(new[] { _httpClientFactoryMock.Object });
            _validateCustomerRegistryMock.Setup(m => m.ValidateCustomerAsync(It.Is<string>(d => d == account.Customer.Document.Number)))
                .ReturnsAsync(true);

            //Call the real http
            var _validateCustomerRegistryReal = new ValidateCustomerRegistry(_httpClientFactoryMock.Object);

            _bankRepositoryMock = new Mock<IBankRepository>();
            _bankRepositoryMock.Setup(b => b.InsertAccountAsync(account))
                .Returns(Task.CompletedTask);

            //dont need create a Mock - is not usage
            _accountRepositoryMock = new Mock<IAccountRepository>();

            _bankService = new BankService(
                _bankRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _iloogerMock.Object,
                _validateCustomerRegistryMock.Object); // Change between _validateCustomerRegistryMock.Object and _validateCustomerRegistryReal

            //Act
            var actual = await _bankService.AddAccountAsync(account);

            //Assert
            actual.Should().Contain($"Customer {account.Customer.Name} created");

            _bankRepositoryMock.Verify(b => b.InsertAccountAsync(It.IsAny<Account>()),
                times: Times.Once);
        }

        // Needs to create the bellow tests and other to Deposit and WithDraw

        //public async Task<string> DepositAsync_WhenIsValid_SallRequestDepositAsync()
        //{
        //    //Arrange

        //    //Act

        //    //Assert
        //}

        //public async Task<string> WithDrawAsync_WhenIsValid_SallRequestDepositAsync()
        //{
        //    //Arrange

        //    //Act

        //    //Assert
        //}
    }
}