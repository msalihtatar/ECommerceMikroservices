using Core.Services;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using OrderApplication.Consumer;
using OrderApplication.Handlers;
using OrderInfrastructure;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

//RabbitMQ default olarak 5672 portundan ayaða kalkýyor, arayüzü 15672 portundan ayaða kalkýyor
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(builder.Configuration["RabbitMQUrl"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
        configuration.ReceiveEndpoint("create-order-service", e => 
        {
            e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
        });
        configuration.ReceiveEndpoint("product-name-changed-event-order-service", e =>
        {
            e.ConfigureConsumer<ProductNameChangedEventConsumer>(context);
        });
    });

    x.AddConsumer<CreateOrderMessageCommandConsumer>();
    x.AddConsumer<ProductNameChangedEventConsumer>();
});

// Add services to the container.
var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_order";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddDbContext<OrderDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
    {
        configure.MigrationsAssembly("OrderInfrastructure");
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
