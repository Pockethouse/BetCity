using BetCity.APIs;
using BetCity.Services;
using BetCity.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();  // Add console logging



builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddStackExchangeRedisCache(options =>
{
    // This points to your Redis server, which by default is localhost at port 6379
    options.Configuration = "localhost:6379"; // Replace with your Redis server if deployed
    options.InstanceName = "BetCity_"; // This will prefix all cache keys
});

// Register BetCityApiService with HttpClient
builder.Services.AddHttpClient<BetCityApiService>(client =>
{
    client.BaseAddress = new Uri("https://bet365-api-inplay.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "66c02930d0msha7b0fc8b10a4cabp1b17d5jsnefa1b1020467");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "bet365-api-inplay.p.rapidapi.com");
});

// Bind ApiSettings section from appsettings.json to ApiSetting class
builder.Services.Configure<ApiSetting>(builder.Configuration.GetSection("ApiSettings"));

// Register SportsEventService
builder.Services.AddScoped<SportsEventService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sports}/{action=Index}/{id?}");

app.Run();
