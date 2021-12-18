using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Features.Users
{
    public class CreateUser
    {
        public class Command : IRequest<AppUserDto>
        {
            public string Gender { get; set; }
            public DateTime BirthDate { get; set; }
            public string Address { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string IdNumber { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Gender);
                RuleFor(x => x.BirthDate);
                RuleFor(x => x.FirstName);
                RuleFor(x => x.LastName);
                RuleFor(x => x.Email);
                RuleFor(x => x.PhoneNumber);
            }
        }

        public class Handler : IRequestHandler<Command, AppUserDto>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;

            public Handler(DataContext context, UserManager<AppUser> userManager, IMapper mapper)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<AppUserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var appUser = new AppUser
                {
                    Address = request.Address,
                    Email = request.Email,
                    Gender = request.Gender.ToLower().Contains("f") ? Sex.Female.ToString() : Sex.Male.ToString(),
                    BirthDate = request.BirthDate,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    UserName = $"{request.FirstName.ToLower()}.{request.LastName.ToLower()}",
                    IdNumber = request.IdNumber
                };

                var result = await _userManager.CreateAsync(appUser, "1Secure*Password1");

                if (result.Succeeded)
                {
                    return _mapper.Map<AppUser, AppUserDto>(appUser);
                }

                throw new Exception(result.Errors.ToString());
            }
        }
    }
}