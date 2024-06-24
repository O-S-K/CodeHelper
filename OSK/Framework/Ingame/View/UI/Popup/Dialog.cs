using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OSK
{
    public class Dialog : MonoBehaviour
    {
   
        public virtual void Show(System.Action onShow = null)
        {
            gameObject.SetActive(true);
            onShow?.Invoke();
        }

        public virtual void Hide(System.Action onHide = null)
        {
            onHide?.Invoke();
            gameObject.SetActive(false);
        }

        public virtual void Destroyed(float timeDelay = 0, System.Action onDestroy = null)
        {
            onDestroy?.Invoke();
            Destroy(gameObject, timeDelay);
        }
    }
}