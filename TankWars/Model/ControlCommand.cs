using Newtonsoft.Json;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
        public class ControlCommand
        {
            // moving direction of the tank.
            [JsonProperty(PropertyName = "moving")]
            public string Orientation { get; set; }
            // attack mode of the fire.
            [JsonProperty(PropertyName = "fire")]
            public string Attack{ get; set; }
            // direction of the turrent.
            [JsonProperty(PropertyName = "tdir")]
            public Vector2D Aiming{ get; set; }

            /// <summary>
            /// constructor of UserAction.
            /// </summary>
            /// <param name="moving">moving direction</param>
            /// <param name="fire">fire mode</param>
            /// <param name="tdir">turrent direction</param>
            public ControlCommand(string moving, string fire, Vector2D tdir)
            {
                Orientation = moving;
                Attack = fire;
                Aiming = tdir;
            }
        }
    }
