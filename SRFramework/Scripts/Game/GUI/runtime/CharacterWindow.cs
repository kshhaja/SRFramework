using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Manager;


namespace SlimeRPG.UI.Runtime
{
    public class CharacterWindow : MonoBehaviour
    {
        public StatItem statItem;
        public GameObject content;

        private List<StatItem> items = new List<StatItem>();


        private void Start()
        {
            Init();
        }

        public void Init()
        {
            var p = GameManager.Instance.GetPlayerCharacter(0);
            var c = p.attributeController.StatsContainer;
            foreach (var def in c.collection.GetDefinitions())
            {
                var r = c.GetRecord(def);
                if (r != null)
                {
                    var sn = Instantiate(statItem.gameObject, content.transform).GetComponent<StatItem>();
                    sn.title.text = r.Definition.DisplayName;
                    sn.value.text = r.GetValue().ToString();
                    items.Add(sn);
                }
            }
        }
    }
}
