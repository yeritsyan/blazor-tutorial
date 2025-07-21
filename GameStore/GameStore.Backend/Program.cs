using GameStore.Backend.Data;
using GameStore.Backend.Dtos;
using GameStore.Backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStoreContext");
builder.Services.AddSqlite<GameStoreContext>(connectionString);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDb();

app.Run();
