
using Microsoft.AspNetCore.Mvc;
using Foodiee.Models;
namespace Foodiee.Controllers
{
    public class AdminController : Controller
    {
        foodieContext ob=new foodieContext();
        public IActionResult Home()
        {
            int user_count = ob.UserRegs.ToList().Count();
            int restaurant_Count=ob.Restaurants.ToList().Count();
            int delivery_Count=ob.DeliverRegs.ToList().Count();
            int pending_List_count = ob.Restaurants.Count(t => t.Status == "Pending");
            ViewBag.user_count=user_count;
            ViewBag.restaurant_count=restaurant_Count;
            ViewBag.delivery_count=delivery_Count;
            ViewBag.pending_List_count=pending_List_count;
            return View();
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(UserReg model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    ob.UserRegs.Add(model);
                    if (ob.SaveChanges() > 0)
                    {
                        ViewBag.e = "User added successfully";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.e = ex.Message;
                }
            }
            else
            {
                ViewBag.e = "add denied";
            }
                return View();
        }
        public IActionResult DeleteUser(int id)
        {
            var user = ob.UserRegs.FirstOrDefault(t => t.UserId == id);
            if (user != null)
            {
                ob.UserRegs.Remove(user);
                if(ob.SaveChanges() >0)
                {
                    ViewBag.i = "User deleted successfully";
                }
            }
            else
            {
                ViewBag.i = "Unable to delete the user";
            }
            return View(); 
        }
        [HttpGet]
        public IActionResult UpdateUser(int id)
        {
            var user = ob.UserRegs.FirstOrDefault(u => u.UserId == id);
            return View(user);
        }
        [HttpPost]
        public IActionResult UpdateUser(UserReg r)
        {
            var existing = ob.UserRegs.FirstOrDefault(u => u.UserId == r.UserId);
            if (existing != null)
            {
                existing.Fullname = r.Fullname;
                existing.Email = r.Email;
                existing.Phone = r.Phone;
                existing.Area = r.Area;
                existing.City = r.City;
                existing.Age = r.Age;
                ob.UserRegs.Update(existing);
                ViewBag.i= ob.SaveChanges();
            }
            return View(r);
        }
        public IActionResult ViewUsers()
        {
            int user_count = ob.UserRegs.ToList().Count();
            ViewBag.user_count = user_count;
            var b = ob.UserRegs.ToList();
            return View(b);
        }
        [HttpGet]
        public IActionResult ViewUserbyId()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ViewUserbyId(int userid)
        {
            UserReg b=new UserReg();
            b = ob.UserRegs.FirstOrDefault(t => t.UserId == userid);
            return View(b);
        }
        public IActionResult AdminProfile()
        {
            var res=from t in ob.Registrations where t.RoleName=="admin" select t;
            return View(res.ToList());
        }

        public IActionResult ViewRestaurants()
        {
            int restaurant_count = ob.Restaurants.ToList().Count();
            ViewBag.user_count = restaurant_count;
            var b = ob.Restaurants.ToList();
            return View(b);
        }
        [HttpGet]
        public IActionResult ViewRestaurantsById()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ViewRestaurantsById(int restid)
        {
            Restaurant b=new Restaurant();
            b= ob.Restaurants.FirstOrDefault(t=>t.RestaurantId==restid);
            return View(b);
        }
        public IActionResult DeleteRestaurant(int id)
        {
            var user = ob.Registrations.FirstOrDefault(t => t.RegId == id);
            if (user != null)
            {
                ob.Registrations.Remove(user);
                ViewBag.i = ob.SaveChanges();
            }
            return View(ViewBag.i);
        }
        [HttpGet]
        public IActionResult UpdateRestaurant(int Restid)
        {
            var res = ob.Restaurants.FirstOrDefault(u => u.RestaurantId == Restid);
            return View(res);
        }
        [HttpPost]
        public IActionResult UpdateRestaurant(Restaurant r)
        {
            var current = ob.Restaurants.FirstOrDefault(u => u.RestaurantId == r.RestaurantId);
            if (current != null)
            {
                current.RestName = r.RestName;
                current.Email = r.Email;
                current.PhoneNo= r.PhoneNo;
                current.Location = r.Location;
                current.Description = r.Description;
                ViewBag.i = ob.SaveChanges();
                ModelState.Clear();
            }
            return View(r);
        }
        [HttpGet]
        public IActionResult AddRestaurant()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRestaurant(Restaurant model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    ob.Restaurants.Add(model);
                    if (ob.SaveChanges() > 0)
                    {
                        ViewBag.e = "Restaurant added successfully";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.e = ex.Message;
                }
            }
            else
            {
                ViewBag.e = "add denied";
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditAdmin(int id)
        {
            var res = ob.Registrations.FirstOrDefault(u => u.RegId == id);
            return View(res);
        }
        [HttpPost]
        public IActionResult EditAdmin(Registration r)
        {
            var current = ob.Registrations.FirstOrDefault(u => u.RegId == r.RegId);
            if (current != null)
            {
                current.Fullname = r.Fullname;
                current.Email = r.Email;
                current.Username = r.Username;
                current.Email = r.Email;
                current.Phone = r.Phone;
                current.Address = r.Address;
                current.Sq1 = r.Sq1;
                current.Sq2 = r.Sq2;
                ob.Registrations.Update(current);
                ViewBag.i = ob.SaveChanges();
                ModelState.Clear();
            }
            return View(r);
        }
       
        public IActionResult RestaurantApproval()
        {
            var res = ob.Restaurants.Where(u => u.Status == "pending");
            return View(res.ToList());
        }
        public IActionResult UpdateRestaurantStatus(int restid, string status)
        {
            var res = ob.Restaurants.FirstOrDefault(t => t.RestaurantId == restid);
            if (res != null)
            {
                res.Status = status;
                ob.SaveChanges();
                TempData["Message"] = $"Status set to '{status}' successfully.";
            }
            else
            {
                TempData["Message"] = "Restaurant not found.";
            }
            return RedirectToAction("RestaurantApproval"); // or your actual listing page
        }
        [HttpGet]
        public IActionResult AddDeliveryAgent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddDeliveryAgent(DeliverReg d)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    ob.DeliverRegs.Add(d); 
                    if (ob.SaveChanges() > 0)
                    {
                        ViewBag.e = "Agent added successfully";
                        ModelState.Clear();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.e = ex.Message;
                }
            }
            else
            {
                ViewBag.e = "add denied";
            }
            return View();
        }
        public IActionResult ViewAgent()
        {
            var res=ob.DeliverRegs.ToList();

            return View(res);
        }
        [HttpGet]
        public IActionResult ViewAgentById()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult ViewAgentById(int id)
        {
            DeliverReg b = new DeliverReg();
            b = ob.DeliverRegs.FirstOrDefault(t => t.UserId == id);
            return View(b);
        }
        public IActionResult UpdateAgent()
        {
            return View();
        }
        public IActionResult DeleteAgent(int id)
        {
            var user = ob.DeliverRegs.FirstOrDefault(t => t.UserId == id);
            if (user != null)
            {
                ob.DeliverRegs.Remove(user);
                if (ob.SaveChanges() > 0)
                {
                    ViewBag.i = "User deleted successfully";
                }
            }
            else
            {
                ViewBag.i = "Unable to delete the user";
            }
            return View();
        }
    }
}
