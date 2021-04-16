using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Security;
using System.Security.Claims;
using System.Text;
using AutoMapper.Mappers;
using Microsoft.IdentityModel.Tokens;

namespace MusicApi.Authentication
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, string password);
    }

    class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string Key;
        private readonly IDictionary<string, string> _users = new Dictionary<string, string>()
        {
            {"test1", "pass1"}, {"test2", "pass1"}
        };

        public JwtAuthenticationManager(string key)
        {
            Key = key;
        }
        
        public string Authenticate(string username, string password)
        {
            if (!_users.Any(u => u.Key == username && u.Value == password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}