using BLL.IRepo;
using BLL.ModelView;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trip.Controllers
{
    [Route("Admin")]
    [ApiController]
    public class AdminController : Controller
    {

        private readonly IRepoWrapper repo;

        public AdminController(IRepoWrapper _repo)
        {
            repo = _repo;
        }


        [HttpGet]
        [Route("PendingDrivers")]
        public JsonResult GetAllPendingDriver()
        {
            var pendingDrivers_vm = repo._User.GetAll().Result.Where(s=>s.SutatusSuspend==1).Select(s => new Driver_VM
            {
                id = s.Id,
                name = s.Name,
                nationalId = s.NationalId,
                licenceId = s.LicenceId,
                phone = s.Phone,
                email=s.Email,
                password=s.Password,
                roleId=s.RoleId
            }).ToList();
            return Json(pendingDrivers_vm, new System.Text.Json.JsonSerializerOptions());
        }



        [HttpPost]
        [Route("VerifiyDriver")]
        public JsonResult VerifiyDriver(int id)
        {
           var exist= repo._User.GetByID(id).Result;
            exist.SutatusSuspend = 0;

                repo._User.Update(exist);
                try
                {
                    repo.Save();
                }
                catch (Exception e) { }
           
            return Json(exist, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public JsonResult GetAllUsers()
        {
            var users_vm = repo._User.GetAll().Result.ToList();
            return Json(users_vm, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        [Route("SuspendUser")]
        public JsonResult SuspendUser(int id)
        {
            var exist = repo._User.GetByID(id).Result;
            exist.SutatusSuspend = 1;

            repo._User.Update(exist);
            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(exist, new System.Text.Json.JsonSerializerOptions());
        }




        [HttpGet]
        [Route("DisplayAllAreas")]
        public JsonResult GetAllAreas()
        {
            var areas_vm = repo._Area.GetAll().Result.ToList();
            return Json(areas_vm, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        [Route("AddArea")]
        public JsonResult AddArea(string address, string latitude, string langitude)
        {

            Area area = new Area()
            {
                Address = address,
                Latitude = latitude,
                Longitude = langitude,
            };
            repo._Area.Create(area);
            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }


        [HttpPost]
        [Route("AddAreaDiscount")]
        public JsonResult AddAreaDiscount(int areaId,float discount)
        {
            repo._AreaDiscount.Create(new AreaDiscount() { AreaId=areaId,Value=discount});
            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        [Route("AddPublicHoliday")]
        public JsonResult AddPublicHoliday(DateTime holidayDate, string  description)
        {
            repo._PublicHoliday.Create(new PublicHoliday() { HolidayDate = holidayDate, Note = description });
            try
            {
                repo.Save();
            }
            catch (Exception e) { }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet]
        [Route("GetAllEventOnSpecificRide")]
        public JsonResult GetAllEventOnSpecificRide(int rideId)
        {
            List<Events_VM> events_VMs = new List<Events_VM>();
            var Event= repo._Event.GetByCondition(s=>s.RideId==rideId).Result.Select(s=>new Events_VM {EventDate=s.EventDate,EventName=s.EventName,RideId=s.RideId });
            var ride = repo._Ride.GetByCondition(s => s.Id == rideId).Result;
            int DriverId = (int)(int?)( ride.Select(s => s.DriverId).FirstOrDefault());
            var DriverName = repo._User.GetByCondition(s=>s.Id==DriverId).Result.Select(s=> string.IsNullOrEmpty(s.Name) ? " " : s.Name).FirstOrDefault();
            var ClientId = (int)(int?)(ride.Select(s => s.ClientId).FirstOrDefault());
            var ClientName = repo._User.GetByID(ClientId).Result.Name;

            events_VMs = (List<Events_VM>)Event;
            foreach (var item in events_VMs)
            {
                item.ClientName = ClientName;
                item.DriverName = DriverName;
            }

            return Json(events_VMs, new System.Text.Json.JsonSerializerOptions());
        }


    }
}
