using MongoDB.Driver;
using NoteService.Data;
using NoteService.Repositories;
using NoteService.Services;

var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB")!;
var mongoDatabaseName = builder.Configuration["MongoDB:DatabaseName"]!;

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteManagementService, NoteManagementService>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    await NoteSeeder.SeedAsync(database);
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();