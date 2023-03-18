using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeRPG.UI
{
    public class UISceneRegistry
    {
        private static UISceneRegistry instance;

        protected UISceneRegistry()
        {
            scenes = new List<UIScene>();
        }

        public static UISceneRegistry Instance
        {
            get
            {
                if (instance == null)
                    instance = new UISceneRegistry();
                return instance;
            }
        }

        private List<UIScene> scenes;
        private UIScene lastScene;

        public UIScene[] Scenes
        {
            get { return scenes.ToArray(); }
        }

        public UIScene LastScene
        {
            get { return lastScene; }
        }
        public void RegisterScene(UIScene scene)
        {
            // Make sure we have the list set
            if (scenes == null)
            {
                scenes = new List<UIScene>();
            }

            // Check if already registered
            if (scenes.Contains(scene))
            {
                Debug.LogWarning("Trying to register a UIScene multiple times.");
                return;
            }

            // Store in the list
            scenes.Add(scene);
        }

        public void UnregisterScene(UIScene scene)
        {
            if (scenes != null)
            {
                scenes.Remove(scene);
            }
        }

        public UIScene[] GetActiveScenes()
        {
            List<UIScene> activeScenes = scenes.FindAll(x => x.IsActivated == true);

            return activeScenes.ToArray();
        }

        public UIScene GetScene(int id)
        {
            if (scenes == null || scenes.Count == 0)
            {
                return null;
            }

            return scenes.Find(x => x.ID == id);
        }

        public int GetAvailableSceneId()
        {
            if (scenes.Count == 0)
            {
                return 0;
            }

            int id = 0;

            foreach (UIScene scene in scenes)
            {
                if (scene.ID > id)
                {
                    id = scene.ID;
                }
            }

            return id + 1;
        }

        public void TransitionToScene(UIScene scene)
        {
            // Transition out of the current scenes
            UIScene[] activeScenes = GetActiveScenes();

            foreach (UIScene activeScene in activeScenes)
            {
                // Transition the scene out
                activeScene.TransitionOut();

                // Set as last scene
                lastScene = activeScene;
            }

            // Transition in the new scene
            scene.TransitionIn();
        }
    }
}
