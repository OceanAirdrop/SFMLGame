using OceanAirdrop.SharedLib;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OceanAirdrop.SharedLib.XboxController;

namespace OceanAirdrop.JoystickInput
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

            CircleShape shape = new CircleShape(10);
            shape.FillColor = new Color(Color.Red);

            RectangleShape square = new RectangleShape();
            square.FillColor    = new Color(Color.Red);
            square.Position     = new Vector2f(app.Size.X / 2, app.Size.Y / 2);
            square.OutlineColor = new Color(0, 0, 0, 255);
            square.Size         = new Vector2f(50, 50);

            // Start the game loop
            while (app.IsOpen())
            {
                // Process events
                app.DispatchEvents();

                // Clear screen
                app.Clear(windowColor);

                app.Draw(shape);
                app.Draw(square);

                Console.WriteLine(XboxController.RefreshButtonPressed());
                Console.WriteLine(XboxController.RefreshAxisPressed());


                if (JoyState.HasFlag(ControllerState.DPAD_UP_PRESSED))
                    square.Position = new Vector2f(square.Position.X, square.Position.Y - 1);

                if (JoyState.HasFlag(ControllerState.DPAD_DOWN_PRESSED))
                    square.Position = new Vector2f(square.Position.X, square.Position.Y + 1);

                if (JoyState.HasFlag(ControllerState.DPAD_LEFT_PRESSED))
                    square.Position = new Vector2f(square.Position.X - 1, square.Position.Y);

                if (JoyState.HasFlag(ControllerState.DPAD_RIGHT_PRESSED))
                    square.Position = new Vector2f(square.Position.X + 1, square.Position.Y);


                // Update the window
                app.Display();
            }
        }
    }
}
