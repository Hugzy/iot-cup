namespace Server.Models
{
    public class TemperatureTO
    {
        string Id { get; set; }
        int Tvalue { get; set; }

        public Temperature Transform()
        {
            const double a = 0.0627918;
            const double b = -20.9698;
            var result = a * Tvalue + b;
            return new Temperature()
            {
                Id = Id,
                Tvalue = result

            };
        }
    }
}                                                                                    