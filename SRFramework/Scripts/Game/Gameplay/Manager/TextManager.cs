using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Utility;
using System;

namespace SlimeRPG.Manager
{
    public class TextManager : MonoSingleton<TextManager>
    {
        // managing l10n etc..
        // text ScriptableObject made by excel or something
        private static TextContainer current;
        public static TextContainer Current
        {
            get
            {
                if (current != null)
                    return current;

                current = Resources.Load<TextContainer>("TextContainer");
                if (current != null)
                    return current;

                current = ScriptableObject.CreateInstance<TextContainer>();

                return current;
            }

            set => current = value;
        }

        public enum TextCategory
        {
            stat,
            mod,
            item,
            character,
            game,
            // ...
        }

        [CreateAssetMenu(fileName = "TextContainer", menuName = "Gameplay/Sample/TextContainer")]
        public class TextContainer : ScriptableObject
        {
            // 로드할 때 키 이름 string을 hash로 바꿔서 저장해놓으면 되겠다.
            Dictionary<SystemLanguage, Dictionary<TextCategory, Dictionary<int, string>>> container = new Dictionary<SystemLanguage, Dictionary<TextCategory, Dictionary<int, string>>>();


            TextContainer()
            {
                // sample texts

                container.Add(SystemLanguage.English, new Dictionary<TextCategory, Dictionary<int, string>>());
                container[SystemLanguage.English].Add(TextCategory.mod, new Dictionary<int, string>());

                container[SystemLanguage.English][TextCategory.mod].Add(5,
                    "{0}% increased Projectile Speed");
                container[SystemLanguage.English][TextCategory.mod].Add(6,
                    "Deals {0} Fire Damage");
                container[SystemLanguage.English][TextCategory.mod].Add(7,
                    "{0}% chance to Ignite enemies");
                container[SystemLanguage.English][TextCategory.mod].Add(8,
                    "Projectiles gain Radius as they travel farther, up to +{0} Radius");
            }

            public string LocalizedText(SystemLanguage language, TextCategory category, int id, out bool result)
            {
                try
                {
                    result = true;
                    return container[language][category][id];
                }
                catch(Exception e)
                {
                    result = false;
                    return e.Message;
                }
            }
        }


        TextContainer textContainer;
        
        public string LocalizedText(SystemLanguage language, TextCategory category, int id /*string hash*/)
        {
            return textContainer.LocalizedText(language, category, id, out var result);
        }

        public string LocalizedText(TextCategory category, int id /*string hash*/)
        {
            return textContainer.LocalizedText(Application.systemLanguage, category, id, out var result);
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
