using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Users
{
    public class UpdateUser
    {
        public class Command : IRequest<AppUserDto>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime BirthDate { get; set; }
            public string IdNumber { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Address).NotEmpty();
                RuleFor(x => x.PhoneNumber).NotEmpty();
                RuleFor(x => x.BirthDate).NotEmpty();
                RuleFor(x => x.IdNumber).NotEmpty().Length(12);
            }
        }

        public class Handler : IRequestHandler<Command, AppUserDto>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public Handler(DataContext context, UserManager<AppUser> userManager, IUserAccessor userAccessor,
                IMapper mapper)
            {
                _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

            public async Task<AppUserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var userInfo = await _userManager.Users
                    .Include(x => x.Doctor)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserUserName(), cancellationToken);

                userInfo.Address = request.Address;
                userInfo.FirstName = request.FirstName;
                userInfo.LastName = request.LastName;
                userInfo.Email = request.Email;
                userInfo.PhoneNumber = request.PhoneNumber;
                userInfo.BirthDate = request.BirthDate;
                userInfo.IdNumber = request.IdNumber;

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<AppUser, AppUserDto>(userInfo);
            }
        }
    }
}