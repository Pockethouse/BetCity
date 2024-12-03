using BetCity.Services;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly SportsEventService _sportsEventService;

    public HomeController(SportsEventService sportsEventService)
    {
        _sportsEventService = sportsEventService;
    }

    public async Task<IActionResult> Index()
    {
        // Fetch recent sports events
        var events = await _sportsEventService.GetSportEventsAsync("soccer");
        return View(events); // Pass the data to the view
    }

    public IActionResult Privacy()
    {
        return View(); // Matches Views/Home/Privacy.cshtml
    }
}