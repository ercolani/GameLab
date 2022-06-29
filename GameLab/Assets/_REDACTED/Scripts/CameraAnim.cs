using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnim : MonoBehaviour
{
    // Start is called before the first frame update
    private float timer = 0;
    public float duration = 5;
    public Vector3 increase;
    private float tik = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        tik += Time.deltaTime;
        if (tik > 0.01f && timer < duration)
        {
            this.transform.position = new Vector3(this.transform.position.x + increase.x, this.transform.position.y + increase.y, this.transform.position.z + increase.z);
            tik = 0;
        }
    }
}
