using UnityEngine;


namespace SRFramework.Attribute
{
    [CreateAssetMenu(fileName = "AttributesSettings", menuName = "Gameplay/Attributes/Settings/Default")]
    public class AttributesSettings : ScriptableObject
    {
        private static AttributesSettings current;
        private const string RESOURCE_PATH = "AttributesSettings";

        private AttributeDefinitionCollection emptyStats;
        private OrderOfOperations emptyOrderOfOperations;
        private AttributeDefinitionsCompiled definitionsCompiled;

        [SerializeField]
        private AttributeDefinitionCollection defaultStats;

        [SerializeField]
        private AttributeDefinitionCollection staticStats;

        [SerializeField]
        private OrderOfOperations orderOfOperations;

        public static AttributesSettings Current
        {
            get
            {
                if (current != null)
                    return current;

                current = Resources.Load<AttributesSettings>(RESOURCE_PATH);
                if (current != null)
                    return current;

                return CreateInstance<AttributesSettings>();
            }

            set => current = value;
        }

        public AttributeDefinitionsCompiled DefinitionsCompiled
        {
            get
            {
                if (definitionsCompiled == null)
                    definitionsCompiled = new AttributeDefinitionsCompiled();

                return definitionsCompiled;
            }
        }

        public AttributeDefinitionCollection DefaultStats
        {
            get
            {
                if (defaultStats == null)
                {
                    if (emptyStats == null) 
                        emptyStats = CreateInstance<AttributeDefinitionCollection>();
                    
                    return emptyStats;
                }

                return defaultStats;
            }
        }

        public AttributeDefinitionCollection StaticStats
        {
            get
            {
                if (staticStats == null)
                    staticStats = CreateInstance<AttributeDefinitionCollection>();

                return staticStats;
            }
        }

        public OrderOfOperations OrderOfOperations
        {
            get
            {
                if (orderOfOperations == null)
                {
                    if (emptyOrderOfOperations == null) 
                        emptyOrderOfOperations = CreateInstance<OrderOfOperations>();
                    
                    return emptyOrderOfOperations;
                }

                return orderOfOperations;
            }

            set => orderOfOperations = value;
        }
    }
}
