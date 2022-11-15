using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monasapat.Models;
using System.Collections.Generic;
using System.Data;

namespace ITI.Monasabat.Control.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPlaceController : Controller
    {
        MonasabatContext Db = new MonasabatContext();
        public IActionResult Index()
        {
          
            var Query=Db.Places?.ToList();
            return View("AllPlaces", Query);
        }

        public IActionResult GetID(int id)
        {



            var places = Db.Places?.Where(i => i.ID == id).FirstOrDefault();
            if (places != null)
            {
                ViewBag.Name = $"Details Of | {places.Name}";
                return View("Details", places);
            }
            else
            {

                var mylist = Db.Places.Select(x => x).ToList();

                return View("AllPlaces", mylist);

        }    }

            [HttpGet]
        public IActionResult Add()
        {
            var cities =Db.Cities?.ToList();
            var placeowner =Db.PlaceOwners?.ToList();
            ViewBag.Cities = cities;
            ViewBag.Owners = placeowner;

            return View();
        }
        [HttpPost]

        public IActionResult Add(Place place)
        {
            List < PlaceImage > IMages=new List< PlaceImage >();
            foreach (IFormFile file in place.Images)
            {

                string NewNameOfFile=Guid.NewGuid().ToString()+file.FileName;
                FileStream str = new FileStream
                    (
                    Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "placeimg", NewNameOfFile)
                    ,FileMode.OpenOrCreate,FileAccess.ReadWrite
                    );
                IMages.Add(new PlaceImage()
                {
                    Path = NewNameOfFile
                }); 
                file.CopyTo(str);
                str.Position=0;
            }
            place.PlaceImages= IMages;
            Db.Places.Add(place);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var places = Db.Places?.Where(i => i.ID == id).FirstOrDefault();
            Db.Places.Remove(places);
            Db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var places = Db.Places?.Where(i => i.ID == id).FirstOrDefault();
            ViewBag.Title = places?.Name;
            ViewBag.PlaceName = places?.Type;
            ViewBag.PlacePrice = places?.Price;
            ViewBag.PlaceOwner = places?.PlaceOwnerID;
            ViewBag.CityId = places?.CityID;
          
            ViewBag.cities = Db.Cities.ToList();
            ViewBag.placeowners = Db.PlaceOwners.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Edit(Place place)
        {
            Place placeedit = Db.Places.Where(i => i.ID == place.ID).FirstOrDefault();

            List<PlaceImage> IMages = new List<PlaceImage>();
            foreach (IFormFile file in place?.Images)
            {
                string NewNameOfile = Guid.NewGuid().ToString() + file.FileName;
                FileStream str = new FileStream
                    (
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "placeimg", NewNameOfile)
                        , FileMode.OpenOrCreate, FileAccess.ReadWrite
                    );
                IMages.Add(new PlaceImage()
                {
                    Path = NewNameOfile
                });
                file.CopyTo(str);
                str.Position = 0;
            }

            placeedit.Name = place.Name;
            placeedit.Type = place.Type;
            placeedit.PlaceOwnerID = place.PlaceOwnerID;
            placeedit.CityID = place.CityID;
            placeedit.Price = place.Price;
            placeedit.PlaceImages = IMages;


            Db.SaveChanges();

            return RedirectToAction("Index");


        }
    }
}
