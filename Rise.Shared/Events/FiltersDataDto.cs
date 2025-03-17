using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Shared.Events;

public class FiltersDataDto
{
    public DateTime? SelectedDate { get; set; }
    public List<string>? SelectedCinemas { get; set; }
}
