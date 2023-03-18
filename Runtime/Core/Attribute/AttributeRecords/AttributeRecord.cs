using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Attribute
{
	public class AttributeRecord
	{
		/// <summary>
		/// 런타임에서 실제로 캐싱되고 사용되는 데이터.
		/// </summary>

		private bool isDirty = true;
		private Dictionary<float, float> valueCache = new Dictionary<float, float>();

		public AttributeModifierCollection modifierAdd = new AttributeModifierCollection(GameplayModifierOperator.Add);
		public AttributeModifierCollection modifierSubtract = new AttributeModifierCollection(GameplayModifierOperator.Subtract);
		public AttributeModifierCollection modifierMultiply = new AttributeModifierCollection(GameplayModifierOperator.Multiply);
		public AttributeModifierCollection modifierDivide = new AttributeModifierCollection(GameplayModifierOperator.Divide);
		public AttributeModifierCollection modifierOverride = new AttributeModifierCollection(GameplayModifierOperator.Override);

		public AttributeDefinition Definition { get; private set; }

		public float LastRetrievedValue { get; private set; }

		public GameplayEffectModifierMagnitude Value { get; private set; }

		public AttributeRecord(AttributeDefinition definition, GameplayEffectModifierMagnitude definitionOverride = null)
		{
			if (definition == null)
			{
				if (Application.isPlaying)
					Debug.LogError("Cannot initialize stat record with a blank definition");

				return;
			}

			Definition = definition;
			Value = definitionOverride ?? Definition.Value;

			foreach (var @operator in AttributesSettings.Current.OrderOfOperations.operators)
			{
				var m = GetModifier(@operator.type);
				m.onDirty += () => { isDirty = true; };
				m.forceRound = @operator.modifierAutoRound;
			}
		}

		// definition 혹은 override때 설정된 기본 value
		public float GetBaseValue(float index = 0)
		{
			return Value.GetValue(index);
		}

		private int GetBaseValueInt(float index)
		{
			return Mathf.RoundToInt(Value.GetValue(index));
		}

		private float GetBaseValueFloat(float index)
		{
			return Value.GetValue(index);
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
			foreach (var @operator in AttributesSettings.Current.OrderOfOperations.operators)
			{
				var o = GetModifier(@operator.type);
				val = o.ModifyValue(val);
			}

			valueCache[indexRound] = val;
			LastRetrievedValue = val;

			return val;
		}

		public AttributeModifierCollection GetModifier(GameplayModifierOperator @operator)
		{
			switch (@operator)
			{
				case GameplayModifierOperator.Add:
					return modifierAdd;
				case GameplayModifierOperator.Subtract:
					return modifierSubtract;
				case GameplayModifierOperator.Multiply:
					return modifierMultiply;
				case GameplayModifierOperator.Divide:
					return modifierDivide;
				case GameplayModifierOperator.Override:
					return modifierOverride;
				default:
					throw new ArgumentOutOfRangeException("operator", @operator, null);
			}
		}
	}
}

