namespace Server.Models
{
    public class Cup
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
        public bool Connected { get; set; }
    }
}