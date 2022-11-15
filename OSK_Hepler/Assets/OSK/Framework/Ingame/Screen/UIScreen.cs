using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public class UIScreen : UIMonoBehaviour
    {
        public string id = "";
        public string Id { get { return id; } }


        public virtual void Initialize()
        {
        }

        public virtual void Show(bool back, bool immediate)
        {
            Transition(showTransition, back, immediate, true);
        }

        public virtual void Hide(bool back, bool immediate)
        {
            Transition(hideTransition, back, immediate, false);
        }
    }
}
