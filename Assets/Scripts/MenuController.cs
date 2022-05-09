using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject howToUI;
    public Text idText;
    public Text prevLevelText;

    void Start(){
        UnShowHowTo();
        //PlayerPrefs.DeleteKey("id");  //used for testing
        if(!PlayerPrefs.HasKey("id")){
            // if this was a database would need to check for available id, for now this is place holder
            PlayerPrefs.SetString("id", Random.Range(0, int.MaxValue).ToString());
        }
        idText.text = "ID: " + PlayerPrefs.GetString("id");
    
        if(!PlayerPrefs.HasKey("prevLevel")){
            PlayerPrefs.SetInt("prevLevel", 0);
        }
        UpdatePreviousLevelText();
    }

    public void ShowHowTo(){
        howToUI.SetActive(true);
        menuUI.SetActive(false);
    }

    public void UnShowHowTo(){
        howToUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void PlayGame(){
        SceneManager.LoadScene(1);
    }

    void UpdatePreviousLevelText(){
        if(PlayerPrefs.GetInt("prevLevel") == 0){
            prevLevelText.text = "";
        }
        else{
            prevLevelText.text = "You last played Level " + PlayerPrefs.GetInt("prevLevel") + ".";
        }
    }
}
