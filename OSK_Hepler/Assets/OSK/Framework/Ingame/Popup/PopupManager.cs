using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class PopupManager : SingletonMono<PopupManager>
    {
        [System.Serializable]
        public class PopupInfo
        {
            [Tooltip("The popups id, used to show the popup. Should be unique between all other popups.")]
            public string popupId = "";

            [Tooltip("The Popup component to show.")]
            public Popup popup = null;
        }


        public List<PopupInfo> popupInfos = null;
        public Transform canvas;
        private List<Popup> activePopups;


        protected void Awake()
        {
            activePopups = new List<Popup>();
            for (int i = 0; i < popupInfos.Count; i++)
            {
                popupInfos[i].popup.Initialize();
            }
        }

        public void Show(string id, bool isHideAllPopup = true)
        {
            Show(id, null, null, isHideAllPopup);
        }

        public void Show(string id, object[] inData, bool isHideAllPopup = true)
        {
            Show(id, inData, null, isHideAllPopup);
        }

        public void Show(string id, object[] inData, bool isHideAllPopup = true, Popup.PopupClosed popupClosed = null)
        {
            Show(id, inData, popupClosed, isHideAllPopup);
        }

        public void Show(string id, object[] inData, Popup.PopupClosed popupClosed, bool isHideAllPopup = true)
        {
            Popup popup = GetPopupById(id);

            if (isHideAllPopup)
            {
                CloseActivePopup();
            }

            if (popup != null)
            {
                activePopups.Add(popup);
                popup.Show(inData, true, false, popupClosed);
            }
            else
            {
                Debug.LogErrorFormat($"[PopupController] Popup with id {id} does not exist");
            }
        }

        public T SpawnPopup<T>(string pathResourceNamePopup, bool isCachePopup, Popup.PopupClosed popupClosed) where T : Popup
        {
            T popup = (T)Instantiate(Resources.Load<T>(pathResourceNamePopup), canvas);
            popup.name = pathResourceNamePopup;
            popup.Show(null, true, false, popupClosed);
            if (isCachePopup)
            {
                activePopups.Add(popup);
            }
            return popup;
        }

        public bool CloseActivePopup()
        {
            if (activePopups.Count > 0)
            {
                int index = activePopups.Count - 1;
                Popup popup = activePopups[index];

                if (popup.CanAndroidBackClose)
                {
                    popup.Hide();
                }
                return true;
            }
            return false;
        }

        public void OnPopupHiding(Popup popup)
        {
            for (int i = activePopups.Count - 1; i >= 0; i--)
            {
                if (popup == activePopups[i])
                {
                    activePopups.RemoveAt(i);
                    break;
                }
            }
        }

        private Popup GetPopupById(string id)
        {
            for (int i = 0; i < popupInfos.Count; i++)
            {
                if (id == popupInfos[i].popupId)
                {
                    return popupInfos[i].popup;
                }
            }
            return null;
        }
    }
}