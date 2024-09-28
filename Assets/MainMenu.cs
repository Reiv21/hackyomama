using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject credits;
    public void Play()
    {
        //pass
    }

    public void Credits()
    {
        credits.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        credits.SetActive(false);
    }
}
