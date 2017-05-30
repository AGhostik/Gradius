using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    
    private static bool needUnloadUnusedAssets = false;

    // Use this for initialization
    void Start () {
        if (Screen.fullScreen)
        {
            Cursor.visible = false;
        }

        if (needUnloadUnusedAssets)
        {
            Resources.UnloadUnusedAssets();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel(int levelIndex)
    {
        if (!needUnloadUnusedAssets)
        {
            needUnloadUnusedAssets = true;
        }

        EventController.cleanAll();

        SceneManager.LoadScene(levelIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
