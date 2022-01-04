using BLL.IRepo;
using BLL.ModelView;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            var pendingDrivers_vm = repo._User.GetByCondition(s=>s.SutatusSuspend==1&&s.RoleId==2);
            //var json = JsonSerializer.Serialize(pendingDrivers_vm, new JsonSerializerOptions()
            //{
            //    WriteIndented = true,
            //    ReferenceHandler = ReferenceHandler.Preserve
            //});
            return Json(pendingDrivers_vm, new System.Text.Json.JsonSerializerOptions());
        }



        [HttpPost]
        [Route("VerifiyDriver")]
        public JsonResult VerifiyDriver(int id)
        {
           var exist= repo._User.GetByID(id);
            if (exist != null)
            {


                exist.SutatusSuspend = 0;

                repo._User.Update(exist);
                try
                {
                    repo.Save();
                    return Json(exist, new System.Text.Json.JsonSerializerOptions());
                }
                catch (Exception e)
                { }

            }

            return Json(new { ID = "401", Result = "Enter valid Driver Id" }, new System.Text.Json.JsonSerializerOptions());

        }

        [HttpGet]
        [Route("GetAllUsers")]
        public JsonResult GetAllUsers()
        {
            var users_vm = repo._User.GetAll().ToList();
            return Json(users_vm, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        [Route("SuspendUser")]
        public JsonResult SuspendUser(int id)
        {
            var exist = repo._User.GetByID(id);
            if (exist != null)
            {
                exist.SutatusSuspend = 1;

                repo._User.Update(exist);
                try
                {
                    repo.Save();
                    return Json(exist, new System.Text.Json.JsonSerializerOptions());
                }
                catch (Exception e) { }

            }

            return Json(new { ID = "401", Result = "Enter valid User Id" }, new System.Text.Json.JsonSerializerOptions());
        }




        [HttpGet]
        [Route("DisplayAllAreas")]
        public JsonResult GetAllAreas()
        {
            var areas_vm = repo._Area.GetAll().ToList();
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
            catch (Exception e) {
                return Json(new { ID = "401", Result = "enter valid data" }, new System.Text.Json.JsonSerializerOptions());
            }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }


        [HttpPost]
        [Route("AddAreaDiscount")]
        public JsonResult AddAreaDiscount(int areaId,float discount)
        {
            var exist = repo._Area.GetByID(areaId);
            if (exist!=null)
            {
                repo._AreaDiscount.Create(new AreaDiscount() { AreaId = areaId, Value = discount });
                try
                {
                    repo.Save();
                    return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
                }
                catch (Exception e)
                {
                    return Json(new { ID = "401", Result = "this area not added yet" }, new System.Text.Json.JsonSerializerOptions());
                }
            }

            return Json(new { ID = "401", Result = "this area not already exist" }, new System.Text.Json.JsonSerializerOptions());

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
            catch (Exception e) {
                return Json(new { ID = "401", Result = "enter valid data" }, new System.Text.Json.JsonSerializerOptions());
            }

            return Json(new { ID = "200", Result = "Ok" }, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet]
        [Route("GetAllEventOnSpecificRide")]
        public JsonResult GetAllEventOnSpecificRide(int rideId)
        {
            var exist = repo._Ride.GetByID(rideId);
            if (exist != null)
            {


                List<string> include = new List<string>();
                include.Add("User");
                List<Events_VM> events_VMs = new List<Events_VM>();
                var Event = repo._Event.GetByConditionWithInclude(s => s.RideId == rideId, include).Select(s => new Events_VM { EventName = s.EventName, EventDate = s.EventDate, ClientName = s.User.Name, RoleId = s.User.RoleId });

                return Json(Event, new System.Text.Json.JsonSerializerOptions());
            }
            return Json(new { ID = "401", Result = "enter valid ride id" }, new System.Text.Json.JsonSerializerOptions());

        }


    }
}
