using System;
using SRFramework.Tag;


namespace SRFramework.Effect
{
    [Serializable]
    public struct GameplayTagRequirements
    {
        public GameplayTagContainer requireTags;
        public GameplayTagContainer ignoreTags;
    }
}
