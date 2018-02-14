using OceanAirdrop.SharedLib;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OceanAirdrop.SharedLib.XboxController;

namespace OceanAirdrop.Fonts
{
    class Program
    {
        static Random RandomNum = new Random();

        static UInt32 ScreenWidth = 800;
        static UInt32 ScreenHeight = 600;

        static Font GameFont;

        static RenderWindow GameApp = null;
                
        static Text headerText;
        static Text buttonText;
        static Text axisText;
        static Text foodCountText;
        static Text killCountext;

        static int foodCount = 0;
        static int killCount = 0;



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

        static void SetupFont()
        {
            // Load font
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            GameFont = new Font(string.Format(@"{0}\Assets\Fonts\zorque.ttf", appPath));
        }

        static void Main(string[] args)
        {
            GameApp = new RenderWindow(new VideoMode(ScreenWidth, ScreenHeight), "Display Fonts");
            GameApp.Closed += new EventHandler(OnClose);

            Color windowColor = new Color(0, 192, 255);

            // Load Game Objects
            RectangleShape player = CreateSquare(Color.Black, RespawnCentreScreen(), new Vector2f(30, 30));
            RectangleShape food   = CreateSquare(Color.Magenta, new Vector2f(100, 200), new Vector2f(20, 20));
            RectangleShape enemy  = CreateSquare(Color.Red, new Vector2f(500, 500), new Vector2f(20, 20));

            SetupFont();

            SetupGameText();

            // quick alias
            var app = GameApp;

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

                RespondToJoystickEvents(player);

                if (IsPlayerOverFood(player, food) == true)
                {
                    // Respawn food and increase health!
                    food.Position = RespawnRandomLocation();
                    foodCountText.DisplayedString = string.Format("Food: {0}", ++foodCount);
                }

                if (IsPlayerOverEnemy(player, enemy) == true)
                {
                    player.Position = RespawnTopLeftScreen();
                    killCountext.DisplayedString = string.Format("Killed: {0}", ++killCount);
                 
                }
                    


                app.Draw(headerText);
                app.Draw(buttonText);
                app.Draw(axisText);
                app.Draw(foodCountText);
                app.Draw(killCountext);

                // Update the window
                app.Display();
            }
        }

        private static void SetupGameText()
        {
            headerText = new Text("SquareBoy Game", GameFont);
            headerText.Position = new Vector2f(GameApp.Size.X / 2, 10);

            buttonText = new Text("Button Pressed", GameFont);
            buttonText.Position = new Vector2f(30, 90);

            axisText = new Text("Axis Pressed", GameFont);
            axisText.Position = new Vector2f(30, 130);

            foodCountText = new Text("Food: 0", GameFont);
            foodCountText.Position = new Vector2f(GameApp.Size.X - 150, GameApp.Size.Y - 40);

            killCountext = new Text("Killed: 0", GameFont);
            killCountext.Position = new Vector2f(10, GameApp.Size.Y - 40);
         
        }

        static void RespondToJoystickEvents(RectangleShape player)
        {
            buttonText.DisplayedString = XboxController.RefreshButtonPressed();
            axisText.DisplayedString = XboxController.RefreshAxisPressed();

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

        static Vector2f RespawnRandomLocation()
        {
            float x = RandomNum.Next(1, Convert.ToInt32(ScreenWidth));
            float y = RandomNum.Next(1, Convert.ToInt32(ScreenHeight));

            return new Vector2f(x, y);
        }

        static Vector2f RespawnCentreScreen()
        {
            return new Vector2f(ScreenWidth / 2, ScreenHeight / 2);
        }

        static Vector2f RespawnTopLeftScreen()
        {
            return new Vector2f(0, 0);
        }
    }
}
