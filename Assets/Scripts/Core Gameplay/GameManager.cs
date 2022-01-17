using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Elements")]
    public Collider2D gameGrid;
    private Bounds gameGridBounds;
    public GameObject snakePrefab;
    public GameObject applePrefab;
    public GameObject snakePartPrefab;
    private SnakeHead snakeHead;
    private GameObject apple;
    private Animator appleAnim;
    private int playerScore;

    private int WIN_CONDITION = 50;

    [Header("UI Elements")]
    public GameOverUIManager gameOverUIManager;

    private Coroutine explodeAppleCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        
        Instance = this;
    }

    private void Start()
    {
        gameGridBounds = gameGrid.bounds;
        ResetGame();
        AudioManager.Instance?.PlayGameplayMusic();
    }

    private void CreateSnake()
    {
        // Create snake and get its main component
        snakeHead = GameObject.Instantiate(snakePrefab, Vector2.zero, Quaternion.identity).GetComponentInChildren<SnakeHead>();
    }

    private void CreateApple()
    {
        // Get random position above the grid's middle point and round the values to align with it
        float appleOriginPosX = Mathf.Round(Random.Range(gameGridBounds.min.x, gameGridBounds.max.x));
        float appleOriginPosY = Mathf.Round(Random.Range(0, gameGridBounds.max.y));
        Vector2 appleOriginPos = new Vector2(appleOriginPosX, appleOriginPosY);

        // Creating the apple and setting it on its origin position
        apple = GameObject.Instantiate(applePrefab, appleOriginPos, Quaternion.identity);

        // Get apple animator
        appleAnim = apple.GetComponent<Animator>();
    }

    public void ResetApplePosition()
    {
        // Give time for apple exploding animation to play and then change its position
        explodeAppleCoroutine = StartCoroutine(ExplodeAppleThenChangePosition());
    }

    public IEnumerator ExplodeAppleThenChangePosition()
    {
        appleAnim.Play("AppleExploding");
        yield return new WaitForSeconds(appleAnim.GetCurrentAnimatorClipInfo(0).Length);

        // Get random position inside the grid bounds and round the values to align with it
        float posX = Mathf.Round(Random.Range(gameGridBounds.min.x, gameGridBounds.max.x));
        float posY = Mathf.Round(Random.Range(gameGridBounds.min.y, gameGridBounds.max.y));

        // Set the new apple position
        apple.transform.position = new Vector2(posX, posY);
        appleAnim.Play("AppleIdle");
    }

    public void AppleWasEaten()
    {
        AddScore();
        AudioManager.Instance?.PlayAppleEatenSFX();

        if(playerScore >= WIN_CONDITION)
        {
            gameOverUIManager.ShowVictoryScreen(playerScore);
            
            // Play Victory Jingle!
            AudioManager.Instance?.PlayVictoryJingleMusic();

            // Destroy previous game objects
            if(snakeHead != null)
            {
                // Get the parent to have their reference after destroying the children
                GameObject snakeParent = snakeHead.transform.parent.gameObject;

                // Destroy the other parts first, since the list reference is in the head
                // Then destroy the head and after that, the parent
                for (int i = 1; i < snakeHead.snakeParts.Count; i++)  Destroy(snakeHead.snakeParts[i].gameObject);
                Destroy(snakeHead.snakeParts[0].gameObject);
                Destroy(snakeParent);
            }

            if(apple != null)
            {
                if(explodeAppleCoroutine != null)   StopCoroutine(explodeAppleCoroutine);
                Destroy(apple);
            }
        }
        else
        {
            ResetApplePosition();
        }
    }

    public void GameOver()
    {
        // Destroy previous game objects
        if(snakeHead != null)
        {
            // Get the parent to have their reference after destroying the children
            GameObject snakeParent = snakeHead.transform.parent.gameObject;

            // Destroy the other parts first, since the list reference is in the head
            // Then destroy the head and after that, the parent
            for (int i = 1; i < snakeHead.snakeParts.Count; i++)  Destroy(snakeHead.snakeParts[i].gameObject);
            Destroy(snakeHead.snakeParts[0].gameObject);
            Destroy(snakeParent);
        }

        if(apple != null)
        {
            if(explodeAppleCoroutine != null)   StopCoroutine(explodeAppleCoroutine);
            Destroy(apple);
        }

        // Show game over screen and shows player score
        gameOverUIManager.ShowGameOverScreen(playerScore);

        // Play game over jingle
        AudioManager.Instance?.PlayGameOverJingleSFX();
    }

    private void AddScore()
    {
        playerScore++;
    }

    public void ResetGame()
    {
        // Reset player score
        playerScore = 0;

        // Create new game objects
        CreateSnake();
        CreateApple();
    }
}
