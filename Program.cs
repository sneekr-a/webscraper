using System;

namespace webscraper
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://sanangelo.craigslist.org/d/free-stuff/search/zip"; // url
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);

            foreach(var item in doc.DocumentNode.SelectNodes("//a[@class='result-title hdrlnk']")){
                Console.WriteLine(item.InnerText);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
