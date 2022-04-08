using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    private static TypeWriter instance;
    public List<TypeWriterSingle> typeWriterSingleList;

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
        typeWriterSingleList.Clear();
        typeWriterSingleList.Add(new TypeWriterSingle(uiText, textToWrite, timePerCharacter, invisibleCharacters));
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
    }

    public class TypeWriterSingle
    {
        private Text uiText;
        private string textToWrite;
        private int characterIndex;
        private float timePerCharacter;
        private float timer;
        private bool invisibleCharacters;

        public TypeWriterSingle(Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
        {
            this.uiText = uiText;
            this.textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            this.invisibleCharacters = invisibleCharacters;
            characterIndex = 0;
        }
        //returns true when complete
        public bool Update() 
        {
            if (uiText != null) {
                timer -= Time.deltaTime;
                while (timer <= 0f)
                {
                    timer += timePerCharacter;
                    characterIndex++;
                        string text = textToWrite.Substring(0, characterIndex);
                    if (invisibleCharacters)
                    {
                        text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                    }
                    uiText.text = text; 

                    if (characterIndex >= textToWrite.Length)
                    {
                        //Entire string displayed
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
    }
}
