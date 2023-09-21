// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using System;
using System.Collections.Generic;
using System.Xml;

namespace TankWars
{
    /// <summary>
    /// This class reads from XML file and store game setting.
    /// </summary>
    public class Settings
    {
        public int WorldSize { get; set; } = 2000;
        public int FramePerMS { get; set; } = 17;
        public int RespawnRate { get; set; } = 300;
        public int FramesPerShot { get; set; } = 80;
        public int MaxHitPoint { get; set; } = 3;
        public double EnginePower { get; set; } = 3;
        public double SuperEnginePower { get; set; } = 10;
        public int MaxNumOfPowers { get; set; } = 2;
        public int MaxPowerDelay { get; set; } = 1650;
        public bool SpeedMode { get; set; } = false;
        public HashSet<Wall> Walls { get; } = new HashSet<Wall>();

        /// <summary>
        /// Constructor for Setting.
        /// </summary>
        /// <param name="path"></param>
        public Settings(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };
            // read the file
            try
            {
                using (XmlReader reader = XmlReader.Create(path, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "UniverseSize":
                                    reader.Read();
                                    WorldSize = int.Parse(reader.Value);
                                    break;
                                case "MSPerFrame":
                                    reader.Read();
                                    FramePerMS = int.Parse(reader.Value);
                                    break;
                                case "FramesPerShot":
                                    reader.Read();
                                    FramesPerShot = int.Parse(reader.Value);
                                    break;
                                case "RespawnRate":
                                    reader.Read();
                                    RespawnRate = int.Parse(reader.Value);
                                    break;
                                case "MaxHitPoint":
                                    reader.Read();
                                    MaxHitPoint = int.Parse(reader.Value);
                                    break;
                                case "EnginePower":
                                    reader.Read();
                                    EnginePower = double.Parse(reader.Value);
                                    break;
                                case "SuperEnginePower":
                                    reader.Read();
                                    SuperEnginePower = double.Parse(reader.Value);
                                    break;
                                case "MaxNumOfPowers":
                                    reader.Read();
                                    MaxNumOfPowers = int.Parse(reader.Value);
                                    break;
                                case "MaxPowerDelay":
                                    reader.Read();
                                    MaxPowerDelay = int.Parse(reader.Value);
                                    break;
                                case "SpeedMode":
                                    reader.Read();
                                    if (reader.Value == "false")
                                    {
                                        SpeedMode = false;
                                    }
                                    else
                                    {
                                        SpeedMode = true;
                                    }
                                    break;
                                case "Wall":
                                    Vector2D p1 = null;
                                    Vector2D p2 = null;

                                    // Reads until reach p1.X.
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    double x = double.Parse(reader.Value);

                                    // Reads until reach p1.Y.
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    double y = double.Parse(reader.Value);
                                    p1 = new Vector2D(x, y);

                                    // Reads until reach p2.X.
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    x = double.Parse(reader.Value);

                                    // Reads until reach p2.Y.
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    y = double.Parse(reader.Value);

                                    p2 = new Vector2D(x, y);
                                    Wall wall = new Wall(p1, p2);
                                    Walls.Add(wall);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}