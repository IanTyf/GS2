using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    private static TypeWriter instance;
    public List<TypeWriterSingle> typeWriterSingleList;

    public List<TimedSubtitle> subtitleHistory = new List<TimedSubtitle>();

    private void Awake()
    {
        instance = this;
        typeWriterSingleList = new List<TypeWriterSingle>();
    }

    public static void AddWriter_Static(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        instance.AddWriter (uiText, textToWrite, timePerCharacter, invisibleCharacters);
    }

    private void AddWriter(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        // only add a new subtitle if the previous one is finished
        if ((typeWriterSingleList.Count == 0) || (typeWriterSingleList.Count == 1 && typeWriterSingleList[0].finished))
        {
            typeWriterSingleList.Clear();
            typeWriterSingleList.Add(new TypeWriterSingle(uiText, textToWrite, timePerCharacter, invisibleCharacters));
        }

    }

    private void AddWriterRev(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        // only add a new subtitle if the previous one is finished
        if ((typeWriterSingleList.Count == 0) || (typeWriterSingleList.Count == 1 && typeWriterSingleList[0].finished))
        {
            typeWriterSingleList.Clear();
            TypeWriterSingle tsw = new TypeWriterSingle(uiText, textToWrite, timePerCharacter, invisibleCharacters);
            tsw.characterIndex = textToWrite.Length;
            typeWriterSingleList.Add(tsw);
        }

    }

    private void Update()
    {
        for (int i=0; i<typeWriterSingleList.Count; i++)
        {
            bool destroyInstance = typeWriterSingleList[i].Update();
            if (destroyInstance)
            {
                typeWriterSingleList.RemoveAt(i);
                i--;
            }
        }

        if (Services.timeManager.skipping)
        {
            /*
            // force remove the current subtitle
            if (typeWriterSingleList.Count > 0) {
                typeWriterSingleList[0].uiText.text = "";
                typeWriterSingleList.Clear();
            }
            */
            if (subtitleHistory.Count > 0)
            {
                TimedSubtitle lastTS = subtitleHistory[subtitleHistory.Count - 1];
                Vector3 currentTime = new Vector3(Services.timeManager.day, Services.timeManager.hour, Services.timeManager.minute);
                if (minutes(lastTS.completionTime) > minutes(currentTime))
                {
                    // replay this subtitle from the back
                    AddWriterRev(UI_Text.speechText, lastTS.text, lastTS.timePerChar, true);
                    // pop it from history
                    subtitleHistory.RemoveAt(subtitleHistory.Count - 1);
                }
            }
        }
    }

    private float minutes(Vector3 t)
    {
        return t.x * 24 * 60 + t.y * 60 + t.z;
    }

    public class TypeWriterSingle
    {
        public Text uiText;
        private string textToWrite;
        public int characterIndex;
        private float timePerCharacter;
        private float timer;
        private bool invisibleCharacters;

        public bool finished;

        public TypeWriterSingle(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
        {
            this.uiText = uiText;
            this.textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            this.invisibleCharacters = invisibleCharacters;
            characterIndex = 0;

            this.finished = false;
        }
        //returns true when complete
        public bool Update() 
        {
            if (uiText != null) 
            {
                if (Services.timeManager.skipping) timer -= Time.deltaTime * Services.timeManager.rewindSpeed;
                else timer -= Time.deltaTime;

                while (timer <= 0f)
                {
                    if (!Services.timeManager.fastForwarding) timer += timePerCharacter;
                    else timer += timePerCharacter / Services.timeManager.fastForwardSpeed;

                    if (Services.timeManager.skipping) characterIndex--;
                    else characterIndex++;

                    if (characterIndex < 0) return true; // this should destroy this

                    // this should never get called
                    if (characterIndex > textToWrite.Length)
                    {
                        //Entire string displayed
                        return true;
                    }

                    string text = textToWrite.Substring(0, characterIndex);
                    if (invisibleCharacters)
                    {
                        text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                    }
                    uiText.text = text; 


                    // when writing ends, keeps writing empty space
                    if (characterIndex == textToWrite.Length)
                    {
                        if (!this.finished) {
                            Vector3 currentTime = new Vector3(Services.timeManager.day, Services.timeManager.hour, Services.timeManager.minute);
                            TimedSubtitle newTS = new TimedSubtitle(textToWrite, currentTime, timePerCharacter, false);
                            instance.subtitleHistory.Add(newTS);

                            if (Services.timeManager.fastForwarding) timer += 1 / Services.timeManager.fastForwardSpeed;
                            else timer += 1f;
                            textToWrite = " ";
                            characterIndex = 0;
                            this.finished = true;
                        }
                    }


                    // pause a bit for every comma and period
                    if (characterIndex > 0)
                    {
                        if (textToWrite[characterIndex - 1].Equals(','))
                        {
                            if (Services.timeManager.fastForwarding) timer += timePerCharacter * 4 / Services.timeManager.fastForwardSpeed;
                            else timer += timePerCharacter * 4;
                        }
                        if (textToWrite[characterIndex - 1].Equals('.'))
                        {
                            if (Services.timeManager.fastForwarding) timer += timePerCharacter * 6 / Services.timeManager.fastForwardSpeed;
                            else timer += timePerCharacter * 6;
                        }
                    }
                }
                return false;
            }
            return false;
        }
    }
}


public class TimedSubtitle
{
    public string text;
    public Vector3 completionTime;
    public float timePerChar;
    public bool notDestroy;

    public TimedSubtitle(string text, Vector3 completionTime, float timePerChar, bool notDestroy)
    {
        this.text = text;
        this.completionTime = completionTime;
        this.timePerChar = timePerChar;
        this.notDestroy = notDestroy;
    }
}