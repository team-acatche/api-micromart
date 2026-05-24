using RegistryService.Models;

namespace RegistryService;

public abstract class BaseRegistryComponents
{
    public static readonly List<RegistryComponent> Components = [
        new()
        {
            ComponentName = "Authenticator",
            Description = "A basic authentication API that can perform in-memory registrations and token-based logins with salted passwords and hashing.",
            Version = "1",
            BaseUrl = Environment.GetEnvironmentVariable("AUTHENTICATION_URI") ?? "http://localhost:5087",
            Endpoints = [
                new EndpointDescription
                {
                    Method = "POST",
                    Path = "/api/authenticate/login",
                    SampleRequest = """
                                    {
                                        "username": "john",
                                        "password": "password",
                                    }
                                    """,
                    SampleResponse = """
                                     {
                                        "componentName": "Authenticator",
                                        "success": true,
                                        "result": {
                                            "token": "...",
                                            "userId": 1,
                                            "username": "john
                                        },
                                        "error": null
                                     }
                                     """
                },
                new EndpointDescription
                {
                    Method = "POST",
                    Path = "/api/authenticate/register",
                    SampleRequest = """
                                    {
                                        "username": "john",
                                        "password": "password",
                                    }
                                    """,
                    SampleResponse = """
                                     {
                                        "componentName": "Authenticator",
                                        "success": true,
                                        "result": {
                                            "userId": 1,
                                            "username": "john
                                        },
                                        "error": null
                                     }
                                     """
                }
            ]
        },
        new()
        {
            ComponentName = "DiscountEngine",
            Description = "The discount engine API that is responsible for applying discounts on initial prices.",
            Version = "1",
            BaseUrl = Environment.GetEnvironmentVariable("DISCOUNT_ENGINE_URI") ?? "http://localhost:5192",
            Endpoints = [
                new EndpointDescription
                {
                    Method = "POST",
                    Path = "/api/discount/apply",
                    SampleRequest = """
                                    {
                                        "originalPrice": 100,
                                        "discountCode": "FLAT50"
                                    }
                                    """,
                    SampleResponse = """
                                     {
                                        "componentName": "DiscountEngine",
                                        "success": true,
                                        "result": {
                                            "discountApplied": "FLAT50",
                                            "originalPrice": 100,
                                            "finalPrice": 50
                                        },
                                        "error": null
                                     }
                                     """,
                }
            ]
        },
        new()
        {
           ComponentName = "InvoiceNumberGenerator",
           Description = "The invoice number generator responsible for generating invoice numbers statefully.",
           Version = "1",
           BaseUrl = Environment.GetEnvironmentVariable("INVOICE_NUMBER_GENERATOR_URI") ?? "http://localhost:5024",
           Endpoints = [
               new EndpointDescription
               {
                   Method = "POST",
                   Path = "/api/invoice/generate",
                   SampleRequest = """
                                   {
                                        "prefix": "INV",
                                        "clientCode": "ACME"
                                   }
                                   """,
                   SampleResponse = """
                                    {
                                        "componentName": "InvoiceNumberGenerator",
                                        "success": true,
                                        "result": "INV-ACME-20000101-00001",
                                        "error": null
                                    }
                                    """
               },
               new EndpointDescription
               {
                   Method = "GET",
                   Path = "/api/invoice/counter",
                   SampleRequest = string.Empty,
                   SampleResponse = """
                                    {
                                        "componentName": "InvoiceNumberGenerator",
                                        "success": true,
                                        "result": 1,
                                        "error": null
                                    }
                                    """
                   
               }
           ]
        },
        new()
        {
            ComponentName = "TaxCalculator",
            Description = "A simple sales tax calculator API supporting various region codes and sales tax rates.",
            Version = "1",
            BaseUrl = Environment.GetEnvironmentVariable("TAX_CALCULATOR_URI") ?? "http://localhost:5266",
            Endpoints = [
                new EndpointDescription
                {
                    Method = "GET",
                    Path = "/api/tax/regions",
                    SampleRequest = string.Empty,
                    SampleResponse = """
                                     {
                                        "componentName": "TaxCalculator",
                                        "success": true,
                                        "result": ["PH-NCR", "PH-CEB", "SG", "US-CA", "US-HI"],
                                        "error": null
                                     }
                                     """
                },
                new EndpointDescription
                {
                    Method = "GET",
                    Path = "/api/tax/rate/{regionCode}",
                    SampleRequest = string.Empty,
                    SampleResponse = """
                                     {
                                        "componentName": "TaxCalculator",
                                        "success": true,
                                        "result": {
                                            "regionCode": "PH-NCR",
                                            "taxRate": 0.12
                                        },
                                        "error": null
                                     }
                                     """
                },
                new EndpointDescription
                {
                    Method = "POST",
                    Path = "/api/tax/calculate",
                    SampleRequest = """
                                    {
                                        "price": "100",
                                        "regionCode": "PH-NCR"
                                    }
                                    """,
                    SampleResponse = """
                                     {
                                        "componentName": "TaxCalculator",
                                        "success": true,
                                        "result": {
                                            "price": 100,
                                            "regionCode": "PH-NCR",
                                            "taxRate": 0.12,
                                            "totalPrice": 112
                                        },
                                        "error": null
                                     }
                                     """
                },
            ]
        }
    ];
}