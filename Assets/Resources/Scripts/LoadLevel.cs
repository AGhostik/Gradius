using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    public Text loadingText;
    public Image progressBar;

    private bool increaseColor;
    private float colorValue;
    private AsyncOperation async;

    void Start () {
        increaseColor = false;
        Resources.UnloadUnusedAssets();
        Global.ResetGameFields();
        StartCoroutine("LoadScene");
	}

    private void Update()
    {
        StrColorChange();
    }

    IEnumerator LoadScene()
    {
        yield return null;

        async = SceneManager.LoadSceneAsync(Global.levelIndex);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            progressBar.fillAmount = async.progress;

            if (async.progress >= 0.9f)
            {
                progressBar.fillAmount = 1;
                loadingText.text = "Press any key";

                if (Global.levelIndex == 0 || Input.anyKey)
                {
                    ActivateScene();
                }
            }

            yield return null;
        }
    }

    private void ActivateScene()
    {
        if (async != null)
        {
            async.allowSceneActivation = true;
        }
    }

    private void StrColorChange()
    {
        loadingText.color = new Color(colorValue, colorValue, colorValue, 1);

        if (increaseColor && colorValue < 1)
        {
            colorValue += Time.deltaTime / 2;
        }
        else
        {
            increaseColor = false;
        }

        if (!increaseColor && colorValue > 0.1f)
        {
            colorValue -= Time.deltaTime / 2;
        }
        else
        {
            increaseColor = true;
        }
    }
}