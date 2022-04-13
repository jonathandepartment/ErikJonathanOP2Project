using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared.DTO
{
    public class AddInitialInterests
    {
        [Required, MinLength(1), MaxLength(5)]
        public List<int> InterestIds { get; set; }
    }
}
