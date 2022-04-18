using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared.DTO
{
    public class AddThreadModel
    {
        public int InterestId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
