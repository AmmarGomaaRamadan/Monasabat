using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monasapat.Models
{
    public class PlaceOwner
    {
       public int ID { get; set; }
       public string Name { get; set; }

        //navigation
        public virtual ICollection<Place> Places { get; set; }

    }
}
