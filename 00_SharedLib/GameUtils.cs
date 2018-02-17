using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OceanAirdrop.SharedLib.XboxController;

namespace OceanAirdrop.SharedLib
{
    public static class GameUtils
    {
        static Random RandomNum = new Random();

        private enum Direction { Up, Down, Left, Right }

        public static Vector2f RandomlyMovePosition(Vector2f pos, Vector2u windowSize)
        {
            // Lets randomly move the enemy in any direction on the screen. 
            // up    = 1
            // down  = 2
            // left  = 3
            // right = 4
            Vector2f newpos = new Vector2f(pos.X, pos.Y);


            int directionChosen = RandomNum.Next(0, 4);

            if (directionChosen == (int)Direction.Up)
            {
                newpos = pos;
                newpos.Y += 1;
                pos = newpos;
            }

            if (directionChosen == (int)Direction.Down)
            {
                newpos = pos;
                if (pos.Y == 0)
                    newpos.Y += 1;
                else
                    newpos.Y -= 1;

                pos = newpos;
            }

            if (directionChosen == (int)Direction.Left)
            {
                newpos = pos;
                newpos.X -= 1;
                pos = newpos;
            }

            if (directionChosen == (int)Direction.Right)
            {
                newpos = pos;
                newpos.X += 1;
                pos = newpos;
            }

            newpos = EnsureObjectIsOnscreen(newpos, windowSize);

            return newpos;
        }

        public static Vector2f EnsureObjectIsOnscreen(Vector2f pos, Vector2u windowSize)
        {
            Vector2f newpos = new Vector2f(pos.X, pos.Y);

            // make sure enemy stays on the screen
            if (pos.Y < 0)
            {
                newpos = pos;
                newpos.Y = 10;
                pos = newpos;
            }

            if (pos.X < 0)
            {
                newpos = pos;
                newpos.X = 10;
                pos = newpos;
            }

            if (pos.X > windowSize.X)
            {
                newpos = pos;
                newpos.X = windowSize.X - 10;
                pos = newpos;
            }

            if (pos.Y > windowSize.Y)
            {
                newpos = pos;
                newpos.Y = windowSize.Y - 10;
                pos = newpos;
            }

            return newpos;
        }
    }
}
