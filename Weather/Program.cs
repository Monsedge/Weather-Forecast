using System;
using MySql.Data.MySqlClient;
using HtmlAgilityPack;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace Weather
{

    class Program
    {
        static string connectionString;

        static bool work;

        static List<string> urls = new List<string>();
        static List<string> dates = new List<string>();
        static List<string> minTemperatures = new List<string>();
        static List<string> maxTemperatures = new List<string>();
        static List<string> descriptions = new List<string>();
        static List<string> names = new List<string>();

        static void Clear()
        {
            urls.Clear();
            dates.Clear();
            minTemperatures.Clear();
            maxTemperatures.Clear();
            descriptions.Clear();
            names.Clear();
        }
        static void GetConnectionString()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }
        static void GetMainData()
        {
            Console.Write("Получение ссылок на города...");
            HtmlWeb webDoc = new HtmlWeb();
            HtmlDocument doc = webDoc.Load("https://www.gismeteo.ru/catalog/russia/");
            HtmlNodeCollection LinkNodes = doc.DocumentNode.SelectNodes("//section[@class='catalog_block catalog_popular']//a");
            if (LinkNodes != null)
            {
                foreach (HtmlNode node in LinkNodes)
                    urls.Add("https://www.gismeteo.ru" + node.Attributes["href"].Value + "month/");
                Console.WriteLine("Ссылки получены\nПолучение данных по каждому городу...");
                foreach (string url in urls)
                    GetSpecificData(url);
                InsertValue();
            }
            else Console.Write("Ошибка: пустая коллекция ссылок\n");
        }

        static void GetSpecificData(string url)
        {
            Console.WriteLine($"обрабатываю {url}");
            HtmlWeb webDoc = new HtmlWeb();
            HtmlDocument doc = webDoc.Load(url);

            //description
            HtmlNodeCollection DescriptionNodes = doc.DocumentNode.SelectNodes("//div[@class='weather-cells']/div[@data-text]");
            if (DescriptionNodes != null)
            {
                foreach (HtmlNode node in DescriptionNodes)
                    descriptions.Add(node.Attributes["data-text"].Value);
            }
            else Console.WriteLine("Ошибка: пустая коллекция DescriptionNodes");

            //date
            HtmlNodeCollection DateNodes = doc.DocumentNode.SelectNodes("//div[@class='weather-cells']/div[@data-text]/div[1]/div[1]");
            if (DateNodes != null)
            {
                string someMonth = "SomeMonth";
                for (int i = 0; i < DateNodes.Count; i++)
                {
                    if (DateNodes[i].InnerText.Trim().Split(" ").Count() == 2)
                    {
                        someMonth = DateNodes[i].InnerText.Trim().Split(" ")[1];
                        dates.Add(DateNodes[i].InnerText.Trim());
                    }
                    else dates.Add(DateNodes[i].InnerText.Trim() + " " + someMonth);
                }

                //city name            
                HtmlNode NameNode = doc.DocumentNode.SelectSingleNode("//div[@class='pageinfo_title index-h1']//h1");
                if (NameNode != null)
                {
                    for (int i = 0; i < DateNodes.Count; i++)
                        names.Add(NameNode.InnerText.Replace(" на месяц", ""));
                }
                else Console.WriteLine("Ошибка: NameNode не обнаружен");
            }
            else Console.WriteLine("Ошибка: пустая коллекция DateNodes");

            //max temperature
            HtmlNodeCollection MaxTemperatureNodes = doc.DocumentNode.SelectNodes("//div[@class='weather-cells']/div[@data-text]//div[@class='temp_max js_meas_container']//span[@class='value unit unit_temperature_c']");
            if (MaxTemperatureNodes != null)
            {
                foreach (HtmlNode node in MaxTemperatureNodes)
                    maxTemperatures.Add(node.InnerText.Trim().Replace("&minus;", "-"));
            }
            else Console.WriteLine("Ошибка: пустая коллекция MaxTemperatureNodes");

            //min temperature
            HtmlNodeCollection MinTemperatureNodes = doc.DocumentNode.SelectNodes("//div[@class='weather-cells']/div[@data-text]//div[@class='temp_min js_meas_container']//span[@class='value unit unit_temperature_c']");
            if (MinTemperatureNodes != null)
            {
                foreach (HtmlNode node in MinTemperatureNodes)
                    minTemperatures.Add(node.InnerText.Trim().Replace("&minus;", "-"));
            }
            else Console.WriteLine("Ошибка: пустая коллекция MinTemperatureNodes");

            

            Thread.Sleep(100);
        }


        static void InsertValue()
        {
            MySqlConnection connect = new MySqlConnection(connectionString);
            try
            {
                Console.Write("Обновление базы данных...");

                connect.Open();
                string sql = "TRUNCATE TABLE weathers";                
                MySqlCommand command = new MySqlCommand(sql, connect);
                command.ExecuteNonQuery();

                for (int i = 0; i < dates.Count; i++)
                {
                    sql = "INSERT INTO weathers (dates,names,minTemperatures,maxTemperatures,descriptions) VALUES" +
                                    $"('{dates[i]}','{names[i]}','{minTemperatures[i]}','{maxTemperatures[i]}','{descriptions[i]}');";
                    command = new MySqlCommand(sql, connect);
                    command.ExecuteNonQuery();
                }
                connect.Close();
                Console.WriteLine("База данных обновлена!");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
        }

        static void Main()
        {
            GetConnectionString();

            Timer timer = new Timer(Start, null, 0, 30000);

            Console.ReadLine();
        }
        static void Start(object obj)
        {
            if (work) return;
            work = !work;

            Clear();
            GetMainData();
            Console.WriteLine(new string('-', 75));

            work = false;
        }
    }
}
