using System;

namespace Server.Models
{
    public class Temperature
    {
        public string Id { get; set; }
        public double Temp { get; set; }
        
        public DateTime Dt { get; set; }
    }
}