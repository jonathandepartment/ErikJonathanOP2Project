using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared.DTO
{
    public class AddMessageModel
    {
        public int ThreadId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
