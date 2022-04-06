using System;

namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectPeriod
    {
        public float interval;
        public bool executeOnApplication;
    }

}
