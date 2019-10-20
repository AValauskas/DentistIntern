using System;
using System.Collections.Generic;

namespace DentistIntern.Models
{
    public partial class FreeTime
    {
        public FreeTime()
        {
            Session = new HashSet<Session>();
        }

        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int IdFreeTime { get; set; }
        public int FkTraineeidTrainee { get; set; }

        public Trainee FkTraineeidTraineeNavigation { get; set; }
        public ICollection<Session> Session { get; set; }
    }
}
