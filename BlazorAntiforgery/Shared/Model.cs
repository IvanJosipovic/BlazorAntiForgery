using System.ComponentModel.DataAnnotations;

namespace BlazorAntiForgery.Shared
{
    public class Model
    {
        [Required]
        public string Value { get; set; }
    }
}
