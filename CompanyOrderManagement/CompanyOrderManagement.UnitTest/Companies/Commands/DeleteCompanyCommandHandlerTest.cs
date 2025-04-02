using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Companies.Commands.Delete;
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
using Xunit;

namespace CompanyOrderManagement.UnitTest.Companies.Commands
{
    public class DeleteCompanyCommandHandlerTest
    {
        private readonly Mock<ICompanyUnitOfWork> _companyUnitOfWorkMock;
        private readonly Mock<ICompanyBusinessRules> _companyBusinessRulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly DeleteCompanyCommandHandler _deleteCompanyCommandHandler;

        public DeleteCompanyCommandHandlerTest()
        {
            _companyUnitOfWorkMock = new Mock<ICompanyUnitOfWork>();
            _companyBusinessRulesMock = new Mock<ICompanyBusinessRules>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _cacheServiceMock = new Mock<ICacheService>();
            _deleteCompanyCommandHandler = new DeleteCompanyCommandHandler(_companyUnitOfWorkMock.Object, _companyBusinessRulesMock.Object, _mapperMock.Object, _loggerServiceMock.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task DeleteCompanyCommandHandler_Handle_WhenCompanyDeleted_ShouldInvalidateCache()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new DeleteCompanyCommandRequest(id) { Id = id };
            var cacheKey = CacheKeys.AllCompanies;
            var company = new Company
            {
                Id = id,
                Name = "Test Company",
                Description = "Test Description",
                Email = "Test@gmail.com"

            };
            var cachedCompanies = new List<Company> { company };

            _companyBusinessRulesMock.Setup(cd => cd.EnsureCompanyIdExists(request.Id)).Returns(Task.CompletedTask);

            _companyUnitOfWorkMock.Setup(cd => cd.GetWriteRepository.RemoveAsync(request.Id)).ReturnsAsync(true);
            _companyUnitOfWorkMock.Setup(cd => cd.SaveAsync()).ReturnsAsync(1);

            _loggerServiceMock.Setup(cc => cc.Info(It.Is<string>(message => message == CompanyLogMessage.SuccessfullyDeleted(request.Id))));

            _cacheServiceMock.Setup(c => c.Get<List<Company>>(It.Is<string>(key => key == cacheKey))).Returns(cachedCompanies);
            _cacheServiceMock.Setup(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<Company>>(), It.IsAny<TimeSpan>(), null));


            // Act

            var result = await _deleteCompanyCommandHandler.Handle(request, CancellationToken.None);

            // Assert 
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _companyBusinessRulesMock.Verify(cd => cd.EnsureCompanyIdExists(request.Id), Times.Once);
            _companyUnitOfWorkMock.Verify(cd => cd.GetWriteRepository.RemoveAsync(request.Id), Times.Once);
            _companyUnitOfWorkMock.Verify(cd => cd.SaveAsync(), Times.Once);
            _loggerServiceMock.Verify(cc => cc.Info(It.Is<string>(message => message == CompanyLogMessage.SuccessfullyDeleted(request.Id))),Times.Once);

            _cacheServiceMock.Verify(c => c.Get<List<Company>>(It.Is<string>(key => key == cacheKey)), Times.Once);
            _cacheServiceMock.Verify(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<Company>>(), It.IsAny<TimeSpan>(), null), Times.Once);

        }

        [Fact]
        public async Task DeleteCompanyCommandHandler_Handle_WhenCompanyIdNotExists_ShouldThrowBusinessException()
        {
            // Arrange 
            var errorMessage = CompanyMessages.CompanyIdNotExists;
            var id = Guid.NewGuid();
            var request = new DeleteCompanyCommandRequest(id) { Id = id };

            _companyBusinessRulesMock.Setup(c => c.EnsureCompanyIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _companyBusinessRulesMock.Object.EnsureCompanyIdExists(request.Id));

            // Assert
            Assert.Equal(exception.Message,errorMessage);
            
            _companyBusinessRulesMock.Verify(c => c.EnsureCompanyIdExists(request.Id), Times.Once);
        }
        [Fact]
        public async Task DeleteCompanyCommandHandler_Handle_WhenCacheIsNull_ShouldNotThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new DeleteCompanyCommandRequest(id) { Id = id };
            var cacheKey = CacheKeys.AllCompanies;

            _companyBusinessRulesMock.Setup(cd => cd.EnsureCompanyIdExists(request.Id)).Returns(Task.CompletedTask);
            _companyUnitOfWorkMock.Setup(cd => cd.GetWriteRepository.RemoveAsync(request.Id)).ReturnsAsync(true);
            _companyUnitOfWorkMock.Setup(cd => cd.SaveAsync()).ReturnsAsync(1);

            _cacheServiceMock.Setup(c => c.Get<List<Company>>(It.Is<string>(key => key == cacheKey))).Returns((List<Company>)null);

            // Act
            var result = await _deleteCompanyCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _companyBusinessRulesMock.Verify(cd => cd.EnsureCompanyIdExists(request.Id), Times.Once);
            _cacheServiceMock.Verify(c => c.Get<List<Company>>(It.Is<string>(key => key == cacheKey)), Times.Once);
            _cacheServiceMock.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<List<Company>>(), It.IsAny<TimeSpan>(), null), Times.Never);
        }



        [Fact]
        public async Task DeleteCompanyCommandHandler_Handle_WhenBusinessRuleFails_ShouldReturnFailResponse()
        { 
            // Arrange
            var errorMessage = CompanyMessages.CompanyIdNotExists;
            var id = Guid.NewGuid();
            var request = new DeleteCompanyCommandRequest(id) { Id = id };

            _companyBusinessRulesMock.Setup(c => c.EnsureCompanyIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var result = await _deleteCompanyCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);

            _companyBusinessRulesMock.Verify(c => c.EnsureCompanyIdExists(request.Id), Times.Once);


        }


    }
}


