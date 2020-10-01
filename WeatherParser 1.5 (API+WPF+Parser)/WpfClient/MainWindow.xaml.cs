using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.IO;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string path = "https://localhost:44354/api/weather";
        static string selectedCity, selectedDate;
        public MainWindow()
        {
            InitializeComponent();
            this.RefreshButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CityCombo.Visibility = Visibility.Hidden;
            DateCombo.Visibility = Visibility.Hidden;
            if (CityCombo.SelectedIndex > -1)
            {
                selectedDate = DateCombo.SelectedItem.ToString();
                selectedCity = CityCombo.SelectedItem.ToString();
                DateCombo.Items.Clear();
                CityCombo.Items.Clear();
            }
            string resp = RequestInfo($"{path}/datelist");
            if (resp.StartsWith("ERROR"))
                CityData.Text = resp;
            else
            {
                string[] array = resp.Split(',');
                if (!array.Contains(selectedDate))
                    selectedDate = array[0];
                foreach (string date in array)
                    DateCombo.Items.Add(date);
                DateCombo.SelectedItem = selectedDate;
                //DateCombo.Items.Add("test");

                resp = RequestInfo($"{path}/citylist");
                if (resp.StartsWith("ERROR"))
                    CityData.Text = resp;
                else
                {
                    array = resp.Split(',');
                    if (!array.Contains(selectedCity))
                        selectedCity = "Погода в Москве";
                    foreach (string city in array)
                        CityCombo.Items.Add(city);
                    CityCombo.SelectedItem = selectedCity;

                    CityCombo.Visibility = Visibility.Visible;
                    DateCombo.Visibility = Visibility.Visible;
                }
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityCombo.SelectedIndex > -1)
            {
                string resp = RequestInfo($"{path}/info?name={CityCombo.SelectedItem}&date={DateCombo.SelectedItem}");
                if (resp.StartsWith("ERROR"))
                {
                    CityCombo.Visibility = Visibility.Hidden;
                    DateCombo.Visibility = Visibility.Hidden;
                }
                CityData.Text = resp;
            }
        }
        public static string RequestInfo(string uri)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream istrm = resp.GetResponseStream();
                StreamReader rdr = new StreamReader(istrm);
                return rdr.ReadToEnd();
            }
            catch (Exception exc)
            {
                return "ERROR: " + exc.Message;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }


    }
}
