using System;
using System.Collections;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var url = "https://www.pja.edu.pl";

            bool result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);


            if (result)
            {
                await GetHttpASyncAsync(url);
            }
            else
            {
                throw new ArgumentException("To nie jest adres URL");
            }
        }

        private static async Task GetHttpASyncAsync(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var html = await response.Content.ReadAsStringAsync();
                    var regex = new Regex("[a-z0-9]+@[a-z]+");
                    var matches = regex.Matches(html);

                    if (matches.Count == 0)
                    {
                        Console.WriteLine("Nie znaleziono adresów email");
                    }
                    else
                    {

                        //Chcialem zastosowac cos w stylu seta, tj taka strukture ktora przyjmuje tylko unikalne wartosci
                        //Niestety w C# to jakos dziwnie chodzi - nie chialo przeniesc matches do seta do matches nie jest stringami
                        //Hashtable przyjmuje unikalne key i na tym opiera sie eliminacja mnogich wystapien. 
                        //Ale musze tez dodawac value, bo Hashtable potrzebuje key-value
                        //a dodatkowo nie dziala var h = new HashSet<string>(matches.ToString());
                        //No wolalbym normalnego seta ale C# jest dla mnie nowy w porownaniu z Java i dalo sie na razie tylko tak
                        //Ale moze to nie jest jakies pogorszenie - nie znam jeszcze struktur danych w C#, na piewrszy rzut oka
                        //wychodzi ze jakby bylo ich mniej niz w Javie ale moze dlatego ze nie znam wlasnie. 

                        Hashtable hash = new Hashtable();

                        foreach (var match in matches)
                        {
                            var s = match.ToString();
                            if (hash.Contains(s) == false)
                            {
                                hash.Add(s, string.Empty);
                            }

                        }
                        foreach (DictionaryEntry entry in hash)
                        {
                            Console.WriteLine("Znalezione unikalne adresy: " + hash.Count);
                            Console.WriteLine(entry.Key);
                        }
                    }
                }


                httpClient.Dispose();

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\n Blad pobierania strony");
                Console.WriteLine("Message :{0} ", e.Message);
            }

        }
    }
}
