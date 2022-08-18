using MediatR;
using Noteflix.API;
using Noteflix.Application.Commands;
using Noteflix.Application.Models;
using Noteflix.Application.Queries;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
});
var app = builder.Build();

app.Run();