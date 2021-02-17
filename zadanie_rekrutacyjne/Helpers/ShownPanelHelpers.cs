using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zadanie_rekrutacyjne.Helpers.Interfaces;

namespace zadanie_rekrutacyjne.Helpers
{
    // class to save shown PanelsId
    public class ShownPanelHelpers : IShownPanelHelpers
    {
        private List<int> PanelsIDs;

        public List<int> GetIDs()
        {
            if(PanelsIDs != null)
            {
                return PanelsIDs;
            }
            else
            {
                return new List<int>();
            }
            
        }

        public void SetIDs(List<int> ids)
        {
            PanelsIDs = ids;
        }
    }
}
