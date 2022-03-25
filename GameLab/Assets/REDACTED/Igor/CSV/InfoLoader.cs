using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Globalization;

public class InfoLoader : MonoBehaviour
{
    private int progress;
    private int numberOfFacts;
    private int numberOfRules;
    private int factsAndRulesOffset = 2; //the index of the line at which the fact headers begin
    private int ruleColumnIndex = 5;
    private int factColumnIndex = 1;

    private DialogueQueries dialogueQueries;

    private void Awake()
    {
        dialogueQueries = GetComponent<DialogueQueries>();
    }

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        StartCoroutine(CSVDownloader.DownloadData(AfterDownload));
    }

    public void AfterDownload(string data)
    {
        if (data == null)
        {
            Debug.LogError("Was not able to download or retrieve data.");
        }
        else
        {
            StartCoroutine(ProcessData(data, AfterProcessData));
        }
    }

    private void AfterProcessData(string errorMessage)
    {
        //nothing to do here really but let's leave it in anyway
    }

    public IEnumerator ProcessData(string data, System.Action<string> onCompleted)
    {
        bool factsFilled = false;

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            //code below will be needed when we are fully done with our CSV and we can store inside the game without requiring a download
            // ResourceRequest dbLoader = Resources.LoadAsync<TextAsset>("db");
            // while (!dbLoader.isDone) {
            //     yield return null;
            // }
            // TextAsset db = dbLoader.asset as TextAsset;
            // data = db.text;

            //line level
            int currentLineIndex = 0;
            bool inQuote = false;
            int linesSinceUpdate = 0;
            int kLinesBetweenUpdate = 15;

            //entry level
            string currentEntry = "";
            int currentCharacterIndex = 0;
            bool currentEntryContainedQuote = false;
            List<string> currentLineEntries = new List<string>();

            char lineEnding = IsIOS() ? '\n' : '\r';
            int lineEndingLength = IsIOS() ? 1 : 2;

            while (currentCharacterIndex < data.Length)
            {
                if (!inQuote && (data[currentCharacterIndex] == lineEnding))
                {
                    //skip the line ending
                    currentCharacterIndex += lineEndingLength;

                    //wrap up the last entry
                    //if it was in a quote, trim bordering quotation marks
                    if (currentEntryContainedQuote)
                    {
                        currentEntry = currentEntry.Substring(1, currentEntry.Length - 2);
                    }

                    currentLineEntries.Add(currentEntry);
                    currentEntry = "";
                    currentEntryContainedQuote = false;

                    //line ended
                    ProcessLineFromCSV(currentLineEntries, currentLineIndex, factsFilled);
                    currentLineIndex++;
                    currentLineEntries = new List<string>();

                    linesSinceUpdate++;
                    if (linesSinceUpdate > kLinesBetweenUpdate)
                    {
                        linesSinceUpdate = 0;
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    if (data[currentCharacterIndex] == '"')
                    {
                        inQuote = !inQuote;
                        currentEntryContainedQuote = true;
                    }

                    //entry level
                    if (data[currentCharacterIndex] == ',')
                    {
                        if (inQuote)
                        {
                            currentEntry += data[currentCharacterIndex];
                        }
                        else
                        {
                            //if it was in a quote, trim bordering quotation marks
                            if (currentEntryContainedQuote)
                            {
                                currentEntry = currentEntry.Substring(1, currentEntry.Length - 2);
                            }

                            currentLineEntries.Add(currentEntry);
                            currentEntry = "";
                            currentEntryContainedQuote = false;
                        }
                    }
                    else
                    {
                        currentEntry += data[currentCharacterIndex];
                    }
                    currentCharacterIndex++;
                }
                progress = (int)((float)currentCharacterIndex / data.Length * 100.0f);
            }
            onCompleted(null);
            factsFilled = true;
        }
        //foreach (KeyValuePair<string, Rule> entry in dialogueQueries.rules)
        //{
        //    Debug.Log("Rule Name: " + entry.Key);
        //    foreach (KeyValuePair<string, Criterion> rule in entry.Value.factList)
        //    {
        //        Debug.Log("Fact Name: " + rule.Key + ", Condition: " + rule.Value.fa + " <= value <= " + rule.Value.fb);
        //    }
        //}
    }

    private void ProcessLineFromCSV(List<string> currentLineElements, int currentLineIndex, bool factsFilled)
    {

        if (currentLineIndex == 0)
        {
            for (int columnIndex = 0; columnIndex < currentLineElements.Count; columnIndex++)
            {
                switch (columnIndex)
                {
                    case 2:
                        numberOfFacts = int.Parse(currentLineElements[columnIndex]);
                        break;
                    case 6:
                        numberOfRules = int.Parse(currentLineElements[columnIndex]);
                        break;
                }
            }
        }
        else if (currentLineElements.Count > 0 && currentLineIndex >= factsAndRulesOffset && currentLineIndex <= factsAndRulesOffset + numberOfFacts)
        {
            for (int columnIndex = 0; columnIndex < currentLineElements.Count; columnIndex++)
            {
                //index 0 are the headers
                if (columnIndex == factColumnIndex && !factsFilled)
                {
                    ProcessFacts(currentLineElements, columnIndex);
                }
                if (columnIndex == ruleColumnIndex && factsFilled && currentLineIndex > factsAndRulesOffset && currentLineIndex <= factsAndRulesOffset + numberOfRules)
                {
                    ProcessRules(currentLineElements, columnIndex);
                }
            }
        }
        else
        {
            Debug.LogError("Database line did not fall into one of the expected categories.");
        }
    }

    private bool IsIOS()
    {
        #if UNITY_IOS
            return true;
        #endif
        return false;
    }

    private void ProcessFacts(List<string> currentLineElements, int columnIndex)
    {
        //this is all whack and you could remove the branches if you can make a general case for all of this, but since it happens once, it doesn't matter THAT much     
        object h;
        // you could just compare the string but this is cleaner and more understandible imo (and cooler haha)
        string s = currentLineElements[columnIndex + 1];
        System.Type t = System.Type.GetType("System." + currentLineElements[columnIndex + 2]);
        if (t == typeof(int))
        {
            h = int.Parse(s);
        }
        else if (t == typeof(bool))
        {
            h = bool.Parse(s);
        }
        else if (t == typeof(float))
        {
            h = float.Parse(s);
        }
        else
        {
            h = s;
        }
        dialogueQueries.facts.Add(currentLineElements[columnIndex], h);
    }

    private void ProcessRules(List<string> currentLineElements, int columnIndex)
    {
        string key;
        string value;
        object o;
        List<string> rules = SplitRules(currentLineElements, columnIndex + 1);
        List<string> conditions = SplitConditions(currentLineElements, columnIndex + 2);
        System.Type objectType;

        if (!dialogueQueries.rules.ContainsKey(currentLineElements[columnIndex]))
        {
            dialogueQueries.rules.Add(currentLineElements[columnIndex], new Rule());
        }

        for (int i = 0; i < rules.Count; i++)
        {
            key = rules[i].Substring(0, rules[i].IndexOf('=') - 1); //fix bug with just being able to use the equals sign
            value = rules[i].Substring(rules[i].IndexOf('=') + 1);
            if (dialogueQueries.facts[key].GetType() == typeof(int))
            {
                o = int.Parse(value);
                objectType = o.GetType();
            }
            else if (dialogueQueries.facts[key].GetType() == typeof(bool))
            {
                o = bool.Parse(value);
                objectType = o.GetType();
            }
            else if (dialogueQueries.facts[key].GetType() == typeof(float))
            {
                o = float.Parse(value);
                objectType = o.GetType();
            }
            else
            {
                o = value;
                objectType = o.GetType();
            }
            dialogueQueries.rules[currentLineElements[columnIndex]].SetRule(key, new Criterion(o));
            SetCondition(conditions[i], dialogueQueries.rules[currentLineElements[columnIndex]].factList[key], objectType);
        }
    }

    //splits the keys column for rules into separate strings based on the comma placement
    private List<string> SplitRules(List<string> currentLineElements, int columnIndex)
    {
        string[] temp = currentLineElements[columnIndex].Split(',');
        List<string> rules = new List<string>();
        for (int i = 0; i < temp.Length; i++)
        {
            rules.Add(temp[i]);
            rules[i] = rules[i].Trim();
            if (rules[i].Length == 0)
            {
                rules.RemoveAt(i);
            }
        }
        return rules;
    }

    //splits the conditional values column into separate strings based on the comma placement
    private List<string> SplitConditions(List<string> currentLineElements, int columnIndex)
    {
        string[] temp = currentLineElements[columnIndex].Split(',');
        List<string> conditions = new List<string>();
        for (int i = 0; i < temp.Length; i++)
        {
            conditions.Add(temp[i]);
            conditions[i] = conditions[i].Trim();
            if (conditions[i].Length == 0)
            {
                conditions.RemoveAt(i);
            }
        }
        return conditions;
    }

    //takes the conditional values column. parses both values into the correct data type, and then puts them into each criterion in rules
    private void SetCondition(string condition, Criterion criterion, System.Type type)
    {
        int indexOfSeperation = condition.IndexOf(':');
        if (type == typeof(string))
        {
            float fa = System.Convert.ToSingle(ParseStringToFloat(condition.Substring(0, indexOfSeperation - 1)));
            float fb = System.Convert.ToSingle(ParseStringToFloat(condition.Substring(indexOfSeperation + 2)));
            criterion.fa = fa;
            criterion.fb = fb;
        }
        else if (type == typeof(bool))
        {
            float fa = System.Convert.ToSingle(bool.Parse(condition.Substring(0, indexOfSeperation - 1)));
            float fb = System.Convert.ToSingle(bool.Parse(condition.Substring(indexOfSeperation + 2)));
            criterion.fa = fa;
            criterion.fb = fb;
        }
        else
        {
            float fa = System.Convert.ToSingle(condition.Substring(0, indexOfSeperation - 1));
            float fb = System.Convert.ToSingle(condition.Substring(indexOfSeperation + 2));
            criterion.fa = fa;
            criterion.fb = fb;
        }
    }

    //turns a string into a numerical value by adding up all of the ASCII codes of its characters
    private float ParseStringToFloat(string s)
    {
        byte[] asciiBytes = Encoding.ASCII.GetBytes(s);
        float parsed = 0;
        foreach (byte character in asciiBytes)
        {
            parsed += System.Convert.ToSingle(character);
        }
        return parsed;
    }
}

