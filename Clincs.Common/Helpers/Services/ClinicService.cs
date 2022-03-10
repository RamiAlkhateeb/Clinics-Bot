using AutoMapper;
using Clincs.Common.Authorization;
using Clincs.Common.Context;
using Clincs.Common.Models.Database.API;
using Clincs.Common.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;
using Clincs.Common.Models.Response;
using Clincs.Common.Models;
using System;
using Clincs.Common.Models.Database;

namespace Clincs.Common.Helpers.Services
{
    public class ClinicService : IClinicService
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;
        private IJwtUtils _jwtUtils;
        public Functions functions;


        private readonly AppSettings _appSettings;

        public ClinicService(
            AppDbContext context,
            IMapper mapper,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _jwtUtils = jwtUtils;
            functions = new Functions();
        }

        public IEnumerable<User> GetAll()
        {

            return _context.Users;
        }

        public IEnumerable<User> GetAllDoctors(FilteringParams filteringParams)
        {
            var sortBy = filteringParams.OrderBy;
            var filterString = filteringParams.FilterString;
            var doctors = _context.Users.Include(d => d.Appointments)
            .Where(d => d.Role == Role.Doctor).ToList();

            var sortedDoctors = functions.SortDoctors(sortBy, doctors);

            if (!String.IsNullOrEmpty(filterString))
            {
                filterString = filterString.ToLower();
                if (filterString == "mostappointments")
                {
                    var mostVisitedDoctor = sortedDoctors.FirstOrDefault();
                    doctors = new List<User>();
                    doctors.Add(mostVisitedDoctor);
                    return doctors;
                }
                if (filterString == "morethan6h")
                {
                    var mostVisitedDoctors = new List<User>();
                    foreach (var item in doctors)
                    {
                        if (functions.HoursCount(item.Appointments) >= 360)
                            mostVisitedDoctors.Add(item);

                    }
                    doctors = mostVisitedDoctors;

                }
            }

            return doctors;
            //return _context.Users.Include(d => d.Appointments)
            //.Where(d => d.Role == Role.Doctor).ToList();
        }

        public User GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public void Create(CreateRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.Email == model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }



        public void Delete(int id)
        {
            var user = getUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // helper methods

        private User getUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Id == id &&
                u.Role == Role.Doctor
            );
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }


        public AuthenticateResponse Login(LoginRequest loginInfo)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email.Equals(loginInfo.Email));
            // validate
            if (user == null || !BCryptNet.Verify(loginInfo.Password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");

            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            return new AuthenticateResponse(user, jwtToken);
        }



        public void Book(BookRequest slot)
        {
            // validate
            var user = _context.Users.FirstOrDefault(
                u => u.Id == slot.PatientId &&
                u.Role == Role.Patient);
            if (user == null) throw new KeyNotFoundException("User not found");

            var doctor = _context.Users.FirstOrDefault(
                u => u.Id == slot.DoctorId &&
                u.Role == Role.Doctor);
            if (doctor == null) throw new KeyNotFoundException("Doctor not found");


            List<Appointment> slots = GetSlotsOfUser(slot.DoctorId);

            
            if (!functions.isDoctorFull(slots))
            {
                var booking = new Appointment()
                {
                    Doctor = doctor,
                    PatientId = slot.PatientId,
                    DoctorId = slot.DoctorId,
                    Number = slot.Number,
                    Duration = slot.Duration,
                    StartTime = slot.StartTime
                };
                // map slot to new Appointment object
                //var newbooking = _mapper.Map<Appointment>(slot);
                //booking.Doctor = doctor;

                // save user
                _context.Appointments.Add(booking);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Doctor is full");

            }

        }





        public List<Appointment> GetSlotsOfUser(int userId)
        {
            var user = _context.Users.Include(u => u.Appointments)
            .FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            var slots = user.Appointments;

            if (user.Role == Role.Patient)
                slots = _context.Appointments.Where(s => s.PatientId == userId).ToList();



            return slots;
        }

        public void Cancel(int id)
        {
            var slot = GetAppointment(id);
            slot.isCanceled = true;
            _context.Appointments.Update(slot);
            _context.SaveChanges();
        }

        public Appointment GetAppointment(int id)
        {
            var slot = _context.Appointments.FirstOrDefault(s =>
                    s.Id == id
                );
            if (slot == null) throw new KeyNotFoundException("Appointment not found");
            return slot;
        }

        public ConversationReferenceEntity SaveConversationReference (ConversationReferenceEntity cr)
        {
            var test =  _context.conversationReferenceEntities.FirstOrDefault(x => x.UserId == cr.UserId);
            if (test != null)
                return test;
            else
            {
                _context.conversationReferenceEntities.Add(cr);
                _context.SaveChanges();
                return cr;
            }
            
           
        }
        
        public IEnumerable<ConversationReferenceEntity> GetConversationReferences ()
        {
            return _context.conversationReferenceEntities;
        }


        public IEnumerable<NotificationEntity> GetNotifications()
        {
            return _context.notificationEntities;
        }

        public void NotificationIsSent(int id)
        {
            var notification = _context.notificationEntities.FirstOrDefault(n =>
                    n.Id == id
                );
            notification.IsSent = true;
            _context.notificationEntities.Update(notification);
            _context.SaveChanges();
        }
    }
}
