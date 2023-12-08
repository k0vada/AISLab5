using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AISLab5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public string Access_token { get; set; }
        public string UserID { get; set; }


        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            string appId = "51794018";
            var uriStr = @"https://oauth.vk.com/authorize?client_id=" + appId +
                @"&scope=offline,friends&redirect_uri=https://oauth.vk.com/blank.html&display=page&v=5.6&response_type=token";
            Browser.AddressChanged += BrowserOnNavigated;
            Browser.Load(uriStr);
        }

        private void BrowserOnNavigated(object sender, DependencyPropertyChangedEventArgs e) // Получение токена и ID пользователя
        {
            var uri = new Uri((string)e.NewValue);
            if (uri.AbsoluteUri.Contains(@"oauth.vk.com/blank.html#")) // Cодержит ли новый адрес подстроку
            {                                       // "oauth.vk.com/blank.html#", что указывает на успешную авторизацию
                string url = uri.Fragment;
                url = url.Trim('#');
                Access_token = HttpUtility.ParseQueryString(url).Get("access_token");
                UserID = HttpUtility.ParseQueryString(url).Get("user_id");
                RequestWindow newWindow = new RequestWindow(this);
                newWindow.Show();
            }
        }

        public string GET(string Url, string Method, string Token) // метод для обработки запроса
        {
            WebRequest req = WebRequest.Create(string.Format(Url, Method, Token)); // добавляет метод и токен 
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            return Out;
        }
    }
}
