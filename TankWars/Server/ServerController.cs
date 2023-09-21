// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

namespace TankWars
{
    /// <summary>
    /// This class is the controller of server.
    /// </summary>
    public class ServerController
    {
        private World theWorld;
        private Settings Settings { get; }
        // stores all cilent socket states.
        private Dictionary<int, SocketState> clients;
        private string startupInfo;

        /// <summary>
        /// Constructor for ServerController.
        /// </summary>
        /// <param name="settings">Game setting.</param>
        public ServerController(Settings settings)
        {
            clients = new Dictionary<int, SocketState>();
            Settings = settings;
            theWorld = new World() { WorldSize = Settings.WorldSize, MaxNumOfPowers = Settings.MaxNumOfPowers, MaxPowerDelay = Settings.MaxPowerDelay, SpeedMode = Settings.SpeedMode };

            foreach (Wall wall in settings.Walls)
            {
                theWorld.Walls[wall.ID] = wall;
            }

            StringBuilder sb = new StringBuilder();

            // store world size and walls info.
            sb.Append(theWorld.WorldSize + "\n");

            foreach (Wall wall in theWorld.Walls.Values)
            {
                sb.Append(wall.ToString());
            }

            startupInfo = sb.ToString();
        }

        /// <summary>
        /// Starts the server and updating model.
        /// </summary>
        internal void Start()
        {
            Networking.StartServer(StartServerCallback, 11000);
            Console.WriteLine("Server is runnin. Accepting new clients.");
            Thread newThread = new Thread(Update);
            newThread.Start();
        }

        /// <summary>
        /// Infinite event loop for sending updated model to clients.
        /// </summary>
        private void Update()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                // Sends to clients every FPS.
                while (watch.ElapsedMilliseconds < Settings.FramePerMS) ;

                watch.Restart();

                StringBuilder sb = new StringBuilder();
                lock (theWorld)
                {
                    theWorld.Update();

                    foreach (Tank tank in theWorld.Tanks.Values)
                    {
                        sb.Append(tank.ToString());
                    }

                    foreach (Powerup power in theWorld.Powerups.Values)
                    {
                        sb.Append(power.ToString());
                    }

                    foreach (Projectile projectile in theWorld.Projectiles.Values)
                    {
                        sb.Append(projectile.ToString());
                    }

                    foreach (Beam beam in theWorld.Beams.Values)
                    {
                        sb.Append(beam.ToString());
                    }
                }

                string updateAtAFrame = sb.ToString();

                lock (clients)
                {
                    foreach (SocketState client in clients.Values)
                    {
                        Networking.Send(client.TheSocket, updateAtAFrame);
                    }
                }
            }
        }

        /// <summary>
        /// Callback for StartServer.
        /// </summary>
        /// <param name="client">SocketState for a cilent</param>
        private void StartServerCallback(SocketState client)
        {
            client.OnNetworkAction = ReceivePlayerName;
            Networking.GetData(client);
        }

        /// <summary>
        /// OnNetworkAction for processing player's name.
        /// </summary>
        /// <param name="client">SocketState for a cilent</param>
        private void ReceivePlayerName(SocketState client)
        {
            string name = client.GetData();

            if (string.IsNullOrEmpty(name) && !name.EndsWith("\n"))
            {
                client.GetData();
                return;
            }

            client.RemoveData(0, name.Length);
            name = name.Trim();

            // Sends client's ID, WorldSize and walls.
            Networking.Send(client.TheSocket, client.ID + "\n" + startupInfo);

            // Allocates a new tank to the new client at a random position.
            lock (theWorld)
            {
                theWorld.Tanks[(int)client.ID] = new Tank((int)client.ID, name, theWorld.RandomLocationForTank(), Settings.FramesPerShot, Settings.RespawnRate, Settings.MaxHitPoint, Settings.EnginePower, Settings.SuperEnginePower);
            }

            lock (clients)
            {
                clients.Add((int)client.ID, client);
            }

            client.OnNetworkAction = ReceiveControlCommands;
            Networking.GetData(client);
        }

        /// <summary>
        /// OnNetworkAction for processing player's command.
        /// </summary>
        /// <param name="client">SocketState for a cilent</param>
        private void ReceiveControlCommands(SocketState client)
        {
            // remove the client and deal with the tank if the socketstate is abnormal.
            if (client.ErrorOccurred)
            {
                lock (clients)
                {
                    RemoveClient((int)client.ID);
                }
                lock (theWorld.Commands)
                {
                    theWorld.Tanks[(int)client.ID].Disconnected = true;
                    theWorld.Tanks[(int)client.ID].HitPoint = 0;
                }
                return;
            }

            string totalData = client.GetData();

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Used for tracking incomplete command.
            int lengthOfIncompletePart = 0;

            foreach (string part in parts)
            {
                if (part.Length == 0)
                    continue;
                if (part[part.Length - 1] != '\n')
                {
                    // Records the length and do not remove it.
                    lengthOfIncompletePart = part.Length;
                    break;
                }

                ControlCommand command = JsonConvert.DeserializeObject<ControlCommand>(part);

                lock (theWorld)
                {
                    theWorld.Commands[(int)client.ID] = command;
                }
            }

            // Removes data except the incomplete command.
            client.RemoveData(0, totalData.Length - lengthOfIncompletePart);

            Networking.GetData(client);
        }

        /// <summary>
        /// Removes a client from the clients dictionary.
        /// </summary>
        /// <param name="id">The ID of the client</param>
        private void RemoveClient(long id)
        {
            Console.WriteLine("Client " + id + " disconnected");
            lock (clients)
            {
                clients.Remove((int)id);
            }
        }
    }
}