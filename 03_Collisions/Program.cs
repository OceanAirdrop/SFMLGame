using OceanAirdrop.SharedLib;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OceanAirdrop.SharedLib.XboxController;

namespace OceanAirdrop.Collisions
{
    class Program
    {
        private static Random RandomNum = new Random();

        private static UInt32 ScreenWidth = 800;
        private static UInt32 ScreenHeight = 600;

        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        static RectangleShape CreateSquare(Color c, Vector2f pos, Vector2f size)
        {
            RectangleShape square = new RectangleShape();
            square.FillColor = new Color(c);
            square.Position = pos;
            square.OutlineColor = new Color(0, 0, 0, 255);
            square.Size = size;

            return square;
        }

        static void Main(string[] args)
        {
            RenderWindow app = new RenderWindow(new VideoMode(ScreenWidth, ScreenHeight), "Simple Collision Detection!");
            app.Closed += new EventHandler(OnClose);

            Color windowColor = new Color(0, 192, 255);

            RectangleShape player = CreateSquare(Color.Black, RespawnCentreScreen(), new Vector2f(30, 30));
            RectangleShape food   = CreateSquare(Color.Magenta, new Vector2f(100, 200), new Vector2f(20, 20));
            RectangleShape enemy  = CreateSquare(Color.Red, new Vector2f(500, 500), new Vector2f(20, 20));

            // Start the game loop
            while (app.IsOpen())
            {
                // Process events
                app.DispatchEvents();

                // Clear screen
                app.Clear(windowColor);

                // Draw things
                app.Draw(food);
                app.Draw(enemy);
                app.Draw(player);

                Console.WriteLine(XboxController.RefreshButtonPressed());
                Console.WriteLine(XboxController.RefreshAxisPressed());

                if (JoyState.HasFlag(ControllerState.DPAD_UP_PRESSED))
                    player.Position = new Vector2f(player.Position.X, player.Position.Y - 1);

                if (JoyState.HasFlag(ControllerState.DPAD_DOWN_PRESSED))
                    player.Position = new Vector2f(player.Position.X, player.Position.Y + 1);

                if (JoyState.HasFlag(ControllerState.DPAD_LEFT_PRESSED))
                    player.Position = new Vector2f(player.Position.X - 1, player.Position.Y);

                if (JoyState.HasFlag(ControllerState.DPAD_RIGHT_PRESSED))
                    player.Position = new Vector2f(player.Position.X + 1, player.Position.Y);

                if (JoyState.HasFlag(ControllerState.B_PRESSED))
                    player.Position = RespawnCentreScreen();

                if (IsPlayerOverFood(player, food) == true)
                {
                    // Respawn food and increase health!
                    food.Position = RespawnNewLocation();
                }

                if (IsPlayerOverEnemy(player, enemy) == true)
                    player.Position = RespawnCentreScreen();


                // Update the window
                app.Display();
            }
        }

        static bool IsPlayerOverFood(RectangleShape player, RectangleShape food)
        {
            bool result = false;

            if (player.GetGlobalBounds().Intersects(food.GetGlobalBounds()) == true)
            {
                result = true;
                Console.WriteLine("YUM-YUM!");
            }

            return result;
        }

        static bool IsPlayerOverEnemy(RectangleShape player, RectangleShape enemy)
        {
            bool result = false;

            if (player.GetGlobalBounds().Intersects(enemy.GetGlobalBounds()) == true)
            {
                result = true;
                Console.WriteLine("You Go Die-Die!!");
            }

            return result;
        }

        static Vector2f RespawnNewLocation()
        {
            float x = RandomNum.Next(1, Convert.ToInt32(ScreenWidth));
            float y = RandomNum.Next(1, Convert.ToInt32(ScreenHeight));

            return new Vector2f(x, y);
        }

        static Vector2f RespawnCentreScreen()
        {
            return new Vector2f(ScreenWidth / 2, ScreenHeight / 2);
        }
    }
}
