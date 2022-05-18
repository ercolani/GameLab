//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// Handles all candle behaviour.
///// </summary>
//public class CandleManager : MonoBehaviour
//{
//    /// <summary>
//    /// Whether the candle is ready to light up other objects.
//    /// </summary>
//    [SerializeField]
//    private bool _beingUsed;

//    /// <summary>
//    /// Caches the rigibody.
//    /// </summary>
//    private Rigidbody rb;

//    [SerializeField]
//    private Transform flamePosition;

//    [SerializeField]
//    private float flameRadius;

//    /// <summary>
//    /// Whether the candle is ready to light up other objects.
//    /// </summary>
//    public bool BeingUsed => _beingUsed;
    
//    /// <summary>
//    /// Is invoked when the candle starts being used.
//    /// </summary>
//    public event Action StartsBeingUsed;

//    /// <summary>
//    /// Is invoked when the candle stops being used.
//    /// </summary>
//    public event Action StopsBeingUsed;

//    /// <summary>
//    /// Changes the rotation angle of the candle when it is being used.
//    /// </summary>
//    [SerializeField]
//    private float rotationAngle;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    private void OnEnable()
//    {
//        StartsBeingUsed += RotateCandleToUse;
//        StartsBeingUsed += RotateCandleToOrigin;
//    }

//    private void OnDisable()
//    { 
//        StartsBeingUsed -= RotateCandleToUse;
//        StartsBeingUsed -= RotateCandleToOrigin;
//    }

//    private void Update()
//    {
//        //manages the active candle state depending on if the player clicks E
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            _beingUsed = true;
//            StartsBeingUsed?.Invoke();
//            LightUpNearbyTorches();
//        }

//        if (Input.GetKeyUp(KeyCode.E))
//        {
//            _beingUsed = false;
//            StopsBeingUsed?.Invoke();
//        }
//    }

//    /// <summary>
//    /// Rotates the candle to indicate that it is being used.
//    /// </summary>
//    private void RotateCandleToUse()
//    {
//        //RigidbodyConstraints previousConstraints = rb.constraints;
//        //rb.constraints = RigidbodyConstraints.None;
//        //rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
//        //transform.Rotate(0.0f, 0.0f, rotationAngle, Space.Self);
//        ////rb.constraints = previousConstraints;
//    }
        
//    /// <summary>
//    /// Rotates the candle back to indicate that it is not being used.
//    /// </summary>
//    private void RotateCandleToOrigin()
//    {
//        //transform.Rotate(0.0f, -rotationAngle, 0.0f, Space.Self);
//    }

//    /// <summary>
//    /// Casts a sphere around the candle to see if any torches or reset switches are nearby. It activates the first one it finds (should be nearest but not implemented yet).
//    /// </summary>
//    private void LightUpNearbyTorches()
//    {
//        Collider[] _torches = Physics.OverlapSphere(flamePosition.position, flameRadius, 1 << 15);
//        foreach (Collider torch in _torches)
//        {
//            if (torch.gameObject.CompareTag("Reset Switch"))
//            {
//                torch.GetComponent<ResetSwitch>().ActivateTorch();
//                return;
//            } 
//            else
//            {
//                if (!torch.GetComponent<PuzzleTorch>().Active)
//                {
//                    torch.GetComponent<PuzzleTorch>().ActivateTorch();
//                    return;
//                }
//            }
//        }
//    }


//}
