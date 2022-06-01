using UnityEngine;

/// <summary>
/// Class responsible for the behavior of a single fish.
/// </summary>
public class Fish : MonoBehaviour
{
    /// <summary>
    /// Reference to the Flocking fish component.
    /// </summary>
    private FlockingFish _fishManager;

    /// <summary>
    /// The speed of the fish.
    /// </summary>
    private float _currentSpeed = 2f;

    /// <summary>
    /// The speed of the fish.
    /// </summary>
    private float _originalSpeed = 2f;

    /// <summary>
    /// The speed at which the fish rotates.
    /// </summary>
    float _rotationSpeed = 4f;

    /// <summary>
    /// The distance between fish.
    /// </summary>
    float _neighbourDistance = 5;

    /// <summary>
    /// Whether or not the fish is turning.
    /// </summary>
    private bool _turning = false;

    private bool _moving = false;

    private float rotationTimer = 0;

    /// <summary>
    /// Initializes a fish.
    /// </summary>
    /// <param name="manager"> Reference to the Flocking fish component</param>
    /// <param name="size">The size for this fish</param>
    /// <param name="speed">The speed for this fish</param>
    /// <param name="rotationSpeed">The speed rotation of this fish</param>
    public void Initialize(FlockingFish manager, float size, float speed, float rotationSpeed, float distance)
    {
        _fishManager = manager;
        this.transform.localScale = new Vector3(size, size, size);
        _currentSpeed = speed;
        _originalSpeed = speed;
        this._rotationSpeed = rotationSpeed;
        _neighbourDistance = distance;
        _moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        rotationTimer += Time.deltaTime;
        if (_moving)
        {
            if (Vector3.Distance(transform.position, _fishManager.GetComponent<FlockingFish>().getGoalPos()) >= _fishManager.GetComponent<FlockingFish>().getTankSize())
            {
                _turning = true;
            }
            else
            {
                _turning = false;
            }

            if (_turning)
            {
                Vector3 direction = _fishManager.GetComponent<FlockingFish>().getGoalPos() - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    _rotationSpeed * Time.deltaTime);
            }
            else
            {
                if (rotationTimer > 2)
                {
                    ApplyRules();
                    rotationTimer = 0;
                }
            }
            transform.Translate(0, 0, Time.deltaTime * _currentSpeed);
        }
    }

    void ApplyRules()
    {
        Fish[] gos;
        gos = _fishManager.GetComponent<FlockingFish>().getAllFish();

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = _originalSpeed; 
        Vector3 goalPos = _fishManager.GetComponent<FlockingFish>().getGoalPos();

        float dist;
        int groupSize = 0;

        foreach (Fish go in gos)
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= _neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < _neighbourDistance)
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
                _currentSpeed = gSpeed / groupSize;

                Vector3 direction = (vcentre + vavoid) - transform.position;
                if(direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, 
                        Quaternion.LookRotation(direction), _rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
