using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OSK
{
    public class Popup : UIMonoBehaviour
    {
        private bool isShowing;
        private PopupClosed callback;

        public bool canAndroidBackClose = true;
        public bool CanAndroidBackClose { get { return canAndroidBackClose; } }
        public delegate void PopupClosed(bool cancelled, object[] outData);


        public virtual void Initialize()
        {
        }

        public void Show(bool back, bool immediate)
        {
            Show(null, back, immediate, null);
        }

        public void Show(object[] inData, bool back, bool immediate, PopupClosed callback)
        {
            this.callback = callback;

            if (isShowing)
            {
                return;
            }

            isShowing = true;

            // Show the popup object
            gameObject.SetActive(true);
            Transition(showTransition, back, immediate, true);
            OnShowing(inData);
        }

        public void Hide(bool back = true, bool immediate = false)
        {
            Transition(hideTransition, back, immediate, false, () =>
            {
                OnHiding();
                gameObject.SetActive(false);
            });
        }

        public void Destroyed(bool back = true, bool immediate = false)
        {
            Transition(hideTransition, back, immediate, false, () =>
            {
                OnHiding();
                Destroy(gameObject);
            });
        }

        public virtual void OnShowing(object[] inData)
        {
        }

        public virtual void OnHiding()
        {
            PopupManager.Instance.OnPopupHiding(this);
        }
    }
}