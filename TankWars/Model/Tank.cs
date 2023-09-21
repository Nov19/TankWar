// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using Newtonsoft.Json;

namespace TankWars
{
    /// <summary>
    /// This class contains necessary elements of Tank object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        public const double TankSize = 60;
        public const int AcceleratingTime = 500;

        public readonly int MaxHitPoint;
        public readonly double EnginePower;        // 'Speed' of the tank.
        public readonly double SuperEnginePower;   // 'Speed' of the tank when it is accelerating.
        public readonly int FramePerShot;        
        public readonly int RespawnRate;           // delay of tank's respawning.

        [JsonProperty(PropertyName = "tank")]
        public int ID { get; set; }                // an int representing the tank's unique ID
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }   // a string representing the player's name
        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { get; set; }     // a Vector2D representing the tank's location
        [JsonProperty(PropertyName = "bdir")]
        public Vector2D Orientation { get; set; }  // a Vector2D representing the tank's orientation
        [JsonProperty(PropertyName = "tdir")]
        public Vector2D Aiming { get; set; }       // a Vector2D representing the direction of the tank's turret 
        [JsonProperty(PropertyName = "hp")]
        public int HitPoint { get; set; }          // and int representing the hit points of the tank
        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }             // an int representing the player's score
        [JsonProperty(PropertyName = "died")]
        public bool Died { get; set; }             // a bool indicating if the tank died on that frame
        [JsonProperty(PropertyName = "dc")]
        public bool Disconnected { get; set; }     // a bool indicating if the player controlling that tank disconnected on that frame
        [JsonProperty(PropertyName = "join")]
        public bool Join { get; set; }             // a bool indicating if the player joined on this frame

        public int EnergyPoints { get; set; } = 0;           // an int indicating how many beams a tank can shot
        public Vector2D Velocity { get; set; }               // a vector indicating the direction and distance of next displacement
        
        public int RespawnCoolDownCounter { get; set; } = 0; // counter for respawning
        public int ShotCoolDownCounter { get; set; } = 0;    // counter for shotting
        public int AcceleratingCounter { get; set; } = 0;    // counter for accelerating

        /// <summary>
        /// Json string of tank.
        /// </summary>
        /// <returns>string of Json</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this)+"\n";
        }
        
        /// <summary>
        /// Default constructor for the tank.
        /// </summary>
        public Tank(){}

        /// <summary>
        /// Constructor for the tank.
        /// </summary>
        /// <param name="id">Tank's ID</param>
        /// <param name="name">Player's name</param>
        /// <param name="framePerShot">Rate of shotting</param>
        /// <param name="respawnRate">Rate of respawning</param>
        public Tank(int id, string name, Vector2D loc, int framePerShot, int respawnRate, int maxHP, double enginePower, double superPower)
        {
            ID = id;
            Name = name;
            Location = loc;
            Orientation = new Vector2D(0, 1);
            Aiming = new Vector2D(0, 1);
            HitPoint = MaxHitPoint;
            Score = 0;
            Died = false;
            Join = true;
            Disconnected = false;
            Velocity = new Vector2D(0, 0);
            FramePerShot = framePerShot;
            RespawnRate = respawnRate;
            MaxHitPoint = maxHP;
            EnginePower = enginePower;
            SuperEnginePower = superPower;
        }

        /// <summary>
        /// Method for detecting if a tank collides a projectile.
        /// </summary>
        /// <param name="projectileLoc"></param>
        /// <param name="projectileOwner"></param>
        /// <returns></returns>
        public bool CollidesProjectile(Vector2D projectileLoc, int projectileOwner)
        {
            return (projectileLoc - this.Location).Length() < TankSize/2 && this.ID != projectileOwner;
        }
    }
}
