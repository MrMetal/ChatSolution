using ChatServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

string[] origens = ["https://0.0.0.0", "http://10.0.2.2", "https://10.0.2.2", "https://localhost:7098"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    
}



app.UseCors();

app.MapHub<ChatHub>("/chat");

app.Run();
