using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Models.Json
{
    public class Location
    {
        public List<Addresses> address_component { get; set; }
        public List<Geometrics> geometry { get; set; }
    }
}
