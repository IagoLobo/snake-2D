using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance?.PlayMainMenuMusic();
    }

    public void StartButton()
    {
        AudioManager.Instance?.PlayButtonClickSFX();
        SceneManager.LoadScene("Gameplay");
    }

    public void ExitButton()
    {
        AudioManager.Instance?.PlayButtonClickSFX();
        Application.Quit();
    }
}
