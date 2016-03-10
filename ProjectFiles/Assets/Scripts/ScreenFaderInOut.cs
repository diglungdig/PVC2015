using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenFaderInOut : MonoBehaviour
{
	private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    private bool fadingBlock = false;

	public float fadeSpeed = 2f;          // Speed that the screen fades to and from black.
	public GUITexture guiTextre;
	public int loadLevelIndex = 0;
	public bool reloadOrNOT = false;


	void Awake ()
	{
		// Set the texture so that it is the the size of the screen and covers it.
		guiTextre.pixelInset = new Rect(100f, 100f, Screen.width, Screen.height);
	}


	void Update ()
	{
		// If the scene is starting...
		if(sceneStarting)
			// ... call the StartScene function.
			StartScene();

        if (Input.GetKeyDown(KeyCode.R) && !fadingBlock)
        {
            reloadOrNOT = true;
        }

		if (reloadOrNOT)
			reloadCurrentScene ();

        if (loadLevelIndex != 0)
            EndScene();
	}

	void reloadCurrentScene(){
		loadLevelIndex = SceneManager.GetActiveScene ().buildIndex;
		EndScene ();
	}


	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		guiTextre.color = Color.Lerp(guiTextre.color, Color .clear, fadeSpeed * Time.deltaTime);
        fadingBlock = true;
	}


	void FadeToBlack ()
	{
		// Lerp the colour of the texture between itself and black.
		guiTextre.color = Color.Lerp(guiTextre.color, Color.black, fadeSpeed * Time.deltaTime);
        fadingBlock = true;
	}


	void StartScene ()
	{
		// Fade the texture to clear.
		FadeToClear();

		// If the texture is almost clear...
		if(guiTextre.color.a <= 0.01f)
		{
			// ... set the colour to clear and disable the GUITexture.
			guiTextre.color = Color.clear;
			guiTextre.enabled = false;

			// The scene is no longer starting.
			sceneStarting = false;
            fadingBlock = false;
		}
	}


	public void EndScene ()
	{
		// Make sure the texture is enabled.
		guiTextre.enabled = true;

		// Start fading towards black.
		FadeToBlack();

		// If the screen is almost black...
		if (guiTextre.color.a >= 0.99f) {
            // ... reload the level.
            reloadOrNOT = false;
            fadingBlock = false;
            SceneManager.LoadScene (loadLevelIndex);
		}
	}
}