using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QoniacTask.Api.Filters;
using System.Reflection;

namespace QoniacTask.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<CurrencyFormatErrorFilter>();
            })
            //NOTE: For validation related errors, http status 422 is now considered
            //more appropriate. Asp by default doesn't return 422 on validation errors
            //Following code is changing that.
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetailsFactory =
                        context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                    var validationProblemDetails =
                        problemDetailsFactory.CreateValidationProblemDetails(
                            context.HttpContext, context.ModelState);

                    validationProblemDetails.Type =
                        "https://datatracker.ietf.org/doc/html/rfc9110#name-422-unprocessable-content";
                    validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;

                    return new UnprocessableEntityObjectResult(validationProblemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var app = builder.Build();

            AttachGlobalHanders(app.Logger);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();

            //NOTE: A habit of mine is to attach global exception handlers as well.
            //If there is an uncaught task exception, it will get logged.
            //specially useful if fire and forget tasks are used.
            static void AttachGlobalHanders(ILogger logger)
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, exArgs) =>
                    logger.LogError(
                        exArgs?.ExceptionObject is Exception ex ? ex : null,
                        "'AppDomain.CurrentDomain.UnhandledException' Sender: {Sender}, Sender Type: {SenderType}, Is Runtime Terminating: {IsTerminating}",
                        sender,
                        sender?.GetType().FullName,
                        exArgs is null ? "Unknown" : exArgs.IsTerminating
                    );

                TaskScheduler.UnobservedTaskException += (sender, exArgs) =>
                    logger.LogError(
                        exArgs?.Exception,
                        "'TaskScheduler.UnobservedTaskException' Sender: {Sender}, Sender Type: {SenderType}, Observed: {Observed}",
                        sender,
                        sender?.GetType().FullName,
                        exArgs is null ? "Unknown" : exArgs.Observed
                    );
            }
        }
    }
}