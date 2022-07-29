



using System.Net.Http.Json;

HttpClient client = new();
string[] game = new string[2000];
Dictionary<string, string> info = new();
var response =
    await client.GetAsync($"https://api.sportsdata.io/v3/nfl/scores/json/ScoresByDate/2021-SEP-12?key=631ffaf2cf5d4555a9a1f7ba2d74a0f3");

if (response.IsSuccessStatusCode)
{
    var rootobject = await response.Content.ReadFromJsonAsync<Rootobject>();
    var date = rootobject.Date;
    //var values = json.Split(',');
    Console.WriteLine(date);
}

else
    Console.WriteLine("No good.");








public class Rootobject
{
    public string GameKey { get; set; }
    public int SeasonType { get; set; }
    public int Season { get; set; }
    public int Week { get; set; }
    public DateTime Date { get; set; }
    public string AwayTeam { get; set; }
    public string HomeTeam { get; set; }
    public int AwayScore { get; set; }
    public int HomeScore { get; set; }
    public string Channel { get; set; }
    public float PointSpread { get; set; }
    public float OverUnder { get; set; }
    public string Quarter { get; set; }
    public object TimeRemaining { get; set; }
    public object Possession { get; set; }
    public object Down { get; set; }
    public string Distance { get; set; }
    public object YardLine { get; set; }
    public object YardLineTerritory { get; set; }
    public object RedZone { get; set; }
    public int AwayScoreQuarter1 { get; set; }
    public int AwayScoreQuarter2 { get; set; }
    public int AwayScoreQuarter3 { get; set; }
    public int AwayScoreQuarter4 { get; set; }
    public int AwayScoreOvertime { get; set; }
    public int HomeScoreQuarter1 { get; set; }
    public int HomeScoreQuarter2 { get; set; }
    public int HomeScoreQuarter3 { get; set; }
    public int HomeScoreQuarter4 { get; set; }
    public int HomeScoreOvertime { get; set; }
    public bool HasStarted { get; set; }
    public bool IsInProgress { get; set; }
    public bool IsOver { get; set; }
    public bool Has1stQuarterStarted { get; set; }
    public bool Has2ndQuarterStarted { get; set; }
    public bool Has3rdQuarterStarted { get; set; }
    public bool Has4thQuarterStarted { get; set; }
    public bool IsOvertime { get; set; }
    public object DownAndDistance { get; set; }
    public string QuarterDescription { get; set; }
    public int StadiumID { get; set; }
    public DateTime LastUpdated { get; set; }
    public object GeoLat { get; set; }
    public object GeoLong { get; set; }
    public int ForecastTempLow { get; set; }
    public int ForecastTempHigh { get; set; }
    public string ForecastDescription { get; set; }
    public int ForecastWindChill { get; set; }
    public int ForecastWindSpeed { get; set; }
    public int AwayTeamMoneyLine { get; set; }
    public int HomeTeamMoneyLine { get; set; }
    public bool Canceled { get; set; }
    public bool Closed { get; set; }
    public string LastPlay { get; set; }
    public DateTime Day { get; set; }
    public DateTime DateTime { get; set; }
    public int AwayTeamID { get; set; }
    public int HomeTeamID { get; set; }
    public int GlobalGameID { get; set; }
    public int GlobalAwayTeamID { get; set; }
    public int GlobalHomeTeamID { get; set; }
    public int PointSpreadAwayTeamMoneyLine { get; set; }
    public int PointSpreadHomeTeamMoneyLine { get; set; }
    public int ScoreID { get; set; }
    public string Status { get; set; }
    public DateTime GameEndDateTime { get; set; }
    public int HomeRotationNumber { get; set; }
    public int AwayRotationNumber { get; set; }
    public bool NeutralVenue { get; set; }
    public int RefereeID { get; set; }
    public int OverPayout { get; set; }
    public int UnderPayout { get; set; }
    public object HomeTimeouts { get; set; }
    public object AwayTimeouts { get; set; }
    public DateTime DateTimeUTC { get; set; }
    public int Attendance { get; set; }
    public Stadiumdetails StadiumDetails { get; set; }
}

public class Stadiumdetails
{
    public int StadiumID { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public int Capacity { get; set; }
    public string PlayingSurface { get; set; }
    public float GeoLat { get; set; }
    public float GeoLong { get; set; }
    public string Type { get; set; }
}



