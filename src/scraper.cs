using System;
using System.Collections.Generic;

namespace Main
{
    class Program
    {

        static void Main(string[] args)
        {

            var CraigScraper = new WebScraper.Scraper();

            CraigScraper.optionScheme = "https";
            CraigScraper.optionHost = "neocities.org";
            CraigScraper.optionPath = "/browse?page=90";
            CraigScraper.optionxPath = "/html/body/div/div[2]/ul/li/div[1]/a";
            CraigScraper.optionNextxPath = "//a[@class='next_page']";
            CraigScraper.optionSearchTerm = "old";

            foreach(var v in CraigScraper.scrape()){

                Console.WriteLine(v.Item1 + ", " + v.Item2);

            }

            Console.ReadKey();

        }

    }
}
