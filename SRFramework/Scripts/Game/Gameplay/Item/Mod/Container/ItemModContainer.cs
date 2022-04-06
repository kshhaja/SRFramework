using UnityEngine;
using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.StatsSystem.StatsContainers;


namespace SlimeRPG.Framework.StatsSystem
{
    [CreateAssetMenu(fileName = "ItemStatGroup", menuName = "Gameplay/Stats/ItemStatGroup")]
    public class ItemModContainer : ScriptableObject
    {
        // modCollection이 필요하다. 기본셋팅을 collection기준으로 자동으로 만들어주고, mod.value만 조절할수 있도록 만든다.
        //[Header("Collection")]
        //public StatDefinitionCollection collection;

        [Header("Explicit Mods")]
        public GameplayModContainer explicits;

        [Header("Implicit Mods")]
        public GameplayModContainer implicits;

        //[Header("Base Stats")]
        //public StatsAdjustment BaseStats;

        //[Header("Additional Stats")]
        //public AddtionalStatsAdjustment AddtionalStats;

        public void ApplyModContainer(StatsContainer to, float by)
        {
            explicits.ApplyMods(to, by);

            if (implicits)
            {
                implicits.id = explicits.id;
                implicits.ApplyMods(to, by);
            }
        }

        public void RemoveModContainer(StatsContainer to)
        {
            explicits.RemoveMods(to);

            if (implicits)
                implicits.RemoveMods(to);
        }
    }
}
