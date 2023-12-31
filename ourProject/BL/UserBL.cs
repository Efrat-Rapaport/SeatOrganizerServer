﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//using AutoMapper.Configuration;
using DL;
using Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BL
{
    public class UserBL : IUserBL
    {
        IUserDL iuserdl;
        IConfiguration iconfiguration;
        IPasswordHashHelper ipasswordHashHelper;
        public UserBL(IUserDL iuserdl, IConfiguration iconfiguration, IPasswordHashHelper ipasswordHashHelper)
        {
            this.iuserdl = iuserdl;
            this.iconfiguration = iconfiguration;
            this.ipasswordHashHelper = ipasswordHashHelper;
        }

        public async Task PostBL(User user)
        {
            user.Salt = ipasswordHashHelper.GenerateSalt(8);
            user.Password = ipasswordHashHelper.HashPassword(user.Password, user.Salt, 1000, 8);
            await iuserdl.PostDL(user);
        }
        public static List<User> WithoutPasswords(List<User> users)
        {
            return users.Select(x => WithoutPassword(x)).ToList();
        }
        public static User WithoutPassword(User user)
        {
            user.Password = null;
            return user;
        }
        public async Task<User> GetByPassAndEmailBL(string email)
        {
            User user= await iuserdl.GetByPassAndEmailDL(email);
            if (user==null)
            {
                return null;
            }
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(iconfiguration.GetSection("key").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        public async Task PutBL(int id, User user)
        {
            user.Salt = ipasswordHashHelper.GenerateSalt(8);
            user.Password = ipasswordHashHelper.HashPassword(user.Password, user.Salt, 1000, 8);
            await iuserdl.PutDL(id, user);
        }
        public async Task DeleteBL(int id)
        {
            await iuserdl.DeleteDL(id);
        }

    }

}
