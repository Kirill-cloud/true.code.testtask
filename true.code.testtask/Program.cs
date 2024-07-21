using @true.code.testtask;
using @true.code.testtask.Application.Services;
using @true.code.testtask.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddOptions<AppOptions>().BindConfiguration("AppOptions");
builder.Services.AddDbContext<TodoDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();

app.Run();