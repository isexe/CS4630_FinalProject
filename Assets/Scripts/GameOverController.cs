using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameOverController : MonoBehaviour
{
    public Text finalScoreUI;
    public Text titleUI;
    public InputField feedbackText;

    private GameObject controller;
    private int finalScore = 0;
    private string feedback;

    // Hid the gamecontroller from the previous game
    // gets rid of UI and we can still access the singleton
    void Start()
    {
        controller = GameObject.Find("GameController");
        if(controller != null){
            controller.SetActive(false);
            CalcFinalScore();
            PlayerPrefs.SetInt("prevLevel", GameController.S.currentLevel);
        }
        UpdateUI();

        StoreData();
    }

    
    public void GoToMenu(){
        // we're done with the controller so it can go now
        if(controller != null) Destroy(controller);
        // loads menu
        SceneManager.LoadScene(0);
    }

    void UpdateUI(){
        if( GameController.S == null){
            titleUI.text = "You shouldn't be here?!";
            finalScoreUI.text = "Final Score: 'ERROR 404'";
        } else{
            if(GameController.S.won) titleUI.text = "You Win!";
            else titleUI.text = "Game Over";
            finalScoreUI.text = "Final Score: " + finalScore;
        }
        
    }

    void CalcFinalScore(){
        finalScore = Mathf.RoundToInt(GameController.S.Score + GameController.S.TimeRemaining);
        // Debug.Log("Final Score: " + finalScore);
    }

    void StoreData(){
        string path = Application.dataPath + "/GameData.txt";

        if(!File.Exists(path)){
            File.WriteAllText(path, "--- Game Data ---\n");
        }

        float timePlayed = Time.time - GameController.S.startTime;

        string playerID = "ID: " + PlayerPrefs.GetString("id") + "\n";
        string date = "Time Played: " + System.DateTime.Now + "\n";
        string playTime = "Total Playtime: " + timePlayed.ToString(".00") + "\n";
        string score = "Final Score: " + finalScore + "\n";

        string data = "\n" + playerID + date + playTime + score;

        File.AppendAllText(path, data);
    }

    public void SubmitFeedback(){
        feedback = "Feedback: " + feedbackText.text + "\n";
        feedbackText.text = "";
        File.AppendAllText(Application.dataPath + "/GameData.txt", feedback);
    }
}
