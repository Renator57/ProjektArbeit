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

    public async Task LoadWeatherAsync()
    {
        string url = $"{baseUrl}?access_key={apiKey}&query={City}";

        try
        {
            using HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(url);
            JObject json = JObject.Parse(response);

            if (json["current"] != null)
            {
                Temperature = json["current"]["temperature"]?.ToString() + "°C";
                WeatherDescription = json["current"]["weather_descriptions"]?.FirstOrDefault()?.ToString() ?? "Keine Beschreibung";
            }
            else
            {
                Temperature = "Keine Daten";
                WeatherDescription = "API-Fehler";
            }
        }
        catch (Exception)
        {
            Temperature = "Fehler";
            WeatherDescription = "Daten nicht verfügbar";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
