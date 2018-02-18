using OceanAirdrop.SharedLib;
using OceanAirdrop.SharedLib.Model;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OceanAirdrop.SharedLib.XboxController;

namespace OceanAirdrop.SoundFX
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


        static Music EatAppleSFX;

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

        static GameEntity CreateSprite(EntityType type, string fileName)
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            var sprite = new Sprite(new Texture(string.Format(@"{0}\Assets\Images\{1}", appPath, fileName)));
            sprite.Position = new Vector2f(100, 100);

            return new GameEntity(type, sprite);
        }

        static GameEntity CreateAppleSprite(EntityType type)
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            var sprite = new Sprite(new Texture(string.Format(@"{0}\Assets\Images\apple.png", appPath)));
            //sprite.Position = new Vector2f(300, 300);

            sprite.Position = RespawnRandomLocation();

            return new GameEntity(type, sprite);
        }

        static void SetupSoundFX()
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            var eatApple = string.Format(@"{0}\Assets\Sounds\drink_straw.wav", appPath);

            //PlaySound();
            //PlayMusic();



            // Load a music to play
            EatAppleSFX = new Music(eatApple);
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        private static void PlaySound()
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var eatApple = string.Format(@"{0}\Assets\Sounds\drink_straw.wav", appPath);

            eatApple = "C:/OceanAirdrop/SFMLGame/Assets/Sounds/drink_straw.wav";

            // Load a sound buffer from a wav file
            SoundBuffer buffer = new SoundBuffer(eatApple);

            // Display sound informations
            Console.WriteLine("canary.wav :");
            Console.WriteLine(" " + buffer.Duration + " sec");
            Console.WriteLine(" " + buffer.SampleRate + " samples / sec");
            Console.WriteLine(" " + buffer.ChannelCount + " channels");

            // Create a sound instance and play it
            Sound sound = new Sound(buffer);
            sound.Play();

            // Loop while the sound is playing
            while (sound.Status == SoundStatus.Playing)
            {
                // Display the playing position
                Console.CursorLeft = 0;
                Console.Write("Playing... " + sound.PlayingOffset + " sec     ");

                // Leave some CPU time for other processes
            }
        }

        /// <summary>
        /// Play a music
        /// </summary>
        private static void PlayMusic()
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var eatApple = string.Format(@"{0}\Assets\Sounds\drink_straw.wav", appPath);
            eatApple = "C:/OceanAirdrop/SFMLGame/Assets/Sounds/drink_straw.wav";

            // Load an ogg music file
            Music music = new Music(eatApple);

            // Display music informations
            Console.WriteLine("orchestral.ogg :");
            Console.WriteLine(" " + music.Duration + " sec");
            Console.WriteLine(" " + music.SampleRate + " samples / sec");
            Console.WriteLine(" " + music.ChannelCount + " channels");

            // Play it
            music.Play();

            // Loop while the music is playing
            while (music.Status == SoundStatus.Playing)
            {
                // Display the playing position
                Console.CursorLeft = 0;
                Console.Write("Playing... " + music.PlayingOffset + " sec     ");


            }
        }

        static void SetupGameObjects()
        {
            var gameObjList = GameManager.GameObjectList;

            gameObjList.Add(CreateSprite(EntityType.MainPlayer, "bird64.png"));

            for (int nLoopCnt = 0; nLoopCnt < 10; nLoopCnt++)
                gameObjList.Add(CreateAppleSprite(EntityType.Food));

            for (int nLoopCnt = 0; nLoopCnt < 10; nLoopCnt++)
                gameObjList.Add(CreateSprite(EntityType.Bomb, "beee42.png"));

            gameObjList.Add(CreateSprite(EntityType.Bomb, "beee64.png"));
            gameObjList.Add(CreateSprite(EntityType.Bomb, "beee64.png"));
            gameObjList.Add(CreateSprite(EntityType.Bomb, "beee16.png"));
            gameObjList.Add(CreateSprite(EntityType.Bomb, "beee16.png"));
        }

        static void Main(string[] args)
        {
            GameApp = new RenderWindow(new VideoMode(ScreenWidth, ScreenHeight), "Display Sprites");
            GameApp.Closed += new EventHandler(OnClose);

            Color windowColor = new Color(0, 192, 255);

            SetupSoundFX();
            SetupFont();
            SetupGameObjects();
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

                // Perform Game Logic
                JoystickEvents();
                MoveBombPositions();
                CheckIfPlayerOverFood();
                CheckIfPlayerOverBomb();

                // Update Screen
                foreach (var gameEntity in GameManager.GameObjectList)
                {
                    app.Draw(gameEntity.GameObj);
                }

                app.Draw(headerText);
                //app.Draw(buttonText);
                //app.Draw(axisText);
                app.Draw(foodCountText);
                app.Draw(killCountext);

                // Update the window
                app.Display();
            }
        }

        private static void CheckIfPlayerOverFood()
        {
            var player = GameManager.GetPlayerObject().ToSprite();

            var foodList = GameManager.GameObjectList.Where(x => x.Type == EntityType.Food);

            var removeList = new List<GameEntity>();

            foreach (var bomb in foodList)
            {
                var sprite = bomb.ToSprite();

                if (IsPlayerOverFood(player.GetGlobalBounds(), sprite.GetGlobalBounds()) == true)
                {
                    // Respawn food and increase health!
                    //sprite.Position = RespawnRandomLocation();
                    //foodCountText.DisplayedString = string.Format("Food: {0}", ++foodCount);

                    EatAppleSFX.Play();

                    removeList.Add(bomb);
                }
            }

            // Player has eaten this apple so lets move on! 
            foreach (var x in removeList)
                GameManager.GameObjectList.Remove(x);
        }

        private static void CheckIfPlayerOverBomb()
        {
            var player = GameManager.GetPlayerObject().ToSprite();

            var bombList = GameManager.GameObjectList.Where(x => x.Type == EntityType.Bomb);

            foreach (var gameObj in bombList)
            {
                var bomb = gameObj.ToSprite();

                if (IsPlayerOverEnemy(player.GetGlobalBounds(), bomb.GetGlobalBounds()) == true)
                {
                    // Respawn food and increase health!
                    player.Position = RespawnTopLeftScreen();
                    killCountext.DisplayedString = string.Format("Killed: {0}", ++killCount);
                }
            }
        }

        private static void JoystickEvents()
        {
            buttonText.DisplayedString = XboxController.RefreshButtonPressed();
            axisText.DisplayedString = XboxController.RefreshAxisPressed();

            var obj = GameManager.GetPlayerObject();

            var playerSprite = (Sprite)obj.GameObj;

            playerSprite.Position = RespondToJoystickEvents(playerSprite.Position);
        }

        private static void MoveBombPositions()
        {
            foreach (var bomb in GameManager.GameObjectList.Where(x => x.Type == EntityType.Bomb))
            {
                var sprite = (Sprite)bomb.GameObj;
                sprite.Position = GameUtils.RandomlyMovePosition(sprite.Position, GameApp.Size, 1);
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
