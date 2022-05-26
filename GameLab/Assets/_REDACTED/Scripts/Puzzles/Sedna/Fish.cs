using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fishManager;
    public float speed = 2f;
    public float offset = 2f;
    public float maxSize = 100f;
    public float minSize = 30f;
    private float ogSpeed = 2f;
    float rotationSpeed = 4f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 5;

    bool turning = false;
    void Start()
    {
        ogSpeed = speed;
        speed = ogSpeed + Random.Range(-offset, offset);
        float r = Random.Range(minSize, maxSize);
        this.transform.localScale = new Vector3(r, r, r);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, fishManager.GetComponent<FlockingFish>().getGoalPos()) >= fishManager.GetComponent<FlockingFish>().getTankSize())
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = fishManager.GetComponent<FlockingFish>().getGoalPos() - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);


        }
        else
        {
            if (Random.Range(0, 60) < 2)
            {
                ApplyRules();
                rotationSpeed = Random.Range(2f, 4f);
                neighbourDistance = Random.Range(1f, 3f);
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = fishManager.GetComponent<FlockingFish>().getAllFish();

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = ogSpeed; 
        Vector3 goalPos = fishManager.GetComponent<FlockingFish>().getGoalPos();

        float dist;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < neighbourDistance)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    //Fish anotherFlock = go.GetComponent<Fish>();
                    //gSpeed = gSpeed + anotherFlock.speed; 
                }
            }

            if(groupSize > 0)
            {
                vcentre = vcentre / groupSize + (goalPos - this.transform.position);
                speed = gSpeed / groupSize;

                Vector3 direction = (vcentre + vavoid) - transform.position;
                if(direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, 
                        Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
