namespace BetCity.Models;

public class EventMarket
{
    public string sport { get; set; }
    public string score { get; set; }
    public string sets { get; set; }
    public string timer { get; set; }
    public string liga { get; set; }
    public string eventId { get; set; }
    public string eventName { get; set; }
    public string team1 { get; set; }
    public string team2 { get; set; }
    public string marketFI { get; set; }
    public string evLink { get; set; }

    public List<Market> markets { get; set; }
}