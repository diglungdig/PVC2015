using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;
using System;


namespace VRStandardAssets.Menu
{
    public class CanvasButton : MonoBehaviour {


        public event Action<CanvasButton> OnButtonSelected;                   // This event is triggered when the selection of the button has finished.

        public bool BackButton = false;
        public bool SceneLoadingButton = false;

        [SerializeField]
        private VRCameraFade m_CameraFade;                 // This fades the scene out when a new scene is about to be loaded.
        [SerializeField]
        private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.

        [SerializeField]
        private string m_SceneToLoad;                      // The name of the scene to load.
        private bool m_GazeOver;                           // Whether the user is looking at the VRInteractiveItem currently.


        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
        }


        private void HandleOver()
        {
            // When the user looks at the rendering of the scene, show the radial.
            m_SelectionRadial.Show();

            m_GazeOver = true;
        }


        private void HandleOut()
        {
            // When the user looks away from the rendering of the scene, hide the radial.
            m_SelectionRadial.Hide();

            m_GazeOver = false;
        }


        private void HandleSelectionComplete()
        {
            // If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
            if (m_GazeOver)
                StartCoroutine(ActivateButton());
        }


        private IEnumerator ActivateButton()
        {


            if (BackButton)
            {
                if (gameObject.transform.parent.parent.name == "HazardGame")
                {
                    gameObject.transform.parent.parent.gameObject.GetComponent<MenuItemPopout>().overrideBool = false;
                }
                Camera.main.transform.parent.GetComponent<Animator>().SetBool("GotoCanvas", false);
                yield return new WaitForSeconds(2f);
                transform.parent.gameObject.SetActive(false);

            }
            else if (SceneLoadingButton)
            {



                // If the camera is already fading, ignore.
                if (m_CameraFade.IsFading)
                    yield break;

                // If anything is subscribed to the OnButtonSelected event, call it.
                if (OnButtonSelected != null)
                    OnButtonSelected(this);

                // Wait for the camera to fade out.
                yield return StartCoroutine(m_CameraFade.BeginFadeOut(true));

                // Load the level or canvas.


                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
        }
    }
}