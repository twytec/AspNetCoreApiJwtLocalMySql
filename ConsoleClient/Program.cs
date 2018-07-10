using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CreateUser(new SharedModels.ViewModels.RegisterViewModel() { Email = "mymail@mail.com", Password = "Test123!" }).Wait();
            var token = GetToken(new SharedModels.ViewModels.LoginViewModel() { Email = "mymail@mail.com", Password = "Test123!" }).Result;
            GetUserInfo(token).Wait();
        }

        static async Task CreateUser(SharedModels.ViewModels.RegisterViewModel model)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var resp = await client.PostAsync("http://localhost:5000/api/auth/register", theContent);
        }

        static async Task<string> GetToken(SharedModels.ViewModels.LoginViewModel model)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var resp = await client.PostAsync("http://localhost:5000/api/auth/login", theContent);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await resp.Content.ReadAsStringAsync();
            }

            return "";
        }

        static async Task GetUserInfo(string token)
        {
            //Trim first and last "
            var t = token.Substring(1, token.Length - 2);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", t);

            var resp = await client.GetAsync("http://localhost:5000/api/auth/test");
        }
    }
}
