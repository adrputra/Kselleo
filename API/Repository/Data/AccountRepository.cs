using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, int>

    {
        public IConfiguration configuration;
        public AccountRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            this.myContext = myContext;
            this.configuration = configuration;
        }

        public int Register(RegisterVM registerVM)
        {
            var regUser = new User
            {
                FullName = registerVM.FullName,
                Email = registerVM.Email,
                Gender = registerVM.Gender,
                Image = "Ini Foto"
            };

            var cekEmail = myContext.Users.Any(e => e.Email == regUser.Email);
            if (cekEmail)
            {
                return 1;
            }
            else
            {
                myContext.Users.Add(regUser);
                myContext.SaveChanges();

                var regAccount = new Account
                {
                    UserId = myContext.Users.SingleOrDefault(e => e.Email == registerVM.Email).Id,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password, BCrypt.Net.BCrypt.GenerateSalt(12)),
                    Role = "User"
                };

                myContext.Accounts.Add(regAccount);
                myContext.SaveChanges();
                return 0;
            }
        }

        public string Login(LoginVM loginVM)
        {
            var checkEmail = myContext.Users.SingleOrDefault(e => e.Email == loginVM.Email);
            if (checkEmail != null)
            {
                var checkPassword = myContext.Accounts.SingleOrDefault(e => e.UserId == checkEmail.Id);
                if (checkPassword != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(loginVM.Password, checkPassword.Password))
                    {
                        var roles = (from user in myContext.Users
                                     join acc in myContext.Accounts on user.Id equals acc.UserId
                                     where user.Email == loginVM.Email
                                     select new
                                     {
                                         role = acc.Role
                                     });
                        var claims = new List<Claim>();
                        claims.Add(new Claim("Id", checkEmail.Id.ToString()));
                        claims.Add(new Claim("Email", loginVM.Email));
                        claims.Add(new Claim("Fullname", checkEmail.FullName));
                        claims.Add(new Claim("Image", checkEmail.Image));
                        foreach (var item in roles)
                        {
                            claims.Add(new Claim("Roles", item.role));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                                    configuration["Jwt:Issuer"],
                                    configuration["Jwt:Audience"],
                                    claims,
                                    expires: DateTime.UtcNow.AddMinutes(60),
                                    signingCredentials: signIn
                                    );
                        var idToken = new JwtSecurityTokenHandler().WriteToken(token);
                        claims.Add(new Claim("TokenSecurity", idToken.ToString()));
                        return idToken;
                    }
                    else
                    {
                        return "1";
                    }
                }
                else
                {
                    return "3";
                }
            }
            else
            {
                return "2";
            }
        }

    }
}
