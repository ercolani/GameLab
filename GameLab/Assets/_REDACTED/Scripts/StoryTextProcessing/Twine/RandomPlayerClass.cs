using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueQueries;
using Query;

[RequireComponent(typeof(DialogueController))]
public class RandomPlayerClass : MonoBehaviour
{
    private void Start()
    {
        FactsToQuery facts = new FactsToQuery();
        facts.SubscribeToQueryEvent();
    }
}
