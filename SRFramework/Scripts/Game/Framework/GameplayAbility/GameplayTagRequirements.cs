using System;
using SlimeRPG.Framework.Tag;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayTagRequirements
    {
        public GameplayTagContainer requireTags;
        public GameplayTagContainer ignoreTags;
    }
}
