using System;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem.StatsContainers;


namespace SlimeRPG.Framework.StatsSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay/Mod/Mod Definition")]
    public class GameplayMod : AbstractModBase
    {
        protected string internalID;
        public int id;
        public List<ModifierGroup> modifiers = new List<ModifierGroup>();

        public bool IsValid
        {
            get
            {
                foreach (var modifier in modifiers)
                {
                    if (!modifier.IsValid)
                        return false;
                }

                return true;
            }
        }

        //public string CreateDescription(int level)
        //{
        //    var textContainer = TextManager.Current;
            
        //    textGrab = textContainer.LocalizedText(Application.systemLanguage, TextManager.TextCategory.mod, id, out var result);
        //    var arguments = modifiers.Select(x => TextManager.MinMaxToString(x.MinMax(level))).ToArray();

        //    // string interpolation을 활용할 수 있는 방법이 가장 편한 개발과정이 될 것 같다.
        //    // $"{value0} chance for {name0} ... {value1} ... {name1}"
        //    // 이런 포맷이 필요할 수도 있겠다. 허나 변수로 저장된것이 없으니 에러가 생길듯.
        //    // https://mstrimpfl.blogspot.com/2020/03/c-dynamic-string-interpolation.html
        //    // 이 내용 참고해서 확장 기능 만들어보자.

        //    return string.Format(textGrab, arguments);
        //}

        public override void ApplyMod(StatsContainer target, float index)
        {
            // modifiers 분해해서 하나씩 uniqueID를 적용해서 넣어주어야 한다.
            foreach (var modifier in modifiers)
            {
                // stat.value.MinMax();
            }
        }
    }
}