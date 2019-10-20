using System;
using System.Collections.Generic;

namespace DentistIntern.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            Trainee = new HashSet<Trainee>();
        }

        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public int IdLecturer { get; set; }

        public ICollection<Trainee> Trainee { get; set; }
    }
}
