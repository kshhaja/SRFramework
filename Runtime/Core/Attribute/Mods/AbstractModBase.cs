using UnityEngine;
using SRFramework.Attribute.StatsContainers;


namespace SRFramework.Attribute
{
    public abstract class AbstractModBase : ScriptableObject
    {
        public abstract void ApplyMod(AttributeSet target, float index);
    }
}