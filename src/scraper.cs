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
            CraigScraper.optionHost = "boston.craigslist.org";
            CraigScraper.optionPath = "/d/for-sale/search/sss?sort=date&";
            CraigScraper.optionxPath = "//a[@class='result-title hdrlnk']";
            CraigScraper.optionNextxPath = "//a[@class='button next']";
            CraigScraper.optionSearchTerm = "metal";

            Console.WriteLine(CraigScraper.optionScheme + "://" + CraigScraper.optionHost + CraigScraper.optionPath);

            foreach(var v in CraigScraper.scrape()){

                Console.WriteLine(v.Item1 + ", " + v.Item2);

            }

            Console.ReadKey();

        }

    }
}
