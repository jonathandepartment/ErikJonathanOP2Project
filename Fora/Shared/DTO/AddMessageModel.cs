using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared.DTO
{
    public class AddMessageModel
    {
        public int ThreadId { get; set; }
        public string Message { get; set; }
    }
}
