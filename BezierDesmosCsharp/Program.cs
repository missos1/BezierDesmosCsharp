using BezierCsharp.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Console.WriteLine("Welcome to BezierDesmosCsharp!");
bool isProcessed = false;

while (!isProcessed)
{
    Console.WriteLine("To process images, enter 1:");
    string? input = Console.ReadLine();

    if (input == "1")
    {
        ImagesProcesor.ProcessImage();
        isProcessed = true;
        Console.WriteLine("Image processing completed. Starting desmos...");
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter 1 to process images.");
    }
}

app.Run();
