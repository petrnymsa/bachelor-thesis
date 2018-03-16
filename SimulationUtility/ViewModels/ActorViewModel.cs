using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationUtility.Common;

namespace SimulationUtility.ViewModels
{
    public class ActorViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }

        public ActorViewModel(int id, string fullName, string role, int roleId)
        {
            Id = id;
            FullName = fullName;
            Role = role;
            RoleId = roleId;
        }
    }
}
