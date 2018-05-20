using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateGame : MonoBehaviour {

    public TMP_Text TextInfo;
    public Button startButton;
    public TMP_Text ComingSoon;
    public Button beginButton;

    private bool start;
    private int sector;
    private int level;
    private string sectorChoosen;

    public Business[] runThese;

    public PlayerInfo player;

    private int oneToRun;

    public void beginGame(){
        oneToRun = 0;
        PlayerPrefs.DeleteAll();
        player.reset();
        player.sector = sector;
        player.business = runThese[oneToRun];
        player.playerEmployee.wage = runThese[oneToRun].wage;
        SceneManager.LoadScene(1);
    }

    public void setText(int position){
        sector = position;
        if (position == 0){
            startButton.gameObject.SetActive(false);
            ComingSoon.gameObject.SetActive(false);
        }
        else if (position == 4){
            startButton.gameObject.SetActive(false);
            ComingSoon.gameObject.SetActive(true);
        }
        else{
            ComingSoon.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
        }

        if (position == 0){
            TextInfo.SetText("INFO");
        }
        else if (position == 1){
            TextInfo.SetText("The service sector is the most simplistic of all the sectors.  Businesses in this sector deal with selling goods to customers. Challenges in this sector are balancing supply and demand, and maintaining customer's happiness. This sector is recommended for new players.");
        }
        else if (position == 2){
            TextInfo.SetText("The extraction sector begins to challenge players. Businesses in this sector deal with the extraction of goods such as lumber or minerals as well as farming. Challenges in this sector is to create efficiency as well as filling the demands of customers.");
        }
        else if (position == 3){
            TextInfo.SetText("The processing sector will challenge players abilities. Businesses in this sector create products out of raw materials(factories). The main challenge in this sector is how to secure the goods needed as well as securing costumers to purchase the large volumes of goods created.");
        }
        else{
            TextInfo.SetText("The black market sector will challenge the most skilled players. The constant risk of capture as well as demands of control by force makes this sector a high risk high reward area. This is the only sector where it is possible for the player to loose the game.");
        }
    }

    public void StartGame(){
        TextInfo.SetText("Please note that this game is currently in early development and the game you are about to play is only a small part of what is to come. To find more information about this game you can find it under future in the options menu.");
        startButton.gameObject.SetActive(false);
        beginButton.gameObject.SetActive(true);
    }
}
