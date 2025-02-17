using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class WeatherViewModel : INotifyPropertyChanged
{
    private string _temperature = "Lade...";
    private string _weatherDescription = "Noch keine Daten";
    private string _city = "Ulm, Germany"; // Standard-Stadt
    private string _weatherIcon = "https://via.placeholder.com/80"; // Platzhalter-Icon

    private readonly string apiKey = "ab8950285137bdce54946e9f039a25c4"; // Ersetze mit deinem API-Schlüssel
    private readonly string baseUrl = "http://api.weatherstack.com/current";

    public string Temperature
    {
        get => _temperature;
        set { _temperature = value; OnPropertyChanged(nameof(Temperature)); }
    }

    public string WeatherDescription
    {
        get => _weatherDescription;
        set { _weatherDescription = value; OnPropertyChanged(nameof(WeatherDescription)); }
    }

    public string City
    {
        get => _city;
        set { _city = value; OnPropertyChanged(nameof(City)); }
    }

    public string WeatherIcon
    {
        get => _weatherIcon;
        set { _weatherIcon = value; OnPropertyChanged(nameof(WeatherIcon)); }
    }

    public async Task LoadWeatherAsync() //!wichtig Beschreibung
    {
        string url = $"{baseUrl}?access_key={apiKey}&query={Uri.EscapeDataString(City)}";

        try
        {
            using HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(url);
            JObject json = JObject.Parse(response);

            if (json["current"] != null)
            {
                Temperature = json["current"]["temperature"]?.ToString() + "°C";
                WeatherDescription = json["current"]["weather_descriptions"]?[0]?.ToString() ?? "Keine Beschreibung";
                WeatherIcon = json["current"]["weather_icons"]?[0]?.ToString() ?? "https://via.placeholder.com/80";
            }
            else
            {
                Temperature = "Keine Daten";
                WeatherDescription = "API-Fehler";
                WeatherIcon = "https://via.placeholder.com/80";
            }
        }
        catch (Exception)
        {
            Temperature = "Fehler";
            WeatherDescription = "Daten nicht verfügbar";
            WeatherIcon = "https://via.placeholder.com/80";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
