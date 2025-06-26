using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    // Butonlara atanacak fonksiyonlar
    public void LoadPlayGameScene()
    {
        SceneManager.LoadScene(2); // Build Settings'teki oyun sahnesi index'i
    }

    public void LoadOptionsScene()
    {
        SceneManager.LoadScene(1); // Options sahnesi index'i
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0); // Menü sahnesi index'i
    }

    public void Level1()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Editor'de çalışırken kapatma
#endif
    }
}