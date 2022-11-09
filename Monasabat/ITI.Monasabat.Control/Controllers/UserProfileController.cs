using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Monasapat.Models;
using System;

namespace ITI.Monasabat.Control.Controllers
{
	public class UserProfileController : Controller
	{
        MonasabatContext monasabatContext = new MonasabatContext();
        private readonly UserManager<User>? _userManager;
		public UserProfileController(UserManager<User>? userManager)
		{
			_userManager = userManager;
		}
		public IActionResult GetUserReservations()
		{

			var UserId = _userManager.GetUserId(HttpContext.User);
			
			User user = _userManager.FindByIdAsync(UserId).Result;

			List<Reservation> reservations = monasabatContext.Reservations.Where(x => x.UserID == UserId).ToList();
		
            foreach (var item in reservations)
            {

                int timeSpan = item.ReservationTime.Day - DateTime.Now.Day;
                if (timeSpan > 2 )
                {
                    monasabatContext.Reservations.Remove(item);
                }
             


                
		}

            ViewBag.LoginUser = user;
            ViewBag.ReservationsList = reservations;


            return View();




        }

    }
}
