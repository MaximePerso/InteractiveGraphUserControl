using System.Collections.Generic;
using InteractiveGraphUserControl.Utility;

namespace InteractiveGraphUserControl.MVVM
{
    public class GridParam : ObservableObject
    {
        public List<string> CbItem { get; set; }

        public GridParam()
        {
            CbItem = new List<string> { "Position", "Load", "Maintain" };
        }
    }
}
