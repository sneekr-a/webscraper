using System;
using System.IO;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace WebScraper{

    class Scraper{

        private List<(string, string)> queryResults = new List<(string, string)>(); /*contains query results (url, item)*/

        //all valid options
        public string optionScheme;
        public string optionHost;
        public string optionPath;
        public string optionxPath;
        public string optionNextxPath;
        public string optionSearchTerm; /*optional*/
        public string optionWriteFile; /*optional*/

        /*
        member function getQuery returns queryResults
        */
        public List<(string, string)> getQuery(){ return queryResults; }

        /*
        member function scrape performs a webscrape and stores items that include optionSearchTerm in queryResults
        also returns queryResults
        */
        public List<(string, string)> scrape(){
            
            if(!allRequiredOptionsFilled()){ return new List<(string, string)>(){ ("none", "none") }; } /*checks if required options are filled*/

            queryResults.Clear(); /*clear current queryResults*/
            string hosturl = optionScheme + "://" + optionHost;
            string nexturl = hosturl + optionPath; /*setup url*/

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;

            while(nexturl != hosturl){

                doc = web.Load(nexturl); /*load document*/
                Console.WriteLine($"Scraping {nexturl} . . .");

                foreach(var item in doc.DocumentNode.SelectNodes(optionxPath)){
                    
                    if(item.InnerText.ToLower().Contains(optionSearchTerm)){

                        queryResults.Add((nexturl, item.InnerText)); /*add terms to query*/

                    }

                }

                if(doc.DocumentNode.SelectSingleNode(optionNextxPath) == null){
                    Console.WriteLine($"optionNextxPath {optionNextxPath} finds no node with href. Terminating.");
                    nexturl = hosturl;
                }else{
                    nexturl = optionScheme + "://" + optionHost + doc.DocumentNode.SelectSingleNode(optionNextxPath).GetAttributeValue("href", string.Empty); /*set next url*/
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

            if(optionScheme == string.Empty){
                optionScheme = "http";
            }else if(optionHost == string.Empty){
                Console.Error.WriteLine("WebScraper.Scraper.checkAllRequiredOptionsFilled error: optionHost not set");
                return false;
            }else if(optionPath == string.Empty){
                Console.Error.WriteLine("WebScraper.Scraper.checkAllRequiredOptionsFilled error: optionPath not set");
                return false;
            }else if(optionxPath == string.Empty){
                Console.Error.WriteLine("WebScraper.Scraper.checkAllRequiredOptionsFilled error: optionxPath not set");
                return false;
            }else if(optionNextxPath == string.Empty){
                Console.Error.WriteLine("WebScraper.Scraper.checkAllRequiredOptionsFilled error: optionNextxPath not set");
                return false;
            }

            return true;

        }

        private void assignConfig(string optionName, string optionEntry){ /* todo: make work with option subclass*/

            switch(optionName){

                case "optionScheme": optionScheme = optionEntry;
                break;
                case "optionHost": optionHost = optionEntry;
                break;
                case "optionPath": optionPath = optionEntry;
                break;
                case "optionxPath": optionxPath = optionEntry;
                break;
                case "optionNextxPath": optionNextxPath = optionEntry;
                break;
                case "optionSearchTerm": optionSearchTerm = optionEntry;
                break;
                case "optionWriteFile": optionWriteFile = optionEntry;
                break;
                default: Console.Error.WriteLine("WebScraper.scraper.assignConfig error: malformed config");
                break;

            }

        }

        public void generateConfigFile(string filename){ /*todo generate default config file*/

        }

        public int loadConfigFile(string filename){ /*todo make work with assignConfig and option subclass*/

            string setting;
            var file = new StreamReader(filename);

            while((setting = file.ReadLine()) != null){

                string[] substr = setting.Split('=', 2);
                assignConfig(substr[0], substr[1]);

            }

            return 0;
        }

    }
    
}