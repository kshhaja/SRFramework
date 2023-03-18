using System;
using UnityEngine;
using UnityEditor.IMGUI.Controls;


namespace UnityEditor
{
    [Serializable]
    public class GameplayTagTreeViewItem : TreeViewItem
    {
        [SerializeField] public SerializedObject serializedObject;
    }
}
