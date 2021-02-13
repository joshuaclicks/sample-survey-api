using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class ApiResponseFormatter
    {
        public static SuccessResponse<ValidationErrorResponse> RequestResponse(ModelStateDictionary modelState)
        {
            List<ValidationModel> validationModel = new List<ValidationModel>();
            foreach (var item in modelState)
            {
                if (item.Value.Errors.Count != 0)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        validationModel.Add(new ValidationModel
                        {
                            Code = StringFormatter.PascalCaseConverter(item.Key),
                            Message = error.ErrorMessage
                        });
                    }
                }
            }

            var validationResponse = new ValidationErrorResponse
            {
                Errors = validationModel
            };

            return new SuccessResponse<ValidationErrorResponse>
            {
                Data = validationResponse,
                ResponseCode = "E001",
                ResponseMessage = "You have supplied an invalid or incomplete information. Kindly check your input and try again.s"
            };
        }

        public static SuccessResponse<ValidationErrorResponse> RequestResponse(List<KeyValuePair<string, string>> errorDictionary)
        {
            var validationModel = new List<ValidationModel>();
            foreach (var error in errorDictionary)
            {
                validationModel.Add(new ValidationModel
                {
                    Code = StringFormatter.PascalCaseConverter(error.Key),
                    Message = error.Value
                });
            }

            var validationResponse = new ValidationErrorResponse
            {
                Errors = validationModel
            };

            return new SuccessResponse<ValidationErrorResponse>
            {
                Data = validationResponse,
                ResponseCode = "E002",
                ResponseMessage = "You have supplied an invalid or incomplete information. Kindly check your input and try again."
            };
        }

        public static SuccessResponse<ValidationErrorResponse> UnAuthorizeRequestResponse()
        {
            return new SuccessResponse<ValidationErrorResponse> { ResponseCode = "E003", ResponseMessage = "Your request cannot be authorized at the moment, kindly try again later.", Data = null };
        }
    }
}
