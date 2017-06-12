using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
        
    public GameObject[] menuLevels;
    public EventSystem thisEvSystem;

    public GameObject previousSelected;

    public InputField seedField;

    private AudioSource audioSource;
    private List<int> history = new List<int>();    

    // Use this for initialization
    void Start () {
        previousSelected = thisEvSystem.currentSelectedGameObject;
        audioSource = GetComponent<AudioSource>();
        if (Screen.fullScreen)
        {
            Cursor.visible = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (thisEvSystem.currentSelectedGameObject != previousSelected)
        {
            if (thisEvSystem.currentSelectedGameObject == null)
            {
                thisEvSystem.SetSelectedGameObject(previousSelected);
            }
            else
            {
                if (thisEvSystem.currentSelectedGameObject != seedField.gameObject)
                {
                    previousSelected = thisEvSystem.currentSelectedGameObject;
                }
            }
        }
	}

    public void gotoMenuLevel(int index)
    {
        int length = menuLevels.Length;
        for (int i = 0; i < length; i++)
        {
            if (i != index)
            {
                menuLevels[i].SetActive(false);
            }
            else
            {
                menuLevels[i].SetActive(true);
            }
        }
        thisEvSystem.SetSelectedGameObject(menuLevels[index].transform.GetChild(0).gameObject);

        history.Add(index);
    }

    public void goBack()
    {
        int temp = history.Count - 1;
        if (temp > 0)
        {
            gotoMenuLevel(history[temp - 1]);
            history.RemoveAt(temp);
        }
        else
        {
            gotoMenuLevel(0);
            history.Clear();            
        }
    }

    public void selectObject(GameObject obj)
    {
        thisEvSystem.SetSelectedGameObject(obj, null);
    }

    public void clearText(Text txt)
    {
        txt.text = string.Empty;
    }

    public void LoadLevel(int levelIndex)
    {
        if (seedField.text != string.Empty)
        {
            Global.randomSeed = int.Parse(seedField.text);
        }
        else
        {
            Global.randomSeed = Random.Range(0,int.MaxValue);
        }
        Global.levelIndex = levelIndex;
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void playSound(AudioClip clip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip, 1);
        }
    }
}
