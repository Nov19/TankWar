// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah

using System;
using Newtonsoft.Json;

namespace TankWars
{
    /// <summary>
    /// This class contains necessary elements of Wall object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        [JsonProperty(PropertyName = "wall")]
        public int ID { get; private set; }                // an int representing the wall's unique ID   
        [JsonProperty(PropertyName = "p1")]
        public Vector2D Endpoint1 { get; private set; }    // a Vector2D representing one endpoint of the wall
        [JsonProperty(PropertyName = "p2")]
        public Vector2D Endpoint2 { get; private set; }    // a Vector2D representing the other endpoint of the wall

        private static int nextID = 0;                     // for auto-increamented ID.

        private double top, bottom, left, right, topForTank, bottomForTank, leftForTank, rightForTank;
        public bool CollideTank = true;
        public const double Thickness = 50;                // Thickness of wall.

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Wall() { }

        /// <summary>
        /// Constructor takes two vectors.
        /// </summary>
        /// <param name="p1">Endpoint1</param>
        /// <param name="p2">Endpoint2</param>
        public Wall(Vector2D p1, Vector2D p2)
        {
            ID = nextID++;
            Endpoint1 = p1;
            Endpoint2 = p2;

            double expensionForTank = Thickness / 2 + Tank.TankSize / 2;
            double expensionForSmallObjects = Thickness / 2 + 5;

            // For detecting wall-objects(powerup, projectile) collision.
            left = Math.Min(p1.GetX(), p2.GetX()) - expensionForSmallObjects;
            right = Math.Max(p1.GetX(), p2.GetX()) + expensionForSmallObjects;
            top = Math.Min(p1.GetY(), p2.GetY()) - expensionForSmallObjects;
            bottom = Math.Max(p1.GetY(), p2.GetY()) + expensionForSmallObjects;

            // For detecting wall-tank collision.
            leftForTank = Math.Min(p1.GetX(), p2.GetX()) - expensionForTank;
            rightForTank = Math.Max(p1.GetX(), p2.GetX()) + expensionForTank;
            topForTank = Math.Min(p1.GetY(), p2.GetY()) - expensionForTank;
            bottomForTank = Math.Max(p1.GetY(), p2.GetY()) + expensionForTank;
        }

        /// <summary>
        /// Detects if a wall collides with a tank
        /// </summary>
        /// <param name="tankLoc">Location of a tank</param>
        /// <returns></returns>
        public bool CollidesTank(Vector2D tankLoc)
        {
            return leftForTank < tankLoc.GetX() && tankLoc.GetX() < rightForTank && topForTank < tankLoc.GetY() && tankLoc.GetY() < bottomForTank;
        }

        /// <summary>
        /// Detects if a wall collides with a small object(projectile, powerup)
        /// </summary>
        /// <param name="projectileLoc"></param>
        /// <returns></returns>
        public bool CollidesObject(Vector2D projectileLoc)
        {
            return left < projectileLoc.GetX() && projectileLoc.GetX() < right && top < projectileLoc.GetY() && projectileLoc.GetY() < bottom;
        }

        /// <summary>
        /// Json string of a wall.
        /// </summary>
        /// <returns>A string represents this object</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }
    }
}
