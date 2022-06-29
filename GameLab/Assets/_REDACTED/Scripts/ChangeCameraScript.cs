using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject playerCam;

    [SerializeField]
    private GameObject flyCam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerCam.SetActive(true);
            flyCam.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            playerCam.SetActive(false);
            flyCam.SetActive(true);
        }
    }
}
