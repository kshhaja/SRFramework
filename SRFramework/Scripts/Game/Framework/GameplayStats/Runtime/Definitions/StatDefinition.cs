using UnityEngine;
using System.Collections.Generic;


namespace SlimeRPG.Framework.StatsSystem
{
	[CreateAssetMenu(fileName = "StatDefinition", menuName = "Gameplay/Stats/Definitions/Default")]
	public class StatDefinition : StatDefinitionBase
	{
		protected const int MIN_SORT_INDEX = 0;
		protected const int MAX_SORT_INDEX = 1000;

		[SerializeField]
		protected string id;

		public string Id => id;

		[Header("Value")]
		[SerializeField] protected StatValueSelector value = new StatValueSelector();
		[SerializeField] protected bool roundModifiers = true;
		[SerializeField] protected bool roundResult;

		[Header("Display")]
		[SerializeField] string displayName = "Untitled";
		[SerializeField] bool hidden;
		[SerializeField] bool percentile;
		[Range(MIN_SORT_INDEX, MAX_SORT_INDEX)]
		[SerializeField] int sortIndex = 500;
		[TextArea]
		[SerializeField] string description;
		
		public string DisplayName => displayName;

		public bool Hidden => hidden;

		public int SortIndex => sortIndex;

		public string Description => description;

		public StatValueSelector Value => value;

		public bool RoundModifiers => roundModifiers;

		public bool RoundResult => roundResult;

		public bool IsPercentile => value.RoundToInt == false && percentile;


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

		public void SetID(string id)
        {
			this.id = id;
        }
	}
}
