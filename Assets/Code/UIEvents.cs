using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIEvents : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private string exposedParam;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private InputActionReference pauseAction;
    private bool isPaused;

    private void OnEnable()
    {
        pauseAction.action.started += Pause;
    }

    private void OnDisable()
    {
        pauseAction.action.started -= Pause;
    }
    public void LoadSceneByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        Debug.Log("You have quitted");
        Application.Quit();
    }

    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.SetFloat(exposedParam, Mathf.Log10(sliderValue) * 20);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Pause(InputAction.CallbackContext obj)
    {
        if(isPaused == false)
        {
            isPaused = true;
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
        }
    }
}
