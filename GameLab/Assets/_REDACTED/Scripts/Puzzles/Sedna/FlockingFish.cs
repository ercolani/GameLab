using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingFish : MonoBehaviour
{
    // Start is called before the first frame update\

    public GameObject fishPrefab;
    public GameObject goalObject;
    public int tankSize = 4;
    public int numFish = 50;
    private GameObject[] allFish;
    int random = 0;

    private Vector3 goalPos = Vector3.zero;
    private Vector3 currentPos = Vector3.zero;

    void Start()
    {
        goalPos = this.transform.position;
        allFish = new GameObject[numFish];
        for (int i = 0; i<numFish; i++)
        {
            Vector3 pos = new Vector3(this.transform.position.x+Random.Range(-tankSize, tankSize), 
                this.transform.position.y+Random.Range(-tankSize, tankSize),
                this.transform.position.z + Random.Range(-tankSize, tankSize));
            allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        random = Random.Range(0, 100);
        if (random < 10)
        {
            goalPos = goalObject.transform.position;
            currentPos = new Vector3(goalPos.x + Random.Range(-tankSize, tankSize),
                             goalPos.y + Random.Range(0, tankSize),
                             goalPos.z + Random.Range(-tankSize, tankSize));
            goalPos = currentPos;
        }

        //Debug.Log("random" +random+ "x" + goalPos.x + "y" +goalPos.y + "z" +goalPos.z);
    }

    public GameObject[] getAllFish()
    {
        return allFish;
    }

    public Vector3 getGoalPos()
    {
        return goalPos;
    }

    public int getTankSize()
    {
        return tankSize;
    }
}
