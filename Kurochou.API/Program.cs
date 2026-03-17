using Kurochou.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile("appsettings.{builder.Environment.EnvironmentName}.json", true)
        .AddEnvironmentVariables();

builder.Services.AddHttpContextAccessor();

// Basically map the tables and columns snake_case.
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddRouting(config =>
{
    config.LowercaseUrls = true;
    config.LowercaseQueryStrings = true;
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontend");
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();