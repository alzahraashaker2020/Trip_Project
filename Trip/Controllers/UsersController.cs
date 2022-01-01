using BLL.IRepo;
using BLL.ModelView;
using BLL.Repo;
using DAL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trip.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IRepoWrapper repo;

        public UsersController(IRepoWrapper _repo)
        {
            repo = _repo;
        }
        //***********************************************Driver operation***********************************//
        [HttpPost]
        [Route("DeiverRegister")]
        public JsonResult DeiverRegister([FromBody] Driver_VM driver_VM)
        {

            User driver = new User()
            {
                Name = driver_VM.name,
                Phone = driver_VM.phone,
                Email = driver_VM.email,
                LicenceId = driver_VM.licenceId,
                Password = driver_VM.password,
                RoleId = 2,
                SutatusSuspend = 1,
            };
            repo._User.Create(driver);
            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }





        [HttpPost]
        [Route("AddFavouriteArea")]
        public JsonResult AddFavouriteArea(int driverID,int areaId)
        {

            FavouriteArea favArea = new FavouriteArea()
            {
                DriverId=driverID,
                AreaId=areaId,
            };
            repo._FavouriteArea.Create(favArea);
            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }


        [HttpGet]
        [Route("GetDriverFavAreas")]
        public string GetDriverFavAreas(int driverId)
        {
            List<string> include = new List<string>();
            include.Add("Area");
            var areas = repo._FavouriteArea.GetAll(include).Result.Where(s=>s.DriverId==driverId).ToList();
            var json = JsonSerializer.Serialize(areas, new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });


            return json;
        }


        [HttpPost]
        [Route("OfferPrice")]
        public JsonResult OfferPrice(int driver_Id, int ride_Id,float price)
        {

            //add offer
            var newOffer = new Offer() { DriverId = driver_Id, Price = price, RideId = ride_Id,Status=false };


            repo._Offer.Create(newOffer);
            try
            {
                repo.Save();
            }
            catch (Exception e) { }
            //add event
            repo._Event.Create(new Event() { RideId = ride_Id, EventDate = DateTime.Now, EventName = "offer_price" });


            //notify passenger
            var client_Id=repo._Ride.GetByCondition(s => s.Id == ride_Id).Result.Select(s => s.ClientId).FirstOrDefault();
            
             repo._Notification.Create(new Notification() { UserId = client_Id, NotificationName = "offer_price", NotifDate = DateTime.Now, SeenStatus = false, OperationId = ride_Id });
            try
            {
                repo.Save();
            }
            catch (Exception e) {
                var ss = e.Message;
            }
            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet]
        [Route("DriverGetAllRates")]
        public JsonResult DriverGetAllRates(int driverId)
        {
            var allRates = repo._Rate.GetByCondition(s => s.DriverId == driverId).Result.ToList();
            int sum = 0;
            foreach (var item in allRates)
            {
                sum =sum +item.Value;
            }
            float ava = sum / allRates.Count();

            return Json(new {allRates, ava}, new JsonSerializerOptions());
        }

        [HttpGet]
        [Route("GetDriverDatawithRates")]
        public string GetDriverDatawithRates(int driverId)
        {
            var allRates = repo._Rate.GetByCondition(s => s.DriverId == driverId).Result.Select(s=>new { s.Review,s.Value}).ToList();
            var driver = repo._User.GetByCondition(s => s.Id == driverId).Result.Select(s=>new {s.Name,s.NationalId,s.LicenceId,s.Phone,s.Email }).FirstOrDefault();
            int sum = 0;
            foreach (var item in allRates)
            {
                sum = sum + item.Value;
            }
            float ava = sum / allRates.Count();
            var json = JsonSerializer.Serialize(new { driver, allRates, ava }, new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return json;
        }

        [HttpGet]
        [Route("GetListOfRideOnArea")]
        public JsonResult GetListOfRideOnArea(int driverId,int areaId)
        {
           var allrides= repo._Ride.GetByCondition(s => s.DriverId == driverId && s.SourceArea == areaId).Result.FirstOrDefault();
            return Json(allrides, new JsonSerializerOptions());
        }


        [HttpPost]
        [Route("DriverArriveToSourse")]
        public JsonResult DriverArriveToSourse(int rideId)
        {

            var ride = repo._Ride.GetByID(rideId).Result;
            ride.RideState = 2;

            repo._Ride.Update(ride);

            //add DriverArriveToDestination  event 
            repo._Event.Create(new Event() { EventDate = DateTime.Now, EventName = "Driver Arrive To Sourse", RideId = ride.Id });

            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new JsonSerializerOptions());
        }


        [HttpPost]
        [Route("DriverArriveToDestination")]
        public JsonResult DriverArriveToDestination(int rideId)
        {

            var ride = repo._Ride.GetByID(rideId).Result;
            ride.RideState = 3;

            repo._Ride.Update(ride);

            //add DriverArriveToDestination  event 
            repo._Event.Create(new Event() { EventDate = DateTime.Now, EventName = "Driver Arrive To Destination", RideId = ride.Id });

            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new JsonSerializerOptions());
        }


        //************************************************Passenger operation********************************//


        [HttpPost]
        [Route("PassengerRegister")]
        public JsonResult PassengerRegister([FromBody] Passenger_VM passenger_VM)
        {

            User passenger = new User()
            {
                Name = passenger_VM.name,
                Phone = passenger_VM.phone,
                Email = passenger_VM.email,
                Password = passenger_VM.password,
                RoleId = 3,
                SutatusSuspend = 0,
                BirthDate = passenger_VM.birthDate
            };
            repo._User.Create(passenger);
            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        [Route("Login")]
        public JsonResult Login([FromBody] Users_VM user_VM)
        {
            var exist = repo._User.GetByCondition(s => s.Id == user_VM.id).Result.FirstOrDefault();
            if (exist != null && exist.SutatusSuspend == 0)
            {

                if (string.Compare(exist.Password, user_VM.password) == 0)
                {
                    return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
                }
                else
                {
                    return Json(new { ID = "401", Result = "check your password and try again" }, new System.Text.Json.JsonSerializerOptions());
                }

            }

            return Json(new { ID = "400", Result = "check user name and try again " }, new System.Text.Json.JsonSerializerOptions());
        }
        [HttpPost]
        [Route("AskRide")]
        public JsonResult AskRide(int source, int destination, int userId, int Nopassenger)
        {
            var newRide = new Ride() { RideState = 0, ClientId = userId, SourceArea = source, DistinationArea = destination, Date = DateTime.Now, PassengerNo = Nopassenger };

            //get only user that set source as favourite area 
            var busyDrivers = repo._Ride.GetByCondition(s => s.RideState == 1).Result.Select(s => s.DriverId).Distinct().ToList();
            var avaliableDriver = repo._FavouriteArea.GetByCondition(s => s.Driver.SutatusSuspend == 0 && s.AreaId == source).Result.Select(s => s.DriverId).Distinct().ToList().Where(f => !busyDrivers.Contains(f));

            //add ride 

            //set Discounts if exist

            //Discounts sales = new Discounts(null);
            //2-compare date of ride with passenger birth day
            var passenger = repo._User.GetByID(userId).Result;
            DateTime dt = DateTime.ParseExact(DateTime.Now.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            string s = dt.ToString("dd/M/", CultureInfo.InvariantCulture);
            string y = passenger.BirthDate.ToString("dd/M/");

            //5- check if it is the frist ride for passenger
            var isfristRie = repo._Ride.GetByCondition(s => s.ClientId == userId).Result.FirstOrDefault();
            //1- get area that have discount and compare with destination area
            var isDiscountArea=repo._AreaDiscount.GetByCondition(s => s.AreaId == destination);
            if (isDiscountArea != null|| s.CompareTo(y) == 0|| isfristRie != null)
            {
                newRide.DiscountVal = 0.1;
                //sales = new Discounts(new AreaDiacountSteratgy());
            }

          
            //3-check date of ride is holiday or not
            var isHoliday = repo._PublicHoliday.GetByCondition(s => s.HolidayDate == dt);
            if (isHoliday != null||Nopassenger == 2)
            {
                newRide.DiscountVal = 0.02;
                //sales = new Discounts(new HolidayDiscountSteratgy());
            }

              repo._Ride.Create(newRide);
            repo.Save();

            //notify avaliable drivers
            foreach (var item in avaliableDriver)
            {
                repo._Notification.Create(new Notification() { UserId = item, NotificationName = "ask_ride", NotifDate = DateTime.Now, SeenStatus = false, OperationId = newRide.Id });

            }


            //add ask ride event 
            repo._Event.Create(new Event() { EventDate = DateTime.Now, EventName = "client ask ride", RideId = newRide.Id });

            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(avaliableDriver, new JsonSerializerOptions());
        }

        [HttpGet]
        [Route("GetAllOffers")]
        public JsonResult GetAllOffers(int rideId)
        {
           var allOffers= repo._Offer.GetByCondition(s => s.RideId == rideId).Result.ToList();


            return Json(allOffers, new JsonSerializerOptions());
        }


        [HttpPost]
        [Route("AcceptOffer")]
        public JsonResult AcceptOffer( int offerId)
        {

            var exist = repo._Offer.GetByID(offerId).Result;
            exist.Status = true;

            repo._Offer.Update(exist);

            //add ask ride event 
            repo._Event.Create(new Event() { EventDate = DateTime.Now, EventName = "user_accept_offer", RideId = exist.RideId });

            //update Ride State with one
           int rideId= (int)exist.RideId;
            var ride=repo._Ride.GetByID(rideId).Result;
            ride.RideState = 1;
            repo._Ride.Update(ride);

            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(exist, new JsonSerializerOptions());
        }

        [HttpPost]
        [Route("ClientRateDriver")]
        public JsonResult ClientRateDriver(int driverId,int clientId,int rate,string review)
        {

            repo._Rate.Create(new Rate() {DriverId=driverId,ClientId=clientId,Value=rate,Review=review });
           
            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new JsonSerializerOptions());
        }

    }
}
