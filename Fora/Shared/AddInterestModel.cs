using System.ComponentModel.DataAnnotations;

namespace Fora.Shared
{
    public class AddInterestModel
    {
        [Required]
        public string Name { get; set; } = String.Empty;

    }
}