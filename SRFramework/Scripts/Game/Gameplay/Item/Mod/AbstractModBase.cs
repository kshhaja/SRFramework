using UnityEngine;
using SlimeRPG.Framework.StatsSystem.StatsContainers;


namespace SlimeRPG.Gameplay.Item.Mod
{
    public abstract class AbstractModBase : ScriptableObject
    {
        public abstract void ApplyMod(StatsContainer target, float index);
    }
}