using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
	public class StatRecord
	{
		/// <summary>
		/// 런타임에서 실제로 캐싱되고 사용되는 데이터.
		/// </summary>

		private bool isDirty = true;
		private Dictionary<float, float> valueCache = new Dictionary<float, float>();

		public StatModifierCollection modifierAdd = new StatModifierCollection(OperatorType.Add);
		public StatModifierCollection modifierSubtract = new StatModifierCollection(OperatorType.Subtract);
		public StatModifierCollection modifierMultiply = new StatModifierCollection(OperatorType.Multiply);
		public StatModifierCollection modifierDivide = new StatModifierCollection(OperatorType.Divide);
		public StatModifierCollection modifierOverride = new StatModifierCollection(OperatorType.Override);

		public StatDefinition Definition { get; private set; }

		public float LastRetrievedValue { get; private set; }

		public StatValueSelector Value { get; private set; }

		public StatRecord(StatDefinition definition, StatValueSelector definitionOverride = null)
		{
			if (definition == null)
			{
				if (Application.isPlaying)
					Debug.LogError("Cannot initialize stat record with a blank definition");

				return;
			}

			Definition = definition;
			Value = definitionOverride ?? Definition.Value;

			foreach (var @operator in StatsSettings.Current.OrderOfOperations.operators)
			{
				var m = GetModifier(@operator.type);
				m.onDirty += () => { isDirty = true; };
				m.forceRound = @operator.modifierAutoRound && definition.RoundModifiers;
			}
		}

		// definition 혹은 override때 설정된 기본 value
		public float GetBaseValue(float index = 0)
		{
			return Value.Evaluate(index);
		}

		private int GetBaseValueInt(float index)
		{
			return Value.EvaluateInt(index);
		}

		private float GetBaseValueFloat(float index)
		{
			return Value.Evaluate(index);
		}

		// modifier가 적용된 값
		public float GetValue(float index = 0)
		{
			var indexRound = Mathf.Round(index * 100f) / 100f;

			if (isDirty)
			{
				valueCache.Clear();
				isDirty = false;
			}
			else
			{
				if (valueCache.TryGetValue(indexRound, out var cacheVal))
				{
					LastRetrievedValue = cacheVal;
					return cacheVal;
				}
			}

			var val = GetBaseValue(indexRound);
			foreach (var @operator in StatsSettings.Current.OrderOfOperations.operators)
			{
				var o = GetModifier(@operator.type);
				val = o.ModifyValue(val);
			}

			if (Definition.RoundResult)
				val = Mathf.Round(val);

			valueCache[indexRound] = val;
			LastRetrievedValue = val;

			return val;
		}

		public StatModifierCollection GetModifier(OperatorType @operator)
		{
			switch (@operator)
			{
				case OperatorType.Add:
					return modifierAdd;
				case OperatorType.Subtract:
					return modifierSubtract;
				case OperatorType.Multiply:
					return modifierMultiply;
				case OperatorType.Divide:
					return modifierDivide;
				case OperatorType.Override:
					return modifierOverride;
				default:
					throw new ArgumentOutOfRangeException("operator", @operator, null);
			}
		}
	}
}

