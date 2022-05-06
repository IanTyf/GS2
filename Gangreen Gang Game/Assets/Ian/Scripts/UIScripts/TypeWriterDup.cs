using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriterDup : MonoBehaviour
{
    private static TypeWriterDup instance;
    public List<TypeWriterSingle> typeWriterSingleList;

    private void Awake()
    {
        instance = this;
        typeWriterSingleList = new List<TypeWriterSingle>();
    }

    public static void AddWriter_Static(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        instance.AddWriter(uiText, textToWrite, timePerCharacter, invisibleCharacters);
    }

    public static void AddWriter_Static(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters, bool notDestroy)
    {
        instance.AddWriter(uiText, textToWrite, timePerCharacter, invisibleCharacters, notDestroy);
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

    private void AddWriter(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters, bool notDestroy)
    {
        // only add a new subtitle if the previous one is finished
        if ((typeWriterSingleList.Count == 0) || (typeWriterSingleList.Count == 1 && typeWriterSingleList[0].finished))
        {
            typeWriterSingleList.Clear();
            typeWriterSingleList.Add(new TypeWriterSingle(uiText, textToWrite, timePerCharacter, invisibleCharacters, notDestroy));
        }

    }

    private void Update()
    {
        for (int i = 0; i < typeWriterSingleList.Count; i++)
        {
            bool destroyInstance = typeWriterSingleList[i].Update();
            if (destroyInstance && !typeWriterSingleList[i].notDestroy)
            {
                typeWriterSingleList.RemoveAt(i);
                i--;
            }
        }

        if (Services.timeManager.skipping)
        {
            // force remove the current subtitle
            if (typeWriterSingleList.Count > 0)
            {
                typeWriterSingleList[0].uiText.text = "";
                typeWriterSingleList.Clear();
            }
        }
    }

    public class TypeWriterSingle
    {
        public Text uiText;
        private string textToWrite;
        private int characterIndex;
        private float timePerCharacter;
        private float timer;
        private bool invisibleCharacters;

        public bool finished;

        public bool notDestroy;

        public TypeWriterSingle(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
        {
            this.uiText = uiText;
            this.textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            this.invisibleCharacters = invisibleCharacters;
            characterIndex = 0;

            this.finished = false;
        }

        public TypeWriterSingle(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters, bool notDestroy)
        {
            this.uiText = uiText;
            this.textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            this.invisibleCharacters = invisibleCharacters;
            characterIndex = 0;

            this.finished = false;
            this.notDestroy = true;
        }
        //returns true when complete
        public bool Update()
        {
            if (uiText != null)
            {
                timer -= Time.deltaTime;
                while (timer <= 0f)
                {
                    if (!Services.timeManager.fastForwarding) timer += timePerCharacter;
                    else timer += timePerCharacter / Services.timeManager.fastForwardSpeed;
                    characterIndex++;

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
                    if (characterIndex == textToWrite.Length && !this.notDestroy)
                    {
                        if (Services.timeManager.fastForwarding) timer += 1 / Services.timeManager.fastForwardSpeed;
                        else timer += 1f;
                        textToWrite = " ";
                        characterIndex = 0;
                        this.finished = true;
                    }

                    // this should never get called
                    if (characterIndex > textToWrite.Length)
                    {
                        //Entire string displayed
                        return true;
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
