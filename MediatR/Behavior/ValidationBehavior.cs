﻿
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = TaskScheduler.MediatR.Exceptions.ValidationException;

namespace TaskScheduler.MediatR.Behavior
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
       
        protected string AccessToken;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators )
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var validationResults =
                     await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));
                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();
                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();

        }
    }
}
