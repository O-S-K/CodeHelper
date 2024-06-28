using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class DialogManager : SingletonMono<DialogManager>
    {
        public List<Dialog> Dialogs = null;
        public Transform canvas;
        public Transform parrentAllDialog;
        public Dialog currentDialog;

        [ContextMenu("GetOrAdd_AllDialogs")]
        public void GetAllDialogsForChild()
        {
            Dialogs = new List<Dialog>();
            for (int i = 0; i < parrentAllDialog.childCount; i++)
            {
                Dialog dialog = parrentAllDialog.GetChild(i).GetComponent<Dialog>();
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
        }

        public static void ShowNotification(string message, float delayHide = 2f, float pointYSpawn = -100, float moveY = 150)
        {
            if (Instance.currentDialog != null && Instance.currentDialog.GetType() == typeof(NotificationDialog))
            {
                Destroy(Instance.currentDialog.gameObject);
            }

            var notificationDialog = Instance.CreateFormRes<NotificationDialog>("View/UI/NotificationDialog");
            notificationDialog.ShowNotification(message, delayHide, pointYSpawn, moveY);
        }

        public static T Create<T>(string path, bool isAddDialogs = false, bool isHideAllDialog = false,
            Action onShow = null)
            where T : Dialog
        {
            return Instance.CreateFormRes<T>(path, isAddDialogs, isHideAllDialog, onShow);
        }

        public static T Show<T>(bool isHideAllDialog = false, Action onShow = null)
            where T : Dialog
        {
            return Instance.ShowDialog<T>(isHideAllDialog, onShow);
        }

        public T ShowDialog<T>(bool isHideAllDialog = false, Action onShow = null) where T : Dialog
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

        public T CreateFormRes<T>(string path, bool isAddDialogs = false, bool isHideAllDialog = false,
            Action onShow = null) where T : Dialog
        {
            if (isHideAllDialog)
            {
                HideAllDialog();
            }

            T popup = (T)Instantiate(Resources.Load<T>(path), parrentAllDialog);
            currentDialog = popup;
            popup.transform.localPosition = Vector3.zero;
            popup.name = path;
            popup.Show(onShow);
            if (isAddDialogs)
                Dialogs.Add(popup);
            return popup;
        }

        public static void Hide<T>(Action onHide = null) where T : Dialog
        {
            Instance.HideDialog<T>(onHide);
        }

        public T HideDialog<T>(Action onHide = null) where T : Dialog
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
                if (Instance.Dialogs[i].gameObject.activeInHierarchy)
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