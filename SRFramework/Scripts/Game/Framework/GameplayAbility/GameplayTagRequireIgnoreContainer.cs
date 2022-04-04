using System;
using SlimeRPG.Framework.Tag;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayTagRequireIgnoreContainer
    {
        public GameplayTagContainer requireTags;
        public GameplayTagContainer ignoreTags;
    }
}
