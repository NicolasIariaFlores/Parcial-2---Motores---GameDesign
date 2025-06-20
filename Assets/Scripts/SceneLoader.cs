using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Slider _loadBar;
    [SerializeField] private GameObject _loadPanel;
    public void LoadSceneByNumber(int sceneNumber)
    {
        _loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneNumber));
    }

    IEnumerator LoadAsync(int sceneNumber)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNumber);
        Time.timeScale = 1f;
        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);
            _loadBar.value = asyncOperation.progress / 0.9f;
            yield return null;
        }
    }
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
