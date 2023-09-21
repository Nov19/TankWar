// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TankWars
{
    /// <summary>
    /// This class is used to draw the panel.
    /// </summary>
    public class DrawingPanel : Panel
    {
        private World _theWorld;
        private readonly Image _wallSprite = Image.FromFile(@"..\..\..\Resources\Images\WallSprite.png");
        private readonly Image _background = Image.FromFile(@"..\..\..\Resources\Images\Background.png");
        private readonly Image _tankSprite1 = Image.FromFile(@"..\..\..\Resources\Images\BlueTank.png");
        private readonly Image _turretSprite1 = Image.FromFile(@"..\..\..\Resources\Images\BlueTurret.png");
        private readonly Image _tankSprite2 = Image.FromFile(@"..\..\..\Resources\Images\DarkTank.png");
        private readonly Image _turretSprite2 = Image.FromFile(@"..\..\..\Resources\Images\DarkTurret.png");
        private readonly Image _tankSprite3 = Image.FromFile(@"..\..\..\Resources\Images\GreenTank.png");
        private readonly Image _turretSprite3 = Image.FromFile(@"..\..\..\Resources\Images\GreenTurret.png");
        private readonly Image _tankSprite4 = Image.FromFile(@"..\..\..\Resources\Images\LightGreenTank.png");
        private readonly Image _turretSprite4 = Image.FromFile(@"..\..\..\Resources\Images\LightGreenTurret.png");
        private readonly Image _tankSprite5 = Image.FromFile(@"..\..\..\Resources\Images\OrangeTank.png");
        private readonly Image _turretSprite5 = Image.FromFile(@"..\..\..\Resources\Images\OrangeTurret.png");
        private readonly Image _tankSprite6 = Image.FromFile(@"..\..\..\Resources\Images\PurpleTank.png");
        private readonly Image _turretSprite6 = Image.FromFile(@"..\..\..\Resources\Images\PurpleTurret.png");
        private readonly Image _tankSprite7 = Image.FromFile(@"..\..\..\Resources\Images\RedTank.png");
        private readonly Image _turretSprite7 = Image.FromFile(@"..\..\..\Resources\Images\RedTurret.png");
        private readonly Image _tankSprite8 = Image.FromFile(@"..\..\..\Resources\Images\YellowTank.png");
        private readonly Image _turretSprite8 = Image.FromFile(@"..\..\..\Resources\Images\YellowTurret.png");
        private readonly Image _projectileSprite1 = Image.FromFile(@"..\..\..\Resources\Images\shot-blue.png");
        private readonly Image _projectileSprite2 = Image.FromFile(@"..\..\..\Resources\Images\shot-brown.png");
        private readonly Image _projectileSprite3 = Image.FromFile(@"..\..\..\Resources\Images\shot-green.png");
        private readonly Image _projectileSprite4 = Image.FromFile(@"..\..\..\Resources\Images\shot-grey.png");
        private readonly Image _projectileSprite5 = Image.FromFile(@"..\..\..\Resources\Images\shot-white.png");
        private readonly Image _projectileSprite6 = Image.FromFile(@"..\..\..\Resources\Images\shot-violet.png");
        private readonly Image _projectileSprite7 = Image.FromFile(@"..\..\..\Resources\Images\shot-red.png");
        private readonly Image _projectileSprite8 = Image.FromFile(@"..\..\..\Resources\Images\shot-yellow.png");
        private readonly Image _powerSprite = Image.FromFile(@"..\..\..\Resources\Images\Power.png");
        private readonly Image _tankExplosions1 = Image.FromFile(@"..\..\..\Resources\Images\TankExplosion 1.png");
        private readonly Image _tankExplosions2 = Image.FromFile(@"..\..\..\Resources\Images\TankExplosion 2.png");
        private readonly Image _tankExplosions3 = Image.FromFile(@"..\..\..\Resources\Images\TankExplosion 3.png");
        private readonly Image _tankExplosions4 = Image.FromFile(@"..\..\..\Resources\Images\TankExplosion 4.png");
        private readonly Image _tankExplosions5 = Image.FromFile(@"..\..\..\Resources\Images\TankExplosion 5.png");
        private readonly Image _tankExplosions6 = Image.FromFile(@"..\..\..\Resources\Images\TankExplosion 6.png");

        /// <summary>
        /// constructor of DrawingPanel. Set the world.
        /// </summary>
        /// <param name="w"></param>
        public DrawingPanel(World w)
        {
            DoubleBuffered = true;
            _theWorld = w;
        }

        // For drawing beams.
        private Dictionary<int, BeamAnimation> _beamAnimations = new Dictionary<int, BeamAnimation>();

        // For drawing tank explosions.
        private Dictionary<int, TankExplosionAnimation> _tankExplosionAnimation = new Dictionary<int, TankExplosionAnimation>();

        /// <summary>
        /// A method to inform DrawingPanel a new beam to draw.
        /// </summary>
        /// <param name="beam"></param>
        public void AddABeamToDraw(Beam beam)
        {
            lock (_theWorld)
            {
                _beamAnimations.Add(beam.ID, new BeamAnimation(beam.ID, beam.Origin, beam.Direction));
            }
        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            e.Graphics.TranslateTransform((int)worldX, (int)worldY);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }

        /// <summary>
        /// Drawer for Wall.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void WallDrawer(object o, PaintEventArgs e)
        {
            int width = 50;
            int height = 50;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(_wallSprite, -(width / 2), -(height / 2), width, height);
        }

        /// <summary>
        /// Drawer for outer Beam.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void OuterBeamDrawer(object o, PaintEventArgs e)
        {
            BeamAnimation b = o as BeamAnimation;

            int width = 2;
            int height = 4000;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw tank explosion animation
            int frameCounter = _beamAnimations[b.ID].FrameCounter;
            using (Pen pen = new Pen(Color.FromArgb(128, 67, 104, 218)))
            {
                if (frameCounter > 26)
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                else if (frameCounter > 22)
                {
                    pen.Width = width * 3;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 18)
                {
                    pen.Width = width * 5;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 12)
                {
                    pen.Width = width * 7;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 8)
                {
                    pen.Width = width * 5;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 4)
                {
                    pen.Width = width * 3;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 0)
                {
                    pen.Width = width;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
            }
        }

        /// <summary>
        /// Drawer for inner Beam.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void InnerBeamDrawer(object o, PaintEventArgs e)
        {
            BeamAnimation b = o as BeamAnimation;

            int width = 2;
            int height = 4000;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw tank explosion animation
            int frameCounter = _beamAnimations[b.ID].FrameCounter;
            using (Pen pen = new Pen(Color.Coral))
            {
                if (frameCounter > 26)
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                else if (frameCounter > 22)
                {
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 18)
                {
                    pen.Width = width * 2;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 12)
                {
                    pen.Width = width * 2;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 8)
                {
                    pen.Width = width * 2;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 4)
                {
                    pen.Width = width * 2;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
                else if (frameCounter > 0)
                {
                    pen.Width = width;
                    e.Graphics.DrawLine(pen, -width / 2, -27, -width, -height);
                }
            }
        }
        /// <summary>
        /// Drawer for Powerup.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void PowerDrawer(object o, PaintEventArgs e)
        {
            Powerup p = o as Powerup;

            if (p.Died == true)
            {
                return;
            }

            int width = 25;
            int height = 25;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(_powerSprite, -(width / 2), -(height / 2), width, height);
        }

        /// <summary>
        /// Drawer for Projectile.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            Projectile p = o as Projectile;

            if (p.Died == true)
            {
                return;
            }

            int width = 30;
            int height = 30;
            // for drawing different projectiles
            int id = p.Owner % 8;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            switch (id)
            {
                case 0:
                    e.Graphics.DrawImage(_projectileSprite1, -(width / 2), -(height / 2), width, height);
                    break;
                case 1:
                    e.Graphics.DrawImage(_projectileSprite2, -(width / 2), -(height / 2), width, height);
                    break;
                case 2:
                    e.Graphics.DrawImage(_projectileSprite3, -(width / 2), -(height / 2), width, height);
                    break;
                case 3:
                    e.Graphics.DrawImage(_projectileSprite4, -(width / 2), -(height / 2), width, height);
                    break;
                case 4:
                    e.Graphics.DrawImage(_projectileSprite5, -(width / 2), -(height / 2), width, height);
                    break;
                case 5:
                    e.Graphics.DrawImage(_projectileSprite6, -(width / 2), -(height / 2), width, height);
                    break;
                case 6:
                    e.Graphics.DrawImage(_projectileSprite7, -(width / 2), -(height / 2), width, height);
                    break;
                case 7:
                    e.Graphics.DrawImage(_projectileSprite8, -(width / 2), -(height / 2), width, height);
                    break;
            }
        }

        /// <summary>
        /// Drawer for background.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void BackgroundDrawer(object o, PaintEventArgs e)
        {
            int width = _theWorld.WorldSize;
            int height = _theWorld.WorldSize;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(_background, -(width / 2), -(height / 2), width, height);
        }

        /// <summary>
        /// Drawer for Tank.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            int width = 60;
            int height = 60;
            // for drawing different tanks.
            int id = t.ID % 8;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            switch (id)
            {
                case 0:
                    e.Graphics.DrawImage(_tankSprite1, -(width / 2), -(height / 2), width, height);
                    break;
                case 1:
                    e.Graphics.DrawImage(_tankSprite2, -(width / 2), -(height / 2), width, height);
                    break;
                case 2:
                    e.Graphics.DrawImage(_tankSprite3, -(width / 2), -(height / 2), width, height);
                    break;
                case 3:
                    e.Graphics.DrawImage(_tankSprite4, -(width / 2), -(height / 2), width, height);
                    break;
                case 4:
                    e.Graphics.DrawImage(_tankSprite5, -(width / 2), -(height / 2), width, height);
                    break;
                case 5:
                    e.Graphics.DrawImage(_tankSprite6, -(width / 2), -(height / 2), width, height);
                    break;
                case 6:
                    e.Graphics.DrawImage(_tankSprite7, -(width / 2), -(height / 2), width, height);
                    break;
                case 7:
                    e.Graphics.DrawImage(_tankSprite8, -(width / 2), -(height / 2), width, height);
                    break;
            }
        }

        /// <summary>
        /// Drawer for Tank explosion.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void TankExplosionDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            int width = 60;
            int height = 60;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // add explosion animation when a tank dies
            lock (_theWorld)
            {
                if (!_tankExplosionAnimation.ContainsKey(t.ID))
                {
                    _tankExplosionAnimation.Add(t.ID, new TankExplosionAnimation(t.Location));
                }
            }

            int frameCounter = _tankExplosionAnimation[t.ID].FrameCounter;

            // draw tank explosion animation
            if (frameCounter > 35)
            {
                e.Graphics.DrawImage(_tankExplosions1, -(width / 2), -(height / 2), width, height);
            }
            else if (frameCounter > 30)
            {
                e.Graphics.DrawImage(_tankExplosions2, -(width / 2), -(height / 2), width, height);
            }
            else if (frameCounter > 25)
            {
                e.Graphics.DrawImage(_tankExplosions3, -(width / 2), -(height / 2), width, height);
            }
            else if (frameCounter > 20)
            {
                e.Graphics.DrawImage(_tankExplosions4, -(width / 2), -(height / 2), width, height);
            }
            else if (frameCounter > 15)
            {
                e.Graphics.DrawImage(_tankExplosions5, -(width / 2), -(height / 2), width, height);
            }
            else if (frameCounter > 0)
            {
                e.Graphics.DrawImage(_tankExplosions6, -(width / 2), -(height / 2), width, height);
            }
            else
            {
                // Don't draw if tank disconnects.
                if (!t.Disconnected)
                {
                    e.Graphics.DrawImage(_tankExplosions6, -(width / 2), -(height / 2), width, height);
                }
                else
                {
                    return;
                }
            }
            

            lock (_theWorld)
            {
                _tankExplosionAnimation[t.ID].FrameCounter--;
            }
        }

        /// <summary>
        /// Drawer for HP.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void HPDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            int width = 54;
            int height = 8;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw different colors for HP.
            using (SolidBrush blueBrush = new SolidBrush(Color.Blue))
            using (SolidBrush greenBrush = new SolidBrush(Color.Green))
            using (SolidBrush redBrush = new SolidBrush(Color.Red))
            {
                Rectangle r;
                if (t.HitPoint == 3)
                {
                    r = new Rectangle(-(width / 2), -40, width, height);
                    e.Graphics.FillRectangle(greenBrush, r);
                }
                else if (t.HitPoint == 2)
                {
                    width = 36;
                    r = new Rectangle(-((18 + width) / 2), -40, width, height);
                    e.Graphics.FillRectangle(blueBrush, r);
                }
                else if (t.HitPoint == 1)
                {
                    width = 18;
                    r = new Rectangle(-((36 + width) / 2), -40, width, height);
                    e.Graphics.FillRectangle(redBrush, r);
                }
            }
        }

        /// <summary>
        /// Drawer for text.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void TextDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            string name = t.Name + ": " + t.Score;

            Font font = new Font("Arial", 12);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            StringFormat format = new StringFormat() { Alignment = StringAlignment.Center };

            int width = 100;
            int height = 100;

            Rectangle r = new Rectangle(-(width / 2), 30, width, height);
            e.Graphics.DrawString(name, font, whiteBrush, r, format);
        }

        /// <summary>
        /// Drawer for Turrent.
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void TurrentDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            // do not draw turrent if the tank dies or disconnects
            if (t.Died == true || t.Disconnected == true)
            {
                return;
            }

            int width = 50;
            int height = 50;
            // for drawing different turrent
            int id = t.ID % 8;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            switch (id)
            {
                case 0:
                    e.Graphics.DrawImage(_turretSprite1, -(width / 2), -(height / 2), width, height);
                    break;
                case 1:
                    e.Graphics.DrawImage(_turretSprite2, -(width / 2), -(height / 2), width, height);
                    break;
                case 2:
                    e.Graphics.DrawImage(_turretSprite3, -(width / 2), -(height / 2), width, height);
                    break;
                case 3:
                    e.Graphics.DrawImage(_turretSprite4, -(width / 2), -(height / 2), width, height);
                    break;
                case 4:
                    e.Graphics.DrawImage(_turretSprite5, -(width / 2), -(height / 2), width, height);
                    break;
                case 5:
                    e.Graphics.DrawImage(_turretSprite6, -(width / 2), -(height / 2), width, height);
                    break;
                case 6:
                    e.Graphics.DrawImage(_turretSprite7, -(width / 2), -(height / 2), width, height);
                    break;
                case 7:
                    e.Graphics.DrawImage(_turretSprite8, -(width / 2), -(height / 2), width, height);
                    break;
            }
        }

        /// <summary>
        /// This method is invoked when the DrawingPanel needs to be re-drawn
        /// </summary>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Do not start to draw until receive ID.
            if (!_theWorld.Tanks.ContainsKey(_theWorld.ID))
            {
                return;
            }

            // view is square, so we can just use width.
            int viewSize = Size.Width;

            // Center the view at the player's tank.
            lock (_theWorld)
            {
                // get player's location.
                float playerX = (float)_theWorld.Tanks[_theWorld.ID].Location.GetX();
                float playerY = (float)_theWorld.Tanks[_theWorld.ID].Location.GetY();
                e.Graphics.TranslateTransform(-playerX + viewSize / 2, -playerY + viewSize / 2);
                DrawObjectWithTransform(e, _theWorld, 0, 0, 0, BackgroundDrawer);
            }

            // Draw walls.
            lock (_theWorld)
            {
                foreach (Wall wall in _theWorld.Walls.Values)
                {
                    double diffInX = wall.Endpoint1.GetX() - wall.Endpoint2.GetX();
                    double diffInY = wall.Endpoint1.GetY() - wall.Endpoint2.GetY();

                    // start and end of the changing dimension
                    double startPoint = 0.0;
                    double endPoint = 0.0;

                    // position of the unchanged dimension
                    double position = 0.0;

                    // if differents of Ys is not zero, the wall is vertical.
                    if (diffInY != 0)
                    {
                        startPoint = wall.Endpoint1.GetY();
                        endPoint = wall.Endpoint2.GetY();
                        position = wall.Endpoint1.GetX();
                        // is differentce is larger than zero, draw the wall from bottom to the top
                        if (diffInY > 0)
                        {
                            while (startPoint >= endPoint)
                            {
                                DrawObjectWithTransform(e, wall, position, startPoint, 0, WallDrawer);
                                startPoint -= 50;
                            }
                        }
                        // is differentce is larger than zero, draw the wall from top to the bottom
                        else
                        {
                            while (startPoint <= endPoint)
                            {
                                DrawObjectWithTransform(e, wall, position, startPoint, 0, WallDrawer);
                                startPoint += 50;
                            }
                        }
                    }
                    // if differents of Xs is not zero, the wall is horizontal.
                    else
                    {
                        startPoint = wall.Endpoint1.GetX();
                        endPoint = wall.Endpoint2.GetX();
                        position = wall.Endpoint1.GetY();
                        // is differentce is larger than zero, draw the wall from right to the left
                        if (diffInX > 0)
                        {
                            while (startPoint >= endPoint)
                            {
                                DrawObjectWithTransform(e, wall, startPoint, position, 0, WallDrawer);
                                startPoint -= 50;
                            }
                        }
                        // is differentce is larger than zero, draw the wall from left to the right
                        else
                        {
                            while (startPoint <= endPoint)
                            {
                                DrawObjectWithTransform(e, wall, startPoint, position, 0, WallDrawer);
                                startPoint += 50;
                            }
                        }
                    }
                }
            }

            // Draw tanks, turrents, HPs, names and IDs.
            lock (_theWorld)
            {
                foreach (Tank tank in _theWorld.Tanks.Values)
                {
                    // draw tank if it is alive
                    if (!tank.Died && !tank.Disconnected)
                    {
                        // remove the tank from exlosion animation if it revives
                        if (_tankExplosionAnimation.ContainsKey(tank.ID))
                        {
                            _tankExplosionAnimation.Remove(tank.ID);
                        }
                        DrawObjectWithTransform(e, tank, tank.Location.GetX(), tank.Location.GetY(), tank.Orientation.ToAngle(), TankDrawer);
                        DrawObjectWithTransform(e, tank, tank.Location.GetX(), tank.Location.GetY(), tank.Aiming.ToAngle(), TurrentDrawer);
                        DrawObjectWithTransform(e, tank, tank.Location.GetX(), tank.Location.GetY(), 0, HPDrawer);
                        DrawObjectWithTransform(e, tank, tank.Location.GetX(), tank.Location.GetY(), 0, TextDrawer);
                    }
                    // draw tank explosion when a tank dies
                    else
                    {
                        DrawObjectWithTransform(e, tank, tank.Location.GetX(), tank.Location.GetY(), 0, TankExplosionDrawer);
                    }
                }
            }

            // Draw projectiles.
            lock (_theWorld)
            {
                foreach (Projectile projectile in _theWorld.Projectiles.Values)
                {
                    DrawObjectWithTransform(e, projectile, projectile.Location.GetX(), projectile.Location.GetY(), projectile.Direction.ToAngle(), ProjectileDrawer);
                }
            }

            // Draw powerups.
            lock (_theWorld)
            {
                foreach (Powerup power in _theWorld.Powerups.Values)
                {
                    DrawObjectWithTransform(e, power, power.Location.GetX(), power.Location.GetY(), 0, PowerDrawer);
                }
            }

            // Draw beams.
            lock (_theWorld)
            {
                foreach (BeamAnimation b in _beamAnimations.Values)
                {
                    // draw outer beam.
                    DrawObjectWithTransform(e, b, b.Origin.GetX(), b.Origin.GetY(), b.Direction.ToAngle(), OuterBeamDrawer);
                    // draw inner beam.
                    DrawObjectWithTransform(e, b, b.Origin.GetX(), b.Origin.GetY(), b.Direction.ToAngle(), InnerBeamDrawer);
                    _beamAnimations[b.ID].FrameCounter--;
                }
            }

            //Do anything that Panel(from which we inherit) needs to do
            lock (_theWorld)
            {
                base.OnPaint(e);
            }
        }
    }
}


