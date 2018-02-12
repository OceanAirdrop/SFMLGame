using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanAirdrop.Setup
{
    class Program
    {
        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        static void Main(string[] args)
        {
            RenderWindow app = new RenderWindow(new VideoMode(800, 600), "OMG, it works!");
            app.Closed += new EventHandler(OnClose);

            Color windowColor = new Color(77, 255, 130);

            // Start the game loop
            while (app.IsOpen())
            {
                // Process events
                app.DispatchEvents();

                // Clear screen
                app.Clear(windowColor);

                // Update the window
                app.Display();
            }
        }
    }
}
