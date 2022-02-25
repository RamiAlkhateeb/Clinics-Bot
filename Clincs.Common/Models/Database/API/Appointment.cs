using System;
using System.ComponentModel.DataAnnotations;

namespace Clincs.Common.Models.Database.API
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        [Range(15, 120)]
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public int DoctorId { get; set; }

        public User Doctor { get; set; }

        public int Number { get; set; }
    }
}
