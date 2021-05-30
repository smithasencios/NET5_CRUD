using Application.Common.Exceptions;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Infrastructure.Common.GlobalExceptionMiddleware
{
	public sealed class ExceptionMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<ExceptionMiddleware> logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
		/// </summary>
		/// <param name="next">A function that can process an HTTP request.</param>
		/// <param name="logger">A generic interface for logging.</param>
		public ExceptionMiddleware(
			RequestDelegate next,
			ILogger<ExceptionMiddleware> logger)
		{
			this.next = next;
			this.logger = logger;
		}

		/// <summary>
		/// This method is going to intercept the request.
		/// </summary>
		/// <param name="httpContext">Encapsulates all HTTP-specific information about an individual HTTP request.</param>
		/// <returns>A representing the asynchronous operation.</returns>
		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await this.next(httpContext).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				await this.HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			this.logger.LogError(exception.ToString());
			string errorMessage;
			if (exception.GetType() == typeof(ValidationException))
			{
				var validationException = exception as ValidationException;
				errorMessage = new ErrorDetails { Message = string.Join(",", validationException.Errors) }.ToString();
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			else if (exception.GetType() == typeof(InvalidParameter))
			{
				errorMessage = new ErrorDetails { Message = exception.Message }.ToString();
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			else if (exception.GetType() == typeof(ProductNotFound))
			{
				errorMessage = new ErrorDetails { Message = exception.Message }.ToString();
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
			else
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				errorMessage = new ErrorDetails { Message = exception.Message }.ToString();
			}

			context.Response.ContentType = MediaTypeNames.Application.Json;
			return context.Response.WriteAsync(errorMessage);
		}
	}
}
