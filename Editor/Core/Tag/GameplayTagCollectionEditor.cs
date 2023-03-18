using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using SRFramework.Tag;


namespace UnityEditor
{
    [CustomEditor(typeof(GameplayTagCollection))]
    public class GameplayTagCollectionEditor : Editor
    {
        private GameplayTagTreeView treeView;
        private TreeViewState treeViewState;
        
        private SerializedProperty elements;
        private SerializedProperty root;
        
        private string searchString = string.Empty;
        

        void OnEnable()
        {
            GameplayTagCollection collection = target as GameplayTagCollection;

            if (treeViewState == null)
                treeViewState = new TreeViewState();

            treeView = new GameplayTagTreeView(collection, treeViewState);

            elements = serializedObject.FindProperty("elements");
            root = serializedObject.FindProperty("root");
        }


        public override void OnInspectorGUI()
        {
            if (!AssetDatabase.IsMainAsset(target))
                return;
            
            serializedObject.Update();

            if (elements.arraySize == 0)
            {
                if (Event.current.type != EventType.Layout)
                    AddTag("Root");
            }
            else
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Add")))
                    AddTag("Tag");

                if (GUILayout.Button(new GUIContent("Remove")))
                    RemoveTag();
                GUILayout.EndHorizontal();

                searchString = EditorGUILayout.TextField(new GUIContent("SearchString"), searchString);
                if (treeView != null)
                {
                    var treeViewRect = EditorGUILayout.GetControlRect(false, treeView.totalHeight + 1);
                    // 외부에서 OnGUI호출하면 에러가 뜰것임. 허나 당장 새로운 방법이 없다. 그리고 딱히 문제도 안됨.
                    treeView.OnGUI(treeViewRect);
                    treeView.searchString = searchString;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private List<GameplayTag> CollectionTags()
        {
            List<GameplayTag> tags = new List<GameplayTag>();
            for (int i = 0; i < elements.arraySize; ++i)
            {
                var obj = elements.GetArrayElementAtIndex(i).objectReferenceValue;
                if (obj != null)
                    tags.Add(obj as GameplayTag);
            }
            return tags;
        }

        public void AddTag(string name)
        {
            var selection = treeView.GetSelection();
            int id = selection.Count > 0 ? selection[0] : -1;

            GameplayTag newTag = CreateInstance<GameplayTag>();
            newTag.name = name;

            AssetDatabase.AddObjectToAsset(newTag, target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            elements.arraySize++;

            SerializedProperty element = elements.GetArrayElementAtIndex(elements.arraySize - 1);
            SerializedObject serializedTag = new SerializedObject(newTag);
            element.objectReferenceValue = newTag;

            if (elements.arraySize == 1)
                root.objectReferenceValue = newTag;
            else
            {
                var tags = CollectionTags();
                GameplayTag parent = tags.Find(x => x.GetInstanceID() == id);

                if (parent == null)
                    parent = tags.Find(x => x.Depth == -1);

                if (parent != null)
                {
                    SerializedObject serializedParent = new SerializedObject(parent);
                    SerializedProperty children = serializedParent.FindProperty("children");

                    children.arraySize++;
                    children.GetArrayElementAtIndex(children.arraySize - 1).objectReferenceValue = newTag;

                    children.serializedObject.ApplyModifiedProperties();
                    serializedParent.ApplyModifiedProperties();
                }
                serializedTag.FindProperty("parent").objectReferenceValue = parent;
            }
            
            serializedTag.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();

            treeView.Reload();
        }

        public void RemoveTag()
        {
            var tags = CollectionTags();
            if (tags.Count <= 1) // root만 있을때.
                return;

            tags.Sort(new GameplayTagComparer());
            tags.RemoveAll(x => x == null);

            int[] selection = treeView.GetSelection() as int[];

            for (int i = 0; i < selection.Length; ++i)
            {
                GameplayTag removal = tags.Find(o => o.GetInstanceID() == selection[i]);

                if (removal)
                {
                    // 자식 태그들도 제거.
                    List<GameplayTag> chlidrenOfRemoval = tags.FindAll(o => o && o.IsChildOf(removal));

                    for (int r = chlidrenOfRemoval.Count - 1; r >= 0; --r)
                    {
                        GameplayTag child = chlidrenOfRemoval[r];

                        if (removal && removal.Parent != null)
                        {
                            int childIndex = child.Parent.Children.IndexOf(child);

                            if (childIndex > -1)
                            {
                                SerializedObject parent = new SerializedObject(child.Parent);
                                SerializedProperty childList = parent.FindProperty("children");
                                if (childList.GetArrayElementAtIndex(childIndex) != null)
                                    childList.DeleteArrayElementAtIndex(childIndex);
                                childList.DeleteArrayElementAtIndex(childIndex);
                                parent.ApplyModifiedProperties();
                            }

                            SerializedObject serializedChild = new SerializedObject(child);
                            SerializedProperty parentFound = serializedChild.FindProperty("parent");
                            parentFound.objectReferenceValue = null;
                            serializedChild.ApplyModifiedProperties();
                        }

                        tags.Remove(removal);
                        DestroyImmediate(child, true);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }

            elements.serializedObject.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
            treeView.Reload();
        }
    }
}
