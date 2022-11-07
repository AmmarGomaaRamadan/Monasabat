using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monasapat.Models
{
    public class Place
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumberOfChair { get; set; }

        public string Type { get; set; }
        [ForeignKey("PlaceOwner")]
        public int PlaceOwnerID { get; set; }
        [ForeignKey("city")]
        public int? CityID { get; set; }


        //navigation
        public virtual PlaceOwner PlaceOwner { get; set; }
        public virtual ICollection<PlaceImage>PlaceImages { get; set; }
        public virtual ICollection<SuggestedDesign> SuggestedDesigns { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual City city { get; set; }


        [NotMapped]
        public List<IFormFile>? Images { get; set; }

    }
}
