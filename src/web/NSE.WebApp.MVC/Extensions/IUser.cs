using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Extensions
{
    public interface IUser
    {
        string Name { get; }

        Guid GetUserId();

        string GetUserEmail();

        string GetUserToken();

        bool IsAuthenticated();

        bool HasRole(string role);

        IEnumerable<Claim> GetClaims();

        HttpContext GetHttpContext();
    }
}
