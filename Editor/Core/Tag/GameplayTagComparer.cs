using System.Collections.Generic;
using SRFramework.Tag;


namespace UnityEditor
{
    public class GameplayTagComparer : IComparer<GameplayTag>
    {
        public int Compare(GameplayTag x, GameplayTag y)
        {
            if (!x || !y)
                return 0;

            int compare = x.Depth.CompareTo(y.Depth);
            // 사전순으로 보는게 가장 편하다.
            if (compare == 0)
                compare = x.name.CompareTo(y.name);

            return compare;
        }
    }
}
