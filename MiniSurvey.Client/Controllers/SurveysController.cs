using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MiniSurvey.Client.Dto;
using MiniSurvey.Client.Enums;
using MiniSurvey.Client.Helpers;
using MiniSurvey.Client.Helpers.Security;
using MiniSurvey.Client.Models;

namespace MiniSurvey.Client.Controllers
{
    [Produces("application/json")]
    [Route("api/surveys")]
    [ApiController]
    public class SurveysController : BaseController
    {
        public SurveysController(IWebHostEnvironment env, IHttpContextAccessor httpContext, IMemoryCache memoryCache) : base(env, httpContext, memoryCache)
        {
        }

        /// <summary>
        /// To Get the List of Surveys and their details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<List<SurveyResponse>>))]
        public async Task<IActionResult> GetSurveys()
        {
            Response<List<SurveyDetail>> response = new Response<List<SurveyDetail>>();

            try
            {
                var isFaulted = IsAuthenticationFaulted(UserIdentity);
                if (isFaulted)
                {
                    response = new Response<List<SurveyDetail>>
                    {
                        ResponseBody = new SuccessResponse<List<SurveyDetail>>
                        {
                            Data = null,
                            ResponseCode = "E009",
                            ResponseMessage = "You do not have the permission to carry out this operation, kindly login and try again."
                        }
                    };

                    return Unauthorized(response.ResponseBody);
                }

                List<Survey> surveys;
                List<SurveyDetail> surveyDetails = new List<SurveyDetail>();
                using (var _context = new MiniSurveyContext())
                {
                    surveys = await _context.Surveys.ToListAsync();
                }

                if(surveys == null)
                {
                    response = new Response<List<SurveyDetail>>
                    {
                        ResponseBody = new SuccessResponse<List<SurveyDetail>>
                        {
                            Data = null,
                            ResponseCode = "00",
                            ResponseMessage = "There are no surveys yet, kindly try again later as we are currently working on it."
                        }
                    };

                    return Ok(response.ResponseBody);
                }

                foreach(var survey in surveys)
                {
                    SurveyDetail surveyDetail = new SurveyDetail
                    {
                        Id = survey.Id,
                        Title = survey.Title,
                        Description = survey.Description,
                        Status = survey.Status,
                        DateCreated = survey.DateCreated
                    };

                    surveyDetails.Add(surveyDetail);
                }
                
                response = new Response<List<SurveyDetail>>
                {
                    ResponseBody = new SuccessResponse<List<SurveyDetail>>
                    {
                        Data = surveyDetails,
                        ResponseCode = "00",
                        ResponseMessage = "You have been successfully fetched all surveys."
                    }
                };

                return Ok(response.ResponseBody);
            }
            catch (Exception)
            {
                response = new Response<List<SurveyDetail>>
                {
                    ResponseBody = new SuccessResponse<List<SurveyDetail>>
                    {
                        Data = null,
                        ResponseCode = "E001",
                        ResponseMessage = "Sorry, we are unable to fetch surveys at the moment, kindly try again later."
                    }
                };
                return StatusCode(500, response.ResponseBody);
            }

        }

        /// <summary>
        /// To get the details of a single survey
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<SurveyResponse>))]
        public async Task<IActionResult> GetSurvey(int id)
        {
            Response<SurveyDetail> response = new Response<SurveyDetail>();

            try
            {
                var isFaulted = IsAuthenticationFaulted(UserIdentity);
                if (isFaulted)
                {
                    response = new Response<SurveyDetail>
                    {
                        ResponseBody = new SuccessResponse<SurveyDetail>
                        {
                            Data = null,
                            ResponseCode = "E009",
                            ResponseMessage = "You do not have the permission to carry out this operation, kindly login and try again."
                        }
                    };

                    return Unauthorized(response.ResponseBody);
                }

                Survey survey;
                using (var _context = new MiniSurveyContext())
                {
                    survey = await _context.Surveys.Where(a => a.Id == id).FirstOrDefaultAsync();
                }

                if (survey == null)
                {
                    response = new Response<SurveyDetail>
                    {
                        ResponseBody = new SuccessResponse<SurveyDetail>
                        {
                            Data = null,
                            ResponseCode = "00",
                            ResponseMessage = "There are no surveys yet, kindly try again later as we are currently working on it."
                        }
                    };

                    return Ok(response.ResponseBody);
                }

                SurveyDetail surveyDetail = new SurveyDetail
                {
                    Id = survey.Id,
                    Title = survey.Title,
                    Description = survey.Description,
                    Status = survey.Status,
                    DateCreated = survey.DateCreated
                };

                response = new Response<SurveyDetail>
                {
                    ResponseBody = new SuccessResponse<SurveyDetail>
                    {
                        Data = surveyDetail,
                        ResponseCode = "00",
                        ResponseMessage = "You have been successfully fetched all surveys."
                    }
                };

                return Ok(response.ResponseBody);
            }
            catch (Exception)
            {
                response = new Response<SurveyDetail>
                {
                    ResponseBody = new SuccessResponse<SurveyDetail>
                    {
                        Data = null,
                        ResponseCode = "E001",
                        ResponseMessage = "Sorry, we are unable to fetch surveys at the moment, kindly try again later."
                    }
                };
                return StatusCode(500, response.ResponseBody);
            }

        }

        /// <summary>
        /// Creates a new Survey.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<SurveyResponse>))]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyRequest request)
        {
            Response<SurveyResponse> response = new Response<SurveyResponse>();

            try
            {
                var isFaulted = IsAuthenticationFaulted(UserIdentity);
                if (isFaulted)
                {
                    response = new Response<SurveyResponse>
                    {
                        ResponseBody = new SuccessResponse<SurveyResponse>
                        {
                            Data = null,
                            ResponseCode = "E009",
                            ResponseMessage = "You do not have the permission to carry out this operation, kindly login and try again."
                        }
                    };

                    return Unauthorized(response.ResponseBody);
                }

                if (!ModelState.IsValid)
                {
                    var requestResponse = ApiResponseFormatter.RequestResponse(ModelState);
                    return BadRequest(requestResponse);
                }

                Survey survey = new Survey
                {
                    DateCreated = DateTime.UtcNow,
                    Description = request.Description,
                    Status = SurveyStatus.Open.ToString(),
                    Title = request.Title
                };

                using (var _context = new MiniSurveyContext())
                {
                    _context.Surveys.Add(survey);
                    await _context.SaveChangesAsync();
                }

                SurveyResponse surveyResponse = new SurveyResponse
                {
                    Id = survey.Id,
                    Title = survey.Title,
                    DateCreated = survey.DateCreated,
                    Description = survey.Description,
                    Status = survey.Status
                };

                response = new Response<SurveyResponse>
                {
                    ResponseBody = new SuccessResponse<SurveyResponse>
                    {
                        Data = surveyResponse,
                        ResponseCode = "00",
                        ResponseMessage = "You have successfully created a survey. Kindly proceed to add questions to the survey."
                    }
                };


                return Ok(response.ResponseBody);
            }
            catch (Exception)
            {
                response = new Response<SurveyResponse>
                {
                    ResponseBody = new SuccessResponse<SurveyResponse>
                    {
                        Data = null,
                        ResponseCode = "E001",
                        ResponseMessage = "Sorry, we are unable to process your request at the moment, kindly try again later."
                    }
                };
                return StatusCode(500, response.ResponseBody);
            }

        }

        /// <summary>
        /// Updates the Survey information.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<SurveyResponse>))]
        public async Task<IActionResult> EditSurvey([FromBody] SurveyUpdateRequest request)
        {
            Response<SurveyResponse> response = new Response<SurveyResponse>();

            try
            {
                var isFaulted = IsAuthenticationFaulted(UserIdentity);
                if (isFaulted)
                {
                    response = new Response<SurveyResponse>
                    {
                        ResponseBody = new SuccessResponse<SurveyResponse>
                        {
                            Data = null,
                            ResponseCode = "E009",
                            ResponseMessage = "You do not have the permission to carry out this operation, kindly login and try again."
                        }
                    };

                    return Unauthorized(response.ResponseBody);
                }

                if (!ModelState.IsValid)
                {
                    var requestResponse = ApiResponseFormatter.RequestResponse(ModelState);
                    return BadRequest(requestResponse);
                }

                Survey survey;
                using (var _context = new MiniSurveyContext())
                {
                    survey = await _context.Surveys.Where(a => a.Id == request.Id).FirstOrDefaultAsync();
                    survey.Title = request.Title;
                    survey.Description = request.Description;
                    await _context.SaveChangesAsync();
                }

                SurveyResponse surveyResponse = new SurveyResponse
                {
                    Id = survey.Id,
                    Title = survey.Title,
                    DateCreated = survey.DateCreated,
                    Description = survey.Description,
                    Status = survey.Status
                };

                response = new Response<SurveyResponse>
                {
                    ResponseBody = new SuccessResponse<SurveyResponse>
                    {
                        Data = surveyResponse,
                        ResponseCode = "00",
                        ResponseMessage = "You have successfully updated the survey details."
                    }
                };


                return Ok(response.ResponseBody);
            }
            catch (Exception)
            {
                response = new Response<SurveyResponse>
                {
                    ResponseBody = new SuccessResponse<SurveyResponse>
                    {
                        Data = null,
                        ResponseCode = "E001",
                        ResponseMessage = "Sorry, we are unable to process your request at the moment, kindly try again later."
                    }
                };
                return StatusCode(500, response.ResponseBody);
            }

        }

        /// <summary>
        /// Serves the statistics of the surveyfor a particular survey participant
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("responses/{surveyId}")]
        //[ValidateAntiForgeryToken]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<SurveyStat>))]
        public async Task<IActionResult> SurveyResponses(int surveyId)
        {
            Response<SurveyStat> response = new Response<SurveyStat>();

            try
            {
                var isFaulted = IsAuthenticationFaulted(UserIdentity);
                if (isFaulted)
                {
                    response = new Response<SurveyStat>
                    {
                        ResponseBody = new SuccessResponse<SurveyStat>
                        {
                            Data = null,
                            ResponseCode = "E009",
                            ResponseMessage = "You do not have the permission to carry out this operation, kindly login and try again."
                        }
                    };

                    return Unauthorized(response.ResponseBody);
                }

                User userCredentials;
                using (var _context = new MiniSurveyContext())
                {
                    userCredentials = await _context.Users.Where(x => x.Id == UserIdentity)
                        .Include(x => x.UserResponses)
                        .FirstOrDefaultAsync();
                }

                if (userCredentials == null)
                {
                    response = new Response<SurveyStat>
                    {
                        ResponseBody = new SuccessResponse<SurveyStat>
                        {
                            Data = null,
                            ResponseCode = "E003",
                            ResponseMessage = "You have not registered so you cannot view the survey review or statistics. Kindly register to participate."
                        }
                    };

                    return Unauthorized(response.ResponseBody);
                }

                if (userCredentials.UserResponses.Count == 0)
                {
                    response = new Response<SurveyStat>
                    {
                        ResponseBody = new SuccessResponse<SurveyStat>
                        {
                            Data = null,
                            ResponseCode = "E005",
                            ResponseMessage = "You have not taken part in the survey and therefore cannot view the survey statistics. Kindly answer questions to participate."
                        }
                    };
                    return Ok(response.ResponseBody);
                }

                List<Models.UserResponse> userResponses = new List<Models.UserResponse>();
                List<Models.QuestionOption> questionOptions = new List<QuestionOption>();
                using (var _context = new MiniSurveyContext())
                {
                    userResponses = await _context.UserResponses.Where(a => a.SurveyId == surveyId).ToListAsync();
                    questionOptions = await _context.QuestionOptions.Include(a => a.Question).Include(a => a.Option).ToListAsync();

                }

                var totalUserIds = userResponses.Select(a => a.UserId).Distinct().ToList();
                var totalQuestionIds = userResponses.Select(a => a.QuestionId).Distinct().ToList();

                var totalUsersCount = totalUserIds.Count;
                var totalQuestionsCount = totalQuestionIds.Count;

                List<Poll> polls = new List<Poll>();
                foreach (var questionId in totalQuestionIds)
                {
                    var selectedQuestionOptions = questionOptions.Where(a => a.QuestionId == questionId);
                    var questionResponses = userResponses.Where(a => a.QuestionId == questionId);

                    var question = selectedQuestionOptions.FirstOrDefault().Question;

                    List<OpinionPoll> questionOpinions = new List<OpinionPoll>();
                    if (selectedQuestionOptions != null)
                    {
                        foreach (var selectedQuestionOption in selectedQuestionOptions)
                        {
                            var option = selectedQuestionOption.Option;
                            var responseCount = questionResponses.Count(a => a.OptionId == option.Id);
                            var numberDivision = (double)responseCount / (double)totalUsersCount;
                            var responsePercentage = numberDivision * 100;
                            var roundedUpPercentage = Math.Round(responsePercentage, 1);

                            OpinionPoll opinionPoll = new OpinionPoll
                            {
                                OptionId = option.Id,
                                Option = option.Text,
                                Percentage = roundedUpPercentage
                            };
                            questionOpinions.Add(opinionPoll);
                        }
                    }

                    Poll poll = new Poll
                    {
                        QuestionId = questionId,
                        Question = question.Text,
                        OpinionPolls = questionOpinions
                    };
                    polls.Add(poll);
                }


                SurveyStat stat = new SurveyStat
                {
                    Polls = polls,
                    TotalParticipants = totalUsersCount,
                    TotalQuestions = totalQuestionsCount
                };

                response = new Response<SurveyStat>
                {
                    ResponseBody = new SuccessResponse<SurveyStat>
                    {
                        Data = stat,
                        ResponseCode = "00",
                        ResponseMessage = "You have successfully shared your opinion in the survey. Kindly view our stats to compare your responses with those of others."
                    }
                };


                return Ok(response.ResponseBody);
            }
            catch (Exception)
            {
                response = new Response<SurveyStat>
                {
                    ResponseBody = new SuccessResponse<SurveyStat>
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
