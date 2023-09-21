// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using Newtonsoft.Json;

namespace TankWars
{
    /// <summary>
    /// This class contains necessary elements of Projectile object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty(PropertyName = "proj")]
        public int ID { get; private set; }                     // an int representing the projectile's unique ID
        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { get; set; }                  // a Vector2D representing the projectile's location  
        [JsonProperty(PropertyName = "dir")]
        public Vector2D Direction { get; private set; }         // a Vector2D representing the projectile's orientation 
        [JsonProperty(PropertyName = "died")]
        public bool Died { get; set; }                          // a bool representing if the projectile died on this frame
        [JsonProperty(PropertyName = "owner")]
        public int Owner { get; private set; }                  // an int representing the ID of the tank that created the projectile
        private static int nextID = 0;
        public Vector2D Velocity { get; set; }
        public const double Speed = 25;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Projectile() { }

        /// <summary>
        /// Constructor takes owner, location, and direction.
        /// </summary>
        /// <param name="owner">The owner of this projectile.</param>
        /// <param name="loc">The orignal location of the projectile.</param>
        /// <param name="dir">The direction of the projectile.</param>
        public Projectile(int owner, Vector2D loc, Vector2D dir)
        {
            ID = nextID++;
            Location = loc;
            Direction = dir;
            Died = false;
            Owner = owner;
        }

        /// <summary>
        /// Json string of the projectile.
        /// </summary>
        /// <returns>A string representing the projectile.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }
    }
}
