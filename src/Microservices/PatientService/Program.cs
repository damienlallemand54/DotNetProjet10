using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Repositories;
using PatientService.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientManagementService, PatientManagementService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
using (var scope = app.Services.CreateScope())
{
    // On récupère PatientDbContext
    var context = scope.ServiceProvider.GetRequiredService<PatientDbContext>();

    // On crée la base Projet10_PatientDb et la table Patient dans Docker
    context.Database.Migrate();

    // On utilise les patients tests pour remplir la bdd
    await DataSeeder.SeedAsync(context);


}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();