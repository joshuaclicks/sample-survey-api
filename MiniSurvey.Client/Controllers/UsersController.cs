using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSurvey.Client.Dto;
using MiniSurvey.Client.Helpers;
using MiniSurvey.Client.Models;

namespace MiniSurvey.Client.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {

        }

        /// <summary>
        /// Registers a new user who wants to share their opinions in the survey exercise.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
      //  [ValidateAntiForgeryToken]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<RegisteredUserResponse>))]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
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
                        .Include(x => x.UserResponses)
                        .FirstOrDefaultAsync();
                }

                if (userCredentials == null)
                {
                    using (var _context = new MiniSurveyContext())
                    {
                        dateRegistered = DateTime.UtcNow;
                        _context.Users.Add(new Models.User { Name = request.Name, EmailAddress = request.Email.Trim(), DateRegistered = dateRegistered });
                        await _context.SaveChangesAsync();
                    }

                    userCredentials = new User { Name = request.Name, DateRegistered = dateRegistered, EmailAddress = request.Email };
                }

                response = new Response<RegisteredUserResponse>
                {
                    ResponseBody = new SuccessResponse<RegisteredUserResponse>
                    {
                        Data = new RegisteredUserResponse { User = new Dto.UserResponse { Email = userCredentials.EmailAddress, Name = userCredentials.Name, RegistrationDate = userCredentials.DateRegistered } },
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
