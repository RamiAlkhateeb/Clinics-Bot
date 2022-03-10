using AutoMapper;
using Clincs.Common.Authorization;
using Clincs.Common.Helpers;
using Clincs.Common.Helpers.Services;
using Clincs.Common.Models;
using Clincs.Common.Models.Database;
using Clincs.Common.Models.Database.API;
using Clincs.Common.Models.Request;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clincs.Api.Controllers
{
    public class ClincController : ControllerBase
    {
        private IClinicService _userService;
        private IMapper _mapper;

        public ClincController(
            IClinicService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;

        }

        [Authorize(Role.Admin)]
        [EnableQuery]
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll().AsQueryable());
        }

        [HttpGet("csv")]
        [Produces("text/csv")]
        public IActionResult GetCSV()
        {
            var users = _userService.GetAll();

            var builder = new StringBuilder();
            builder.AppendLine("Id,UserName");
            foreach (var user in users)
            {
                builder.AppendLine($"{user.Id},{user.FirstName}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");
        }



        [HttpGet("doctors/{id}")]
        public IActionResult GetById([FromRoute]int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }


        [HttpPost]
        [Route("register")]
        public IActionResult Create([FromBody]CreateRequest model)
        {
            _userService.Create(model);
            return Ok(new { message = "User created" });
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var user = _userService.Login(model);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });


            return Ok(user);
        }


        [HttpGet]
        [EnableQuery]
        [Route("doctors")]
        public IActionResult GetDoctors([FromQuery] FilteringParams filterParameters)//[FromQuery] FilteringParams filterParameters)
        {
            var users = _userService.GetAllDoctors(filterParameters);
            //List<object> Result = new List<object>();
            //Functions func = new Functions();
            //foreach (var item in users)
            //{
            //    bool isFull = func.isDoctorFull(item.Appointments);
            //    var doctor = new
            //    {
            //        Id = item.Id,
            //        Name = item.FirstName + " " + item.LastName,
            //        Email = item.Email,
            //        AppointmentsCount = item.Appointments.Count,
            //        isAvailable = !isFull
            //    };
            //    Result.Add(doctor);
            //}
            return Ok(users.AsQueryable());
        }



        [HttpGet("doctors/{docId}/slots")]
        public IActionResult GetSlots([FromRoute] int docId)
        {
            var slots = _userService.GetSlotsOfUser(docId);
            List<object> Result = new List<object>();
            Functions func = new Functions();
            bool isFull = func.isDoctorFull(slots);
            if (isFull)
                return Ok("Doctor is Full");
          
                List<Appointment> list = new List<Appointment>();
                for(int i = 1; i <= 12; i++)
                {
                    var slot = slots.FirstOrDefault(s => s.Number == i);
                    if(slot == null)
                    {
                        Appointment free = new Appointment();
                        free.Number = i;
                        free.DoctorId = docId;
                        list.Add(free);
                    }
                    else
                    {
                        slot.Doctor = null;
                        list.Add(slot);


                    }
                }
                    

                return Ok(list);
            }

        


        [HttpGet("{userId}/slots")]
        public IActionResult GetSlotsHistory([FromRoute] int userId)
        {
            var slots = _userService.GetSlotsOfUser(userId);
            List<object> Result = new List<object>();
            Functions func = new Functions();
            
                foreach (var item in slots)
                {
                    var doctor = new
                    {
                        item.Id,
                        item.DoctorId,
                        item.PatientId,
                        item.Duration,
                        item.Number,
                        item.isCanceled,
                        item.StartTime
                    };
                    Result.Add(doctor);
                }
                return Ok(Result);
            

        }



        /// <summary>
        /// Book an appointment.
        /// </summary>
        [HttpPost]
        [Route("book")]
        public IActionResult Book([FromBody]BookRequest slot)
        {
            _userService.Book(slot);
            return Ok(new { message = "Book created" });
        }



        [HttpPut("cancel/{id}")]
        public IActionResult Cancel(int id)
        {
            _userService.Cancel(id);
            return Ok(new { message = "Appointment Canceled" });
        }

        [HttpGet("{id}/slotinfo")]
        public IActionResult GetSlot(int id)
        {
            var slot = _userService.GetAppointment(id);
            return Ok(slot);
        }

        
    }
}
