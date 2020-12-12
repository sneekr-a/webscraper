using System;
using System.Collections.Generic;

namespace webscraper
{
    class Program
    {
        /*performs a linear search*/
        static private bool newInList(string s, List<string> v){

            foreach(var item in v){
                if(item == s){
                    return false;
                }
            }

            return true;
        }

        static void Main(string[] args)
        {

            string baseurl = "https://boston.craigslist.org";
            string url = "/d/for-sale/search/sss?sort=date&"; // url
            string nxurl = baseurl + url;
            string nextButton = "//a[@class='button next']";
            string xpath = "//a[@class='result-title hdrlnk']";

            Console.WriteLine("Please enter a search term.");
            string searchTerm = Console.ReadLine().ToLower();

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc;

            List<string> newItemList = new List<string>();

            while(nxurl != "https://boston.craigslist.org"){

                doc = web.Load(nxurl);

                foreach(var item in doc.DocumentNode.SelectNodes(xpath)){
                    
                    if(item.InnerText.ToLower().Contains(searchTerm)){

                        if(newInList(item.InnerText, newItemList)){
                            Console.WriteLine("New item : " + item.InnerText);
                            newItemList.Add(item.InnerText);
                        }

                    }

                }

                nxurl = baseurl + doc.DocumentNode.SelectSingleNode(nextButton).GetAttributeValue("href", url);

            }

            Console.WriteLine("Query completed. Press any key to exit.");
            Console.ReadKey();

        }

    }
}
