using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour {
    public TMP_Text infoForUser;
    public GameObject PromptMenu;
    public GameObject NewGameMenu;
    public GameObject MainMenu;
    
    void Start(){
        bool played = getPlayed();
        if (played == false){
            infoForUser.SetText("There is no game to continue, do you want to begin a new game?");
        }
        else{
            infoForUser.SetText("There is a game already created, do you want to restart deleting this game?");
        }
    }

    public void BeginGame(){
        bool played = getPlayed();
        if (played == false){
            MainMenu.SetActive(false);
            NewGameMenu.SetActive(true);
        }
        else{
            MainMenu.SetActive(false);
            PromptMenu.SetActive(true);
        }
	}

	public void ContinueGame(){
        bool played = getPlayed();
        if (played == true){
            SceneManager.LoadScene(PlayerPrefs.GetInt("stageC"));
        }
        else{
            MainMenu.SetActive(false);
            PromptMenu.SetActive(true);
        }
    }

    public void QuitGame(){
        Application.Quit();
    }

    private static bool getPlayed()
    {
        if (PlayerPrefs.HasKey("savedG") == false)
        {
            return false;
        }
        else if (PlayerPrefs.GetString("savedG") == "true")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
