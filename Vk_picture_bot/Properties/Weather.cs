using System;
namespace Vk_picture_bot.Properties
{
    public class Weather
    {
        public double value = 0;
        public string location = "null";
        public string description = "null";
        public string input;
        string id = "524901"; //default value
        public System.Collections.Generic.List<City> cityList = new System.Collections.Generic.List<City>();
        string cityJSON;
        public Weather()
        {
            try
            {   
                using (System.IO.StreamReader sr = new System.IO.StreamReader("city.txt"))
                {
                    cityJSON = sr.ReadToEnd();
                    Console.WriteLine("JSON file read successfuly");
                }
                initilize();
            }
            catch (Exception e)
            {
                Console.WriteLine("The weather JSON file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        public void Update(string city){
            try{
                System.Text.StringBuilder str = new System.Text.StringBuilder(city);
                str.Remove(0, 13);
                city = str.ToString();
            } catch (Exception){
                //todo replace city with user's or use default
            }
            Console.WriteLine(city);
            foreach (var i in cityList){
                if (city.Equals(i.name)|city.Equals(i.name_lowecase)){
                    id = i.id;
                }
            }
            var client = new RestSharp.RestClient("http://api.openweathermap.org/data/2.5");
            var request = new RestSharp.RestRequest("weather?id="+id+"&APPID=cbe768adbe16ad6ce8c15294944172ac");
            RestSharp.IRestResponse responce = client.Execute(request);
            var content = responce.Content;
            //Console.WriteLine(content);
            Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JToken.Parse(content);
            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            var main = obj.main;
            value = main.temp - 273.15;
            var weather = obj.weather;
            description = weather[0].description;
            location = obj.name;
            Console.WriteLine("weather data updated");
           
        }

        public void initilize(){
            cityList.Clear();
            int counter = 0;
            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(cityJSON);
            var cities = obj.cities;
            foreach (var city in cities){
                cityList.Add(new City
                {
                    id = city.id,
                    name = city.name,
                    name_lowecase = city.name_lowercase
                });
                counter++;
            }
            Console.WriteLine("JSON initilized. " + counter + " cities");
        }
    }

    public class City
    {
        public string id;
        public string name;
        public string name_lowecase;
    }
}
