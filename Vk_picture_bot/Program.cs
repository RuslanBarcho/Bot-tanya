using System;
using VkNet.Model.RequestParams;
using VkNet.Model.Attachments;

namespace VkNet
{
    class MainClass
    {
        static VkNet.VkApi api;
        static bool ifchat = false;
        static string city;
        static Vk_picture_bot.Properties.Weather weather = new Vk_picture_bot.Properties.Weather();
        static System.Collections.Generic.List<MediaAttachment> emptyAttachments = new System.Collections.Generic.List<MediaAttachment>();
        public static void Main(string[] args){
            try{
                login();
                Console.WriteLine("login successful");
                Console.WriteLine("press any key to stop bot");
                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromSeconds(1);

                var timer = new System.Threading.Timer((e) =>
                {
                    checkls();
                }, null, startTimeSpan, periodTimeSpan);

            } catch(System.Exception e){
                Console.WriteLine("error" +e);
            }
            Console.ReadLine();
        }

        public static void sendPhoto(long? ID, int ownerID, Enums.SafetyEnums.PhotoAlbumType album, string message, ulong? count, int photoAmount){
            try{
                var get = api.Photo.Get(new PhotoGetParams()
                {
                    OwnerId = ownerID,
                    AlbumId = album,
                    Count = count,
                    Extended = true,
                    Reversed = true
                });
                System.Random random = new System.Random();
                System.Collections.Generic.List<MediaAttachment> attachments = new System.Collections.Generic.List<MediaAttachment>();
                int index = random.Next(0, get.Count - 1);
                for (int c = 0; c < photoAmount; c++){
                    attachments.Add(new Photo
                    {
                        OwnerId = ownerID,
                        Id = get[index + c].Id
                    });
                }
                SendPrivateMessage(ID, message, attachments);
            } catch (System.Exception){
                Console.WriteLine("ошибка");
            }
        }
       
        public static void sendVideo(long? ID, int ownerID, string message, long? count){
            try{
                var get = api.Video.Get(new VideoGetParams()
                {
                    OwnerId = ownerID,
                    Count = count
                });
                System.Random random = new System.Random();
                System.Collections.Generic.List<MediaAttachment> attachments = new System.Collections.Generic.List<MediaAttachment>();
                attachments.Add(new Video
                {
                    OwnerId = ownerID,
                    Id = get[random.Next(0, get.Count - 1)].Id
                });
                SendPrivateMessage(ID, message, attachments);
            } catch (System.Exception){
                Console.WriteLine("ошибка");
            }
        }
        public static void sendWallPostText(long? ID, string domain, ulong count){
            try{
                var get = api.Wall.Get(new WallGetParams()
                {
                    Domain = domain,
                    Count = count,
                });
                System.Random random = new System.Random();
                SendPrivateMessage(ID, get.WallPosts[random.Next(0, get.WallPosts.Count - 1)].Text, emptyAttachments);
            } catch (System.Exception){
                
            }
        }

        public static void checkls(){
            try{
                var get = api.Messages.Get(new MessagesGetParams
                {
                    Count = 50,
                    Filters = Enums.MessagesFilter.All,
                    PreviewLength = 0
                });
                foreach (var i in get.Messages)
                {
                    try
                    {
                        string message = i.Body;
                        System.Text.StringBuilder st = new System.Text.StringBuilder(message);
                        st.Replace("Т", "т");
                        message = st.ToString();
                        if ((message.Substring(0, 4).Equals("таня")) & message[4] != ',')
                        {
                            System.Text.StringBuilder str = new System.Text.StringBuilder(message);
                            str.Insert(4, ",");
                            message = str.ToString();
                        }
                        if (message.Equals("помощь") || message.Equals("Помощь") || message.Equals("Help")) message = "таня, команды";
                        if (message.Equals("Годно") || message.Equals("годно")) message = "таня, годно";
                        if (message.Contains("таня, погода")) {
                            city = message;
                            message = "таня, погода";
                        }
                        if (message.Contains("хардбас") | message.Contains("Хардбас")) message = "таня, хардбас";
                        if (i.ReadState == 0 & message.Substring(0, 5) == "таня,")
                        {
                            if (i.ChatId == null){
                                ifchat = false;
                                Console.WriteLine(i.Body + " от пользователя " + i.UserId);
                                System.Collections.Generic.List<long> id = new System.Collections.Generic.List<long>();
                                id.Add((long)i.Id);
                                var markAsRead = api.Messages.MarkAsRead(id, i.ChatId.ToString());
                                checkCommand(message, i.UserId);
                            } else {
                                ifchat = true;
                                System.Collections.Generic.List<long> id = new System.Collections.Generic.List<long>();
                                id.Add((long)i.Id);
                                var markAsRead = api.Messages.MarkAsRead(id, (i.ChatId + 2000000000).ToString());
                                Console.WriteLine(i.Body + " от беседы " + i.ChatId);
                                checkCommand(message, i.ChatId);
                            }
                        }
                    }
                    catch (System.Exception)
                    {

                    }
                }
            } catch (System.Exception){
            }
        }

        public static void checkCommand(string message, long? id){
            switch(message){
                case "таня, тест":{
                        SendPrivateMessage(id, "ответ на тест (тест бота)", emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, спасибо":
                    {
                        SendPrivateMessage(id, "Всегда пожалуйста ^^", emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, хардбас":
                    {
                        SendPrivateMessage(id, "&#129305;", emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, оскорбление":
                    {
                        SendPrivateMessage(id, "Ты пидор", emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, годно":
                    {
                        SendPrivateMessage(id, "Рада стараться ^^", emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, а":
                    {
                        sendWallPostText(id, "jumoreski", 100);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, стори":
                    {
                        sendWallPostText(id, "just_str", 100);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, масть":
                    {
                        Random rand = new Random();
                        string[] masti = { "петух", "чушка", "черт", "вафлер", "мусор", "шестерка", "фуфлыжник", "козел", "мужик", "стремящийся", "блатной", "блатной" };
                        SendPrivateMessage(id, masti[rand.Next(0,11)], emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, картинка":
                    {
                        sendPhoto(id, 264837796, Enums.SafetyEnums.PhotoAlbumType.Saved, "твоя пикча ^^", 200, 1);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, мем":
                    {
                        sendPhoto(id, -80353691, Enums.SafetyEnums.PhotoAlbumType.Wall, "Деградировать подано", 500, 1);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, котики":
                    {
                        sendPhoto(id, -122103467, Enums.SafetyEnums.PhotoAlbumType.Wall, "Мур :3", 500, 3);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, тянки":
                    {
                        sendPhoto(id, -130914885, Enums.SafetyEnums.PhotoAlbumType.Wall, "&#128527;", 500, 3);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, 2д":
                    {
                        sendPhoto(id, -159407878, Enums.SafetyEnums.PhotoAlbumType.Wall, "&#128527;", 500, 3);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, погода":
                    {
                        weather.Update(city);
                        SendPrivateMessage(id, "Location: " + weather.location + "\n" +
                                           "Temperature: " + weather.value.ToString() + " °C\n" +
                                           "Type: " + weather.description, emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, расписание":
                    {
                        Vk_picture_bot.Schedule schedule = new Vk_picture_bot.Schedule();
                        schedule.Get();
                        SendPrivateMessage(id, schedule.nameOddStrFull, emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, webm":
                    {
                        sendVideo(id, -30316056, "Твой webm", 200);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                case "таня, команды":
                    {
                        SendPrivateMessage(id, "Все, что я пока умею: \n" +
                                           "1) Тест (тестирование бота)\n" +
                                           "2) Оскорбление (могу обозвать тебя)\n" +
                                           "3) Картинка (скину тебе что-то из сохраненок на свой выбор :3)\n" +
                                           "4) Мем (пришлю мем из паблика Unical Network Memes)\n" +
                                           "5) Масть (скажу кто ты по жизни)\n" +
                                           "6) Котики (пришлю милого котейку :3)\n" +
                                           "7) Webm (пришлю webm видосик)\n" +
                                           "8) Тянки (пришлю тяночек^^)\n" +
                                           "9) 2д (пришлю 2д тяночек &#128527;)\n" +
                                           "10) Погода *город* BETA (скажу какая сейчас погода) ", emptyAttachments);

                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
                default: {
                        SendPrivateMessage(id, "Такой команды, к сожалению, нет. Для получение списка команд напиши мне 'Таня, команды'", emptyAttachments);
                        Console.WriteLine("бот ответил пользователю " + id);
                    }
                    break;
            }
        }

        public static void SendPrivateMessage(long? ID, string message, System.Collections.Generic.List<MediaAttachment> attachments)
        {
            try
            {
                if (ifchat)
                {
                    var sendc = api.Messages.Send(new MessagesSendParams
                    {
                        ChatId = ID,
                        Message = message,
                        Attachments = attachments
                    });
                }
                else
                {
                    var sendm = api.Messages.Send(new MessagesSendParams
                    {
                        UserId = ID,
                        Message = message,
                        Attachments = attachments
                    });
                }
            }
            catch (VkNet.Exception.CaptchaNeededException e)
            {
                Console.WriteLine(e.Sid.ToString(), e.Img);
            }
        }

        public static void login(){
            ulong appId = 6236745;
            string email = "79166054162";
            string password = "Ruslan99";
            VkNet.Enums.Filters.Settings settings = VkNet.Enums.Filters.Settings.All;
            api = new VkNet.VkApi();
            api.Authorize(new VkNet.ApiAuthParams
            {
                ApplicationId = appId,
                Login = email,
                Password = password,
                Settings = settings
            });
        }
   }
}
