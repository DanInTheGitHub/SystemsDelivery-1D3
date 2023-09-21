using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RankingList : MonoBehaviour
{
    DatabaseReference mDatabase;
    string UserId;

    int score = 0;
    public List<TextMeshProUGUI> usernameTexts;
    public List<TextMeshProUGUI> scoreTexts;

    public event Action<int> OnScoreUpdated;
    private userData[] user_to_deploy = new userData[5];
    private Dictionary<string, int> all_users = new Dictionary<string, int>();

    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        GetUserScore();
        GetUsersHighestScores();

        FirebaseDatabase.DefaultInstance.GetReference("Users")
            .OrderByChild("Score").LimitToLast(5)
            .ValueChanged += HandleLeaderboardValueChanged;
    }

    void HandleLeaderboardValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
        {
            Dictionary<string, int> leaderboardData = new Dictionary<string, int>();
            int i = 0;

            foreach (var userDoc in (Dictionary<string, object>)args.Snapshot.Value)
            {
                var userObject = (Dictionary<string, object>)userDoc.Value;
                string username = userObject["Username"].ToString();
                int userScore = int.Parse(userObject["Score"].ToString());
                leaderboardData.Add(username, userScore);

                if (i < usernameTexts.Count && i < scoreTexts.Count)
                {
                    usernameTexts[i].text = username;
                    scoreTexts[i].text = userScore.ToString();
                    i++;
                }
            }
        }
    }
    public void GetUsersHighestScores()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users").OrderByChild("Score").LimitToLast(5)
            .GetValueAsync().ContinueWithOnMainThread(task =>
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
                        string username = (string)userObject["Username"];
                        int score = Convert.ToInt32(userObject["Score"]);
                        if (all_users.ContainsKey(username))
                        {
                            if (all_users[username] < score)
                            {
                                all_users[username] = score;
                            }
                        }
                        else
                        {
                            all_users.Add(username, score);
                        }

                    }

                    var sortedUsers = all_users.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    var list_Users_Name = sortedUsers.Keys.ToList();
                    var list_Users_Score = sortedUsers.Values.ToList();

                    for (int i = 0; i < sortedUsers.Count || i < 5; i++)
                    {
                        userData user = new userData();
                        user.username = list_Users_Name[i];
                        user.score = list_Users_Score[i];
                        user_to_deploy[i] = user;
                        usernameTexts[i].text = user_to_deploy[i].username;
                        scoreTexts[i].text = user_to_deploy[i].score.ToString();
                    }
                }
            });
    }
    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users/" + UserId + "/Score")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string _score = "" + snapshot.Value;
                    score = int.Parse(_score);
                }
            });
    }

    [System.Serializable]
    public class userData
    {
        public string username;
        public int score;
    }
}
