using System.ComponentModel.DataAnnotations;

namespace AccentInt.Application.Models;

public class HolidaysRequest
{
    public int? Year { get; set; }

    [Required]
    public required List<string> Countries { get; set; }
}
