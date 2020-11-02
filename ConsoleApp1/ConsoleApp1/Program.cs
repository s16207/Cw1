using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks; 

namespace ConsoleApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            if (args.Length == 0)
            {
                throw new ArgumentNullException("Nie podano żadnego adresu strony");
            }
            var url = args[0];

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync();
                var regex = new Regex("[a-z0-9]+@[a-z]+");
                var matches = regex.Matches(html);


                foreach (var match in matches)
                {
                    Console.WriteLine(match.ToString());
                }
            }
        }
    }
}
