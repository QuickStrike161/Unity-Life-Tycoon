using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class MainControl : MonoBehaviour {

    /*
     * this is the main control for the game, it contains general information about the player,
     * controls the money and wage of the player, and sets the stage that is being used
     */

    private string[] stagesNames = new string[] {"","Employee","Manager","Business Owner","Mayor","Premier","President","Global Domination","System Domination","Galactic Domination","Universal Domination"};

    public TMP_Text[] SkillsText;

    public TMP_Text[] GeneralInfoText;

    public GameObject[] stagesBackGround;

    public CreateName createName;
    
    private int moneyNow;
    private int moneyLast;
    private float timeSec;

    public Business reset;
    public PlayerInfo player;
    public Business setUp;

    //Set up the skill and info displays as well as activate the current stages menu
    void Start(){

        reset.reset();
        player.reset();

        for(short x = 1; x < stagesBackGround.Length; x++)
        {
            stagesBackGround[x].SetActive(false);
        }

        stagesBackGround[player.stage].SetActive(true);

        for (short x = 0; x < 5; x++){
            SkillsText[x].SetText(player.skillPercent[x] + "%");
        }

        //moneyNow = player.money;
        moneyNow = 10000000;
        moneyLast = player.money;
        GeneralInfoText[0].SetText(stagesNames[player.stage]);
        GeneralInfoText[2].SetText(player.business.businessList[player.business.type].name);
        GeneralInfoText[4].SetText(player.universeControl.ToString());

        //temp only so that the right business is being used
        player.business.businessType = player.business.businessList[0];

        timeSec = 0;
    }


    // Update is called once per frame
    void Update(){
        //create a loop that is called ever secound
        float tempTime = Time.fixedTime % 1;
        if (tempTime < timeSec) {
            incomeSecond();
        }
        timeSec = tempTime;
    }

    //add money to the player
    public void addMoney(int value)
    {
        moneyNow = moneyNow + value;
    }

    //remove money from the player
    public void spendMoney(int value){
        moneyNow = moneyNow - value;
        GeneralInfoText[3].SetText(getWage(moneyNow));
    }

    //return the current money 
    public int getMoney(){
        return moneyNow;
    }

    //set up the amount of money that the player has made since the last second
    private void incomeSecond(){
        if (player.stage == 1)
        {
            moneyNow = moneyNow + (player.playerEmployee.wage * player.skillPercent[3] / 100);
        }

        int temp = moneyNow - moneyLast;
        moneyLast = moneyNow;
        if (player.stage != 1)
        {
            player.playerEmployee.wage = temp;
        }
        player.money = moneyNow;
        GeneralInfoText[1].SetText(getWage(temp));
        GeneralInfoText[3].SetText(getWage(moneyNow));
    }

    //make a string to display money
    public string getWage(int wage){
        string temp = "";
        if (wage >= 1000000){
            temp = "K";
            wage = (int)Mathf.Ceil(wage / 1000F);
        }
        var temp1 = Mathf.Floor(wage / 100);
        var temp2 = (wage % 10);
        var temp3 = (((wage - temp2) / 10) % 10);
        return "$" + temp1 + "." + temp3 + temp2 + temp;
    }

    //add points to the skills
    private void updateSkills(float amount, int place){
        player.skillPoints[place] = player.skillPoints[place] + amount;
        if (player.skillPoints[place] > player.skillNeeded[place]){
            player.skillPercent[place] = player.skillPercent[place] + 1;
            player.skillPoints[place] = player.skillPoints[place] - player.skillNeeded[place];
            player.skillNeeded[place] = (int)Mathf.Ceil((float)player.skillNeeded[place] * 1.05F);
            SkillsText[place].SetText(player.skillPercent[place] + "%");
        }
    }

    //add the amount of point that were made from serving a customer depending on sector
    public void addToSkillsAmounts(int amount, int sector){
        switch (sector)
        {
            case 1:
                updateSkills(amount * 2, 0);
                updateSkills(amount * 1, 2);
                updateSkills((float)amount / 2, 3);
                updateSkills((float)amount / 2, 4);
                break;
            case 2:
                updateSkills(amount * 2, 1);
                updateSkills(amount * 1, 3);
                updateSkills((float)amount / 2, 0);
                updateSkills((float)amount / 2, 4);
                break;
            case 3:
                updateSkills(amount * 2, 3);
                updateSkills(amount * 1, 1);
                updateSkills((float)amount / 2, 0);
                updateSkills((float)amount / 2, 4);
                break;
            case 4:
                updateSkills(amount * 3, 4);
                updateSkills(amount * 2, 2);
                updateSkills(amount * 1, 0);
                updateSkills((float)amount / 2, 3);
                break;
        }
    }

    //generate a name
    public string getName()
    {
        return createName.getName();
    }
}
