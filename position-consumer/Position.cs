using System;
using Newtonsoft.Json;

namespace position_consumer
{
    public class Position
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Converts this object into its string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
