using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace RegexTask
{
    public class PostCodeChecker
    {
        //The regular expression that the postcode is checked against
        Regex postcode = new Regex(@"(GIR\s0AA)|((([A-PR-UWYZ][0-9][0-9]?)|(([A-PR-UWYZ][A-HK-Y][0-9]( ?<!(BR|FY|HA|HD|HG|HR|HS|HX|JE|LD|SM|SR|WC|WN|ZE)[0-9])[0-9])|([A-PR-UWYZ][A-HK-Y]( ?<!AB|LL|SO)[0-9])|(WC[0-9][A-Z])|(([A-PR-UWYZ][0-9][A-HJKPSTUW])|([A-PR-UWYZ][A-HK-Y][0-9][ABEHMNPRVWXY]))))\s[0-9][ABD-HJLNP-UW-Z]{2})");
        
        //a refference to the base project folder, to make it easier to search for files.
        string baseProjectFolder = Directory.GetCurrentDirectory() + @"..\..\..\..\";
      
        //Main method
        static void Main(string[] args)
        {
            //create a postcode checker object
            PostCodeChecker posty = new PostCodeChecker();

            //run the sorting method
            posty.SortImportedFile();
        }

        //this method will check the passed string against the provided regular expression
        public bool CheckPostcode(string str)
        {
            //returns a boolean value based on whether or not the string that is passed in is matched to the regex
            return postcode.IsMatch(str);
        }

        public void SortImportedFile()
        {
            //try to do the following...
            try
            {
                //Set up a list to contain information on the different postcodes from the import file
                List<Postcode> postcodes = new List<Postcode>(); 

                //will contain the information read by the StreamReader object
                String line;

                //Setting up a stream reader to read through the csv file
                StreamReader importedPostcodes = new StreamReader(baseProjectFolder + @"Imported files\import_data.csv");

                //skips the first heading lines so it wont add the column headings to the list of postcodes
                importedPostcodes.ReadLine();

                //create an array to hold the two sections that the reader will take from the file (ID and postcode)
                String[] lineSections = new String[2];

                //whilst the currently read line contains text...
                while ((line = importedPostcodes.ReadLine()) != null)
                {
                    //split the line into the postcode and row id (which are seperated by a comma)
                    lineSections = line.Trim().Split(',');

                    //creates a new postcode object with the information read from the document and adds it to the list
                    postcodes.Add(new Postcode(int.Parse(lineSections[0]), lineSections[1]));
                }

                //once the post code list is created it is then sorted in ascending order.
                postcodes.Sort();

                //creates two string builders which will hold the information tha is to be added to the two output files
                StringBuilder failedContent = new StringBuilder();
                StringBuilder matchedContent = new StringBuilder();

                //add the column headings to each string builder first.
                failedContent.AppendLine("row_id, postcode");
                matchedContent.AppendLine("row_id, postcode");

                //for every item in the list of postcodes...
                for (int i = 0; i < (postcodes.Count - 1); i++)
                {
                    //if it matches the regular expresion then add it to the content that is to be added to the document of matched postcodes
                    if (CheckPostcode(postcodes[i].postcodeText))
                    {
                        matchedContent.AppendLine(postcodes[i].postcodeID + ", " + postcodes[i].postcodeText);
                    }
                    //if it didn't then add it to the information on failed postcodes instead
                    else
                    {
                        failedContent.AppendLine(postcodes[i].postcodeID + ", " + postcodes[i].postcodeText);
                    }
                }
                //set up the file paths for the two documents
                string failedCSVPath = baseProjectFolder + @"Output files\failed_validation.csv";
                string matchedCSVPath = baseProjectFolder + @"Output files\succeeded_validation.csv";
               
                //create the two documents using the filepath and information to be added to the documents.
                File.WriteAllText(failedCSVPath, failedContent.ToString());
                File.WriteAllText(matchedCSVPath, matchedContent.ToString());
            }
            //if the above fails...
            catch (Exception error)
            {
                Console.WriteLine(error.Message);//print out the error message in console
                Console.Read();//pause
            }
        }
    }
}
