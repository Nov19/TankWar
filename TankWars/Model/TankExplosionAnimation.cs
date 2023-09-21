// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah

namespace TankWars
{
    /// <summary>
    /// This class contains necessary elements of TankExplosionAnimation object.
    /// </summary>
    public class TankExplosionAnimation
    {
        public Vector2D Location { get; set; }
        public int FrameCounter { get; set; }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="t"></param>
        public TankExplosionAnimation(Vector2D loc)
        {
            Location = loc;
            FrameCounter = 40;
        }
    }
}