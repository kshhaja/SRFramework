using SlimeRPG.Framework.StatsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SlimeRPG.Framework.StatsSystem.StatsContainers;

[CreateAssetMenu(fileName = "AddtionalAdjustment", menuName = "Gameplay/Stats/AddtionalAdjustment")]
public class AddtionalStatsAdjustment : ItemStatsAdjustment
{
    // mode 로 대체될 예정
    public override string Id => base.Id + "_additional";

    public void SetID(string id)
    {
        base.id = id;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AddtionalStatsAdjustment))]
public class AdditionalStatsAdjustmentEditor : StatAdjustmentCollectionEditor
{
    protected override bool ShowID => false;
}
#endif
