using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanAirdrop.SharedLib.Model
{

    public enum EntityType {  MainPlayer, Food, Bomb, Enemy }

    public class GameEntity
    {
        public EntityType Type { get; set; }

        public Drawable GameObj { get; set; }

        public GameEntity(EntityType type, Drawable d)
        {
            Type = type; GameObj = d;
        }

        public Sprite ToSprite()
        {
            return (Sprite)GameObj;
        }

        public Shape ToShape()
        {
            return (Shape)GameObj;
        }
    }
}
