using Clima.Helpers;
using Clima.Models;
using Clima.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.TextToSpeech;
using Plugin.Geolocator;

namespace Clima.ViewModels
{
    public class ClimaViewModel : INotifyPropertyChanged
    {
        ClimaService climaService { get; } = new ClimaService();

        string location = Settings.City;
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged();
                Settings.City = value;
            }
        }

        bool useGPS;
        public bool UseGPS
        {
            get { return useGPS; }
            set
            {
                useGPS = value;
                OnPropertyChanged();
            }
        }




        bool isImperial = Settings.IsImperial;
        public bool IsImperial
        {
            get { return isImperial; }
            set
            {
                isImperial = value;
                OnPropertyChanged();
                Settings.IsImperial = value;
            }
        }



        string temp = string.Empty;
        public string Temp
        {
            get { return temp; }
            set { temp = value; OnPropertyChanged(); }
        }
        string tempF = string.Empty;
        public string TempF
        {
            get { return tempF; }
            set { tempF = value; OnPropertyChanged(); }
        }

        string condition = string.Empty;
        public string Condition
        {
            get { return condition; }
            set { condition = value; OnPropertyChanged(); }
        }



        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }

        string minTemp = string.Empty;
        public string MinTemp
        {
            get { return minTemp; }
            set { minTemp = value; OnPropertyChanged(); }
        }
        string maxTemp = string.Empty;
        public string MaxTemp
        {
            get { return maxTemp; }
            set { maxTemp = value; OnPropertyChanged(); }
        }
        string pressure = string.Empty;
        public string Pressure
        {
            get { return pressure; }
            set { pressure = value; OnPropertyChanged(); }
        }
        string humidity = string.Empty;
        public string Humidity
        {
            get { return humidity; }
            set { humidity = value; OnPropertyChanged(); }
        }

      

        ICommand getWeather;
        public ICommand GetWeatherCommand =>
                getWeather ??
                (getWeather = new Command(async () => await ExecuteGetWeatherCommand()));

        private async Task ExecuteGetWeatherCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                ClimaRoot ClimaC = null;
                ClimaRoot climaF = null;
                if (UseGPS)
                {
					
                    var gps = await CrossGeolocator.Current.GetPositionAsync(10000);
                    ClimaC = await climaService.GetWeather(gps.Latitude, gps.Longitude, Units.Metric);
                    climaF = await climaService.GetWeather(gps.Latitude, gps.Longitude, Units.Imperial);
                }
                else
                {
                    //Get weather by city
                    ClimaC = await climaService.GetWeather(Location.Trim(), Units.Metric);
                    climaF = await climaService.GetWeather(Location.Trim(), Units.Imperial);


                }

                Temp = $"Temp: {ClimaC?.MainWeather?.Temperature ?? 0}°C";
                TempF = $"Temp: {climaF?.MainWeather?.Temperature ?? 0}°F";
                Condition = $"{ClimaC.Name}: {ClimaC?.Weather?[0]?.Description ?? string.Empty}";
                MaxTemp = "Max Temp: " + ClimaC.MaxTemp.ToString()+ "°C";
                MinTemp = "Min Temp: " + ClimaC.MinTemp.ToString() + "°C"; ;
                Pressure = "Presion: "+ClimaC.Pression.ToString()+" Velocidad: "+ClimaC.Wind.Speed+" Direccion: " +ClimaC.Wind.WindDirectionDegrees + "°"; ;
                

                //CrossTextToSpeech.Current.Speak(Temp + " " + Condition);
            }
            catch (Exception ex)
            {
                Temp = "Unable to get Clima";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
