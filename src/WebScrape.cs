using System;
using System.IO;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace WebScraper{

    class Scraper{

        /*
        field queryResults stores query results
        results are stored from scrape() in a list of tuples
        item1 will be the url the item was found at
        item2 will be the complete item

        queryResults will be initialized to ("none", "none"), or changed to that value following errors.
        */
        private List<(string, string)> queryResults = new List<(string, string)>{ ("none", "none") }; /*contains query results (url, item)*/
        public List<(string, string)> QueryResults {
            get{return queryResults;}
        }

        //all options
        public string OptionScheme {get; set;} /*required*/
        public string OptionHost  {get; set;} /*required*/
        public string OptionPath  {get; set;} /*required*/
        public string OptionxPath  {get; set;} /*required*/
        public string OptionNextxPath  {get; set;} /*required (may be changed)*/
        public string OptionSearchTerm  {get; set;} /*optional*/
        private string OptionWriteFile  {get; set;} /*todo, do not fill*/

        /*
        member function scrape performs a webscrape
        all required options must be filled (scheme, host, path, xpath, nextxpath)
        returns QueryResults and stores query results in QueryResults
        */
        public List<(string, string)> scrape(){ /*checks if required options are filled*/
            
            queryResults.Clear(); /*clear current queryResults*/            
            if(!allRequiredOptionsFilled()){

                QueryResults.Add( ("none", "none") );
                return QueryResults;

            }

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

            if(OptionWriteFile != string.Empty){
                writeQuery(OptionWriteFile);
            }

            return queryResults;

        }
        
        //-------------------------------------------------------------------------------------
        //!!!Member functions and data members past this point will be listed alphabetically!!!
        //-------------------------------------------------------------------------------------

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

        /*
        member function assignConfig assigns options based off strings
        string optionName should be the string name of a valid option
        string optionEntry should be a valid value for that option
        */
        private void assignConfig(string optionName, string optionEntry){

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

        private void assignConfig(string optionName, bool optionEntry){

            switch(optionName){

                default:    Console.Error.WriteLine("WebScraper.scraper.assignConfig error: malformed config");
                            Console.Error.WriteLine("TODO: Program should not enter this overloaded function (WebScraper.Scraper.assignConfig(string, bool)");
                break;

            }

        }

        /*
        member function generateConfigFile generates a config file
        string filename should be the name of the file generated
        TODO: allow filepath outside directory with executable
        */
        public void generateConfigFile(string filename){

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

        /*
        member function loadConfigFile loads a config file named "filename"
        string filename should be the path to the config file

        options should be listed in the config file as follows
        [OptionVarName]=[OptionSetting]
        */
        public void loadConfigFile(string filename){

            string setting;
            string[] substr;
            var file = new StreamReader(filename);

            if(!File.Exists(filename)){
                generateConfigFile(filename);
                Console.Error.WriteLine("Webscrape.scraper.loadConfigFile error: file does not exist. generated file");
                return;
            }

            while((setting = file.ReadLine()) != null){

                substr = setting.Split('=', 2);
                
                if(substr.Length == 2){
                    assignConfig(substr[0], substr[1]);
                }else{
                    Console.Error.WriteLine("Webscrape.scraper.loadConfigFile error: malformed config line");
                }

            }

            return;
        }

        /*
        member function writeFile writes the current queryResults to a file
        string filename should be the name of the file
        */
        private void writeQuery(string filename){
            
            StreamWriter sw = File.CreateText("query-" + filename + '-' + DateTime.Now + ".txt");

            sw.WriteLine($"\n----Query {0}----", DateTime.Now);

            foreach(var v in queryResults){

                sw.WriteLine($"\"{0}\", \"{1}\"", v.Item1, v.Item2);

            }

        }

    }
    
}
