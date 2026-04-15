using System;
using Microsoft.AspNetCore.HttpOverrides;
using HNG_Stage_0.Interfaces;
using HNG_Stage_0.Service;

var builder = WebApplication.CreateBuilder(args);

// Bind to the port provided by hosting platforms.
// Railway sometimes exposes the port in HTTP_PORTS instead of PORT,
// so check both and prefer an explicit ASPNETCORE_URLS when present.
var port = Environment.GetEnvironmentVariable("PORT")
           ?? Environment.GetEnvironmentVariable("HTTP_PORTS")
           ?? Environment.GetEnvironmentVariable("ASPNETCORE_PORT")
           ?? "80";
// If ASPNETCORE_URLS is set by the platform prefer it, otherwise bind to the discovered port.
var aspnetcoreUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
if (!string.IsNullOrWhiteSpace(aspnetcoreUrls))
{
    builder.WebHost.UseUrls(aspnetcoreUrls);
}
else
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Add services to the container.

// Support forwarded headers from reverse proxies (Railway, nginx, etc.) so
// HTTPS redirection and scheme detection work correctly behind the platform.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Clear default restrictions so common container platforms are accepted
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<IGenderizeService, GenderizeService>();
builder.Services.AddCors(options => options.AddPolicy("AllowAll",
    policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseSwagger();
//app.UseSwaggerUI();

app.MapControllers();

app.Run();
