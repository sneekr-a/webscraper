using System;
using System.Collections.Generic;

namespace Main
{
    class Program
    {

        static void Main(string[] args)
        {

            var CraigScraper = new WebScraper.Scraper();

            /*
            CraigScraper.OptionScheme = "https";
            CraigScraper.OptionHost = "neocities.org";
            CraigScraper.OptionPath = "/browse?page=90";
            CraigScraper.OptionxPath = "//a[@class='result-title hdrlnk']";
            CraigScraper.OptionNextxPath = "//a[@class='next_page']";
            CraigScraper.OptionSearchTerm = "";
            */

            CraigScraper.loadConfigFile("config.txt");
            //all options should be loaded in fine now

            foreach(var v in CraigScraper.scrape()){

                Console.WriteLine(v.Item1 + ", " + v.Item2);

            }

            Console.ReadKey();

        }

    }
}
