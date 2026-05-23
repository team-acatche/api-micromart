using AcatcheApiMicromart.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient for calling the Registry and component APIs from Blazor server-side
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient();
    // Allow the client to be used for any origin (registry, tax, invoice, discount)
    return client;
});

// Read registry URI from env or config
builder.Configuration.AddEnvironmentVariables();

// CORS – allow the Blazor app to be called from any origin during development
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseCors();
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();