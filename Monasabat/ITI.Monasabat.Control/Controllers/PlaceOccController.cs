using ITI.Monasabat.Control.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Monasapat.Models;
using System.Drawing.Printing;
using X.PagedList;

namespace ITI.Monasabat.Control.Controllers
{
	public class PlaceOccController : Controller
	{
		MonasabatContext context = new MonasabatContext();
		IPagedList<Place> Placeslist;
        List<string> citties;

        private UserManager<User> userManager;


        public PlaceOccController(UserManager<User>? _userManager)
		{
            userManager = _userManager;
            Placeslist = context.Places.Include(t => t.PlaceImages).ToPagedList(1, 6);
            citties = context.Cities.Select(c => c.Name).ToList();
        }
		public IActionResult Places(int pageIndex = 1, int pageSize = 6)
		{
            
            ViewBag.cit=citties;

            Placeslist = context.Places.Include(t => t.PlaceImages).ToPagedList(pageIndex,pageSize);
            
            return View("Places",Placeslist);
		}

        //filters
        [HttpPost]
        public IActionResult FilterPlaces(FilterModel fil)
        {
           // PagedList<Place> list1=(PagedList<Place>)context.Places.ToPagedList(1, 4); 
            //filter by categiory
            if (fil.categiory != "none")
            {

                Placeslist = context.Places.Where(p => p.Type == fil.categiory).ToPagedList(1, 6);
            }
            //filter by location
            if (fil.city!="none")
			{
				int city_id = context.Cities.Where(c => c.Name == fil.city).Select(c=>c.ID).FirstOrDefault();
                Placeslist = context.Places.Where(p =>p.CityID == city_id).ToPagedList(1, 6);
            }
            //filter by price
			if(fil.price>0)
			{
                Placeslist = context.Places.Where(p => p.Price<fil.price).ToPagedList(1, 6);

            }
			
            //the cities names for the dropdownlist
            ViewBag.cit = citties;
           
                return View("Places", Placeslist);

           
        }



            //get place details , show reviews 

            public IActionResult PlaceDetails(int id)
        {
            List<Review> reviews=new List<Review>();
            bool isAbleToComment=true;

            Place DetailedPlace = context.Places.Include(pl1=>pl1.PlaceImages).Where(p => p.ID == id).FirstOrDefault();

            //handling the access of the detils
            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                List<Comments> PlaceReviews = context.Comments?.Where(c => c.PlaceID == id).ToList();
                foreach (var item in PlaceReviews)
                {
                    Review review = new Review();
                    review.Comment = item.CommentText;
                    review.rate = (int)item.commentRate;
                    review.UserName = null; //context.Users.Where(u => u.Id == item.UserID).Select(u => u.UserName).FirstOrDefault();
                    reviews.Add(review);
                }
                Review review1 = new Review() { Comment = "There is no comment", rate = 4, UserName = "Ammar" };
                reviews.Add(review1);
                ViewBag.Comments = reviews;
                //handling that the user had atleast one previous done reservation to can comment it
                var UserId = userManager.GetUserId(HttpContext.User);
                if (context.Reservations.Where(r => r.UserID == UserId && r.placeID == id && r.Status == "done").FirstOrDefault() == null)
                {
                    isAbleToComment = false;
                }
                ViewBag.CommentPlace = isAbleToComment;

                //the available designs
                List<string> designspath = context.SuggestedDesigns.Where(s => s.placeID == id).Select(s => s.Path).ToList();

                //return the reserved dates of the place 
                List<DateTime> dates = context.Reservations.Where(r => r.placeID == id).Select(r =>  r.ReservationTime).ToList();
                ViewBag.Dates = dates;
                return View("PlaceDetails", DetailedPlace);
            }
            else
                return RedirectToAction("SignIn", "User");
        }

        [HttpPost]
        public IActionResult search(string searchtext)
        {
            IPagedList<Place> places = context.Places.Where(p => p.Name.Contains(searchtext)).ToPagedList(1, 6);
            ViewBag.cit = citties;

            if (places.Count > 0)
            {
                return View("Places", places);
            }
            
            return View("Places", Placeslist);

        }
        //[HttpPost]
        //public IActionResult reserve(DateTime date)
        //{
        //    return
        //}

    }
}
