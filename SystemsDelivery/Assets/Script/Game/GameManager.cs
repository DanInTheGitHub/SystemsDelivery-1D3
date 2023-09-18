using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Button rockButton;
    public Button paperButton;
    public Button scissorsButton;

    public Button cpuRockButton;
    public Button cpuPaperButton;
    public Button cpuScissorsButton;

    private List<Choice> initialCpuChoices = new List<Choice> { Choice.Rock, Choice.Paper, Choice.Scissors};
    private List<Choice> availableCpuChoices = new List<Choice>();
    private Choice previousCpuChoice = Choice.None;


    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;

    private int playerScore = 0;
    private int cpuScore = 0;

    private int round = 0;

    private int score = 0;

    private bool gameInProgress = true;

    private void Start()
    {
        availableCpuChoices.AddRange(new List<Choice> { Choice.Rock, Choice.Paper, Choice.Scissors });

        rockButton.onClick.AddListener(() => MakePlayerChoice(Choice.Rock));
        paperButton.onClick.AddListener(() => MakePlayerChoice(Choice.Paper));
        scissorsButton.onClick.AddListener(() => MakePlayerChoice(Choice.Scissors));
    }

    private void MakePlayerChoice(Choice playerChoice)
    {
        if (gameInProgress)
        {
            //DisableAllButtons();

            availableCpuChoices.Remove(previousCpuChoice);

            // Choose a random option from the available choices
            Choice cpuChoice = availableCpuChoices[Random.Range(0, availableCpuChoices.Count)];
          
            DisablePlayerChoiceButton(playerChoice);
            // Disable the CPU's choice
            DisableCpuChoiceButton(cpuChoice);

            // Compare playerChoice and cpuChoice to determine the result
            Result gameResult = DetermineResult(playerChoice, cpuChoice);

            // Update the UI
            resultText.text = gameResult.ToString();
            UpdateScore(gameResult);

            round++;

            previousCpuChoice = cpuChoice;
            if (round == 3)
            {
                // Allow the player to restart the game
                StartCoroutine(RestartGame());

                round = 0;
            }
            
        }
    }

    private Result DetermineResult(Choice player, Choice cpu)
    {
        // Implement your logic to determine the game result here
        // You can use enums for Choice and Result to make the code more readable
        // For example:
        if (player == cpu) return Result.Draw;
        if ((player == Choice.Rock && cpu == Choice.Scissors) ||
            (player == Choice.Paper && cpu == Choice.Rock) ||
            (player == Choice.Scissors && cpu == Choice.Paper))
        {
            return Result.Win;
        }
        return Result.Lose;
    }

    private void UpdateScore(Result result)
    {
        // Update the player and CPU scores based on the result
        if (result == Result.Win)
        {
            playerScore++;
        }
        else if (result == Result.Lose)
        {
            cpuScore++;
        }

        // Update the score display
        scoreText.text = "Player: " + playerScore + " CPU: " + cpuScore;
    }

    private IEnumerator RestartGame()
    {
        gameInProgress = false;
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before allowing a new game
        resultText.text = "Choose an option:";

        availableCpuChoices.Clear();
        availableCpuChoices.AddRange(initialCpuChoices);

        EnableAllButtons();
        EnableAllCpuChoiceButtons();

        previousCpuChoice = Choice.None;

        gameInProgress = true;
    }

    private void DisableAllButtons()
    {
        rockButton.interactable = false;
        paperButton.interactable = false;
        scissorsButton.interactable = false;
    }

    private void EnableAllButtons()
    {
        rockButton.interactable = true;
        paperButton.interactable = true;
        scissorsButton.interactable = true;
    }

    private void DisableCpuChoiceButton(Choice cpuChoice)
    {
        switch (cpuChoice)
        {
            case Choice.Rock:
                cpuRockButton.interactable = false;
                break;
            case Choice.Paper:
                cpuPaperButton.interactable = false;
                break;
            case Choice.Scissors:
                cpuScissorsButton.interactable = false;
                break;
        }
    }
    private void DisablePlayerChoiceButton(Choice playerChoice)
    {
        switch (playerChoice)
        {
            case Choice.Rock:
                rockButton.interactable = false;
                break;
            case Choice.Paper:
                paperButton.interactable = false;
                break;
            case Choice.Scissors:
                scissorsButton.interactable = false;
                break;
        }
    }


    private void EnableAllCpuChoiceButtons()
    {
        cpuRockButton.interactable = true;
        cpuPaperButton.interactable = true;
        cpuScissorsButton.interactable = true;
    }


    public enum Choice
    {
        Rock,
        Paper,
        Scissors,
        None
    }

    public enum Result
    {
        Win,
        Lose,
        Draw
    }
}