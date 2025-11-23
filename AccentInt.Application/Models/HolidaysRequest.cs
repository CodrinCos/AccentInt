using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentInt.Application.Models;

public class HolidaysRequest
{
    public int Year { get; set; }
    public required List<string> Countries { get; set; }
}
