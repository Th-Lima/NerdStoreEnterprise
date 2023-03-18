using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Extensions
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AspNetUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Name => _contextAccessor.HttpContext.User.Identity.Name;

        public IEnumerable<Claim> GetClaims()
        {
            return _contextAccessor.HttpContext.User.Claims;
        }

        public HttpContext GetHttpContext()
        {
            return _contextAccessor.HttpContext;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _contextAccessor.HttpContext.User.GetClaimUserEmail() : "";
        }

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_contextAccessor.HttpContext.User.GetClaimUserId()) : Guid.Empty;
        }

        public string GetUserToken()
        {
            return IsAuthenticated() ? _contextAccessor.HttpContext.User.GetClaimUserToken() : "";
        }

        public bool HasRole(string role)
        {
            return _contextAccessor.HttpContext.User.IsInRole(role);
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
