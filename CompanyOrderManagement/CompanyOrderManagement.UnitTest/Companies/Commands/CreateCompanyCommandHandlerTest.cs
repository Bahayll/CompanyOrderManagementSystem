using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Companies.ConstantMessages;
using CompanyOrderManagement.Application.Logging.LogMessages.Companies;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Sdk;

namespace CompanyOrderManagement.UnitTest.Companies.Commands
{
    public class CreateCompanyCommandHandlerTest
    {
        private readonly Mock<ICompanyUnitOfWork> _companyUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICompanyBusinessRules> _businessRulesMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly CreateCompanyCommandHandler _createCompanyCommandHandler;

        public CreateCompanyCommandHandlerTest()
        {
            _companyUnitOfWorkMock = new Mock<ICompanyUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _businessRulesMock = new Mock<ICompanyBusinessRules>();
            _cacheServiceMock = new Mock<ICacheService>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _createCompanyCommandHandler = new CreateCompanyCommandHandler(_companyUnitOfWorkMock.Object,_businessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task CreateCompanyCommandHandler_Handle_WhenCompanySuccessfullyCreated_ShouldReturnCreatedCompany()
        {
            // Arrange
            var cacheKey = CacheKeys.AllCompanies;
            var request = new CreateCompanyCommandRequest
            {
                Name = "Test Company",
                Description = "Test Description",
                Email = "Test@gmail.com"
            };
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Email = request.Email,
            };
            var response = new CreateCompanyCommandResponse
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                Email = company.Email,               
            };

            _businessRulesMock.Setup(cc => cc.EnsureUniqueCompanyNameAsync(It.Is<string>(name => name == request.Name))).Returns(Task.CompletedTask);

            _mapperMock.Setup(cc => cc.Map<Company>(It.Is<CreateCompanyCommandRequest>(r => r == request))).Returns(company);
            _companyUnitOfWorkMock.Setup(cc => cc.GetWriteRepository.AddAsync(It.Is<Company>(c => c == company))).ReturnsAsync(true);
            _companyUnitOfWorkMock.Setup(cc => cc.SaveAsync()).ReturnsAsync(1);

            _loggerServiceMock.Setup(cc => cc.Info(It.Is<string>(message => message == CompanyLogMessage.SuccessfullyCreated(request.Name))));

            _cacheServiceMock.Setup(cc => cc.Get<List<Company>>(It.Is<string>(key => key == cacheKey))).Returns(new List<Company>());
            _cacheServiceMock.Setup(x => x.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<Company>>(), It.IsAny<TimeSpan>(), null));

            _mapperMock.Setup(cc => cc.Map<CreateCompanyCommandResponse>(It.Is<Company>(c => c == company))).Returns(response);

            // Act
            var result = await _createCompanyCommandHandler.Handle(request, CancellationToken.None);


            // Assert
            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success,result.ResponseType);
            Assert.Equal(response.Id, result.Data.Id);
            Assert.Equal(response.Name, result.Data.Name);
            Assert.Equal(response.Description, result.Data.Description);
            Assert.Equal(response.Email, result.Data.Email);

            _businessRulesMock.Verify(cc => cc.EnsureUniqueCompanyNameAsync(It.Is<string>(name => name == request.Name)), Times.Once);
            _mapperMock.Verify(cc => cc.Map<Company>(It.Is<CreateCompanyCommandRequest>(r => r == request)), Times.Once);
            _companyUnitOfWorkMock.Verify(cc => cc.GetWriteRepository.AddAsync(It.Is<Company>(c => c == company)), Times.Once);
            _companyUnitOfWorkMock.Verify(cc => cc.SaveAsync(), Times.Once);
            _cacheServiceMock.Verify(cc => cc.Get<List<Company>>(It.Is<string>(key => key == cacheKey)), Times.Once);
            _cacheServiceMock.Verify(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<Company>>(), It.IsAny<TimeSpan>(), null), Times.Once);
            _loggerServiceMock.Verify(cc => cc.Info(It.Is<string>(msg => msg == CompanyLogMessage.SuccessfullyCreated(request.Name))), Times.Once);


        }
        [Fact]
        public async Task CreateCompanyCommandHandler_Handle_WhenCompanyNameAlreadyExists_ThrowsBusinessException()
        {
            // Arrange
            var errorMessage = CompanyMessages.CompanyNameExists;
            var request = new CreateCompanyCommandRequest
            {
                Name = "Existing Company",
                Description = "Test Description",
                Email = "test@gmail.com"
            };

            _businessRulesMock.Setup(c => c.EnsureUniqueCompanyNameAsync(request.Name)).ThrowsAsync(new BusinessException(errorMessage));

            
            // Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _businessRulesMock.Object.EnsureUniqueCompanyNameAsync(request.Name));

          
            Assert.Equal(exception.Message,errorMessage);

            _businessRulesMock.Verify(c => c.EnsureUniqueCompanyNameAsync(request.Name), Times.Once);
        }

        [Fact]
        public async Task CreateCompanyCommandHandler_Handle_WhenBusinessRuleFails_ReturnFailResponse()
        {
            // Arrange
            var errorMessage = CompanyMessages.CompanyNameExists;
            var request = new CreateCompanyCommandRequest
            {
                Name = "Existing Company",
                Description = "Test Description",
                Email = "test@gmail.com"
            };

            _businessRulesMock.Setup(c => c.EnsureUniqueCompanyNameAsync(request.Name)).ThrowsAsync(new BusinessException(errorMessage));

            // Act
            var result = await _createCompanyCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);

            _businessRulesMock.Verify(c => c.EnsureUniqueCompanyNameAsync(request.Name), Times.Once);
        }

    }


}
