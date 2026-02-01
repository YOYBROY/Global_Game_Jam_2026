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
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
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

    private void Start()
    {
         GameEvents.current.onGameWin += YouWin;
        GameEvents.current.onGameOver += YouLose;
    }

    private void OnDisable()
    {
        pauseAction.action.started -= Pause;
        checkScroll.action.started -= TargetCheck;
        GameEvents.current.onGameWin -= YouWin;
        GameEvents.current.onGameOver -= YouLose;
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
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public void YouLose()
    {
        Debug.Log("you lose", this);
        isPaused = true;
        Time.timeScale = 0f;
        loseCanvas.SetActive(true);
    }

    public void YouWin()
    {
        Debug.Log("you win", this);
        isPaused = true;
        Time.timeScale = 0f;
        winCanvas.SetActive(true);
    }
}
