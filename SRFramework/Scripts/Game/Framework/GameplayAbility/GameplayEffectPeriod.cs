using System;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectPeriod
    {
        public ScalableMagnitude magnitude;
        public bool executeOnApplication;
        public InhibitionPolicy inhibitionPolicy;
    }

}
