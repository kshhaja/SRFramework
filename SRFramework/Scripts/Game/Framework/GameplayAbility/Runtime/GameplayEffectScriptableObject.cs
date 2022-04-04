using UnityEngine;

namespace SlimeRPG.Framework.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Ability/Effect Definition")]
    public class GameplayEffectScriptableObject : ScriptableObject
    {
        public GameplayEffectDefinitionContainer gameplayEffect;
        public GameplayEffectTags gameplayEffectTags;
        public GameplayEffectPeriod period;
    }
}
