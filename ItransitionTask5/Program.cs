using ItransitionTask5.Middlewares;
using ItransitionTask5.Models;
using ItransitionTask5.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MessagesContext>();
builder.Services.AddDbContext<NamesContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHostInfo, HostInfo>();
builder.Services.AddTransient<ISocketDataHandler, SocketDataHandler>();
builder.Services.AddSingleton<ISocketsWorker, SocketsWorker>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseWebSockets();

app.UseSocketDataHandler();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}");

app.Run();