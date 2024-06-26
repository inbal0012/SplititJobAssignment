using Microsoft.EntityFrameworkCore;
using SplititJobAssignment.Movies;
using SplititJobAssignment.Actors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<MovieScraper>();
builder.Services.AddScoped<BaseActorScraper, IMBDActorScraper>();

builder.Services.AddScoped<IActorsService, ActorsService>();

builder.Services.AddDbContext<MoviesContext>(opt => opt.UseInMemoryDatabase("MoviesList"));
builder.Services.AddDbContext<ActorsContext>(opt => opt.UseInMemoryDatabase("ActorsList"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
