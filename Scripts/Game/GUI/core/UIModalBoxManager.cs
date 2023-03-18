using UnityEngine;
using System.Collections.Generic;


namespace SlimeRPG.UI
{
    [CreateAssetMenu(menuName = "Assets/Create/UI Managers/Modal Box Manager")]
    public class UIModalBoxManager : ScriptableObject
    {
        private static UIModalBoxManager instance;
        public static UIModalBoxManager Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load("ModalBoxManager") as UIModalBoxManager;

                return instance;
            }
        }

        [SerializeField] 
        private GameObject modalBoxPrefab;
        private List<UIModalBox> activeBoxes = new List<UIModalBox>();

        public GameObject ModalBoxPrefab
        {
            get { return modalBoxPrefab; }
        }

        public UIModalBox[] ActiveBoxes
        {
            get
            {
                activeBoxes.RemoveAll(item => item == null);
                return activeBoxes.ToArray();
            }
        }

        public UIModalBox Create(GameObject relative)
        {
            if (modalBoxPrefab == null || relative == null)
                return null;

            Canvas canvas = UIUtility.FindInParents<Canvas>(relative);

            if (canvas != null)
            {
                GameObject obj = Instantiate(modalBoxPrefab, canvas.transform, false);
                return obj.GetComponent<UIModalBox>();
            }

            return null;
        }

        public void RegisterActiveBox(UIModalBox box)
        {
            if (!activeBoxes.Contains(box))
                activeBoxes.Add(box);
        }

        public void UnregisterActiveBox(UIModalBox box)
        {
            if (activeBoxes.Contains(box))
                activeBoxes.Remove(box);
        }
    }
}
