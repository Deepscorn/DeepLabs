// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Sources.Util.Pattern;
using System.Collections;

namespace Assets.Sources.Scripts.Basic
{
    // Simple usage example:
    // as OnClick listener to button:
    // ScreenTransitionManager.OpenScreen( <drag-n-drop screen game object> )
    // It's InSceneSingleton because screens are allways in the same scene
    // Another scene => another ScreenManager. For correct navigation. See OpenPreviousScreen()
    // Note: after call OpenScreen(nextScreen) and nextScreen already opened, then ScreenTransitionManager doesn't close / open anything
    public class ScreenTransitionManager : InSceneSingleton<ScreenTransitionManager>
    {
        //Screen to open automatically at the start of the Scene
        public AbstractScreen InitiallyOpen = null;

        //Currently & previously open screen
        public AbstractScreen CurrentlyOpen { get; private set; }
        private AbstractScreen previouslyOpen;

        protected void OnEnable()
        {
            //If set, open the initial Screen now.
            if (InitiallyOpen != null)
            {
                OpenScreen(InitiallyOpen);
            }
        }

        //Closes the currently open panel and opens the provided one.
        //It also takes care of handling the navigation, setting the new Selected element.
        public void OpenScreen(AbstractScreen anim)
        {
            if (CurrentlyOpen == anim)
                return;

            previouslyOpen = CurrentlyOpen;

            //Set the new Screen as then open one.
            CurrentlyOpen = anim;

            anim.gameObject.SetActive(true);

            //Move the Screen to front.
            anim.transform.SetAsLastSibling();

            CloseScreen(previouslyOpen);

            //Start the open animation
            anim.SetOpen(true);
        }

        public bool HasPreviousScreen()
        {
            return previouslyOpen != null;
        }

        public void OpenPreviousScreen()
        {
            OpenScreen(previouslyOpen);
        }

        //Closes the currently open Screen
        //It also takes care of navigation.
        public void CloseCurrentScreen()
        {
            if (CurrentlyOpen == null)
                return;

            CloseScreen(CurrentlyOpen);

            previouslyOpen = CurrentlyOpen;

            //No screen open.
            CurrentlyOpen = null;
        }

        private void CloseScreen(AbstractScreen screen)
        {
            if (screen == null)
            {
                return;
            }

            //Start the close animation.
            screen.SetOpen(false);

            StartCoroutine(DeactivateOnClosed(screen));
        }

        private IEnumerator DeactivateOnClosed(AbstractScreen screen)
        {
            yield return StartCoroutine(screen.WaitForClosing());

            if (!screen.IsOpeningOrOpened())
            {
                screen.gameObject.SetActive(false);
            }
        }
    }
}
