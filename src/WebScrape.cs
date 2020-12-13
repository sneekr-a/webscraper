using System;
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
        public string optionSearchTerm;

        /*
        member function checkAllRequiredOptionsValid ensures all required options are set (and TODO checks validity)
        returns true if so, throws error and returns false otherwise
        */
        private bool AllRequiredOptionsFilled(){

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

        /*
        member function getQuery returns queryResults
        */
        public List<(string, string)> getQuery(){ return queryResults; }

        /*
        member function scrape performs a webscrape and stores items that include optionSearchTerm
        also returns queryResults
        */
        public List<(string, string)> scrape(){
            
            if(!AllRequiredOptionsFilled()){ return new List<(string, string)>(){ ("none", "none") }; } /*checks if required options are filled*/

            queryResults.Clear(); /*clear current queryResults*/
            string hosturl = optionScheme + "://" + optionHost;
            string nexturl = hosturl + optionPath; /*setup url*/

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;

            while(nexturl != hosturl){

                doc = web.Load(nexturl); /*load document*/

                foreach(var item in doc.DocumentNode.SelectNodes(optionxPath)){
                    
                    if(item.InnerText.ToLower().Contains(optionSearchTerm)){

                        queryResults.Add((nexturl, item.InnerText)); /*add terms to query*/

                    }

                }

                nexturl = optionScheme + "://" + optionHost + doc.DocumentNode.SelectSingleNode(optionNextxPath).GetAttributeValue("href", string.Empty); /*set next url*/

            }

            return queryResults;

        }

    }
    
}