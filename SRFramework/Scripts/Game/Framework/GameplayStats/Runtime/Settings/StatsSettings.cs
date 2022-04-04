using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [CreateAssetMenu(fileName = "StatsSettings", menuName = "Gameplay/Stats/Settings/Default")]
    public class StatsSettings : ScriptableObject
    {
        private static StatsSettings current;
        private const string RESOURCE_PATH = "StatsSettings";

        private StatDefinitionCollection emptyStats;
        private OrderOfOperations emptyOrderOfOperations;
        private StatDefinitionsCompiled definitionsCompiled;

        [SerializeField]
        private StatDefinitionCollection defaultStats;

        [SerializeField]
        private OrderOfOperations orderOfOperations;

        public static StatsSettings Current
        {
            get
            {
                if (current != null)
                    return current;

                current = Resources.Load<StatsSettings>(RESOURCE_PATH);
                if (current != null)
                    return current;

                return CreateInstance<StatsSettings>();
            }

            set => current = value;
        }

        public StatDefinitionsCompiled DefinitionsCompiled
        {
            get
            {
                if (definitionsCompiled == null)
                    definitionsCompiled = new StatDefinitionsCompiled();

                return definitionsCompiled;
            }
        }

        public StatDefinitionCollection DefaultStats
        {
            get
            {
                if (defaultStats == null)
                {
                    if (emptyStats == null) 
                        emptyStats = CreateInstance<StatDefinitionCollection>();
                    
                    return emptyStats;
                }

                return defaultStats;
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
