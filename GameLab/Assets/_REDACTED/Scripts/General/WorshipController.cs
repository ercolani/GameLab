using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioController;

public class WorshipController : MonoBehaviour
{
    [SerializeField]
    private CandleWickController _wickController;
    
    [SerializeField]
    private KeyCode _worshipInput;

    [SerializeField]
    private bool _canWorship;

    [SerializeField]
    private float _worshipCheckDelay;

    [SerializeField]
    private bool _isWorshipping;

    private void Update()
    {
        if (_canWorship)
        {
            if (Input.GetKeyDown(_worshipInput))
            {
                _isWorshipping = true;
                StartCoroutine(BeginWorship());
            }
        }

        if (_isWorshipping)
        {

        }
    }

    public void EnableWorship()
    {
        _canWorship = true;
    }

    private IEnumerator BeginWorship()
    { 
        if (!_isWorshipping)
        {
            PlaySound("Candle Worship");
        }

        yield return new WaitForSeconds(_worshipCheckDelay);
    }

}
