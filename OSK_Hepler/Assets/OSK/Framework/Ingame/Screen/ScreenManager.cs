using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public class ScreenManager : SingletonMono<ScreenManager>
    {
        [Tooltip("The home screen to of the app, ei. the first screen that shows when the app starts.")]
        public string mainScreenId = "Main";

        [Tooltip("The list of Screen components that are used in the game.")]
        public List<UIScreen> screens = null;

        // Screen id back stack
        private List<string> backStack;

        // The screen that is currently being shown
        private UIScreen currentScreen;
        private bool isAnimating;

        public string HomeScreenId { get { return mainScreenId; } }
        public string CurrentScreenId { get { return currentScreen == null ? "" : currentScreen.Id; } }

        /// <summary>
        /// Invoked when the ScreenController is transitioning from one screen to another. The first argument is the current showing screen id, the
        /// second argument is the screen id of the screen that is about to show (null if its the first screen). The third argument id true if the screen
        /// that is being show is an overlay
        /// </summary>
        public System.Action<string, string> OnSwitchingScreens;

        /// <summary>
        /// Invoked when ShowScreen is called
        /// </summary>
        public System.Action<string> OnShowingScreen;

        protected void Awake()
        {
            backStack = new List<string>();

            // Initialize and hide all the screens
            for (int i = 0; i < screens.Count; i++)
            {
                UIScreen screen = screens[i];

                // Add a CanvasGroup to the screen if it does not already have one
                if (screen.gameObject.GetComponent<CanvasGroup>() == null)
                {
                    screen.gameObject.AddComponent<CanvasGroup>();
                }

                // Force all screens to hide right away
                screen.Initialize();
                screen.gameObject.SetActive(true);
                screen.Hide(false, true);
            }

            // Show the home screen when the app starts up
            //Show(homeScreenId, false, true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // First try and close an active popup (If there are any)
                if (!PopupManager.Instance.CloseActivePopup())
                {
                    // No active popups, if we are on the home screen close the app, else go back one screen
                    if (CurrentScreenId == HomeScreenId)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        Back();
                    }
                }
            }
        }

        public void Show(string screenId)
        {
            if (CurrentScreenId == screenId)
            {
                return;
            }

            Show(screenId, false, false);
        }

        public void Back()
        {
            if (backStack.Count <= 0)
            {
                Debug.LogWarning($"[ScreenController] There is no screen on the back stack to go back to.");

                return;
            }

            // Get the screen id for the screen at the end of the stack (The last shown screen)
            string screenId = backStack[backStack.Count - 1];

            // Remove the screen from the back stack
            backStack.RemoveAt(backStack.Count - 1);

            // Show the screen
            Show(screenId, true, false);
        }

        public void Home()
        {
            if (CurrentScreenId == mainScreenId)
            {
                return;
            }

            Show(mainScreenId, true, false);
            ClearBackStack();
        }


        private void Show(string screenId, bool back, bool immediate)
        {
            Debug.Log("[ScreenController] Showing screen " + screenId);

            // Get the screen we want to show
            UIScreen screen = GetScreenById(screenId);

            if (screen == null)
            {
                Debug.LogError($"[ScreenController] Could not find screen {screenId} with the given screenId");
                return;
            }

            // Check if there is a current screen showing
            if (currentScreen != null)
            {
                // Hide the current screen
                currentScreen.Hide(back, immediate);

                if (!back)
                {
                    // Add the screens id to the back stack
                    backStack.Add(currentScreen.Id);
                }

                if (OnSwitchingScreens != null)
                {
                    OnSwitchingScreens(currentScreen.Id, screenId);
                }
            }

            // Show the new screen
            screen.Show(back, immediate);

            // Set the new screen as the current screen
            currentScreen = screen;

            if (OnShowingScreen != null)
            {
                OnShowingScreen(screenId);
            }
        }

        private void ClearBackStack()
        {
            backStack.Clear();
        }

        private UIScreen GetScreenById(string id)
        {
            for (int i = 0; i < screens.Count; i++)
            {
                if (id == screens[i].Id)
                {
                    return screens[i];
                }
            }

            Debug.LogError($"[ScreenTransitionController] No Screen exists with the id {id}");
            return null;
        }
    }
}
