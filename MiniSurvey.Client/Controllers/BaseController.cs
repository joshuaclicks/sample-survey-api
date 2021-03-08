using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniSurvey.Client.Helpers;

namespace MiniSurvey.Client.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        public string RootPath { get; set; }

        public BaseController(IWebHostEnvironment env, IHttpContextAccessor httpContext, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            var httpContextAccessor = httpContext;
            RootPath = env.ContentRootPath;

            string userId = string.Empty;
            if (httpContextAccessor.HttpContext.User.Claims.ToList().Find(identity => identity.Type == "id") != null)
            {
                userId = httpContextAccessor.HttpContext.User.Claims.ToList().Find(identity => identity.Type == "id").Value;
                UserIdentity = Convert.ToInt64(userId);
            }
        }

        public long UserIdentity { get; set; }

        [NonAction]
        public bool IsAuthenticationFaulted(long userId)
        {
            if (userId < 1)
                return true;

            return false;
        }

    }
}
