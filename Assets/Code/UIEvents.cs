using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private string exposedParam;

    private void Update()
    {

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
}
