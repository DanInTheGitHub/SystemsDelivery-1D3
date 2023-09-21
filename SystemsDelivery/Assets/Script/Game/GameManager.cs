using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Firebase.Database;

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
    public TextMeshProUGUI scoreText2;

    private int playerScore = 0;
    private int cpuScore = 0;

    private int round = 0;

    DatabaseReference mDatabase;
    string UserId;
    private int score;

    private bool gameInProgress = true;
    [SerializeField] GameObject exitbutton;

    private void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        GetUserScore();

        exitbutton.SetActive(true);

        availableCpuChoices.AddRange(new List<Choice> { Choice.Rock, Choice.Paper, Choice.Scissors });

        rockButton.onClick.AddListener(() => MakePlayerChoice(Choice.Rock));
        paperButton.onClick.AddListener(() => MakePlayerChoice(Choice.Paper));
        scissorsButton.onClick.AddListener(() => MakePlayerChoice(Choice.Scissors));
    }

    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users/" + UserId + "/Score")
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string _score = "" + snapshot.Value;
                    score = int.Parse(_score);

                    setLabel();
                }
            });
    }

    private void setLabel()
    {
        scoreText2.text = "Score: " + score;
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
            DisableCpuChoiceButton(cpuChoice);

            // Compare playerChoice and cpuChoice to determine the result
            Result gameResult = DetermineResult(playerChoice, cpuChoice);

            resultText.text = gameResult.ToString();
            UpdateScore(gameResult);

            round++;

            previousCpuChoice = cpuChoice;
            if (round == 3)
            {
                StartCoroutine(RestartGame());

                round = 0;
            }
            if (round != 0)
            {
                exitbutton.SetActive(false);
            }
            
        }
    }

    private Result DetermineResult(Choice player, Choice cpu)
    {
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
        if (result == Result.Win)
        {
            playerScore++;
        }
        else if (result == Result.Lose)
        {
            cpuScore++;
        }
        scoreText.text = "Player: " + playerScore + " CPU: " + cpuScore;
    }

    private IEnumerator RestartGame()
    {
        gameInProgress = false;
        yield return new WaitForSeconds(2f);
        resultText.text = "Choose an option:";

        availableCpuChoices.Clear();
        availableCpuChoices.AddRange(initialCpuChoices);

        ChangeScore();
        SetNewScore();
        ResetScoreOfMach();

        EnableAllButtons();
        EnableAllCpuChoiceButtons();

        previousCpuChoice = Choice.None;

        gameInProgress = true;
        exitbutton.SetActive(true);
    }

    private void ResetScoreOfMach()
    {
        playerScore = 0;
        cpuScore = 0;
    }

    private void SetNewScore()
    {
        scoreText2.text = ("Score: "+ score);
        mDatabase.Child("Users").Child(UserId).Child("Score").SetValueAsync(score);
    }

    private void ChangeScore()
    {
        if (playerScore == 3 && cpuScore == 0)
        {
            score += 4;
        }
        else if (playerScore == 2 && cpuScore == 1)
        {
            score += 3;
        }
        else if (playerScore == 2 && cpuScore == 0)
        {
            score += 2;
        }
        else if (playerScore == cpuScore)
        {
            score += 1;
        }
        else if (playerScore == 0 && cpuScore == 2)
        {
            score -= 2;
        }
        else if (playerScore == 1 && cpuScore == 2)
        {
            score -= 3;
        }
        else if (playerScore == 0 && cpuScore == 3)
        {
            score -= 4;
        }
    }

    public void GetUsersHighestScores()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Users").OrderByChild("Score").LimitToLast(5).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
                {
                    var userObject = (Dictionary<string, object>)userDoc.Value;
                    Debug.Log(userObject["Username"] + " : " + userObject["Score"]);
                }
            }
        });
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