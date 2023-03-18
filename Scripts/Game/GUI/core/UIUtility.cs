using UnityEngine;
using System;

namespace SlimeRPG.UI
{
    public static class UIUtility
    {

        public static void BringToFront(GameObject go)
        {
            BringToFront(go, true);
        }

        public static void BringToFront(GameObject go, bool allowReparent)
        {
            Transform root = null;

            // Check if this game object is part of a UI Scene
            UIScene scene = FindInParents<UIScene>(go);

            // If the object has a parent ui scene
            if (scene != null && scene.Content != null)
            {
                root = scene.Content;
            }
            else
            {
                // Use canvas as root
                Canvas canvas = UIUtility.FindInParents<Canvas>(go);
                if (canvas != null) root = canvas.transform;
            }

            // If the object has a parent canvas
            if (allowReparent && root != null)
                go.transform.SetParent(root, true);

            // Set as last sibling
            go.transform.SetAsLastSibling();

            // Handle the always on top components
            if (root != null)
            {
                UIAlwaysOnTop[] alwaysOnTopComponenets = root.gameObject.GetComponentsInChildren<UIAlwaysOnTop>();

                if (alwaysOnTopComponenets.Length > 0)
                {
                    // Sort them by order
                    Array.Sort(alwaysOnTopComponenets);

                    foreach (UIAlwaysOnTop component in alwaysOnTopComponenets)
                    {
                        component.transform.SetAsLastSibling();
                    }
                }
            }
        }

        public static T FindInParents<T>(GameObject go) where T : Component
        {
            if (go == null)
                return null;

            var comp = go.GetComponent<T>();

            if (comp != null)
                return comp;

            Transform t = go.transform.parent;

            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }

            return comp;
        }
    }
}
