using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject MainMenu;
    public GameObject Authors;
    public GameObject Settings;

    private void CloseAll()
    {
        PauseMenu.SetActive(false);
        MainMenu.SetActive(false);
        Authors.SetActive(false);
        Settings.SetActive(false);
        Settings.transform.Find("Buttons").Find("ButtonExitToMenu").gameObject.SetActive(true);
        Settings.transform.Find("Buttons").Find("ButtonExitToPause").gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void AuthorsPanel()
    {
        CloseAll();
        Authors.SetActive(true);
    }
    public void BackPanelMenu()
    {
        CloseAll();
        MainMenu.SetActive(true);
    }

    public void BackPanelPause()
    {
        CloseAll();
        PauseMenu.SetActive(true);
    }

    public void SettingsPanelMenu()
    {
        CloseAll();
        Settings.SetActive(true);
        Settings.transform.Find("Buttons").Find("ButtonExitToPause").gameObject.SetActive(false);
    }
    public void SettingsPanelPause()
    {
        CloseAll();
        Settings.SetActive(true);
        Settings.transform.Find("Buttons").Find("ButtonExitToMenu").gameObject.SetActive(false);
    }


    public void SetQuality(string Quality)
    {
        string[] names = QualitySettings.names;

        for (int i = 0; i < names.Length; i++) {
            if (Quality == names[i])
                QualitySettings.SetQualityLevel(i);
        }
    }

    public void StartGame() 
    {
        SceneManager.LoadScene("World");
    }
}
