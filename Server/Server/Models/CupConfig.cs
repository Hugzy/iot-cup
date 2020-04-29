namespace Server.Models
{
    public class CupConfig : Config
    {
        public string Id { get; set; }
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
        
    }
}