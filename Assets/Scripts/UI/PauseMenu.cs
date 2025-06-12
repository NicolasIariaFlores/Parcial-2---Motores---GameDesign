using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _menuPause;
    [SerializeField] private GameObject _menuOptions;

    private bool _gamePaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gamePaused)
            {
                Renude();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        _gamePaused = true;
        Time.timeScale = 0f;
        _pauseButton.SetActive(false);
        _menuPause.SetActive(true);
        _menuOptions.SetActive(false);
    }

    public void Renude()
    {
        _gamePaused = false;
        Time.timeScale = 1f;
        _pauseButton.SetActive(true);
        _menuPause.SetActive(false);
        _menuOptions.SetActive(false);
    }
    public void Options()
    {
        _gamePaused = true;
        Time.timeScale = 0f;
        _pauseButton.SetActive(false);
        _menuPause.SetActive(false);
        _menuOptions.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Cerrando... jiji");
        Application.Quit();
    }
}
