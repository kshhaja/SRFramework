using System;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Tag
{
    [Serializable]
    [CreateAssetMenu(fileName = "Sample Tag Collection", menuName = "Gameplay/Tag Collection")]
    public sealed class GameplayTagCollection : ScriptableObject
    {
        [SerializeField]  private GameplayTag root;
        [SerializeField]  private List<GameplayTag> elements = new List<GameplayTag>();

        public List<GameplayTag> Elements => elements;

        public GameplayTag Root => root;
    }
}