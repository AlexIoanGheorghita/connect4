using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject player1, player2; // they represent the circles (green and blue) that belong to each player.
    bool isPlayer1Turn, isGameFinished;

    [SerializeField]
    TextMeshProUGUI playerTurnMessage;

    const string PLAYER1_TURN_MESSAGE = "Player 1's Turn";
    const string PLAYER2_TURN_MESSAGE = "Player 2's Turn";
    Color PLAYER1_COLOR = new Color(0f / 255f, 197f / 255f, 31f / 255f, 1f);
    Color PLAYER2_COLOR = new Color(0f / 255f, 139f / 255f, 255f / 255f, 1f);

    Board board;

    // initial configuration of the game. Player 1 starts when the game starts.
    private void Awake()
    {
        isPlayer1Turn = true;
        isGameFinished = false;
        playerTurnMessage.text = PLAYER1_TURN_MESSAGE;
        playerTurnMessage.color = PLAYER1_COLOR;
        board = new Board();
        UnityEngine.Debug.Log(playerTurnMessage.text);
    }

    public void StartGame()
    {
        // Starting a game means loading a scene.
        // Because we have only one scene we want to load it.
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void StopGame()
    {
        // The following is a preprocessor directive.
        // It tells the compiler to compile only if the statement is met (meaning, only if we are in the Unity Editor).
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // function that gets called every frame while the game is still not finished.
    private void Update()
    {
        // whenever the user clicks its mouse,
        // check whether the user has won. 
        // Otherwise, update the position of the user's circle.
        if (Input.GetMouseButtonDown(0))
        {
            if (isGameFinished) return;

            // gives us 3 point coordinates, but we want 2D
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero); 
            // we want to see if we hit a collider (column). If not, we return nothing.
            if (!hit.collider) return;

            // if we manage to hit a collider (column), then we need to check which column it is.
            if (hit.collider.CompareTag("Clickable"))
            {
                // we check if the object is out of bounds
                if (hit.collider.gameObject.GetComponent<ColumnCollider>().targetPosition.y > 1.5f) return;

                // otherwise, create a new player object (a circle)
                Vector3 spawnPosition = hit.collider.gameObject.GetComponent<ColumnCollider>().initialPosition;
                Vector3 endPosition = hit.collider.gameObject.GetComponent<ColumnCollider>().targetPosition;
                GameObject playerCircle = Instantiate(isPlayer1Turn ? player1 : player2);
                playerCircle.transform.position = spawnPosition;
                playerCircle.GetComponent<MoveCircle>().targetPosition = endPosition;

                // after we move the circle, we need to increase the height so that
                // the next circle gets on top of it
                hit.collider.gameObject.GetComponent<ColumnCollider>().targetPosition = new Vector3(endPosition.x, endPosition.y + 0.676f, endPosition.z);
                board.UpdateBoard(hit.collider.gameObject.GetComponent<ColumnCollider>().column - 1, isPlayer1Turn);
                if (board.hasWon(isPlayer1Turn))
                {
                    playerTurnMessage.text = (isPlayer1Turn ? "Player 1" : "Player 2") + " Wins!";
                    isGameFinished = true;
                    return;
                }

                // update UI
                playerTurnMessage.text = !isPlayer1Turn ? PLAYER1_TURN_MESSAGE : PLAYER2_TURN_MESSAGE;
                playerTurnMessage.color = !isPlayer1Turn ? PLAYER1_COLOR : PLAYER2_COLOR;

                isPlayer1Turn = !isPlayer1Turn;
            }
        }
    }
}
