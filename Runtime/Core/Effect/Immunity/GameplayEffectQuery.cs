using UnityEngine;


namespace SRFramework.Effect
{
	[System.Serializable]
    public struct GameplayEffectQuery
    {
		// FActiveGameplayEffectQueryCustomMatch CustomMatchDelegate;

		// FActiveGameplayEffectQueryCustomMatch_Dynamic CustomMatchDelegate_BP;

		// FGameplayTagQuery OwningTagQuery;

		// FGameplayTagQuery EffectTagQuery;

		// FGameplayTagQuery SourceTagQuery;

		// FGameplayAttribute ModifyingAttribute;

		// const UObject* EffectSource;

		// TSubclassOf<UGameplayEffect> EffectDefinition;

		// TArray<FActiveGameplayEffectHandle> IgnoreHandles;

		[SerializeField] public Object effectSource;
		[SerializeField] public GameplayEffect effectDefinition;

		bool Matches(/*const FActiveGameplayEffect& Effect*/)
        {
			return false;
        }

		//bool Matches(/*const FGameplayEffectSpec& Effect*/)
		//{
		//	return false;
		//}

		bool IsEmpty()
        {
			return false;
        }

		// static FGameplayEffectQuery MakeQuery_MatchAnyOwningTags(const FGameplayTagContainer& InTags){}
		// static FGameplayEffectQuery MakeQuery_MatchAllOwningTags(const FGameplayTagContainer& InTags){}
		// static FGameplayEffectQuery MakeQuery_MatchNoOwningTags(const FGameplayTagContainer& InTags);
		// static FGameplayEffectQuery MakeQuery_MatchAnyEffectTags(const FGameplayTagContainer& InTags);
		// static FGameplayEffectQuery MakeQuery_MatchAllEffectTags(const FGameplayTagContainer& InTags);
		// static FGameplayEffectQuery MakeQuery_MatchNoEffectTags(const FGameplayTagContainer& InTags);
		// static FGameplayEffectQuery MakeQuery_MatchAnySourceTags(const FGameplayTagContainer& InTags);
		// static FGameplayEffectQuery MakeQuery_MatchAllSourceTags(const FGameplayTagContainer& InTags);
		// static FGameplayEffectQuery MakeQuery_MatchNoSourceTags(const FGameplayTagContainer& InTags);
    }
}
