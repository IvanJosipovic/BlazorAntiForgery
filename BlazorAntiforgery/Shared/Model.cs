
using System.ComponentModel.DataAnnotations;

namespace BlazorAntiforgery.Shared
{
    public class Model
    {
        [Required]
        public string Value { get; set; }
    }
}
