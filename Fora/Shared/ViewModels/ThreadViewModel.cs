using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared.ViewModels
{
    public class ThreadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MessageCount { get; set; }
        public UserViewModel User { get; set; }
    }
}
