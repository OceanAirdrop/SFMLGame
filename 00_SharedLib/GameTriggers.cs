using OceanAirdrop.SharedLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanAirdrop.SharedLib
{
    public static class GameTriggers
    {
        private static List<IGameTrigger> TriggerList = new List<IGameTrigger>();

        public static void AddTrigger(IGameTrigger eventTrigger)
        {
            TriggerList.Add(eventTrigger);
        }

        public static void RemoveTrigger(IGameTrigger eventTrigger)
        {
            TriggerList.Remove(eventTrigger);
        }

        public static void Run()
        {
            foreach (var gameTrigger in TriggerList)
                gameTrigger.Update();
        }
    }
}
