using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Visitare.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Visitare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage : ContentPage
    {
        public List<Points> routePoints = new List<Points>();
        public CreatorPage()
        {
            InitializeComponent();
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.010281, 18.604922), Distance.FromMiles(1.0)));
        }
        private void OnClearClicked(object sender, EventArgs e)
        {
            customMap.Pins.Clear();
            customMap.MapElements.Clear();
            routePoints.Clear();
        }
        private async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(nazwaEntry.Text))
            {
                await DisplayAlert("Błąd", "Podaj nazwę punktu", "Ok");
                return;
            }

            CustomPin pin = new CustomPin
            {
                Type = PinType.SavedPin,
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
                Label = nazwaEntry.Text,
                Address = opisEntry.Text,
                Name = "Xamarin",
                Url = "http://xamarin.com/about/",
                Question = zagadkaEntry.Text,
                Answer = odpowiedzEntry.Text
            };

            pin.MarkerClicked += async (s, args) =>
            {
                args.HideInfoWindow = true;
                string pinName = ((CustomPin)s).Label;
                // string pytanie = ((CustomPin)s).Question;
                string opis = ((CustomPin)s).Address;
                // string odpowiedz = ((CustomPin)s).Answer;
                if(await DisplayAlert($"{pinName}", $"{opis}","Quiz", "Anuluj"))
                {
                    await Navigation.PushAsync(new QuestionPage(new Question()));
                }
          
              
            };
            customMap.CustomPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin);
            routePoints.Add(new Points()
            {
                X = e.Position.Latitude,
                Y = e.Position.Longitude,
                RouteId = 1,
                Name = nazwaEntry.Text,
                Description = opisEntry.Text
            });

            /*var json = JsonConvert.SerializeObject(new Points()
            {
                X = e.Position.Latitude,
                Y = e.Position.Longitude,
                RouteId = 1,
                Name = nazwaEntry.Text,
                Description = opisEntry.Text
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var result = await client.PostAsync("http://dearjean.ddns.net:44301/api/Points3", content);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Komunikat", "Dodanie puntku przebiegło pomyślnie", "Ok");
            }
            else
                await DisplayAlert("Błąd", "Spróbuj ponownie później", "Ok");*/

        }
        private async void OnNewRouteClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(trasaEntry.Text))
            {
                await DisplayAlert("Błąd", "Podaj nazwę trasy", "Ok");
                return;
            }
            List<int> idList = new List<int>();
            foreach (Points tmp in routePoints)
            {
                var json = JsonConvert.SerializeObject(tmp);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                var result = await client.PostAsync("http://dearjean.ddns.net:44301/api/Points3", content);
                var pointResult = JsonConvert.DeserializeObject<Points>(result.Content.ReadAsStringAsync().Result);
                if (result.StatusCode != HttpStatusCode.Created)
                {
                    await DisplayAlert("Błąd", "Spróbuj ponownie później", "Ok");
                    Debug.WriteLine(result);
                    return;
                }
                idList.Add(pointResult.Id);
            }
            var model = new ChangeModel
            {
                UpdateList = idList,
                Name = trasaEntry.Text,
                Description = opistrasyEntry.Text
            };
            var json2 = JsonConvert.SerializeObject(model);
            var content2 = new StringContent(json2, Encoding.UTF8, "application/json");
            HttpClient client2 = new HttpClient();
            await client2.PostAsync("http://dearjean.ddns.net:44301/api/Points3/Change", content2);
            

        }

    }
}