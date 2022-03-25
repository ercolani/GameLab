using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQueries : MonoBehaviour
{
    public Dictionary<string, object> facts = new Dictionary<string, object>(); //stores every fact, updated every query
    public Dictionary<string, Rule> rules = new Dictionary<string, Rule>(); //stores every rule

    public delegate void UpdateAllFacts(); 
    public static event UpdateAllFacts onQuery; //classes that subscribe have their instance properties update the fact dictionary every query

    public static void InitializeQuery()
    {
        onQuery?.Invoke();
    }
}

[System.Serializable]
public class Rule
{
    public Dictionary<string, Criterion> factList = new Dictionary<string, Criterion>();

    //fill an entry in the fact list of the rule
    public void SetRule(string key, Criterion value)
    {
        factList[key] = value;
    }
}

public class Criterion
{
    public float fa;
    public float fb;
    public object value;

    //constructor
    public Criterion(object value)
    {
        value = this.value;
    }

    //set the values of the comparison values
    public void SetComparisonValues(float fa, float fb)
    {
        fa = this.fa;
        fb = this.fb;
    }

    //reducing branch penalties by having all comparisons follow the same interval; if it is a direct comparison, fa equals negative infinity and fb is positive infinity
    public bool Compare()
    {
        return (System.Convert.ToSingle(value) <= fa && System.Convert.ToSingle(value) <= fb);
    }
}

