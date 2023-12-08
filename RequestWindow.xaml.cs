using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AISLab5
{
    /// <summary>
    /// Логика взаимодействия для RequestWindow.xaml
    /// </summary>
    /// строго типизированные объекты
    public class User
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
    public class Rootobject
    {
        public Response response { get; set; }
    }

    public class Response
    {
        public int id { get; set; }
        public string home_town { get; set; }
        public string status { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string bdate { get; set; }
        public Country country { get; set; }
        public string phone { get; set; }
    }

    public class Country
    {
        public int id { get; set; }
        public string title { get; set; }
    }

    public class myFriends
    {
        public int[] response { get; set; }
    }


    public partial class RequestWindow : Window
    {
        private MainWindow mw;
        private string f; //строка для хранения данных
        public RequestWindow(MainWindow MW)
        {
            InitializeComponent();
            mw = MW;
            f = "";
        }

        private void Button_Click_Data(object sender, RoutedEventArgs e)
        {
            string reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=5.154";
            string method = "account.getProfileInfo";
            f = mw.GET(reqStrTemplate, method, mw.Access_token);
            var user = JsonSerializer.Deserialize<Rootobject>(f).response;
            string[] list =
            {
                "id: " + user.id.ToString(),
                "status: " + user.status,
                "lastname: " + user.last_name,
                "firstname: " + user.first_name,
                "birth date: " + user.bdate,
                "phone number: " + user.phone,
                "country: " + user.country.title
            };
            UserInformationTextBox.Text = string.Join("\n", list);

        }

        private void Button_Click_Friends(object sender, RoutedEventArgs e)
        {
            string reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=5.154&user_id=" + mw.UserID;
            string method = "friends.getOnline";
            f = mw.GET(reqStrTemplate, method, mw.Access_token);
            var ArrayOfFriends = JsonSerializer.Deserialize<myFriends>(f).response;

            reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=5.154&user_ids=" + string.Join(",", ArrayOfFriends.Select(x => x.ToString()));
            method = "users.get";
            f = mw.GET(reqStrTemplate, method, mw.Access_token);
            var friends = JsonDocument.Parse(f).RootElement.GetProperty("response");
            var Users = JsonSerializer.Deserialize<User[]>(friends);
            UserInformationTextBox.Text = string.Join("\n", Users.Select(x => x.last_name + " " + x.first_name));
        }
    }
}



#region gift
/*
public class GiftItem
{
    public int id { get; set; }
    public string message { get; set; }
    public long date { get; set; }
}

public class Rootobject
{
    public Response response { get; set; }
    public List<GiftItem> gifts { get; set; }

}

string reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=5.154" + mw.UserID;
string method = "gifts.get";
f = mw.GET(reqStrTemplate, method, mw.Access_token);
var giftsResponse = JsonSerializer.Deserialize<Rootobject>(f);

var giftsList = giftsResponse.gifts;
UserInformationTextBox.Clear();
foreach (var gift in giftsList)
{
    string[] giftInfo =
    {
                    $"Gift ID: {gift.id}",
                    $"Message: {gift.message}",
                    $"Date: {DateTimeOffset.FromUnixTimeSeconds(gift.date).ToString()}",
                };
    UserInformationTextBox.AppendText(string.Join("\n", giftInfo) + "\n\n");
}
*/
#endregion