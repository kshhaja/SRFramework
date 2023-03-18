using UnityEngine;
using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem.StatsContainers;

namespace SlimeRPG.Framework.StatsSystem
{
	[CreateAssetMenu(fileName = "StatDefinition", menuName = "Gameplay/Stats/Definitions/Default")]
	public class StatDefinition : StatDefinitionBase
	{
		[SerializeField]
		protected string id;

		public string Id => id;
		public int HashCode => GetHashCode();

		[Header("Value")]
		[SerializeField] protected GameplayEffectModifierMagnitude value = new GameplayEffectModifierMagnitude();

		[Header("Display")]
		[SerializeField] string displayName = "Untitled";
		[SerializeField] bool hidden;
		[SerializeField] bool percentile;

		[SerializeField] int sortIndex = 500;
		
		[TextArea]
		[SerializeField] string description;
		
		public string DisplayName => displayName;

		public bool Hidden => hidden;

		public int SortIndex => sortIndex;

		public string Description => description;

		public GameplayEffectModifierMagnitude Value => value;

		public bool IsPercentile => value.RoundToInt == false && percentile;


		public virtual void Setup(StatsContainer container)
        {
        }

		public override List<StatDefinition> GetDefinitions(HashSet<StatDefinitionBase> visited)
		{
			if (visited == null || visited.Contains(this))
			{
				if (Application.isPlaying)
				{
					Debug.LogWarningFormat("Duplicate StatDefinition detected {0}", name);
				}

				return new List<StatDefinition>();
			}

			visited.Add(this);

			return new List<StatDefinition> { this };
		}
    }
}
