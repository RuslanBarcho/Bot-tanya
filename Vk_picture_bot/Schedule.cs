using System;
namespace Vk_picture_bot
{
    public class Schedule
    {
        public System.Collections.Generic.List<string> nameOdd = new System.Collections.Generic.List<string>();
        public string nameOddStrFull;
        bool success = true;
        public Schedule()
        {
        }
        public void Get(){
            var client = new RestSharp.RestClient("http://mirea.feed4rz.ru/api");
            var request = new RestSharp.RestRequest("schedule/get", RestSharp.Method.POST);
            request.AddParameter("institute", 0);
            request.AddParameter("group", "ikbo-02-17");
            RestSharp.IRestResponse responce = client.Execute(request);
            //Console.WriteLine(responce.Content);
            string JSON = responce.Content;
            ParceJSON(JSON);
            if (success){
                int num = 1;
                for (int i = 0; i < 36; i++)
                {
                    if (i == 0) nameOddStrFull = nameOddStrFull + "\n" + "Понедельник:";
                    if (i == 6) {
                        nameOddStrFull = nameOddStrFull + "\n" + "\n" + "Вторник:";
                        num = 1;
                    }
                    if (i == 12) {
                        nameOddStrFull = nameOddStrFull + "\n"+ "\n" + "Среда:";
                        num = 1;
                    }
                    if (i == 18) {
                        nameOddStrFull = nameOddStrFull + "\n" + "\n" + "Четверг:";
                        num = 1;
                    }
                    if (i == 24) {
                        nameOddStrFull = nameOddStrFull + "\n" + "\n" + "Пятница:";
                        num = 1;
                    }
                    if (i == 30) {
                        nameOddStrFull = nameOddStrFull + "\n" + "\n" + "Суббота:";
                        num = 1;
                    }
                    nameOddStrFull = nameOddStrFull + "\n" + num + ") " + nameOdd[i];
                    num++;
                }
            }
        }

        public void ParceJSON(string JSON2parce)
        {
            nameOdd.Clear();
            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(JSON2parce);
            success = obj.success;
            if (success)
            {
                var response = obj.response;
                var schedule = response.schedule;
                var days = schedule.days;
                foreach (var d in days)
                {
                    foreach (var da in d)
                    {
                        var odd = da.odd;
                        string name = odd.name;
                        nameOdd.Add(Replace(name));
                    }
                }
            }
            else nameOddStrFull = "Ошибка";
        }

        public string Replace(string obj)
        {
            if (obj == null)
            {
                return "-";
            }
            else
            {
                return obj;
            }
        }
    }
}
