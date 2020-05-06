using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Visitare.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Visitare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var uri = new Uri(string.Format("http://dearjean.ddns.net:44301/api/Account/Register", string.Empty));
            var client = new HttpClient();
            var model = new RegisterModel
            {
                Email = email.Text,
                Password = password.Text,
                ConfirmPassword = cpassword.Text
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Jest OK!");
                await Navigation.PushAsync(new LoginPage());
                await DisplayAlert("Sukces", "Rejestracja zakończona sukcesem", "OK");
            }
            else
            {
                Debug.WriteLine(response);
                await DisplayAlert("Błąd", "Spróbuj ponownie", "OK");
            }         
        }
    }
}