// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using Newtonsoft.Json;

namespace TankWars
{
    /// <summary>
    /// This class contains necessary elements of Powerup object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Powerup
    {
        [JsonProperty(PropertyName = "power")]
        public int ID { get; private set; }                  // an int representing the powerup's unique ID
        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { get; private set; }       // a Vector2D representing the location of the powerup
        [JsonProperty(PropertyName = "died")]
        public bool Died { get; set; }                       // a bool indicating if the powerup "died" on this frame
        private static int nextID = 0;                       // For auto-incremented ID.

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Powerup() { }

        /// <summary>
        /// Constructor takes location.
        /// </summary>
        /// <param name="loc"></param>
        public Powerup(Vector2D loc)
        {
            ID = nextID++;
            Location = loc;
            Died = false;
        }

        /// <summary>
        /// Json string of a wall.
        /// </summary>
        /// <returns>A string represents this object</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }

        /// <summary>
        /// Detects if a power collides with a tank.
        /// </summary>
        /// <param name="tankLoc"></param>
        /// <returns></returns>
        public bool CollidesTank(Vector2D tankLoc)
        {
            return (tankLoc - this.Location).Length() < 30;
        }
    }
}
