using InvoiceNumberGeneratorApi;
using InvoiceNumberGeneratorApi.Core;
using InvoiceNumberGeneratorApi.Models;
using InvoiceNumberGeneratorApi.Models.Errors;
using InvoiceNumberGeneratorApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ISequenceNumberService, InvoiceSequenceNumberService>();

var api = builder.Build();

// Configure the HTTP request pipeline.
if (api.Environment.IsDevelopment())
{
    api.MapOpenApi();
    api.MapScalarApiReference();
}

api.UseHttpsRedirection();

var app = api.MapGroup("api/invoice");

// add endpoints here
app.MapPost("/generate", IResult (ISequenceNumberService numberService, InvoiceNumberGeneratorRequest request) =>
    {
        string[] requiredNonEmptyFields = [request.Prefix, request.ClientCode];
        if (requiredNonEmptyFields.Any(string.IsNullOrEmpty))
        {
            string[] fieldNames = ["prefix", "clientCode"];
            bool[] emptyFieldsFlag = [string.IsNullOrEmpty(request.Prefix), string.IsNullOrEmpty(request.ClientCode)];

            var emptyFields = fieldNames.Where(((_, idx) => emptyFieldsFlag[idx]))
                .ToArray();
            
            return TypedResults.BadRequest(new ApiResponse<string>()
            {
                ComponentName = "InvoiceNumberGenerator",
                Success = false,
                Error = new EmptyFieldError(emptyFields),
            });
        }
        
        var dayToday = DateTimeOffset.Now.Date.ToString("yyyyMMdd");
        var invoiceNumber =
            $"{request.Prefix}-{request.ClientCode}-{dayToday}-{numberService.GetAndIncrementSequenceNumber():D5}";
        return TypedResults.Ok(new ApiResponse<string>()
        {
            ComponentName = "InvoiceNumberGenerator",
            Success = true,
            Result = invoiceNumber
        });
    })
    .Produces<ApiResponse<string>>()
    .Produces<ApiResponse<string>>(StatusCodes.Status400BadRequest)
    .WithName("GenerateInvoiceNumber");

app.MapGet("/counter", IResult (ISequenceNumberService numberService) =>
    {
        var response = new ApiResponse<int>()
        {
            ComponentName = "InvoiceNumberGenerator",
            Success = true,
            Result = numberService.GetSequenceNumber()
        };
        return TypedResults.Ok(response);
    })
    .Produces<ApiResponse<int>>()
    .WithName("GetCurrentSequenceNumber");

api.Run();