using UnityEngine;
using SlimeRPG.Framework.StatsSystem.StatsContainers;


namespace SlimeRPG.Framework.StatsSystem
{
    public abstract class AbstractModBase : ScriptableObject
    {
        public abstract void ApplyMod(StatsContainer target, float index);
    }
}