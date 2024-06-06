using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class DialogManager : SingletonMono<DialogManager>
    {
        public List<Dialog> Dialogs = null;
        public Transform canvas;

        [ContextMenu("GetOrAdd_AllDialogs")]
        public void GetAllDialogsForChild()
        {
            Dialogs = new List<Dialog>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Dialog dialog = transform.GetChild(i).GetComponent<Dialog>();
                if (dialog != null && !Dialogs.Contains(dialog))
                {
                    dialog.gameObject.name = dialog.GetType().Name;
                    Dialogs.Add(dialog);
                }
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif   
        }

        public void Setup()
        {
            for (int i = 0; i < Dialogs.Count; i++)
            {
                Dialogs[i].Initialize();
            }
        }

        public static T Create<T>(string path, object[] data = null, bool isHideAllDialog = false, Action onShow = null)
            where T : Dialog
        {
            return Instance.CreateFormRes<T>(path, data, isHideAllDialog, onShow);
        }

        public static T Show<T>(bool isHideAllDialog = false, Action onShow = null)
            where T : Dialog
        {
            return Instance.ShowDialog<T>(isHideAllDialog, onShow);
        }

        private T ShowDialog<T>(bool isHideAllDialog = false, Action onShow = null)
            where T : Dialog
        {
            Dialog dialog = Get<T>();

            if (isHideAllDialog)
            {
                HideAllDialog();
            }

            if (dialog != null)
            {
                dialog.Show(onShow);
            }
            else
            {
                Debug.LogErrorFormat($"[PopupController] Popup does not exist");
            }

            return (T)dialog;
        }

        private T CreateFormRes<T>(string path, object[] data = null, bool isHideAllDialog = false,
            Action onShow = null) where T : Dialog
        {
            if (isHideAllDialog)
            {
                HideAllDialog();
            }

            T popup = (T)Instantiate(Resources.Load<T>(path), canvas);
            popup.name = path;
            popup.Show(onShow);
            Dialogs.Add(popup);
            return popup;
        }

        public static void Hide<T>(Action onHide = null) where T : Dialog
        {
            Instance.HideDialog<T>(onHide);
        }

        private T HideDialog<T>(Action onHide = null) where T : Dialog
        {
            foreach (var item in Dialogs)
            {
                if (item as T)
                {
                    item.Hide(onHide);
                    return item as T;
                }
            }

            Debug.LogErrorFormat($"[PopupController] Popup does not exist");
            return null;
        }

        public static void HideAllDialog()
        {
            for (int i = Instance.Dialogs.Count - 1; i >= 0; i--)
            {
                Instance.Dialogs[i].Hide();
            }
        }
        
        public static T Get<T>() where T : Dialog
        {
            return Instance.GetDialog<T>();
        }

        public T GetDialog<T>() where T : Dialog
        {
            foreach (var item in Dialogs)
            {
                if (item is T) return (T)item;
            }

            return null;
        }
    // }

    //
    // public class DialogBuilder
    // {
    //     private string path;
    //     private object[] data;
    //     private bool isHideAllDialog;
    //     private Action onStart;
    //     private Action onHide;
    //     private Action onDestroy;
    //
    //     public DialogBuilder SetPath(string _path)
    //     {
    //         this.path = _path;
    //         return this;
    //     }
    //
    //     public DialogBuilder SetData(object[] _data)
    //     {
    //         this.data = _data;
    //         return this;
    //     }
    //
    //     public DialogBuilder SetHideAllDialog(bool _isHideAllDialog)
    //     {
    //         this.isHideAllDialog = _isHideAllDialog;
    //         return this;
    //     }
    //
    //     public DialogBuilder SetOnShow(Action _onShow)
    //     {
    //         this.onStart = _onShow;
    //         return this;
    //     }
    //
    //     public DialogBuilder SetOnHide(Action _onHide)
    //     {
    //         this.onHide = _onHide;
    //         return this;
    //     }
    //
    //     public DialogBuilder SetOnDestroy(Action _onDestroy)
    //     {
    //         this.onDestroy = _onDestroy;
    //         return this;
    //     }
    //
    //
    //     public T BuildCreate<T>() where T : Dialog
    //     {
    //         return DialogManager.Create<T>(path, data, isHideAllDialog, onStart);
    //     }
    //
    //     public T BuildShow<T>() where T : Dialog
    //     {
    //         return DialogManager.Show<T>(data, isHideAllDialog, onStart);
    //     }
    //
    //     public void BuildHide<T>() where T : Dialog
    //     {
    //         DialogManager.Hide<T>(onHide);
    //     }
    //
    //     public void BuildDestroy<T>(float timeDelay = 0) where T : Dialog
    //     {
    //         DialogManager.Get<T>().Destroyed(timeDelay, onDestroy);
    //     }
    }
}