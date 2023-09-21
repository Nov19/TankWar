// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah

namespace TankWars
{
    /// <summary>
    /// This class contains necessary elements of BeamAnimation object.
    /// </summary>
    public class BeamAnimation
    {
        public Vector2D Origin { get; set; }
        public Vector2D Direction { get; set; }
        public int ID { get; set; }
        public int FrameCounter { get; set; }
        
        /// <summary>
        /// Constructor of BeamAnimation class.
        /// </summary>
        /// <param name="b"></param>
        public BeamAnimation(int id, Vector2D ori, Vector2D dir)
        {
            ID = id;
            Origin = ori;
            Direction = dir;
            FrameCounter = 30;
        }
    }
}