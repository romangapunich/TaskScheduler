using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using TaskScheduler.DTOs.BaseResponse;

namespace TaskScheduler.MediatR.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public BaseResultResponeVm ResponeVm;

        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
            ResponeVm = new BaseResultResponeVm();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);
            var lstErrors = new List<string>();
            foreach (var failureGroup in failureGroups)
            {
                var errors = failureGroup.ToArray().ToList();
                foreach (var error in errors)
                {
                    lstErrors.Add("Поле: " + error);
                }
            }

            if (lstErrors.Count>0)
            {
                Errors.Add("Errors", lstErrors.ToArray());
                ResponeVm.Errors = lstErrors;
            }
            ResponeVm.Message =  lstErrors.Count > 0 ? String.Join(", ", lstErrors.ToArray())  : "";
            ResponeVm.Status = !(lstErrors.Count > 0);
        }
    }
}
