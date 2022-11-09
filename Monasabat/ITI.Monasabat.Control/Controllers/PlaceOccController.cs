using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monasapat.Models;

namespace ITI.Monasabat.Control.Controllers
{
    public class PlaceOccController : Controller
    {
        MonasabatContext context;
        List<Place> Placeslist;
        List<string> citties;

        public PlaceOccController(MonasabatContext _context)
        {
            context = _context;
            Placeslist = context.Places.Include(t => t.PlaceImages).ToList();
            citties = context.Cities.Select(c => c.Name).ToList();
        }
        public IActionResult Places()
        {



            ViewBag.cit = citties;

            return View("Places", Placeslist);
        }
        [HttpPost]
        public IActionResult FilterPlaces(FilterModel fil)
        {


            if (fil.categiory != "none")
            {
                Placeslist = Placeslist.Where(p => p.Type == fil.categiory).ToList();
            }
            if (fil.city != "none")
            {
                int city_id = context.Cities.Where(c => c.Name == fil.city).Select(c => c.ID).FirstOrDefault();
                Placeslist = Placeslist.Where(p => p.CityID == city_id).ToList();
            }
            if (fil.price > 0)
            {
                Placeslist = Placeslist.Where(p => p.Price < fil.price).ToList();

            }
            if (fil.OccasionDate > (DateTime.Now))
            {
                foreach (Place pl in Placeslist)
                {
                    int x = context.Reservations.Where(r => r.placeID == pl.ID && r.ReservationTime == fil.OccasionDate).ToList().Count();
                    if (x > 0)
                    {
                        Placeslist.Remove(pl);
                    }
                }
            }

            ViewBag.cit = citties;
            return View("Places", Placeslist);
        }

    }
}
