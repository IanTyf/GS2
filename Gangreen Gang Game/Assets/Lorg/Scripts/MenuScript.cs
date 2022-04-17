using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Button NewGameButton;
    public Button OptionsButton;
    public Button QuitButton;

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Options()
    {
        Debug.Log("ClickedOnOptions");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
