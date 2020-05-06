using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Visitare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutesPage : ContentPage
    {
        public RoutesPage()
        {
            InitializeComponent();
            GetRoutes();
        }
        
        private async void GetRoutes()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://dearjean.ddns.net:44301/api/Routes");
            var trasy = JsonConvert.DeserializeObject<List<Routes>>(response);
            routesList.ItemsSource = trasy;
        }
        private async void MethodSearchRoutes(object sender, TextChangedEventArgs e)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://dearjean.ddns.net:44301/api/Routes");
            var trasy = JsonConvert.DeserializeObject<List<Routes>>(response);
            routesList.ItemsSource = trasy;

            var keyword = SearchRoutes.Text;
            var result = trasy.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));
            routesList.ItemsSource = result;
        }
        private async void OnTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = (Routes)e.Item;
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://dearjean.ddns.net:44301/api/points");
            List<Points> punkty = JsonConvert.DeserializeObject<List<Points>>(response);
            List<Points> list = new List<Points>();
            foreach (Points tmp in punkty)
            {
                if(tmp.RouteId == tapped.Id)
                {
                    list.Add(tmp);
                }
            }
            await Navigation.PushAsync(new MainPage(new RoutePoints(list)));

        }
    }
}