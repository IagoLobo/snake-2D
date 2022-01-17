using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeHead : SnakePart
{
    public List<SnakePart> snakeParts = new List<SnakePart>();
    private int startingSize = 3;

    public BoxCollider2D snakeHeadCollider;
    private Coroutine moveSnakeCoroutine;
    private float secondsBeforeMovement = 0.1f;

    private void Awake()
    {
        SetSnakeStartingParts();
        snakeHeadCollider = GetComponent<BoxCollider2D>();
        moveSnakeCoroutine = StartCoroutine(MoveSnake(secondsBeforeMovement));
    }

    private void Update()
    {
        SnakeInput();
    }

    private IEnumerator MoveSnake(float secondsBeforeMovement)
    {
        // Update the snake parts' position backwards because they have the informations
        // needed to adjust the part that just got there
        for (int i = snakeParts.Count - 1; i > 0; i--)
        {
            snakeParts[i].transform.position = snakeParts[i - 1].transform.position;
            snakeParts[i].direction = snakeParts[i - 1].direction;
            RotateSnakeSprite(snakeParts[i].gameObject, snakeParts[i].direction);
        }

        // Move the snake to their facing direction and round the values to align to the grid
        float x = Mathf.Round(transform.position.x) + direction.x;
        float y = Mathf.Round(transform.position.y) + direction.y;

        Vector2 newSnakeHeadPosition = new Vector2(x, y);
        transform.position = newSnakeHeadPosition;

        DetectSnakeCollision(newSnakeHeadPosition);

        yield return new WaitForSeconds(secondsBeforeMovement);

        moveSnakeCoroutine = StartCoroutine(MoveSnake(secondsBeforeMovement));
    }

    private void DetectSnakeCollision(Vector2 colliderOrigin)
    {
        // Raycast to detect collision with apple or obstacle
        RaycastHit2D[] hits = Physics2D.RaycastAll(colliderOrigin, direction, 0.1f);
        
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != null)
            {
                if(hit.collider.tag == "Apple")
                {
                    Grow();
                    GameManager.Instance.AppleWasEaten();
                }
                else if(hit.collider.tag == "Obstacle")
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
    }

    private void SnakeInput()
    {
        // Allow moving up or down only when direction is in x-axis
        if (direction.x != 0f)
        {
            if (Input.GetButtonDown("Up"))         direction = Vector2.up;
            else if (Input.GetButtonDown("Down"))  direction = Vector2.down;
        }
        // Allow moving right or left only when the direction is in y-axis
        else if (direction.y != 0f)
        {
            if (Input.GetButtonDown("Right"))      direction = Vector2.right;
            else if (Input.GetButtonDown("Left"))  direction = Vector2.left;
        }

        // Rotate head to face the direction it should
        RotateSnakeSprite(this.gameObject, this.direction);
    }

    public void Grow()
    {
        SnakePart snakePart = Instantiate(GameManager.Instance.snakePartPrefab, Vector3.zero, Quaternion.identity, this.transform.parent).GetComponent<SnakePart>();
        snakePart.transform.position = snakeParts[snakeParts.Count - 1].transform.position;
        snakePart.direction = snakeParts[snakeParts.Count - 1].direction;
        snakeParts.Add(snakePart);
        RotateSnakeSprite(snakePart.gameObject, snakePart.direction);
    }

    public void SetSnakeStartingParts()
    {
        // Set starting direction and position
        direction = Vector2.up;
        transform.position = new Vector2(0, -5);

        // Start at 1 to avoid destroying the head
        for (int i = 1; i < snakeParts.Count; i++)  Destroy(snakeParts[i].gameObject);

        // Clear the list and then add back the head
        snakeParts.Clear();
        snakeParts.Add(this);

        // Ends with startingSize - 1 because the list already has the head
        for (int i = 0; i < startingSize - 1; i++)  Grow();
    }

    private void RotateSnakeSprite(GameObject obj, Vector2 dir)
    {
        Vector3 newRot = Vector3.zero;

        if(dir == Vector2.up)
        {
            obj.transform.eulerAngles = newRot;
        }
        else if(dir == Vector2.down)
        {
            newRot = new Vector3(0, 0, 180);
            obj.transform.eulerAngles = newRot;
        }
        else if (dir == Vector2.left)
        {
            newRot = new Vector3(0, 0, 90);
            obj.transform.eulerAngles = newRot;
        }
        else if (dir == Vector2.right)
        {
            newRot = new Vector3(0, 0, 270);
            obj.transform.eulerAngles = newRot;
        }
    }
}
