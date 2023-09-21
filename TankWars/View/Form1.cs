// Author: Xing Liu, Jinwen Lei, Fall 2021
// University of Utah
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TankWars
{
    /// <summary>
    /// This class is the view of TankWars.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly GameController _theController = new GameController();
        private readonly DrawingPanel _drawingPanel;
        
        private readonly int ViewSize = 900;
        private const int MenuSize = 40;

        /// <summary>
        /// initialize the form and set up drawing panel.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            
            // Set the window size
            World theWorld = _theController.GetWorld();
            ClientSize = new Size(ViewSize, ViewSize + MenuSize);

            // set up drawing panel
            _drawingPanel = new DrawingPanel(theWorld);
            _drawingPanel.Location = new Point(0, MenuSize);
            _drawingPanel.Size = new Size(ViewSize, ViewSize);
            Controls.Add(_drawingPanel);

            //register event handlers
            _theController.UpdateArrived += OnFrame;
            _theController.BeamArrived += HandleBeam;
            _theController.Error += RetryConnect;

            _drawingPanel.MouseDown += HandleMouseDown;
            _drawingPanel.MouseUp += HandleMouseUp;
            _drawingPanel.KeyDown += HandleKeyDown;
            _drawingPanel.KeyUp += HandleKeyUp;
            _drawingPanel.MouseMove += HandleMouseMove;
            _drawingPanel.MouseClick += HandleMouseClick;
        }

        /// <summary>
        /// Handler for the controller's UpdateArrived event.
        /// </summary>
        private void OnFrame()
        {
            // Invalidate this form and all its children
            // This will cause the form to redraw as soon as it can
            Invoke(new MethodInvoker(() => Invalidate(true)));
        }

        /// <summary>
        /// Tell the DrawingPanel to draw beams.
        /// </summary>
        /// <param name="b">a Beam object passed from Controller and converted from a Json obj</param>
        private void HandleBeam(Beam b)
        {
            _drawingPanel.AddABeamToDraw(b);
        }

        /// <summary>
        /// Handle mouse move events.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            // get the location offset by half view size.
            _theController.X = e.X-ViewSize/2;
            _theController.Y = e.Y-ViewSize/2;
        }

        /// <summary>
        /// Handle keyboard keys down event.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                _theController.PlayerWantsUp = true;
            }

            if (e.KeyCode == Keys.S)
            {
                _theController.PlayerWantsDown = true;
            }

            if (e.KeyCode == Keys.A)
            {
                _theController.PlayerWantsLeft = true;
            }

            if (e.KeyCode == Keys.D)
            {
                _theController.PlayerWantsRight = true;
            }

            if (e.KeyCode == Keys.Space)
            {
                _theController.PlayerWantsFire = true;
            }
        }
        
        /// <summary>
        /// Handle keyboard keys up events.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                _theController.PlayerWantsUp = false;
            }

            if (e.KeyCode == Keys.S)
            {
                _theController.PlayerWantsDown = false;
            }

            if (e.KeyCode == Keys.A)
            {
                _theController.PlayerWantsLeft = false;
            }

            if (e.KeyCode == Keys.D)
            {
                _theController.PlayerWantsRight = false;
            }

            if (e.KeyCode == Keys.Space)
            {
                _theController.PlayerWantsFire = false;
            }

        }

        /// <summary>
        /// Handle mouse keys down events.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _theController.PlayerWantsBeam = true;
            }

            if (e.Button == MouseButtons.Left)
            {
                _theController.PlayerWantsFire = true;
            }

        }

        /// <summary>
        /// Handle mouse keys up events.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _theController.PlayerWantsBeam = false;
            }

            if (e.Button == MouseButtons.Left)
            {
                _theController.PlayerWantsFire = false;
            }
        }

        /// <summary>
        /// Focus to panel if panel is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseClick(object sender, MouseEventArgs e)
        {
            _drawingPanel.Focus();
        }

        /// <summary>
        /// Retry when connection failed.
        /// </summary>
        /// <param name="s">The error message to be displayed</param>
        private void RetryConnect(string s)
        {
            var result = MessageBox.Show(s, "Connection Failed", MessageBoxButtons.RetryCancel);
            if (result == DialogResult.Retry) 
            {
                _theController.Connect(playerNameBox.Text, serverAddressBox.Text);
                _drawingPanel.Focus();
            }
        }

        /// <summary>
        /// Handle "Connect" Button click events.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void connectButton_Click(object sender, EventArgs e)
        {
            _theController.Connect(playerNameBox.Text, serverAddressBox.Text);
            _drawingPanel.Focus();
        }

        /// <summary>
        /// Handle playerNameBox_TextChanged events and decided whether the "Connect" button is enable or not.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void playerNameBox_TextChanged(object sender, EventArgs e)
        {
            // button is enabled when both textboxes are filled
            if (!string.IsNullOrEmpty(playerNameBox.Text) && !string.IsNullOrEmpty(serverAddressBox.Text))
            {
                connectButton.Enabled = true;
            }

            if (string.IsNullOrEmpty(playerNameBox.Text))
            {
                connectButton.Enabled = false;
            }
        }

        /// <summary>
        /// Handle serverAddressBox_TextChanged events and decided whether the "Connect" button is enable or not.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void serverAddressBox_TextChanged(object sender, EventArgs e)
        {
            // button is enabled when both textboxes are filled.
            if (!string.IsNullOrEmpty(playerNameBox.Text) && !string.IsNullOrEmpty(serverAddressBox.Text))
            {
                connectButton.Enabled = true;
            }

            if (string.IsNullOrEmpty(serverAddressBox.Text))
            {
                connectButton.Enabled = false;
            }
        }

        /// <summary>
        /// Handler for clicking help button.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void Help_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader(@"..\..\..\Resources\Help.txt");
            MessageBox.Show(reader.ReadToEnd(), "Help", MessageBoxButtons.OK);
            _drawingPanel.Focus();
        }
    }
}
