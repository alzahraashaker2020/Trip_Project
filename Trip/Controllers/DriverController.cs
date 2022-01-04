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
    [Route("Drivers")]
    [ApiController]
    public class DriverController : Controller
    {
        private readonly IRepoWrapper repo;

        public DriverController(IRepoWrapper _repo)
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
                NationalId=driver_VM.nationalId,
                Password = driver_VM.password,
                RoleId = 2,
                SutatusSuspend = 1,
            };
            repo._User.Create(driver);
            try
            {
                repo.Save();

            }
            catch (Exception e) {
                return Json(new { ID = "401", Result = "Enter valid data" }, new System.Text.Json.JsonSerializerOptions());
            }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }





        [HttpPost]
        [Route("AddFavouriteArea")]
        public JsonResult AddFavouriteArea(int driverID, int areaId)
        {
           var exist= repo._User.GetByID(driverID);
            if (exist.RoleId == 2 && exist.SutatusSuspend == 0)
            {
                FavouriteArea favArea = new FavouriteArea()
                {
                    DriverId = driverID,
                    AreaId = areaId,
                };
                repo._FavouriteArea.Create(favArea);
                try
                {
                    repo.Save();
                    return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
                }
                catch (Exception e)
                {
                    return Json(new { ID = "401", Result = "Enter valid data" }, new System.Text.Json.JsonSerializerOptions());
                }
            }

            return Json(new { ID = "300", Result = "it is not allowed for you" }, new System.Text.Json.JsonSerializerOptions());
        }


        [HttpGet]
        [Route("GetDriverFavAreas")]
        public string GetDriverFavAreas(int driverId)
        {
            var exist = repo._User.GetByCondition(s => s.SutatusSuspend == 0 && s.RoleId == 2&&s.Id==driverId);
            if (exist.Count!=0)
            {


                List<string> include = new List<string>();
                include.Add("Area");
                var areas = repo._FavouriteArea.GetAllWithInc(include).Where(s => s.DriverId == driverId).ToList();
                var json = JsonSerializer.Serialize(areas, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });


                return json;
            }
            return "please check your Id";
        }


        [HttpPost]
        [Route("OfferPrice")]
        public JsonResult OfferPrice(int driver_Id, int ride_Id, float price)
        {

            //add offer
            var newOffer = new Offer() { DriverId = driver_Id, Price = price, RideId = ride_Id, Status = false };


            repo._Offer.Create(newOffer);
            try
            {
                repo.Save();
            }
            catch (Exception e) { }
            //add event
            repo._Event.Create(new Event() { RideId = ride_Id, EventDate = DateTime.Now, EventName = "offer_price" ,UserId=driver_Id});


            //notify passenger
            var client_Id = repo._Ride.GetByCondition(s => s.Id == ride_Id).Select(s => s.ClientId).FirstOrDefault();

            repo._Notification.Create(new Notification() { UserId = client_Id, NotificationName = "offer_price", NotifDate = DateTime.Now, SeenStatus = false, OperationId = ride_Id });
            try
            {
                repo.Save();
                return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
            }
            catch (Exception e)
            {
                
            }
            return Json(new { ID = "401", Result = "please enter valid data" }, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet]
        [Route("DriverGetAllRates")]
        public JsonResult DriverGetAllRates(int driverId)
        {
            var allRates = repo._Rate.GetByCondition(s => s.DriverId == driverId).ToList();
            int sum = 0;
            foreach (var item in allRates)
            {
                sum = sum + item.Value;
            }
            float ava = sum / allRates.Count();

            return Json(new { allRates, ava }, new JsonSerializerOptions());
        }

        [HttpGet]
        [Route("GetDriverDatawithRates")]
        public string GetDriverDatawithRates(int driverId)
        {
            var allRates = repo._Rate.GetByCondition(s => s.DriverId == driverId).Select(s => new { s.Review, s.Value }).ToList();
            var driver = repo._User.GetByCondition(s => s.Id == driverId).Select(s => new { s.Name, s.NationalId, s.LicenceId, s.Phone, s.Email }).FirstOrDefault();
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
        public JsonResult GetListOfRideOnArea(int driverId, int areaId)
        {
            var allrides = repo._Ride.GetByCondition(s => s.DriverId == driverId && s.SourceArea == areaId).ToList();
            return Json(allrides, new JsonSerializerOptions());
        }


        [HttpPost]
        [Route("DriverArriveToSourse")]
        public JsonResult DriverArriveToSourse(int rideId,int driverId)
        {

            var ride = repo._Ride.GetByID(rideId);
            ride.RideState = 2;

            repo._Ride.Update(ride);

            //add DriverArriveToDestination  event 
            repo._Event.Create(new Event() { EventDate = DateTime.Now, EventName = "Driver Arrive To Sourse", RideId = ride.Id,UserId=driverId });

            try
            {
                repo.Save();
                return Json(new { ID = "200", Result = "Ok" }, new JsonSerializerOptions());
            }
            catch (Exception e) { }

            return Json(new { ID = "401", Result = "enter valid data" }, new JsonSerializerOptions());
        }


        [HttpPost]
        [Route("DriverArriveToDestination")]
        public JsonResult DriverArriveToDestination(int rideId,int driverId)
        {

            var ride = repo._Ride.GetByID(rideId);
            ride.RideState = 3;

            repo._Ride.Update(ride);

            //add DriverArriveToDestination  event 
            repo._Event.Create(new Event() { EventDate = DateTime.Now, EventName = "Driver Arrive To Destination", RideId = ride.Id,UserId=driverId });

            try
            {
                repo.Save();
                return Json(new { ID = "200", Result = "Ok" }, new JsonSerializerOptions());
            }
            catch (Exception e) { }

            return Json(new { ID = "401", Result = "enter valid data" }, new JsonSerializerOptions());
        }

        [HttpGet]
        [Route("GetDriverNotification")]
        public JsonResult GetDriverNotification(int driverId)
        {
            var allDriverNotification = repo._Notification.GetByCondition(s => s.UserId == driverId);
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
    } }


