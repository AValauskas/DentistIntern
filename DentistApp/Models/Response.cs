using System;
using System.Collections.Generic;

namespace DentistApp.Models
{
    public partial class Response
    {
        public string Text { get; set; }
        public int? Mark { get; set; }
        public int IdResponse { get; set; }
        public int FkTraineeidTrainee { get; set; }
        public int FkClientidClient { get; set; }

        public Client FkClientidClientNavigation { get; set; }
        public Trainee FkTraineeidTraineeNavigation { get; set; }
    }
}
