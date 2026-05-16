using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaxCalculatorApi.Services.Helpers;

public class TaxCodeEnumConverter() : JsonStringEnumConverter(JsonNamingPolicy.KebabCaseUpper);