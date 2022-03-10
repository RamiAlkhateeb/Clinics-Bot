using System;
using System.ComponentModel.DataAnnotations;

namespace Clincs.Common.Models.Request
{
    public class BookRequest
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }
        public DateTime StartTime { get; set; }

        [Range(15, 120)]
        [Required]
        public int Duration { get; set; }
        [Required]
        public int Number { get; set; }
    }
}
