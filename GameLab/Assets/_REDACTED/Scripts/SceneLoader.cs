using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("VideoTitle");
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Igor Main");
    }
}
