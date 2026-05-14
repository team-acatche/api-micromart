using InvoiceNumberGeneratorApi;
using InvoiceNumberGeneratorApi.Models;
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
        var dayToday = DateTimeOffset.Now.Date.ToString("yyyyMMdd");
        var invoiceNumber =
            $"{request.Prefix}-{request.ClientCode}-{dayToday}-{numberService.GetAndIncrementSequenceNumber():D5}";
        return TypedResults.Ok(invoiceNumber);
    })
    .Produces<string>()
    .WithName("GenerateInvoiceNumber");

app.MapGet("/counter", IResult (ISequenceNumberService numberService) => TypedResults.Ok(numberService.GetSequenceNumber()))
    .Produces<int>()
    .WithName("GetCurrentSequenceNumber");

api.Run();