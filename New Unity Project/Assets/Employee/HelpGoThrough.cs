using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpGoThrough : MonoBehaviour {

    /*
     * is started by the help option, goes through the diffrent parts of the first level
     * of the game explaining how they work to the player.
     */

    public GameObject skills;
    public GameObject statsWorld;
    public GameObject statsBus;
    public GameObject runButton;
    public GameObject buttons;
    public GameObject first;
    public GameObject secound;
    public GameObject third;
    public GameObject fourth;

    public GameObject menu;
    public GameObject help;

    public Button button1;
    public Button button2;

    public TMP_Text text1;
    public TMP_Text text2;

    private int stage;

    //call this to start the walkthrough
    public void startHelp(){
        stage = 0;
        run();
    }

    //this sets diffrent parts active or unactive depending what is needed 
    public void run(){
        switch (stage){
            case 0:
                menu.SetActive(false);
                help.SetActive(true);
                skills.SetActive(true);
                statsWorld.SetActive(false);
                statsBus.SetActive(false);
                runButton.SetActive(false);
                buttons.SetActive(false);
                first.SetActive(false);
                secound.SetActive(false);
                third.SetActive(false);
                fourth.SetActive(false);
                button1.gameObject.SetActive(true);
                text1.gameObject.SetActive(true);
                text1.SetText("This section displays the 5 character skills and the percentage associated with each. These amounts will increase slowly as the game proceeds and makes your character stronger.");
                stage = 1;
                break;
            case 1:
                skills.SetActive(false);
                statsWorld.SetActive(true);
                text1.SetText("This section displays the stage you are in, the amount of money your empire makes, the business you are currently in, your total money saved and the amount of the universe you control.");
                stage = 2;
                break;
            case 2:
                statsWorld.SetActive(false);
                statsBus.SetActive(true);
                text1.SetText("This section displays information about the business you are currently in. The amount of customers that have been served, and amount you make per second and the businesses happiness.");
                stage = 3;
                break;
            case 3:
                text1.SetText("The customers served will display customers served/amount you need to serve. If your happiness is above 90% the amount of customers will increase, if it is bellow it will decrease. The further from 90% the more it will change.");
                stage = 4;
                break;
            case 4:
                text1.SetText("When in the employee stage focus is what determines the speed at which processes are done, the more focus you have the faster each of the work areas will run. Focus can be purchased in the store.");
                stage = 5;
                break;
            case 5:
                statsBus.SetActive(false);
                runButton.SetActive(true);
                text1.SetText("This section displays what area of the business that your character is currently working in. The work selection will add 10 focus to the selected area. This section also displays the time, customers will be added at the start of ever day.");
                stage = 6;
                break;
            case 6:
                runButton.SetActive(false);
                buttons.SetActive(true);
                text1.SetText("This section allows you to open the map, training, shop and menu options. The map will allow you to see the town that you are located in and information that will be useful. The menu controls the appearance of the game and saving/exiting the game.");
                stage = 7;
                break;
            case 7:
                text1.SetText("Shop allows you to purchase focus and items to make your focus more effective. The Training allows you to reduce the time that it takes for you to preform tasks. Note you can add up to 4 training to be worked on in the order they were selected.");
                stage = 8;
                break;
            case 8:
                buttons.SetActive(false);
                first.SetActive(true);
                text1.SetText("This section is the first work station, this generally relates to the distribution of goods. If you have too many of these orders  waiting it will decrease happiness.");
                stage = 9;
                break;
            case 9:
                first.SetActive(false);
                secound.SetActive(true);
                text1.SetText("This section is the second work station, this generally relates to the creation of goods. If you do not have goods in this section you can not fulfill the orders in the first section.");
                stage = 10;
                break;
            case 10:
                secound.SetActive(false);
                third.SetActive(true);
                text1.gameObject.SetActive(false);
                button1.gameObject.SetActive(false);
                text2.gameObject.SetActive(true);
                button2.gameObject.SetActive(true);
                text2.SetText("This section is the third work station, this generally relates to the maintenance of the business and workplaces. The percentages here effect the happiness one for one, depending on the business this will be perfect at 0% or 100%.");
                stage = 11;
                break;
            case 11:
                third.SetActive(false);
                fourth.SetActive(true);
                text1.gameObject.SetActive(true);
                button1.gameObject.SetActive(true);
                text2.gameObject.SetActive(false);
                button2.gameObject.SetActive(false);
                text1.SetText("This section will work on selected training. When the bar is full the training is completed and the benefits have been added. You must then add a new training.");
                stage = 12;
                break;
            case 12:
                fourth.SetActive(false);
                text1.SetText("Note: your wage will increase with a set amount of customers served depending on the business. Also you can click on the title of the work station to quickly change to it. Hope you enjoy this game, if you want to find more information go to future under options.");
                stage = 13;
                break;
            case 13:
                text1.gameObject.SetActive(false);
                button1.gameObject.SetActive(false);
                skills.SetActive(true);
                statsWorld.SetActive(true);
                statsBus.SetActive(true);
                runButton.SetActive(true);
                buttons.SetActive(true);
                first.SetActive(true);
                secound.SetActive(true);
                third.SetActive(true);
                fourth.SetActive(true);
                help.SetActive(false);
                break;
        }
    }
}
