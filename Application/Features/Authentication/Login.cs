using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Errors;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Authentication
{
    public class Login
    {
        public class Command : IRequest<LoginResponse>
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, LoginResponse>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            private readonly SignInManager<AppUser> _signInManager;

            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator,
                IUserAccessor userAccessor, SignInManager<AppUser> signInManager)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
                _signInManager = signInManager;
            }

            public async Task<LoginResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users
                    .Include(x => x.Doctor)
                    .FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

                if (user == null)
                {
                    throw new ApiException(HttpStatusCode.Unauthorized, "Failed to login");
                }
                
                
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    return new LoginResponse
                    {
                        Token = _jwtGenerator.Generate(user),
                        Email = user.Email,
                        UserName = user.UserName,
                        Doctor = user.Doctor
                    };
                }


                throw new  ApiException(HttpStatusCode.InternalServerError, "Problem Signing");
            }
        }
    }
}