using Clincs.Common.Models.Database.API;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Clincs.Common.Helpers
{
    public class Functions
    {


        public bool isDoctorFull(List<Appointment> slots)
        {
            if (slots.Count() > 11)
                return true;

            int hours = HoursCount(slots);

            if (hours > 465)
                return true;
            else
                return false;


        }

        public int HoursCount(List<Appointment> slots)
        {
            int hours = 0;
            foreach (var s in slots)
            {
                hours += s.Duration;
            }

            return hours;
        }

        public List<User> SortDoctors(string sortBy, List<User> doctors)
        {
            switch (sortBy)
            {
                case "names":
                    doctors =
                        doctors
                            .OrderBy(d => d.FirstName)
                            .ToList();
                    break;

                default:
                    doctors =
                        doctors
                            .OrderByDescending(d => d.Appointments.Count())
                            .ToList();
                    break;
            }
            return doctors;


        }

  
    }
}
