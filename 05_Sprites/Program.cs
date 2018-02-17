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

namespace OceanAirdrop.Sprites
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
        static Sprite PlayerSprite;
        static Sprite BombSprite;
        static Sprite AppleSprite;

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
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            GameFont = new Font(string.Format(@"{0}\Assets\Fonts\zorque.ttf", appPath));
        }

        static void SetupPlayerSprite()
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            PlayerSprite = new Sprite(new Texture(string.Format(@"{0}\Assets\Images\player_128.png", appPath)));
            PlayerSprite.Position = RespawnRandomLocation();
        }

        static void SetupBombSprite()
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            BombSprite = new Sprite(new Texture(string.Format(@"{0}\Assets\Images\bomb.png", appPath)));
            BombSprite.Position = new Vector2f(100, 100);
        }



        static void SetupAppleSprite()
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            AppleSprite = new Sprite(new Texture(string.Format(@"{0}\Assets\Images\apple.png", appPath)));
            AppleSprite.Position = new Vector2f(300, 300);
        }

        static void Main(string[] args)
        {
            GameApp = new RenderWindow(new VideoMode(ScreenWidth, ScreenHeight), "Display Sprites");
            GameApp.Closed += new EventHandler(OnClose);

            Color windowColor = new Color(0, 192, 255);

            SetupFont();
            SetupPlayerSprite();
            SetupBombSprite();
            SetupAppleSprite();
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

                BombSprite.Position = GameUtils.RandomlyMovePosition(BombSprite.Position, app.Size);


                buttonText.DisplayedString = XboxController.RefreshButtonPressed();
                axisText.DisplayedString = XboxController.RefreshAxisPressed();

                PlayerSprite.Position = RespondToJoystickEvents(PlayerSprite.Position);

                if ( IsPlayerOverFood(PlayerSprite.GetGlobalBounds(), AppleSprite.GetGlobalBounds()) == true )
                {
                    // Respawn food and increase health!
                    AppleSprite.Position = RespawnRandomLocation();
                    foodCountText.DisplayedString = string.Format("Food: {0}", ++foodCount);
                }

                if (IsPlayerOverEnemy(PlayerSprite.GetGlobalBounds(), BombSprite.GetGlobalBounds()) == true)
                {
                    PlayerSprite.Position = RespawnTopLeftScreen();
                    killCountext.DisplayedString = string.Format("Killed: {0}", ++killCount);
                }

                app.Draw(headerText);
                app.Draw(buttonText);
                app.Draw(axisText);
                app.Draw(foodCountText);
                app.Draw(killCountext);
                app.Draw(PlayerSprite);
                app.Draw(BombSprite);
                app.Draw(AppleSprite);

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

        static Vector2f RespondToJoystickEvents(Vector2f pos)
        {
            Vector2f newpos = new Vector2f(pos.X, pos.Y);

            if (JoyState.HasFlag(ControllerState.DPAD_UP_PRESSED))
                newpos = new Vector2f(pos.X, pos.Y - 1);

            if (JoyState.HasFlag(ControllerState.DPAD_DOWN_PRESSED))
                newpos = new Vector2f(pos.X, pos.Y + 1);

            if (JoyState.HasFlag(ControllerState.DPAD_LEFT_PRESSED))
                newpos = new Vector2f(pos.X - 1, pos.Y);

            if (JoyState.HasFlag(ControllerState.DPAD_RIGHT_PRESSED))
                newpos = new Vector2f(pos.X + 1, pos.Y);

            if (JoyState.HasFlag(ControllerState.B_PRESSED))
                newpos = RespawnCentreScreen();

            return newpos;
        }

        static bool IsPlayerOverFood(FloatRect player, FloatRect food)
        {
            bool result = false;

            if (player.Intersects(food) == true)
            {
                result = true;
                Console.WriteLine("YUM-YUM!");
            }

            return result;
        }

        static bool IsPlayerOverEnemy(FloatRect player, FloatRect enemy)
        {
            bool result = false;

            if (player.Intersects(enemy) == true)
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
