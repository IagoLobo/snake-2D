using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI playerScoreText;
    public GameObject confettis;
    public void ShowGameOverScreen(int playerScore)
    {
        gameOverText.text = "GAME OVER";
        playerScoreText.text = $"Player Score: {playerScore}";
        playerScoreText.color = Color.red;
        confettis.SetActive(false);

        gameOverScreen.SetActive(true);
    }

    public void ShowVictoryScreen(int playerScore)
    {
        gameOverText.text = "VICTORY!";
        playerScoreText.text = $"Player Score: {playerScore}";
        playerScoreText.color = new Color(1,0.843137f,0); // Gold
        confettis.SetActive(true);

        gameOverScreen.SetActive(true);
    }

    public void TryAgainButton()
    {
        AudioManager.Instance?.PlayGameplayMusic();
        AudioManager.Instance?.PlayButtonClickSFX();
        GameManager.Instance.ResetGame();
        gameOverScreen.SetActive(false);
    }

    public void BackToMainMenu()
    {
        AudioManager.Instance?.PlayButtonClickSFX();
        SceneManager.LoadScene("MainMenu");
    }
}
