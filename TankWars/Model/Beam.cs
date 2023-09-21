// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using Newtonsoft.Json;
using System;

namespace TankWars
{
    /// <summary>
    /// This class contains necessary elements of Beam object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Beam
    {
        [JsonProperty(PropertyName = "beam")]
        public int ID { get; private set; }                 // an int representing the beam's unique ID
        [JsonProperty(PropertyName = "org")]
        public Vector2D Origin { get; private set; }        // a Vector2D representing the origin of the beam
        [JsonProperty(PropertyName = "dir")]
        public Vector2D Direction { get; private set; }     // a Vector2D representing the direction of the beam
        [JsonProperty(PropertyName = "owner")]
        public int Owner { get; private set; }              // an int representing the ID of the tank that fired the beam
        private static int nextID = 0;                      // For auto-incremented ID.   
        
        /// <summary>
        /// Json string of the beam.
        /// </summary>
        /// <returns>A string representing the beam.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Beam() { }

        /// <summary>
        /// Constructor takes owner's ID, location, and direction.
        /// </summary>
        /// <param name="owner">The owner of the beam.</param>
        /// <param name="ori">The orinial location of the beam.</param>
        /// <param name="dir">The direction of the beam.</param>
        public Beam(int owner, Vector2D ori, Vector2D dir)
        {
            ID = nextID++;
            Origin = ori;
            Direction = dir;
            Owner = owner;
        }

        /// <summary>
        /// Determines if a ray interescts a circle.
        /// </summary>
        /// <param name="rayOrig">The origin of the ray</param>
        /// <param name="rayDir">The direction of the ray</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="r">The radius of the circle</param>
        /// <returns></returns>
        public static bool Intersects(Vector2D rayOrig, Vector2D rayDir, Vector2D center, double r)
        {
            // ray-circle intersection test
            // P: hit point
            // ray: P = O + tV
            // circle: (P-C)dot(P-C)-r^2 = 0
            // substituting to solve for t gives a quadratic equation:
            // a = VdotV
            // b = 2(O-C)dotV
            // c = (O-C)dot(O-C)-r^2
            // if the discriminant is negative, miss (no solution for P)
            // otherwise, if both roots are positive, hit

            double a = rayDir.Dot(rayDir);
            double b = ((rayOrig - center) * 2.0).Dot(rayDir);
            double c = (rayOrig - center).Dot(rayOrig - center) - r * r;

            // discriminant
            double disc = b * b - 4.0 * a * c;

            if (disc < 0.0)
                return false;

            // find the signs of the roots
            // technically we should also divide by 2a
            // but all we care about is the sign, not the magnitude
            double root1 = -b + Math.Sqrt(disc);
            double root2 = -b - Math.Sqrt(disc);

            return (root1 > 0.0 && root2 > 0.0);
        }
    }
}
