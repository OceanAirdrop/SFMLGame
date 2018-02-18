using OceanAirdrop.SharedLib;
using OceanAirdrop.SharedLib.Model;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanAirdrop.CleanUp.GameTriggers
{
    class PlayerOverFood : IGameTrigger
    {
        public void Update()
        {
            var player = GameManager.GetPlayerObject().ToSprite();

            var foodList = GameManager.GameObjectList.Where(x => x.Type == EntityType.Food);

            var removeList = new List<GameEntity>();

            foreach (var bomb in foodList)
            {
                var sprite = bomb.ToSprite();

                if (IsPlayerOverFood(player.GetGlobalBounds(), sprite.GetGlobalBounds()) == true)
                {
                    removeList.Add(bomb);
                }
            }

            // Player has eaten this apple so lets move on! 
            foreach (var x in removeList)
                GameManager.GameObjectList.Remove(x);
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
    }
}
