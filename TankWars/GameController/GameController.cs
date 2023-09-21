// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TankWars
{
    /// <summary>
    /// This class is the game controller. It send user's action to the server,
    /// receives updated information from server, and reminds view to redraw.
    /// </summary>
    public class GameController
    {
        // controller received updated information from server.
        public event Action UpdateArrived;

        // Error occurs when connecting to the server.
        public delegate void ErrorHandler(string err);
        public event ErrorHandler Error;

        public delegate void BeamHandler(Beam b);
        public event BeamHandler BeamArrived;

        public delegate void TankDieHandler(Beam b);

        private SocketState _server = null;

        private World _theWorld;
        private int _worldSize;
        
        private int _playerId;
        private string _playerName;

        private UserAction _action = new UserAction("none", "none", new Vector2D(0, 1));

        // user's action.
        public bool PlayerWantsUp = false;
        public bool PlayerWantsDown = false;
        public bool PlayerWantsLeft = false;
        public bool PlayerWantsRight = false;
        public bool PlayerWantsFire = false;
        public bool PlayerWantsBeam = false;

        // location of cursion with respect to the world.
        public double X = 1;
        public double Y = 0;
        
        /// <summary>
        /// This class contains actions that player wants to do.
        /// </summary>
        [JsonObject(MemberSerialization.OptIn)]
        public class UserAction
        {
            // moving direction of the tank.
            [JsonProperty(PropertyName = "moving")]
            public string orientation;
            // attack mode of the fire.
            [JsonProperty(PropertyName = "fire")]
            public string attack;
            // direction of the turrent.
            [JsonProperty(PropertyName = "tdir")]
            public Vector2D aiming;

            /// <summary>
            /// constructor of UserAction.
            /// </summary>
            /// <param name="moving">moving direction</param>
            /// <param name="fire">fire mode</param>
            /// <param name="tdir">turrent direction</param>
            public UserAction(string moving, string fire, Vector2D tdir)
            {
                orientation = moving;
                attack = fire;
                aiming = tdir;
            }
        }

        /// <summary>
        /// Get the world(model) of the game.
        /// </summary>
        /// <returns>the world, which is the model of this game</returns>
        public World GetWorld()
        {
            return _theWorld;
        }

        /// <summary>
        /// Constructor of GameController, initialize the model.
        /// </summary>
        public GameController()
        {
            _theWorld = new World();
        }

        /// <summary>
        /// Connect to the host and set the player name.
        /// </summary>
        /// <param name="player">player name</param>
        /// <param name="hostname">address of the host</param>
        public void Connect(string player, string hostname)
        {
            _playerName = player;
            Networking.ConnectToServer(FirstContact, hostname, 11000);
        }

        /// <summary>
        /// OnNetworkAction after connected.
        /// </summary>
        /// <param name="state">the SocketState</param>
        private void FirstContact(SocketState state)
        {
            //If anything happened during connection.
            if (state.ErrorOccurred)
            {
                // inform the view that error occurred.
                Error(state.ErrorMessage);
                return;
            }

            Networking.Send(state.TheSocket, _playerName + '\n');
            
            //inform the view that the connection has being established.
            state.OnNetworkAction = ReceiveStarupInfo;
            Networking.GetData(state);
        }

        /// <summary>
        /// OnNetworkAction. Start to receive starup info after connected.
        /// </summary>
        /// <param name="state">the SocketState</param>
        private void ReceiveStarupInfo(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                // inform the view that error occurs.
                Error("Lost connection to server");
                return;
            }

            string info = state.GetData();
            //If anything happened during connection.
            if (state.ErrorOccurred)
            {
                // inform the view that error occurred.
                Error(state.ErrorMessage);
                return;
            }

            string[] parts = Regex.Split(info, @"(?<=[\n])");

            // If didn't receive enough data, keep receiving
            if (parts.Length < 2 || !parts[1].EndsWith("\n"))
            {
                Networking.GetData(state);
                return;
            }

            try
            {
                _playerId = int.Parse(parts[0]);
                _worldSize = int.Parse(parts[1]);
            }
            catch (Exception e)
            {
                Error(e.Message);
            }

            _theWorld.ID = _playerId;
            _theWorld.WorldSize = _worldSize;

            state.RemoveData(0, parts[0].Length + parts[1].Length);

            _server = state;

            // start to receive Json
            state.OnNetworkAction = ReceiveJson;
            Networking.GetData(state);
        }

        /// <summary>
        /// OnNetworkAction. Start to receive Json after receiving starup info.
        /// </summary>
        /// <param name="state">the socketState</param>
        private void ReceiveJson(SocketState state)
        {
            string message = state.GetData();
            string[] parts = Regex.Split(message, @"(?<=[\n])");

            // Use to keep incompelete part .
            int lengthOfIncompeltePart = 0;
            lock (_theWorld)
            {
                foreach (string part in parts)
                {
                    // Ignore empty strings added by the regex splitter.
                    if (part.Length == 0)
                        continue;
                    // The regex splitter will include the last string even if it doesn't end with a '\n',
                    // So we need to ignore it if this happens.
                    if (part[part.Length - 1] != '\n')
                    {
                        lengthOfIncompeltePart = part.Length;
                        break;
                    }

                    JObject json = JObject.Parse(part);
                    // check the type of the object, and add it to corresponding dictionary
                    if (json["wall"] != null)
                    {
                        Wall wall = JsonConvert.DeserializeObject<Wall>(part);
                        _theWorld.Walls[wall.ID] = wall;
                    }
                    else if (json["tank"] != null)
                    {
                        Tank tank = JsonConvert.DeserializeObject<Tank>(part);
                        if (tank.HitPoint == 0)
                        {
                            tank.Died = true;
                        }
                        _theWorld.Tanks[tank.ID] = tank;
                    }
                    else if (json["proj"] != null)
                    {
                        Projectile projectile = JsonConvert.DeserializeObject<Projectile>(part);
                        _theWorld.Projectiles[projectile.ID] = projectile;
                    }
                    else if (json["power"] != null)
                    {
                        Powerup powerup = JsonConvert.DeserializeObject<Powerup>(part);
                        _theWorld.Powerups[powerup.ID] = powerup;
                    }
                    else if (json["beam"] != null)
                    {
                        Beam beam = JsonConvert.DeserializeObject<Beam>(part);
                        BeamArrived(beam);
                    }
                }
            }
            // remove processed data, keep data if it is incomplete.
            state.RemoveData(0, message.Length - lengthOfIncompeltePart);

            UpdateArrived?.Invoke();

            lock (_theWorld)
            {
                // determine the moving direction.
                if (PlayerWantsUp)
                {
                    _action.orientation = "up";
                }
                else if (PlayerWantsDown)
                {
                    _action.orientation = "down";
                }
                else if (PlayerWantsLeft)
                {
                    _action.orientation = "left";
                }
                else if (PlayerWantsRight)
                {
                    _action.orientation = "right";
                }
                else
                {
                    _action.orientation = "none";
                }

                // check the fire mode.
                if (PlayerWantsFire)
                {
                    _action.attack = "main";
                }
                else if (PlayerWantsBeam)
                {
                    _action.attack = "alt";
                }
                else 
                {
                    _action.attack = "none";
                }

                // set turrent direction.
                Vector2D v = new Vector2D(X, Y);
                v.Normalize();
                _action.aiming = v;

                // send Json string.
                string m = JsonConvert.SerializeObject(_action);
                lock (_theWorld)
                {
                    Networking.Send(_server.TheSocket, m + "\n");
                }
            }

            Networking.GetData(state);
        }

        /// <summary>
        /// Send a message to the server.
        /// </summary>
        /// <param name="message">The message we are sending to the server</param>
        public void MessageEntered(string message)
        {
            if (_server != null)
                Networking.Send(_server.TheSocket, message + "\n");
        }
    }
}
