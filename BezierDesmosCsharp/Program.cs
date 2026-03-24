using BezierCsharp.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();


// Configure the HTTP request pipeline.

builder.Services.AddSingleton<AppState>();
builder.Services.AddSingleton<ImagesProcessor>();
builder.Services.AddSingleton<ConsoleService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseDefaultFiles();
app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var consoleService = scope.ServiceProvider.GetRequiredService<ConsoleService>();
    consoleService.UpdateFromCommand();
}

app.Run();
