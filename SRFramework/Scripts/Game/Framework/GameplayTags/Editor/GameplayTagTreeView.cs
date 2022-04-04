using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using SlimeRPG.Framework.Tag;


namespace UnityEditor
{
    [Serializable]
    public class GameplayTagTreeView : TreeView
    {
        [SerializeField] private GameplayTagCollection target;
        [SerializeField] private SerializedObject serializedObject;


        public GameplayTagTreeView(GameplayTagCollection collection, TreeViewState state) : base(state)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is Null.");

            target = collection;

            showAlternatingRowBackgrounds = true;
            showBorder = true;
            
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            if (!target) 
                return null;
            
            serializedObject = new SerializedObject(target);
            SerializedProperty listProperty = serializedObject.FindProperty("elements");
            GameplayTagTreeViewItem root = new GameplayTagTreeViewItem() 
            { 
                id = 0, 
                depth = -1, 
                displayName = "Root",
                children = new List<TreeViewItem>() 
            };
            List<GameplayTagTreeViewItem> items = new List<GameplayTagTreeViewItem>();
            List<GameplayTag> tags = new List<GameplayTag>();
            for (int i = 0; i < listProperty.arraySize; ++i)
            {
                var obj = listProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                if (obj != null)
                    tags.Add(obj as GameplayTag);
            }

            tags.Sort(new GameplayTagComparer());

            // root
            if (tags.Count > 0 && tags[0] != null)
            {
                root.serializedObject = new SerializedObject(tags[0]);
                root.displayName = tags[0].name;
            }

            // 실제 보여지는 태그.
            for (int i = 1; i < tags.Count; ++i)
            {
                GameplayTag currentTag = tags[i];
                SerializedObject tagObject = new SerializedObject(currentTag);

                int parentIndex = items.FindIndex(x => x.serializedObject != null && x.serializedObject.targetObject == currentTag.Parent);
                
                GameplayTagTreeViewItem parentItem = parentIndex >= 0 ? items[parentIndex] : root;
                GameplayTagTreeViewItem newItem = new GameplayTagTreeViewItem()
                {
                    serializedObject = tagObject,
                    displayName = currentTag.name,
                    depth = currentTag.Depth,
                    id = currentTag.GetInstanceID(),
                    parent = parentItem,
                    children = new List<TreeViewItem>()
                };

                if (parentItem != null)
                    parentItem.children.Add(newItem);

                int childIndex = parentItem != null ? parentItem.children.IndexOf(newItem) : 0;
                items.Insert(parentIndex + 1 + childIndex, newItem);
            }

            return root;
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            float indent = GetContentIndent(args.item);
            Rect rect = new Rect(args.rowRect.x + indent, args.rowRect.y, args.rowRect.width - indent, args.rowRect.height);
            EditorGUI.LabelField(rect, new GUIContent((args.item as GameplayTagTreeViewItem).displayName));
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            // 추가할 때 선택된건 여러개지만 첫번째에만 추가가 되기때문에 보여지는것 자체를 잠금.
            return false;
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return item != rootItem;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            if (args.acceptedRename)
            {
                GameplayTagTreeViewItem item = FindItem(args.itemID, rootItem) as GameplayTagTreeViewItem;

                if (item != null && item.serializedObject != null)
                {
                    item.displayName = args.newName;
                    item.serializedObject.targetObject.name = args.newName;
                    AssetDatabase.SaveAssets();

                    // 정렬을 위해...
                    Reload();
                }
            }
        }
    }
}
