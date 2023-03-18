using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SRFramework.Tag
{
    [Serializable]
    public class GameplayTagContainer
    {
        private class TagContainerEvent : UnityEvent<GameplayTagContainer, GameplayTag> { }
        
        [SerializeField]
        private List<GameplayTag> list = new List<GameplayTag>();


        private TagContainerEvent onTagAdded;
        private TagContainerEvent onTagRemoved;


        public List<GameplayTag> Tags => list;

        public int Num()
        {
            return list.Count;
        }

        public bool HasTag(GameplayTag tag)
        {
            return list.Contains(tag);
        }

        public bool HasTagChildOf(GameplayTag tag)
        {
            return list.Exists(x => x != null && x.IsChildOf(tag));
        }

        public bool HasTagParentOf(GameplayTag tag)
        {
            return list.Exists(x => tag.IsChildOf(x));
        }

        public int GetTagQuantity(GameplayTag tag)
        {
            return list.FindAll(x => x == tag).Count;
        }

        public bool HasAnyTags(GameplayTagContainer container)
        {
            foreach (GameplayTag tag in container.list)
            {
                if (HasTagChildOf(tag))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasAllTags(GameplayTagContainer container)
        {
            foreach (GameplayTag tag in container.list)
            {
                if (!HasTagChildOf(tag))
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasNoneTags(GameplayTagContainer container)
        {
            foreach (GameplayTag tag in container.list)
            {
                if (HasTagChildOf(tag))
                {
                    return false;
                }
            }
            return true;
        }

        public void ClearTags()
        {
            list.Clear();
        }

        public void AddTag(GameplayTag tag)
        {
            if (tag != null && !list.Contains(tag))
            {
                list.Add(tag);
                TriggerTagAdded(tag);
            }
        }

        public void RemoveTag(GameplayTag tag)
        {
            if (tag != null && list.Contains(tag))
            {
                list.Remove(tag);
                TriggerTagRemoved(tag);
            }
        }

        public void AddTags(GameplayTagContainer container)
        {
            for (int i = 0; i < container.list.Count; ++i)
            {
                AddTag(container.list[i]);
            }
        }

        public void RemoveTags(GameplayTagContainer contaienr)
        {
            for (int i = 0; i < contaienr.list.Count; ++i)
            {
                RemoveTag(contaienr.list[i]);
            }
        }

        private void TriggerTagAdded(GameplayTag tag)
        {
            if (onTagAdded != null && tag != null)
                onTagAdded.Invoke(this, tag);
        }

        private void TriggerTagRemoved(GameplayTag tag)
        {
            if (onTagRemoved != null && tag != null)
                onTagRemoved.Invoke(this, tag);
        }

        public void OnTagAddedSubscribe(UnityAction<GameplayTagContainer, GameplayTag> @event)
        {
            if (onTagAdded == null)
                onTagAdded = new TagContainerEvent();

            onTagAdded.AddListener(@event);
        }

        public void OnTagRemovedSubscribe(UnityAction<GameplayTagContainer, GameplayTag> @event)
        {
            if (onTagRemoved == null)
                onTagRemoved = new TagContainerEvent();

            onTagRemoved.AddListener(@event);
        }

        public void OnTagAddedUnsubscribe(UnityAction<GameplayTagContainer, GameplayTag> @event)
        {
            if (onTagAdded == null)
                return;

            onTagAdded.RemoveListener(@event);
        }

        public void OnTagRemovedUnsubscribe(UnityAction<GameplayTagContainer, GameplayTag> @event)
        {
            if (onTagRemoved == null)
                return;

            onTagRemoved.RemoveListener(@event);
        }
    }
}