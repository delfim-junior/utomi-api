using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users
{
    public class GetLoggedUser
    {
        public class Query : IRequest<AppUserDto>
        {
        }

        public class QueryHandler : IRequestHandler<Query, AppUserDto>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public QueryHandler(UserManager<AppUser> userManager, IUserAccessor userAccessor, IMapper mapper)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

            public async Task<AppUserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var loggedUser = await _userManager.Users
                    .Include(x => x.Doctor)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserUserName(), cancellationToken);

                return _mapper.Map<AppUser, AppUserDto>(loggedUser);
            }
        }
    }
}