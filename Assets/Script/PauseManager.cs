using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Movement.inputBloccato = false;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Movement.inputBloccato = true;

        Time.timeScale = 0f;

        // Mostra il cursore nel frame successivo
        StartCoroutine(ShowCursorNextFrame());

        isPaused = true;
    }

    IEnumerator ShowCursorNextFrame()
    {
        yield return null; // attende un frame
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("🏁 Hai chiuso il gioco");
    }
}
