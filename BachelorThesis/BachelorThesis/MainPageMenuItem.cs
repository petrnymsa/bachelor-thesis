using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorThesis
{

    public class MainPageMenuItem
    {
        public MainPageMenuItem()
        {
            TargetType = typeof(Views.DashboardPage);
        }
        public string Title { get; set; }
        public string Icon { get; set; }

        public Type TargetType { get; set; }
    }
}