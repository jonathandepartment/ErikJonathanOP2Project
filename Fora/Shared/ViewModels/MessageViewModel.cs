using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; } = String.Empty;
        public DateTime Created { get; set; }
        public bool Edited { get; set; }
        public bool Removed { get; set; }

        public UserViewModel User { get; set; }
    }
}
