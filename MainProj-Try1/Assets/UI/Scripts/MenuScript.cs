using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    public Canvas quitMenu;
    public Button startText;
    public Button exitText;

    // Use this for initialization
    void Start()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();

        quitMenu.enabled = false;
    }

    // If Exit button in 'Sub Menu' is clicked
    public void ExitPressed()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
    }

    // If user chooses 'No' in the Sub Menu
    public void NoPressed()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
    }

    public void StartLevel()
    {
        //Code to start Level 1 goes here
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
