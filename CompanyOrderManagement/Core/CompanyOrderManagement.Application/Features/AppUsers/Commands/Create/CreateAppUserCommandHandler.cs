using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Users;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Users;
using CompanyOrderManagement.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Create
{
    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommandRequest, ApiResponse<CreateAppUserCommandResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IUserBusinessRules _userBusinessRules;

        public CreateAppUserCommandHandler(UserManager<AppUser> userManager, IMapper mapper, ILoggerService logger, IUserBusinessRules userBusinessRules)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<ApiResponse<CreateAppUserCommandResponse>> Handle(CreateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {

                _userBusinessRules.EnsurePasswordsMatch(request.Password, request.PasswordConfirm);

                var user = _mapper.Map<AppUser>(request);
                user.Id = Guid.NewGuid();
                IdentityResult result = await _userManager.CreateAsync(user, request.Password);


                if (result.Succeeded)
                {
                    _logger.Info(UserLogMessage.SuccessfullyCreated(request.UserName));
                    var response = _mapper.Map<CreateAppUserCommandResponse>(user);
                    return ApiResponse<CreateAppUserCommandResponse>.Success(response);
                }
                else
                {
                    var error = new ErrorDTO(result.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList(), true);
                    return ApiResponse<CreateAppUserCommandResponse>.Fail(error);
                }
            }
            catch(Exception ex)
            {
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("CreateError", ex.Message) }, true);
                return ApiResponse<CreateAppUserCommandResponse>.Fail(error);
            }       
          
            }
        }
        
       
    }

