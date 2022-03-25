using System;
using System.Collections;
using System.Collections.Generic;
using static DialogueQueries;
using UnityEngine;

namespace Query
{
    public class FactsToQuery
    {
        public List<object> factsToQuery;
        private DialogueQueries dialogueQueries;

        public void SubscribeToQueryEvent()
        {
            onQuery += GiveFactsToQuery;
        }

        public void GiveFactsToQuery()
        {
            //give facts off to query
            Debug.Log("Meow");
        }
    }
}