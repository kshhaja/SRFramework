using SRFramework.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UnityEditor
{
    [CreateAssetMenu(fileName = "TextSample", menuName = "Gameplay/Stats/Settings/Default")]
    public class TextSample : ScriptableObject
    {
        private const string RESOURCE_PATH = "TextSample";
        Dictionary<SystemLanguage, Dictionary<int, string>> container = new Dictionary<SystemLanguage, Dictionary<int, string>>();

        private static TextSample instance;
        public static TextSample Instance
        {
            get
            {
                if (instance)
                    return instance;

                instance = Resources.Load<TextSample>(RESOURCE_PATH);
                if (instance)
                    return instance;

                instance = CreateInstance<TextSample>();
                
                instance.container.Add(SystemLanguage.English, new Dictionary<int, string>());
                instance.container[SystemLanguage.English].Add(5, "{0}% increased Projectile Speed");
                instance.container[SystemLanguage.English].Add(6, "Deals {0} Fire Damage");
                instance.container[SystemLanguage.English].Add(7, "{0}% chance to Ignite enemies");
                instance.container[SystemLanguage.English].Add(8, "Projectiles gain Radius as they travel farther, up to +{0} Radius");

                return instance;
            }
        }

        public string ModText(GameplayMod mod, int level, SystemLanguage language = SystemLanguage.English)
        {
            try
            {
                var text = container[language][mod.id];
                var arguments = mod.modifiers.Select(x => MinMaxToString(x.MinMax(level))).ToArray();
                return string.Format(text, arguments);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string MinMaxToString((float, float)? minmax)
        {
            if (minmax == null)
                return string.Empty;

            if (minmax.Value.Item1 == minmax.Value.Item2)
                return minmax.Value.Item1.ToString();

            return minmax.Value.Item1.ToString() + " to " + minmax.Value.Item2.ToString();
        }
    }
}
