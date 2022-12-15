using Whatsapp.Data;
using Whatsapp.SignalRChat;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSignalR();

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
}));

var app = builder.Build();

app.MapControllers();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseEndpoints(endpoints => {
    endpoints.MapHub<ChatHub>("/chat");
});

app.Run();
