using System;
using SRFramework.Attribute;


namespace SRFramework.Effect
{
    [Serializable]
    public struct GameplayEffectPeriod
    {
        // magnitude�� null�� ��찡 �ִ�. gameplayEffect�� ��������� period�� �ش��ϴ� �����ʹ� �׻� scalable�� �����־��ұ�?
        public GameplayEffectModifierMagnitude magnitude;
        // public ScalableMagnitude magnitude;
        public bool executeOnApplication;
        public InhibitionPolicy inhibitionPolicy;
    }
}
