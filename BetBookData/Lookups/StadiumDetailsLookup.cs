using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetBookData.Lookups;
public class StadiumDetailsLookup
{
    public int StadiumID { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public int Capacity { get; set; }
    public string PlayingSurface { get; set; }
    public float GeoLat { get; set; }
    public float GeoLong { get; set; }
    public string Type { get; set; }
}
