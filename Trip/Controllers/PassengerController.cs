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
    [Route("Passenger")]
    [ApiController]
    public class PassengerController : Controller
    {
        private readonly IRepoWrapper repo;

        public PassengerController(IRepoWrapper _repo)
        {
            repo = _repo;
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
                return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
            }
            catch (Exception e) { }

            return Json(new { ID = "401", Result = "enter valid data" }, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        [Route("Login")]
        public JsonResult Login([FromBody] Users_VM user_VM)
        {
            var exist = repo._User.GetByCondition(s => s.Id == user_VM.id).FirstOrDefault();
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
            var busyDrivers = repo._Ride.GetByCondition(s => s.RideState !=0).Select(s => s.DriverId).Distinct().ToList();
            var drivers = repo._FavouriteArea.GetByCondition(s => s.AreaId == source).Select(s => s.DriverId);
            var drivero = drivers.Distinct().ToList().Where(f => !busyDrivers.Contains(f));

            var avaliableDriver = repo._FavouriteArea.GetByCondition(s => s.Driver.SutatusSuspend == 0 && s.AreaId == source).Select(s => s.DriverId).Distinct().ToList().Where(f => !busyDrivers.Contains(f));

            //add ride 

            //set Discounts if exist

            //Discounts sales = new Discounts(null);
            //2-compare date of ride with passenger birth day
            var passenger = repo._User.GetByID(userId);
           if( passenger.BirthDate.Value.Month== DateTime.Now.Month&& passenger.BirthDate.Value.Day== DateTime.Now.Day)
            {
                newRide.DiscountVal = 0.1;
            }
  
            //5- check if it is the frist ride for passenger
            var isfristRie = repo._Ride.GetByCondition(s => s.ClientId == userId).FirstOrDefault();

            //1- get area that have discount and compare with destination area
            var isDiscountArea=repo._AreaDiscount.GetByCondition(s => s.AreaId == destination);
            if (isDiscountArea != null|| isfristRie != null)
            {
                newRide.DiscountVal = 0.1;
                //sales = new Discounts(new AreaDiacountSteratgy());
            }

          
            //3-check date of ride is holiday or not
            var isHoliday = repo._PublicHoliday.GetByCondition(s => s.HolidayDate.Value.Month == DateTime.Now.Month && s.HolidayDate.Value.Day == DateTime.Now.Day);
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
            repo._Event.Create(new Event() { EventDate = DateTime.Now, EventName = "client ask ride", RideId = newRide.Id,UserId=userId });

            try
            {
                repo.Save();
            }
            catch (Exception e) { return Json(e, new JsonSerializerOptions()); }

            return Json(avaliableDriver, new JsonSerializerOptions());
        }

        [HttpGet]
        [Route("GetAllOffers")]
        public JsonResult GetAllOffers(int rideId)
        {
           var allOffers= repo._Offer.GetByCondition(s => s.RideId == rideId).ToList();


            return Json(allOffers, new JsonSerializerOptions());
        }


        [HttpPost]
        [Route("AcceptOffer")]
        public JsonResult AcceptOffer( int offerId,int clientId)
        {

            var exist = repo._Offer.GetByID(offerId);
            exist.Status = true;
            int diverId = (int)exist.DriverId;

            repo._Offer.Update(exist);

            //add ask ride event 
            repo._Event.Create(new Event() { EventDate = DateTime.Now, EventName = "user_accept_offer", RideId = exist.RideId,UserId=clientId });

            //update Ride State with one
           int rideId= (int)exist.RideId;
            var ride=repo._Ride.GetByID(rideId);
            ride.RideState = 1;
            ride.DriverId = diverId;
            repo._Ride.Update(ride);

            try
            {
                repo.Save();
                return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());

            }
            catch (Exception e) {
               
            }


            return Json(new { ID = "401", Result = "enter valid data" }, new System.Text.Json.JsonSerializerOptions());

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

        [HttpGet]
        [Route("GetPassengerNotification")]
        public JsonResult GetPassengerNotification(int passengerId)
        {
            var allDriverNotification = repo._Notification.GetByCondition(s => s.UserId == passengerId);
            foreach (var item in allDriverNotification)
            {
                item.SeenStatus = true;
                repo._Notification.Update(item);
            }
            try
            {
                repo.Save();

            }
            catch (Exception)
            {

                throw;
            }
            return Json(allDriverNotification, new JsonSerializerOptions());
        }


    }
}
