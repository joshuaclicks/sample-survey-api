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
using MiniSurvey.Client.Dto;
using MiniSurvey.Client.Helpers;
using MiniSurvey.Client.Models;

namespace MiniSurvey.Client.Controllers
{
    [Produces("application/json")]
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : BaseController
    {
        public QuestionsController(IWebHostEnvironment env, IHttpContextAccessor httpContext, IMemoryCache memoryCache) : base(env, httpContext, memoryCache)
        {
        }

        /// <summary>
        /// To Get te List of questions the Survey Participant is to answer or respond to.
        /// </summary>
        /// <returns></returns>
        [HttpGet("survey")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<SurveyResponse>))]
        public async Task<IActionResult> GetSurveyQuestions(int id)
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

                Survey survey;
                List<QuestionType> questionTypes = new List<QuestionType>();
                List<QuestionResponse> questionResponses = new List<QuestionResponse>();
                using (var _context = new MiniSurveyContext())
                {
                    survey = await _context.Surveys.Where(a => a.Id == id).Include(a => a.UserResponses).Include(a => a.SurveyQuestions).ThenInclude(a => a.Question).ThenInclude(a => a.QuestionOptions).ThenInclude(a => a.Option).FirstOrDefaultAsync();

                    questionTypes = await _context.QuestionTypes.ToListAsync();
                }

                if (survey == null)
                {
                    response = new Response<SurveyResponse>
                    {
                        ResponseBody = new SuccessResponse<SurveyResponse>
                        {
                            Data = null,
                            ResponseCode = "00",
                            ResponseMessage = "There are no surveys yet, kindly try again later as we are currently working on it."
                        }
                    };

                    return Ok(response.ResponseBody);
                }

                if (survey.SurveyQuestions.Count == 0)
                {
                    SurveyResponse surveyResponse = new SurveyResponse
                    {
                        Id = survey.Id,
                        Title = survey.Title,
                        Description = survey.Description,
                        Status = survey.Status,
                        DateCreated = survey.DateCreated
                    };

                    response = new Response<SurveyResponse>
                    {
                        ResponseBody = new SuccessResponse<SurveyResponse>
                        {
                            Data = surveyResponse,
                            ResponseCode = "00",
                            ResponseMessage = "There are no survey questions configured for this survey yet, kindly try again later."
                        }
                    };

                    return Ok(response.ResponseBody);
                }

                List<long> questionIds = survey.SurveyQuestions.Select(a => a.QuestionId.Value).Distinct().ToList();
                foreach (var questionId in questionIds)
                {
                    var question = survey.SurveyQuestions.FirstOrDefault(a => a.QuestionId == questionId).Question;
                    var options = question.QuestionOptions.Select(a => a.Option).ToList();
                    var questionType = questionTypes.FirstOrDefault(a => a.Id == question.QuestionTypeId);

                    QuestionResponse questionResponse = new QuestionResponse { Id = question.Id, Value = question.Text };
                    QuestionTypeResponse questionTypeResponse = new QuestionTypeResponse { Id = questionType.Id, Name = questionType.Name, MinimumRequired = questionType.MinimumRequired, MaximumRequired = questionType.MaximumRequired, IsOptionRequired = questionType.IsOptionRequired ?? false };

                    List<DefaultResponse> selectedOptions = new List<DefaultResponse>();

                    foreach (var option in options)
                    {
                        DefaultResponse selectedOption = new DefaultResponse
                        {
                            Id = option.Id,
                            Value = option.Text
                        };
                        selectedOptions.Add(selectedOption);
                    }

                    questionResponse.Options = selectedOptions;
                    questionResponse.QuestionType = questionTypeResponse;
                    questionResponses.Add(questionResponse);
                }

                SurveyResponse surveyQuestionResponse = new SurveyResponse
                {
                    Id = survey.Id,
                    Title = survey.Title,
                    Description = survey.Description,
                    Status = survey.Status,
                    DateCreated = survey.DateCreated,
                    SurveyQuestions = questionResponses
                };

                response = new Response<SurveyResponse>
                {
                    ResponseBody = new SuccessResponse<SurveyResponse>
                    {
                        Data = surveyQuestionResponse,
                        ResponseCode = "00",
                        ResponseMessage = "You have been successfully fetched all survey questions. Get ready to answer them in your own opinion."
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
                        ResponseMessage = "Sorry, we are unable to fetch survey questions at the moment, kindly try again later."
                    }
                };
                return StatusCode(500, response.ResponseBody);
            }
        }

        /// <summary>
        /// Submits the responss of the survey participant to the server.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("survey/responses")]
        //[ValidateAntiForgeryToken]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = 200, Type = typeof(SuccessResponse<SurveyStat>))]
        public async Task<IActionResult> SendSurveyResponses([FromBody] SurveyAnswerRequest request)
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

                if (request.Responses.Count == 0)
                {
                    ModelState.AddModelError("questionId", "questionId is required!");
                    ModelState.AddModelError("optionId", "optionId is required!");
                }

                if (!ModelState.IsValid)
                {
                    var requestResponse = ApiResponseFormatter.RequestResponse(ModelState);
                    return BadRequest(requestResponse);
                }

                User userCredentials;
                Survey survey;
                using (var _context = new MiniSurveyContext())
                {
                    userCredentials = await _context.Users.Where(x => x.Id == UserIdentity)
                        .Include(x => x.UserResponses)
                        .FirstOrDefaultAsync();

                    survey = await _context.Surveys.Where(a => a.Id == request.SurveyId).FirstOrDefaultAsync();
                }

                if (userCredentials == null) 
                {
                    response = new Response<SurveyStat>
                    {
                        ResponseBody = new SuccessResponse<SurveyStat>
                        {
                            Data = null,
                            ResponseCode = "E003",
                            ResponseMessage = "You do not have the permission to submit your response. Kindly login or register to share your opinion."
                        }
                    };


                    return Unauthorized(response.ResponseBody);
                }

                if(survey == null)
                {
                    response = new Response<SurveyStat>
                    {
                        ResponseBody = new SuccessResponse<SurveyStat>
                        {
                            Data = null,
                            ResponseCode = "E012",
                            ResponseMessage = "The survey you selected does not exist. Kindly try again later."
                        }
                    };


                    return NotFound(response.ResponseBody);
                }

                if (userCredentials.UserResponses.Count == 0)
                {
                    List<Models.UserResponse> selectedResponses = new List<Models.UserResponse>();
                    foreach (var userResponse in request.Responses)
                    {
                        Models.UserResponse selectedResponse = new Models.UserResponse
                        {
                            QuestionId = userResponse.QuestionId,
                            OptionId = userResponse.OptionId,
                            UserId = userCredentials.Id,
                            DateResponded = DateTime.UtcNow,
                            SurveyId = request.SurveyId,
                            TextResponse = userResponse.TextAnswer
                        };
                        selectedResponses.Add(selectedResponse);
                    }

                    using (var _context = new MiniSurveyContext())
                    {
                        _context.UserResponses.AddRange(selectedResponses);
                        await _context.SaveChangesAsync();
                    }
                }

                List<Models.UserResponse> userResponses = new List<Models.UserResponse>();
                List<Models.QuestionOption> questionOptions = new List<QuestionOption>();
                using (var _context = new MiniSurveyContext())
                {
                    userResponses = await _context.UserResponses.ToListAsync();
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
