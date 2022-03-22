using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexTask
{
    class Postcode : IComparable<Postcode>
    {
        /// <summary>
        /// This is a simple class used to create postcode objects, whch will then be formed into a list and sorted.
        /// It hold info on the postcode and its corresponding ID.
        /// It also inherits from the 'IComparable' class, which is needed to sort the list of postcode objects.
        /// </summary>
        
        //variables to hold information on the postcode and its ID
        public int postcodeID;
        public string postcodeText;

        //simple constructor that requires two inputs to create the postcode (the ID and postcode).
        public Postcode(int rowID, string postcode)
        {
            //set the the variables to the values passed in
            postcodeText = postcode;
            postcodeID = rowID;
        }

        //This is method is required to sort the list of postcode objects.
        public int CompareTo(Postcode other)
        {
            //if the postcode is greater than the other postcode it is being compared to, it will retun 1.
            if (this.postcodeID > other.postcodeID)
                return 1;
            //if the postcode is lesser than the other postcode then it will return -1
            if (this.postcodeID < other.postcodeID)
                return -1;
            //if it's equal to the other postcode, it will retun 0
            else
                return 0;
        }
    }
}
