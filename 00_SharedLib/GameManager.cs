using OceanAirdrop.SharedLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanAirdrop.SharedLib
{
    public static class GameManager
    {
        public static List<GameEntity> GameObjectList = new List<GameEntity>();

        public static GameEntity GetPlayerObject()
        {
            return GameObjectList.Where(x => x.Type == EntityType.MainPlayer).FirstOrDefault();
        }

    }
}
