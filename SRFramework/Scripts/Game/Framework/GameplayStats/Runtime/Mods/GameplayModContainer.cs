using System;
using UnityEngine;
using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem.StatsContainers;


namespace SlimeRPG.Framework.StatsSystem
{
    [Serializable]
    public class GameplayModContainer : ScriptableObject
    {
        protected string randomId;

        [SerializeField]
        public string id;

        public List<GameplayMod> mods = new List<GameplayMod>();

        public virtual string Id
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    if (string.IsNullOrEmpty(randomId))
                        randomId = Guid.NewGuid().ToString();

                    return randomId;
                }

                return id;
            }
        }

        public virtual void ApplyMods(StatsContainer target, float index = 0)
        {
            foreach (var mod in mods)
            {
                if (mod == null || !mod.IsValid)
                    continue;

            }
        }

        public virtual void RemoveMods(StatsContainer target)
        {
            foreach (var mod in mods)
            {
                if (mod == null || mod.IsValid == false)
                    continue;

                // target.RemoveModifier()
            }
        }

        public List<StatAdjustment> ModsToStatList()
        {
            List<StatAdjustment> stats = new List<StatAdjustment>();
            foreach (var mod in mods)
            {
                stats.AddRange(mod.modifiers);
            }
            return stats;
        }

        //public virtual List<string> ModDescriptions(float index)
        //{
        //    List<string> descriptions = new List<string>();
        //    mods.ForEach(x => descriptions.Add(x.CreateDescription((int)index)));
        //    return descriptions;
        //}
    }
}