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
    public partial class QuestionPage : ContentPage
    {
        private Question question;

        public bool Result { get; set; }

        public QuestionPage(Question question)
        {
            InitializeComponent();
            BindingContext = question;
            this.question = question;

            Result = false;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.ItemIndex == question.GoodAnswer)
            {
                Result = true;
            }

            Navigation.PopModalAsync();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var question = new Question
            {
                Answers = new List<string> {
                odpowiedzEntry1.Text, odpowiedzEntry2.Text, odpowiedzEntry3.Text, odpowiedzEntry4.Text },
                GoodAnswer = Convert.ToInt16(odpowiedzPrawidlowa.Text),
                Question1 = zagadkaEntry.Text
            };
            var questionPage = new QuestionPage(question);
            questionPage.Disappearing += QuestionPageClosed;

            await Navigation.PushModalAsync(questionPage);

            var json = JsonConvert.SerializeObject(new
            {
                Answers = new List<string> {
                odpowiedzEntry1.Text, odpowiedzEntry2.Text, odpowiedzEntry3.Text, odpowiedzEntry4.Text },
                GoodAnswer = Convert.ToInt16(odpowiedzPrawidlowa.Text),
                Question1 = zagadkaEntry.Text
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var result = await client.PostAsync("http://dearjean.ddns.net:44301/api/Test4", content);
        }

       private async void QuestionPageClosed(object sender, EventArgs e)
       {
         var questionPage = sender as QuestionPage;
         await DisplayAlert("", "Człowiek odpowiedział: " + (questionPage.Result ? "dobrze" : "źle"), "OK");
       }
    }
}