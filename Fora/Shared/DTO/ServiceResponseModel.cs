using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared.DTO
{
    public class ServiceResponseModel<T>
    {
        public T? Data { get; set; }
        public bool success { get; set; }
        public string message { get; set; } = string.Empty;
    }
}
