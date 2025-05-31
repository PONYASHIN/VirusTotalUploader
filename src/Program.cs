using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VTuploader
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //проверяем есть не просто ли мы пустышку запускаем
            if (args.Count() > 0)
            {
                //проверка наличия апи ключа
                if (ApiKey == null)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new VTUploader(args));
                    return;
                }
                UploadFileAsync(args[0]).Wait();
            }
        }
        //api ключ для ВТ
        private static string ApiKey = Environment.GetEnvironmentVariable("VT_key", EnvironmentVariableTarget.User);
        public static async Task UploadFileAsync(string filePath)
        {
            //настраиваем хттп клиент
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("x-apikey", ApiKey);
            //создаём поток содержания файла
            StreamContent fileContent = new StreamContent(File.OpenRead(filePath));
            //создаём контейнер для файла и добавляем сам файл
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(fileContent, "file", Path.GetFileName(filePath));
            //шоб скрыть ошибки
            try
            {
                //отправляем контейнер
                var response = await httpClient.PostAsync("https://www.virustotal.com/api/v3/files", formData);
                response.EnsureSuccessStatusCode();
                //если ответ удачный
                if (response.IsSuccessStatusCode)
                {
                    //читаем ответ
                    var responseContent = await response.Content.ReadAsStringAsync();
                    //используем regex как костыль чтобы парсить json ответ
                    Regex regex = new Regex("id\": \"\\w*\\S*\"");
                    string link = "https://www.virustotal.com/gui/file-analysis/" + regex.Match(responseContent).ToString().Replace("id\": \"", "").Replace("\"", "");
                    //запускаем браузер с ссылкой на файл
                    Process.Start(link);
                    return;
                }
                //на случай конфликта что файл уже есть
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    //получаем хешсумму файла
                    var stream = File.OpenRead(filePath);
                    SHA256 sha256 = SHA256.Create();
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    string fileId = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    //запускаем страницу
                    Process.Start($"https://www.virustotal.com/gui/file/{fileId}/detection");
                    return;
                }
            }
            catch{}
        }
    }
}
