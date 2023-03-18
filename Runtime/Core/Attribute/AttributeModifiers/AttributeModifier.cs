namespace SRFramework.Attribute
{
    // 아마 사라질것..
    public class AttributeModifier
    {
        public readonly string id;
        public float value;

        public AttributeModifier(string id, float value)
        {
            this.id = id;
            this.value = value;
        }
    }
}
