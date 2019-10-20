using System;
using System.Collections.Generic;

namespace DentistIntern.Models
{
    public partial class Trainee
    {
        public Trainee()
        {
            FreeTime = new HashSet<FreeTime>();
            Response = new HashSet<Response>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int? Experience { get; set; }
        public int IdTrainee { get; set; }
        public int FkLectureridLecturer { get; set; }

        public Lecturer FkLectureridLecturerNavigation { get; set; }
        public ICollection<FreeTime> FreeTime { get; set; }
        public ICollection<Response> Response { get; set; }
    }
}
