using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace zadanie_rekrutacyjne.Models
{
    // model to store json string and extract iDs from json
    public class JsonLocalStorageModel
    {
        public string Panels { get; set; }
        public List<int> GetIDs()
        {
            List<int> iDs = new List<int>();

            string[] panelSplitted = Panels.Split(',');
            if (panelSplitted[0].Equals("[]"))
            {
                return iDs;
            }

            foreach(string panelID in panelSplitted)
            {
                string panelIDNumber = Regex.Match(panelID, @"\d+").Value;
                iDs.Add(int.Parse(panelIDNumber));
            }

            return iDs;
    }
    }
   
}
