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
    [SerializeField] private GameObject scrollCanvas;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private InputActionReference checkScroll;
    [SerializeField] private GameObject tabTooltip;
    [SerializeField] private Animator scrollAnimator;
    private bool isPaused;
    private bool checkingScroll;

    private void OnEnable()
    {
        pauseAction.action.started += Pause;
        checkScroll.action.started += TargetCheck;
    }

    private void OnDisable()
    {
        pauseAction.action.started -= Pause;
        checkScroll.action.started += TargetCheck;
    }
    public void LoadSceneByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        isPaused = false;
        Time.timeScale = 1.0f;
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
        isPaused = false;
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

    void TargetCheck(InputAction.CallbackContext obj)
    {
        if(checkingScroll == false)
        {
            checkingScroll = true;
            Destroy(tabTooltip);
            scrollAnimator.SetTrigger("isChecking");
        }
        else
        {
            checkingScroll = false;
            scrollAnimator.SetTrigger("isChecking");
        }
        
    }
}
