using API.Extensions;
using API.Middlewares;
using API.Services;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplicationService(builder.Configuration);

// Configure JWT settings from appsettings.json
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));

// Add Identity and JWT services
builder.Services.AddIdentityService(builder.Configuration);

// Add custom services
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

// Add Controllers
builder.Services.AddControllers();

// ✅ Add Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: Allow Angular client
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // ✅ Enable Swagger UI in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

// SignalR Hub
app.MapHub<ChatHub>("/chathub");

// Global Error Handling Middleware
app.UseMiddleware<GlobalErrorHandlingMiddleware>();

// Middleware Pipeline
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

// Log incoming requests
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

// Controllers
app.MapControllers();

// DB Initialization
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var initializer = services.GetRequiredService<DbInitializer>();
    await initializer.InitializeIdentityAsync();
}

app.Run();
