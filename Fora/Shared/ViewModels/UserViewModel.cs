using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Deleted { get; set; }
        public bool Banned { get; set; }
        public bool Admin { get; set; }
    }
}
