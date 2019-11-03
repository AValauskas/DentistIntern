using System;
using System.Collections.Generic;

namespace DentistApp.Models
{
    public partial class Client
    {
        public Client()
        {
            Response = new HashSet<Response>();
            Session = new HashSet<Sessionn>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int IdClient { get; set; }

        public ICollection<Response> Response { get; set; }
        public ICollection<Sessionn> Session { get; set; }
    }
}
