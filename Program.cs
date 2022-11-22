using MassTransit;
using POC.Outbox.WebAPI;
using POC.Outbox.WebAPI.Models.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PocOutboxContext>();
builder.Services.AddHostedService<HostedService>();

builder.Services.AddMassTransit(x =>
{
    ConfigureRabbitmq(x);
    x.AddConsumer<OutboxConsumer>();
});

static void ConfigureRabbitmq(IBusRegistrationConfigurator busRegistrationConfigurator)
{
    busRegistrationConfigurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
