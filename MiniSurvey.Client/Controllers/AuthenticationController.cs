using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MiniSurvey.Client.Dto;
using MiniSurvey.Client.Helpers;
using MiniSurvey.Client.Helpers.Security;
using MiniSurvey.Client.Models;
using Newtonsoft.Json;

namespace MiniSurvey.Client.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly IJwtFactory _jwtFactory;
        private readonly IOptions<JwtIssuerOptions> _jwtOptions;

        public AuthenticationController(ICryptographyService cryptographyService, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IWebHostEnvironment env, IHttpContextAccessor httpContext, IMemoryCache memoryCache) : base(env, httpContext, memoryCache)
        {
            _cryptographyService = cryptographyService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions;
        }

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<RegisteredUserResponse>))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            Response<RegisteredUserResponse> response = new Response<RegisteredUserResponse>();

            try
            {
                if (!ModelState.IsValid)
                {
                    var requestResponse = ApiResponseFormatter.RequestResponse(ModelState);
                    return BadRequest(requestResponse);
                }

                User userCredentials;
                DateTime dateRegistered;
                using (var _context = new MiniSurveyContext())
                {
                    userCredentials = await _context.Users.Where(x => x.EmailAddress == request.Email.Trim())
                        .FirstOrDefaultAsync();
                }

                if (userCredentials == null)
                {
                    response = new Response<RegisteredUserResponse>
                    {
                        ResponseBody = new SuccessResponse<RegisteredUserResponse>
                        {
                            Data = null,
                            ResponseCode = "E001",
                            ResponseMessage = "Your email and password combination was incorrect, kindly try again later."
                        }
                    };
                    return Unauthorized(response.ResponseBody);
                }

                bool isPassword = userCredentials != null && _cryptographyService.ValidateHash(request.Password, userCredentials.PasswordSalt, userCredentials.PasswordHash);
                if (!isPassword)
                {
                    response = new Response<RegisteredUserResponse>
                    {
                        ResponseBody = new SuccessResponse<RegisteredUserResponse>
                        {
                            Data = null,
                            ResponseCode = "E001",
                            ResponseMessage = "Your email and password combination was incorrect, kindly try again later."
                        }
                    };
                    return Unauthorized(response.ResponseBody);
                }

                var identity = _jwtFactory.GenerateClaimsIdentity(userCredentials.EmailAddress, userCredentials.Id.ToString());
                var jwtToken = await ValueGenerator.GenerateJwt(identity, _jwtFactory, userCredentials.EmailAddress, _jwtOptions.Value, new JsonSerializerSettings { Formatting = Formatting.None });

                // deserialize generated auth token to be passed to client application.
                var authToken = JsonConvert.DeserializeObject<Token>(jwtToken);

                response = new Response<RegisteredUserResponse>
                {
                    ResponseBody = new SuccessResponse<RegisteredUserResponse>
                    {
                        Data = new RegisteredUserResponse { User = new Dto.UserResponse { Email = userCredentials.EmailAddress, Name = userCredentials.Name, DateRegistered = userCredentials.DateRegistered }, Role = new DefaultResponse { Id = userCredentials.RoleId, Value = HelperFunctions.GetRole(userCredentials.RoleId) } },
                        ResponseCode = "00",
                        ResponseMessage = "You have been successfully enrolled to participate in the survey."
                    }
                };


                return Ok(response.ResponseBody);
            }
            catch (Exception)
            {
                response = new Response<RegisteredUserResponse>
                {
                    ResponseBody = new SuccessResponse<RegisteredUserResponse>
                    {
                        Data = null,
                        ResponseCode = "E001",
                        ResponseMessage = "Sorry, we are unable to process your request at the moment, kindly try again later."
                    }
                };
                return StatusCode(500, response.ResponseBody);
            }

        }
    }
}
