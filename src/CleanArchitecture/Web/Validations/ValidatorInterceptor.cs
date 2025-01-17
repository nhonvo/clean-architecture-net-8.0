using System.Net;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Shared.Models.Errors;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ValidationException = CleanArchitecture.Application.Common.Exceptions.ValidationException;

namespace CleanArchitecture.Web.Validations;

public class ValidatorInterceptor : IValidatorInterceptor
{
    public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
    {
        if (!result.IsValid)
        {
            actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            List<Error> errors = [];
            foreach (var error in result.Errors)
            {
                var validationError = new Error($"{ApplicationConstants.Name}.{ErrorRespondCode.BAD_REQUEST}", error.ErrorMessage);
                validationError.AddErrorProperty(new ErrorProperty(error.PropertyName, error.AttemptedValue == null ? "null" : error.AttemptedValue?.ToString() ?? "null"));
                errors.Add(validationError);

                var errorResponse = new ErrorResponse(errors);
                actionContext.ModelState.TryAddModelException(ApplicationConstants.FluentValidationErrorKey, new ValidationException(errorResponse));
            }
        }
        return result;
    }

    public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
    {
        return commonContext;
    }
}
