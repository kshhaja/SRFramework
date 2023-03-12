using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public enum StackingExpirationPolicy
    {
		ClearEntireStack,
		RemoveSingleStackAndRefreshDuration,
		RefreshDuration,
	}
}
