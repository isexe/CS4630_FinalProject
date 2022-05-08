using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Singleton is literally the best thing ever invented
    public static GameController S;

    // used for player respawns
    public GameObject playerPrefab;

    // gametime
    public float timerInSec = 240f;
    
    // self explanatory
    [Header("UI Elements")]
    public Text scoreUI;
    public Text timerUI;
    public GameObject lifeCounterUI; //ensure the children of this object are orders from top to bottom
    public GameObject respawnButton;
    public GameObject pauseScreen;

    // stuff for stuff
    private float score;
    private float minScore;
    private int lives;
    private int livesLost;
    private float playTime;
    private bool isPaused;

    // set singleton
    void Awake(){
        if(S == null){
            S = this;
        }
    }

    // Zero out stuff and get game ready
    void Start()
    {
        isPaused = false;
        minScore = 0;
        score = 0;
        lives = lifeCounterUI.transform.childCount;
        livesLost = 0;
        playTime = 0;
        UpdateTimerUI();
        UpdateScoreUI();
        UpdateLifeCounterUI();
    }

    // update timer and check for level completion
    void Update()
    {
        playTime += Time.deltaTime;
        UpdateTimerUI();

        // if level beat, move to next
        if(LevelBeat()){
            NextLevel();
        }
    }

    // called in playercontroller and updates life counter stuff
    public void HitEnemy(){
        livesLost += 1;
        respawnButton.SetActive(true);
        UpdateLifeCounterUI();
    }

    // used to respawn player using button
    public void RespawnPlayer(){
        // looks for players pretending to be dead
        // uses list just in case but should only ever be one
        GameObject[] prevPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject plyr in prevPlayers){
            Destroy(plyr);
        }

        // create new player at button loc
        GameObject playerTemp = Instantiate<GameObject>(playerPrefab);
        Vector3 spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        spawnPoint.z = 0;
        playerTemp.transform.position = spawnPoint;

        // disable button
        respawnButton.SetActive(false);
    }

    // called in playercontroller and updates score
    public void HitBalloon(GameObject obj){
        // value based on magnitude of balloon
        // bigger == better
        float magVal = obj.transform.localScale.magnitude;
        // change to int
        int scoreVal = Mathf.RoundToInt(magVal);
        // add to score and update UI
        IncScore(scoreVal);
        // destroy old balloon
        Destroy(obj);
    }

    public void IncScore(int val){
        score += val;
        UpdateScoreUI();
    }

    public void DecScore(int val){
        score -= val;
        UpdateScoreUI();
    }

    void UpdateScoreUI(){
        scoreUI.text = score.ToString();
    }
    
    // updates timer UI
    //! currently if playTime finishes it just resets
    void UpdateTimerUI(){
        if(playTime > timerInSec){
            playTime = 0;
        }
        timerUI.text = Mathf.Floor((timerInSec-playTime)/60).ToString("0") + ":" + Mathf.Floor((timerInSec-playTime)%60).ToString("00");
    }

    // updates life counter UI and restarts level if they die too much
    void UpdateLifeCounterUI(){
        if(livesLost > lives-1){
            RestartLevel();
        }
        for(int i = 0; i<livesLost; i++){
            lifeCounterUI.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // restarts level and updates data stuff, then UI stuff
    void RestartLevel(){
        ResetLifeCounterUI();
        livesLost = 0;
        // since they died the score is set to score from beginning of level
        score = minScore;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UpdateScoreUI();
        UpdateLifeCounterUI();
    }

    // resets the life counter UI so all children are visible
    void ResetLifeCounterUI(){
        for(int i = 0; i<lives; i++){
            lifeCounterUI.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    // checks if level is over by looking for balloons
    bool LevelBeat(){
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Balloon");
        return(targets.Length == 0);
    }

    // sets some data stuff then goes to next level
    //! currently just reloads current level
    void NextLevel(){
        ResetLifeCounterUI();
        livesLost = 0;
        minScore = score;

        UpdateLifeCounterUI();
        respawnButton.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //! need to add something to prevent player from committing suicide
    public void PauseGame(){
        pauseScreen.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }
    public void ResumeGame(){
        isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}
