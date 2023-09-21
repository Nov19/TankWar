// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using System;
using System.IO;

namespace TankWars
{
    /// <summary>
    /// Class for starting the Server.
    /// </summary>
    class Server
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Resources\\settings.xml");
            Settings settings = new Settings(path);

            ServerController serverController = new ServerController(settings);

            serverController.Start();
            Console.Read();
        }
    }
}