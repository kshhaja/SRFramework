using System;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Tag
{
    [Serializable]
    public sealed class GameplayTag : ScriptableObject, IEquatable<GameplayTag>, IComparable<GameplayTag>
    {
        [SerializeField] 
        private GameplayTag parent;

        [SerializeField] 
        private List<GameplayTag> children = new List<GameplayTag>();

        public GameplayTag Parent => parent;
        public List<GameplayTag> Children => children;


        public bool IsChildOf(GameplayTag tag)
        {
            if (!tag) 
                return false;

            List<GameplayTag> depthList = new List<GameplayTag>();
            GameplayTag recursiveParent = this;

            while (recursiveParent != null)
            {
                if (depthList.Contains(recursiveParent)) 
                    return false;

                if (recursiveParent == tag) 
                    return true;

                depthList.Add(recursiveParent);
                recursiveParent = recursiveParent.parent;
            }

            return false;
        }

        public string GetFullPath(char separator = '.')
        {
            List<GameplayTag> depthList = new List<GameplayTag>();
            GameplayTag recursiveParent = parent;
            string fullPath = name;

            while (recursiveParent != null)
            {
                if (recursiveParent.parent == null || depthList.Contains(recursiveParent)) 
                    break;

                depthList.Add(recursiveParent);
                fullPath = recursiveParent.name + separator + fullPath;
                recursiveParent = recursiveParent.parent;
            }

            return fullPath;
        }

        public int Depth
        {
            get
            {
                List<GameplayTag> depthList = new List<GameplayTag>();
                GameplayTag recursiveParent = parent;

                while (recursiveParent != null)
                {
                    if (depthList.Contains(recursiveParent)) 
                        break;

                    depthList.Add(recursiveParent);
                    recursiveParent = recursiveParent.parent;
                }
                return depthList.Count - 1;
            }
        }


        public bool Equals(GameplayTag other) => other == this ? true : false;
        public int CompareTo(GameplayTag other) => other != null ? GetFullPath().CompareTo(other.GetFullPath()) : 1;
    }


    public delegate void TagEventHandler(object sender, TagEventArgs args);
    
    public sealed class TagEventArgs : EventArgs
    {
        private GameplayTag tag;
        public GameplayTag Tag => tag;

        public TagEventArgs(GameplayTag tag)
        {
            this.tag = tag;
        }
    }
}