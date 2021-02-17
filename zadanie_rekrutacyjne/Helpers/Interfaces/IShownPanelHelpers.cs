using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zadanie_rekrutacyjne.Helpers.Interfaces
{
    public interface IShownPanelHelpers
    {
        List<int> GetIDs();
        void SetIDs(List<int> ids);
    }
}
