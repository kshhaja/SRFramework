using System;
using SRFramework.Attribute;


namespace SRFramework.Effect
{
    [Serializable]
    public struct GameplayEffectPeriod
    {
        // magnitude가 null인 경우가 있다. gameplayEffect가 만들어질때 period에 해당하는 데이터는 항상 scalable로 만들어둬야할까?
        public GameplayEffectModifierMagnitude magnitude;
        // public ScalableMagnitude magnitude;
        public bool executeOnApplication;
        public InhibitionPolicy inhibitionPolicy;
    }
}
