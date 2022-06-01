using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingFish : MonoBehaviour
{
    /// <summary>
    /// The fish prefab used to create the flock.
    /// </summary>
    [SerializeField]
    private Fish _fishPrefab;

    /// <summary>
    /// The targets.
    /// </summary>
    private Transform[] _targets;

    /// <summary>
    /// The target object that the fish should surround,
    /// </summary>
    [SerializeField]
    private GameObject _targetObject;

    /// <summary>
    /// The size of the tank, this defines how far the fish can swim.
    /// </summary>
    [SerializeField]
    private int _tankSize = 4;

    /// <summary>
    /// The amount of fish in the flock.
    /// </summary>
    [SerializeField]
    private int _numFish = 50;

    /// <summary>
    /// The distance between the fish.
    /// </summary>
    [SerializeField]
    private float _distancing = 3f;

    /// <summary>
    /// The amount of fish in the flock.
    /// </summary>
    [SerializeField]
    private Vector2 _fishSize;

    /// <summary>
    /// The amount of fish in the flock.
    /// </summary>
    [SerializeField]
    private Vector2 _fishSpeed;

    /// <summary>
    /// Rotation speed for the fish.
    /// </summary>
    [SerializeField]
    private float _rotationSpeed;

    /// <summary>
    /// How often the fish should reposition.
    /// </summary>
    [SerializeField]
    private float _repositionSpeed;

    /// <summary>
    /// The reposition speed with an offset.
    /// </summary>
    private float _modifiedRepositionSpeed;

    /// <summary>
    /// The list of all fish in the flock.
    /// </summary>
    private Fish[] _allFish;

    /// <summary>
    /// Timer for the target position of the flock.
    /// </summary>
    private float _positionTimer = 0;

    private Vector3 goalPos = Vector3.zero;
    private Vector3 newGoalPosition = Vector3.zero;

    void Start()
    {
        _modifiedRepositionSpeed = _repositionSpeed;
        goalPos = this.transform.position;
        _allFish = new Fish[_numFish];
        for (int i = 0; i<_numFish; i++)
        {
            Vector3 pos = new Vector3(this.transform.position.x+Random.Range(-_tankSize, _tankSize), 
                this.transform.position.y+Random.Range(-_tankSize, _tankSize),
                this.transform.position.z + Random.Range(-_tankSize, _tankSize));
            _allFish[i] = Instantiate(_fishPrefab, pos, Quaternion.identity);
            _allFish[i].Initialize(this, Random.Range(_fishSize.x, _fishSize.y), Random.Range(_fishSpeed.x, _fishSpeed.y), _rotationSpeed, _distancing);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _positionTimer += Time.deltaTime;
        if (_positionTimer > _modifiedRepositionSpeed)
        {
            goalPos = _targetObject.transform.position;
            newGoalPosition = new Vector3(goalPos.x + Random.Range(-_tankSize, _tankSize),
                             goalPos.y + Random.Range(0, _tankSize),
                             goalPos.z + Random.Range(-_tankSize, _tankSize));
            goalPos = newGoalPosition;
            _positionTimer = 0;
            _modifiedRepositionSpeed = Random.Range(_repositionSpeed - 1, _repositionSpeed + 1);
        }
        //Debug.Log("random" +random+ "x" + goalPos.x + "y" +goalPos.y + "z" +goalPos.z);
    }

    public Fish[] getAllFish()
    {
        return _allFish;
    }

    public Vector3 getGoalPos()
    {
        return goalPos;
    }

    public int getTankSize()
    {
        return _tankSize;
    }
}
