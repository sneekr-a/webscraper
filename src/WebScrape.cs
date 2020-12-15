using System;
using System.IO;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace WebScraper{

    class Scraper{

        private List<(string, string)> queryResults = new List<(string, string)>(); /*contains query results (url, item)*/
        public List<(string, string)> QueryResults {
            get{return queryResults;}
        }

        //all valid options
        public string OptionScheme {get; set;}
        public string OptionHost  {get; set;}
        public string OptionPath  {get; set;}
        public string OptionxPath  {get; set;}
        public string OptionNextxPath  {get; set;}
        public string OptionSearchTerm  {get; set;}
        public string OptionWriteFile  {get; set;}

        /*
        member function scrape performs a webscrape and stores items that include OptionSearchTerm in queryResults
        also returns queryResults
        */
        public List<(string, string)> scrape(){
            
            if(!allRequiredOptionsFilled()){ return new List<(string, string)>(){ ("none", "none") }; } /*checks if required options are filled*/

            queryResults.Clear(); /*clear current queryResults*/
            string hosturl = OptionScheme + "://" + OptionHost;
            string nexturl = hosturl + OptionPath; /*setup url*/

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;

            while(nexturl != hosturl){

                doc = web.Load(nexturl); /*load document*/
                Console.WriteLine($"Scraping {nexturl} . . .");

                foreach(var item in doc.DocumentNode.SelectNodes(OptionxPath)){
                    
                    if(item.InnerText.ToLower().Contains(OptionSearchTerm)){

                        queryResults.Add((nexturl, item.InnerText)); /*add terms to query*/

                    }

                }

                if(doc.DocumentNode.SelectSingleNode(OptionNextxPath) == null){
                    Console.WriteLine($"OptionNextxPath {OptionNextxPath} finds no node with href. Terminating.");
                    nexturl = hosturl;
                }else{
                    nexturl = OptionScheme + "://" + OptionHost + doc.DocumentNode.SelectSingleNode(OptionNextxPath).GetAttributeValue("href", string.Empty); /*set next url*/
                }

            }

            return queryResults;

        }

        //!!!Member functions and data members past this point will be listed alphabetically!!!

        /*
        member function checkAllRequiredOptionsFilled ensures all required options are set (and TODO checks validity)
        returns true if so, throws error and returns false otherwise
        */
        private bool allRequiredOptionsFilled(){

            if(OptionScheme == string.Empty){
                OptionScheme = "http";
            }else if(OptionHost == string.Empty){
                Console.Error.WriteLine("WebScraper.Scraper.checkAllRequiredOptionsFilled error: OptionHost not set");
                return false;
            }else if(OptionPath == string.Empty){
                Console.Error.WriteLine("WebScraper.Scraper.checkAllRequiredOptionsFilled error: OptionPath not set");
                return false;
            }else if(OptionxPath == string.Empty){
                Console.Error.WriteLine("WebScraper.Scraper.checkAllRequiredOptionsFilled error: OptionxPath not set");
                return false;
            }else if(OptionNextxPath == string.Empty){
                Console.Error.WriteLine("WebScraper.Scraper.checkAllRequiredOptionsFilled error: OptionNextxPath not set");
                return false;
            }

            return true;

        }

        private void assignConfig(string optionName, string optionEntry){ /*untested*/

            switch(optionName){

                case "OptionScheme": OptionScheme = optionEntry;
                break;
                case "OptionHost": OptionHost = optionEntry;
                break;
                case "OptionPath": OptionPath = optionEntry;
                break;
                case "OptionxPath": OptionxPath = optionEntry;
                break;
                case "OptionNextxPath": OptionNextxPath = optionEntry;
                break;
                case "OptionSearchTerm": OptionSearchTerm = optionEntry;
                break;
                case "OptionWriteFile": OptionWriteFile = optionEntry;
                break;
                default: Console.Error.WriteLine("WebScraper.scraper.assignConfig error: malformed config");
                break;

            }

        }

        public void generateConfigFile(string filename){ /*untested*/

            using (StreamWriter sw = File.CreateText(filename)){

                sw.WriteLine("OptionScheme=http");
                sw.WriteLine("OptionHost=www.example.com");
                sw.WriteLine("OptionPath=/url");
                sw.WriteLine("OptionxPath=/html/body/div/p");
                sw.WriteLine("OptionNextxPath=");
                sw.WriteLine("OptionSearchTerm=");
                sw.WriteLine("OptionWriteFile=");

            }

        }

        public int loadConfigFile(string filename){ /*untested*/

            string setting;
            var file = new StreamReader(filename);

            if(!File.Exists(filename)){
                generateConfigFile(filename);
                Console.Error.WriteLine("Webscrape.scraper.loadConfigFile error: file does not exist. generated file");
            }

            while((setting = file.ReadLine()) != null){

                string[] substr = setting.Split('=', 2);
                assignConfig(substr[0], substr[1]);

            }

            return 0;
        }

    }
    
}
