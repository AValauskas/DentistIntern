using System;
using System.Collections.Generic;

namespace DentistIntern.Models
{
    public partial class Session
    {
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int IdSession { get; set; }
        public int FkClientidClient { get; set; }
        public int FkFreeTimeidFreeTime { get; set; }

        public Client FkClientidClientNavigation { get; set; }
        public FreeTime FkFreeTimeidFreeTimeNavigation { get; set; }
    }
}
