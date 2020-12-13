using System;
using System.Collections.Generic;

namespace Main
{
    class Program
    {

        static void Main(string[] args)
        {

            var CraigScraper = new WebScraper.Scraper();

            CraigScraper.OptionScheme = "https";
            CraigScraper.OptionHost = "neocities.org";
            CraigScraper.OptionPath = "/browse?page=90";
            CraigScraper.OptionxPath = "/html/body/div/div[2]/ul/li/div[1]/a";
            CraigScraper.OptionNextxPath = "//a[@class='next_page']";
            CraigScraper.OptionSearchTerm = "old";

            foreach(var v in CraigScraper.scrape()){

                Console.WriteLine(v.Item1 + ", " + v.Item2);

            }

            Console.ReadKey();

        }

    }
}
