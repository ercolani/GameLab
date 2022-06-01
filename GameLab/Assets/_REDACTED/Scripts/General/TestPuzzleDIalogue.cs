using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPuzzleDIalogue : MonoBehaviour
{
    [SerializeField]
    private Deity deity;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            deity.FireTorchThought();
        } 
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            deity.FirePuzzleComment("dw");
        }
    }
}
