// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TankWars
{
    /// <summary>
    /// This class have dictionarys that contains other elments in the game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class World
    {
        [JsonProperty(PropertyName = "worldSize")]
        public int WorldSize { get; set; }
        public readonly Dictionary<int, Wall> Walls;
        public Dictionary<int, Tank> Tanks;
        public Dictionary<int, Powerup> Powerups;
        public Dictionary<int, Projectile> Projectiles;
        public Dictionary<int, Beam> Beams;
        public Dictionary<int, ControlCommand> Commands = new Dictionary<int, ControlCommand>();
        public int ID;
        public Random rnd = new Random();

        // Parameters spawning a power.
        public int MaxNumOfPowers = 2;
        public int MaxPowerDelay = 1650;
        public int PowerSpawningCoolDown = 0;

        // Boolean for additional mode.
        public bool SpeedMode = false;

        /// <summary>
        /// Constructor of world class.
        /// </summary>
        public World()
        {
            Walls = new Dictionary<int, Wall>();
            Tanks = new Dictionary<int, Tank>();
            Powerups = new Dictionary<int, Powerup>();
            Projectiles = new Dictionary<int, Projectile>();
            Beams = new Dictionary<int, Beam>();
        }

        /// <summary>
        /// Updating the model for next frame.
        /// </summary>
        public void Update()
        {
            lock (Commands)
            {
                // Clears beams after they are shotted.
                Beams.Clear();

                foreach (KeyValuePair<int, ControlCommand> command in Commands)
                {
                    Tank tank = Tanks[command.Key];

                    // Decrements the cooldown for new power
                    if (PowerSpawningCoolDown != 0)
                    {
                        PowerSpawningCoolDown--;
                    }

                    // Only alive tanks can take action.
                    if (tank.HitPoint != 0)
                    {
                        switch (command.Value.Orientation)
                        {
                            case "up":
                                tank.Velocity = new Vector2D(0, -1);
                                tank.Orientation = new Vector2D(0, -1);
                                break;
                            case "down":
                                tank.Velocity = new Vector2D(0, 1);
                                tank.Orientation = new Vector2D(0, 1);
                                break;
                            case "left":
                                tank.Velocity = new Vector2D(-1, 0);
                                tank.Orientation = new Vector2D(-1, 0);
                                break;
                            case "right":
                                tank.Velocity = new Vector2D(1, 0);
                                tank.Orientation = new Vector2D(1, 0);
                                break;
                            default:
                                tank.Velocity = new Vector2D(0, 0);
                                break;
                        }

                        // Updates direction of turrent.
                        tank.Aiming = command.Value.Aiming;

                        switch (command.Value.Attack)
                        {
                            case "main":
                                // A tank can shot when cooldown is ready.
                                if (tank.ShotCoolDownCounter == 0)
                                {
                                    Projectile projectile = new Projectile(tank.ID, tank.Location, tank.Aiming);
                                    Projectiles.Add(projectile.ID, projectile);
                                    tank.ShotCoolDownCounter = tank.FramePerShot;
                                }
                                break;
                            case "alt":
                                // A tank can shot beam when it has power.
                                if (tank.EnergyPoints > 0)
                                {
                                    Beam beam = new Beam(tank.ID, tank.Location, tank.Aiming);
                                    Beams.Add(beam.ID, beam);
                                    tank.EnergyPoints--;
                                }
                                break;
                        }

                        // if speed mode is on, accelerate the tank.
                        if (tank.AcceleratingCounter != 0)
                        {
                            tank.Velocity *= tank.SuperEnginePower;
                            tank.AcceleratingCounter--;
                        }
                        else
                        {
                            tank.Velocity *= tank.EnginePower;
                        }
                    }
                }
                Commands.Clear();


                // Deletes died projectiles.
                Dictionary<int, Projectile> projectilesCopy = new Dictionary<int, Projectile>(Projectiles);
                foreach (Projectile projectile in Projectiles.Values)
                {
                    if (projectile.Died)
                    {
                        projectilesCopy.Remove(projectile.ID);
                    }
                }
                Projectiles = projectilesCopy;

                // Delete died powerups.
                Dictionary<int, Powerup> powerupsCopy = new Dictionary<int, Powerup>(Powerups);
                foreach (Powerup power in Powerups.Values)
                {
                    if (power.Died)
                    {
                        powerupsCopy.Remove(power.ID);
                    }
                }
                Powerups = powerupsCopy;

                // Deletes disconnected tanks.
                Dictionary<int, Tank> tanksCopy = new Dictionary<int, Tank>(Tanks);
                foreach (Tank tank in Tanks.Values)
                {
                    if (tank.Died && tank.Disconnected)
                    {
                        tanksCopy.Remove(tank.ID);
                    }
                }
                Tanks = tanksCopy;

                foreach (Tank tank in Tanks.Values)
                {
                    // Sets disconneted tank to died.
                    if (tank.Disconnected && !tank.Died)
                    {
                        tank.Died = true;
                        continue;
                    }

                    // Sets Died to false after explision was drawn.
                    tank.Died = false;

                    // Respawn tank if respawn cooldown is ready.
                    if (tank.RespawnCoolDownCounter == 0 && tank.HitPoint == 0)
                    {
                        tank.HitPoint = tank.MaxHitPoint;
                        tank.Location = RandomLocationForTank();
                    }

                    // Decrements time if a tank is in shot cooldown.
                    if (tank.ShotCoolDownCounter != 0)
                    {
                        tank.ShotCoolDownCounter--;
                    }

                    // Decrements time if a tank is in respawn cooldown.
                    if (tank.RespawnCoolDownCounter != 0)
                    {
                        tank.RespawnCoolDownCounter--;
                    }

                    if (tank.Velocity.Length() == 0 || tank.HitPoint == 0 || tank.Died)
                        continue;

                    Vector2D newLoc = tank.Location + tank.Velocity;

                    // Wraps around if a tank reaches the border of the world.
                    if (newLoc.GetX() > WorldSize / 2 - Tank.TankSize / 2)
                    {
                        newLoc = new Vector2D(newLoc.GetX() + Tank.TankSize - WorldSize, newLoc.GetY());
                    }
                    else if (newLoc.GetX() < -WorldSize / 2 + Tank.TankSize / 2)
                    {
                        newLoc = new Vector2D(newLoc.GetX() - Tank.TankSize + WorldSize, newLoc.GetY());
                    }
                    else if (newLoc.GetY() > WorldSize / 2 - Tank.TankSize / 2)
                    {
                        newLoc = new Vector2D(newLoc.GetX(), newLoc.GetY() + Tank.TankSize - WorldSize);
                    }
                    else if (newLoc.GetY() < -WorldSize / 2 + Tank.TankSize / 2)
                    {
                        newLoc = new Vector2D(newLoc.GetX(), newLoc.GetY() - Tank.TankSize + WorldSize);
                    }

                    bool collision = false;

                    // Checks tank-wall collision.
                    foreach (Wall wall in Walls.Values)
                    {
                        if (wall.CollidesTank(newLoc))
                        {
                            collision = true;
                            break;
                        }
                    }

                    if (!collision)
                    {
                        tank.Location = newLoc;
                    }

                    // Checks tank-power collision. 
                    foreach (Powerup power in Powerups.Values)
                    {
                        if (power.CollidesTank(tank.Location))
                        {
                            // replenish energy point for beam
                            if (!SpeedMode)
                            {
                                tank.EnergyPoints++;
                            }
                            // replenish energy point for accelerating
                            else
                            {
                                tank.AcceleratingCounter = Tank.AcceleratingTime;
                            }
                            power.Died = true;
                        }
                    }
                }

                // Puts power at a random position.
                if (Powerups.Count < MaxNumOfPowers)
                {
                    Powerup newPower = new Powerup(RandomLocation());
                    Powerups.Add(newPower.ID, newPower);
                    PowerSpawningCoolDown = rnd.Next(MaxPowerDelay);
                }

                // Checks projectile-wall, projectile-tank collision.
                foreach (Projectile projectile in Projectiles.Values)
                {
                    projectile.Velocity = projectile.Direction * Projectile.Speed;
                    Vector2D newLoc = projectile.Location + projectile.Velocity;

                    bool collision = false;

                    // Checks projectile-wall collisions.
                    foreach (Wall wall in Walls.Values)
                    {
                        if (wall.CollidesObject(newLoc))
                        {
                            collision = true;
                            break;
                        }
                    }

                    // Checks projectile-tank collisions.
                    foreach (Tank tank in Tanks.Values)
                    {
                        if (tank.HitPoint > 0 && tank.CollidesProjectile(newLoc, projectile.Owner))
                        {
                            collision = true;
                            tank.HitPoint--;

                            // Gives a point to owner and set the tank that was hit to die.
                            if (tank.HitPoint == 0)
                            {
                                tank.Died = true;
                                tank.RespawnCoolDownCounter = tank.RespawnRate;
                                Tanks[projectile.Owner].Score++;
                            }

                            break;
                        }
                    }

                    if (!collision)
                    {
                        projectile.Location = newLoc;
                    }
                    // Sets projectile to die if it surpass border of the world.
                    else if (projectile.Location.GetX() > WorldSize / 2 || projectile.Location.GetX() < -WorldSize / 2 || projectile.Location.GetY() > WorldSize / 2 || projectile.Location.GetY() < -WorldSize / 2)
                    {
                        projectile.Died = true;
                    }
                    else
                    {
                        projectile.Died = true;
                    }
                }

                // check beam-tank collision.
                foreach (Beam beam in Beams.Values)
                {
                    foreach (Tank tank in Tanks.Values)
                    {
                        if (Beam.Intersects(beam.Origin, beam.Direction, tank.Location, Tank.TankSize))
                        {
                            tank.Died = true;
                            tank.RespawnCoolDownCounter = tank.RespawnRate;
                            tank.HitPoint = 0;
                            Tanks[beam.Owner].Score++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method for getting a random valid position for a powerup.
        /// </summary>
        /// <returns></returns>
        private Vector2D RandomLocation()
        {
            Vector2D randomLoc;
            // keep looking for position until it is valid.
            while (true)
            {
                bool collide = false;
                Vector2D randomVec = RandomVector(); ;
                foreach (Wall wall in Walls.Values)
                {
                    if (wall.CollidesObject(randomVec))
                    {
                        collide = true;
                        break;
                    }
                }
                if (!collide)
                {
                    randomLoc = randomVec;
                    break;
                }
            }
            return randomLoc;
        }

        /// <summary>
        /// Method for getting a random valid position for a tank.
        /// </summary>
        /// <returns></returns>
        public Vector2D RandomLocationForTank()
        {
            Vector2D randomLoc;
            // keep looking for position until it is valid.
            while (true)
            {
                bool collide = false;
                Vector2D randomVec = RandomVector();
                foreach (Wall wall in Walls.Values)
                {
                    if (wall.CollidesTank(randomVec))
                    {
                        collide = true;
                        break;
                    }
                }
                if (!collide)
                {
                    randomLoc = randomVec;
                    break;
                }
            }
            return randomLoc;
        }

        /// <summary>
        /// Gets a random vector.
        /// </summary>
        /// <returns>A 2D vector.</returns>
        private Vector2D RandomVector()
        {
            return new Vector2D((double)rnd.Next(WorldSize) - WorldSize / 2, (double)rnd.Next(WorldSize) - WorldSize / 2);
        }
    }
}
