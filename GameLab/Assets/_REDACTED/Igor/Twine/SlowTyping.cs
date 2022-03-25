using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class SlowTyping : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;
    int numShowing;

    float delay = 0.018f; // bigger number is slower typing
    private float kPunctuationDelay = 0.3f;

    string data;
    UnityAction callback;
    bool init = false;
    bool openTag = false;
    int numLinesNeeded = 0;
    bool pauses;

    float kSoundDelay = 0.1f;
    float curSoundDelay = 0;

    public string GetDisplayText()
    {
        return data;
    }

    private void Init()
    {
        if (init)
            return;

        init = true;
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Begin(bool pauses, string data, UnityAction callback = null)
    {
        Init();
        numShowing = 0;
        this.data = data.Trim();
        this.callback = callback;
        this.pauses = pauses;

        text.enableAutoSizing = true;
        text.text = data;
        numLinesNeeded = CountNumLines();
        float desiredSize = text.fontSize;
        text.enableAutoSizing = false;
        text.fontSize = desiredSize * 0.95f; // Not always right so underestimate

        text.text = "";
        text.ForceMeshUpdate();

        if (numLinesNeeded == -1)
            text.text += "";
        else if (numLinesNeeded <= 1)
            text.text += "<size=130%><alpha=#00>x<alpha=#FF></size>" + "\n";
        else if (numLinesNeeded == 2)
        {
            text.text += "<size=80%><alpha=#00>x<alpha=#FF></size>" + "\n";
            numLinesNeeded = 0;
        }
        else if (numLinesNeeded >= 3)
        {
            text.text += "<size=30%><alpha=#00>x<alpha=#FF></size>" + "\n";
            numLinesNeeded = 0;
        }

        InvokeRepeating("ShowOneMore", 0, delay);
    }

    private int CountNumLines()
    {
        text.ForceMeshUpdate();
        int lineCount = 1;
        if (null != text.textInfo)
        {
            lineCount = text.textInfo.lineCount;
        }

        // This is buggy for low numbers, manually override
        if (lineCount <= 1)
        {
            lineCount = (int)(text.text.Length / 55.0f);
            //Debug.Log( "Overode to : " + lineCount );
        }

        return lineCount;
    }

    public void Clear()
    {
        Init();
        CancelInvoke();
        data = "";
        text.text = "";
    }

    internal bool IsDone()
    {
        if (string.IsNullOrEmpty(data))
            Clear();
        return numShowing >= data.Length;
    }

    private void Update()
    {
        curSoundDelay -= Time.deltaTime;
    }

    private void ShowOneMore()
    {
        if (IsDone())
        {
            CancelInvoke();
            callback?.Invoke();
            callback = null;
        }
        else
        {
            //// Remove line endings we added
            //if ( numLinesNeeded > 1 )
            //    text.text = text.text.Substring( 0, numShowing );

            char nextChar = data[numShowing];
            text.text += nextChar;
            numShowing++;

            if (curSoundDelay < 0)
            {
                //ServiceLocator.instance.GetService<SoundMachine>().StartSoundEffect("Tock");
                curSoundDelay = kSoundDelay;
            }

            SkipThroughTags(nextChar);

            //// Add line endings
            //if ( numLinesNeeded > 1 ) {
            //    int lineEndingsPresent = CountNumLines();
            //    int numToAdd = numLinesNeeded - lineEndingsPresent;
            //    for ( int i = 0; i < numToAdd; i++ )
            //        text.text += '\n' + "<alpha=#00>x";
            //    // Add actual content so TMP respects me
            //    //text.text += ;
            //}

            if (pauses && (nextChar == '.' || nextChar == '!' || nextChar == '?' || nextChar == '>'))
            {
                CancelInvoke();
                if (gameObject.activeInHierarchy)
                    StartCoroutine(WaitAfterPunctuation());
            }
        }
    }

    IEnumerator WaitAfterPunctuation()
    {
        yield return new WaitForSeconds(kPunctuationDelay);
        InvokeRepeating("ShowOneMore", 0, delay);
    }

    private void SkipThroughTags(char lastCharTyped)
    {
        if (lastCharTyped == '<')
            openTag = true;

        if (lastCharTyped == '>')
            openTag = false;

        if (openTag)
            ShowOneMore();
    }

    internal void SpeedUp()
    {
       // ServiceLocator.instance.GetService<SoundMachine>().StartSoundEffect("forceComplete");

        StopAllCoroutines();
        CancelInvoke();

        while (!IsDone())
        {
            char nextChar = data[numShowing];
            text.text += nextChar;
            numShowing++;
        }

        callback?.Invoke();
        callback = null;
    }
}
