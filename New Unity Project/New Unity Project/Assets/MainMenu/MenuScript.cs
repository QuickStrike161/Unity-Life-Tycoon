using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour {

    /*
     * this is the first menu the player sees when they enter the game, has continue/create new game options
     * as well as leads to the option menu and a button to exit the game 
     */

    public TMP_Text infoForUser;
    public GameObject PromptMenu;
    public GameObject NewGameMenu;
    public GameObject MainMenu;
    
    //set up the display that will be shown to the player depending on if they have a saved game or not
    void Start(){
        bool played = getPlayed();
        if (played == false){
            infoForUser.SetText("There is no game to continue, do you want to begin a new game?");
        }
        else{
            infoForUser.SetText("There is a game already created, do you want to restart deleting this game?");
        }
    }

    //start a new game start setUp
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

    //go to the saved game and load 
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

    //call to determin if there is a saved game 
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
