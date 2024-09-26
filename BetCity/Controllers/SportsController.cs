using BetCity.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BetCity.Models;

namespace BetCity.Controllers
{
    public class SportsController : Controller
    {
        private readonly SportsEventService _sportsEventService;

        public SportsController(SportsEventService sportsEventService)
        {
            _sportsEventService = sportsEventService;
        }
    
       
        
        public async Task<IActionResult> Events(string sportName)
        {
            var sportEvents = await _sportsEventService.GetSportEventsAsync(sportName);
            return View(sportEvents); // Pass the sport events to the view
        }
        
       
        
        // This action fetches sports events and passes the list to the view
        public async Task<IActionResult> Index()
        {
            var sports = await _sportsEventService.GetSportsAsync(); // List<string>
            return View(sports);  // Pass the list of sport names (strings) to the view
        }

    }
}
