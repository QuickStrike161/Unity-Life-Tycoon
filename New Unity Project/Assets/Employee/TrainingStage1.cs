using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainingStage1 : MonoBehaviour
{

    /*
     * this is the first level(employee level) of training, it uses the trainingList from the business
     * to display 4 randomly generated trainings for the player to choose from. Once training has been
     * added it can not be removed. And once complete it benifits are added to the player.
     */

    //get the objects that will display information for the game
    public TMP_Text[] titles;
    public TMP_Text[] info;
    public TMP_Text[] button;
    public TMP_Text[] time;
    public Button[] buttons;

    public EmployeeGame employeeGame;
    public MainControl mainControl;
    public PlayerInfo player;
    
    public int[] displayed = new int[] { -1, -1, -1, -1 };
    private Business business;

    // Use this for initialization
    void Start()
    {
        business = player.business;
        setUp();
        updateTrainingDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        updateTrainingDisplay();
    }

    //return the time that the current training will take based on how fast it is currently running (min:sec) or (hour:sec)
    public string getTimeForTraining(int amount)
    {
        int tempFocus = player.focus[3];

        if (tempFocus == 0)
        {
            return "--:--";
        }
        else
        {
            tempFocus = (int)Mathf.Ceil(amount / (tempFocus * player.playerEmployee.focusMultiplyer[3] * player.playerEmployee.timeMultiplyer[3]));
            if (tempFocus > 3600)
            {
                int tempTime = (int)Mathf.Floor(tempFocus / 3600);
                tempFocus = tempFocus % 3600;
                return tempTime.ToString() + "h" + tempFocus.ToString() + "m";
            }
            else if (tempFocus > 60)
            {
                int tempTime = (int)Mathf.Floor(tempFocus / 60);
                tempFocus = tempFocus % 60;
                return tempTime.ToString() + "m" + tempFocus.ToString() + "s";
            }
            else
            {
                return tempFocus.ToString() + "s";
            }
        }
    }

    //update the class when a training has been finished, remove that training and set it to inactive as well as activate any following training
    public void finishTraining()
    {
        int tempNumber = business.workStations[3].orders[0].wants[0];
        trainingIteam tempTraining = business.trainingList[tempNumber];
        for (short x = 0; x < tempTraining.affectAreas.Length; x++)
        {
            player.playerEmployee.timeMultiplyer[tempTraining.affectAreas[x]] = player.playerEmployee.timeMultiplyer[tempTraining.affectAreas[x]] + (tempTraining.affectAmount / 100F);
        }
    }

    //set up the availible trainings as well as the display
    private void setUp()
    {
        if (player.playerEmployee.timeMultiplyer[0] == 1 && player.playerEmployee.trainingAvailible.Count == 0)
        {
            for (short x = 0; x < business.trainingList.Length; x++)
            {
                if (business.trainingList[x].active == true)
                {
                    player.playerEmployee.trainingAvailible.Add(x);
                }
            }
        }
        suffle();
    }

    //update the displayes for the training background 
    private void updateTrainingDisplay()
    {
        for (short x = 0; x < displayed.Length; x++)
        {
            if (displayed[x] == -1)
            {
                titles[x].SetText("None");
                info[x].SetText("out of training");
                button[x].SetText("--:");
                time[x].SetText("--");
                buttons[x].interactable = false;
            }
            else
            {
                titles[x].SetText(business.trainingList[displayed[x]].name);
                info[x].SetText(business.trainingList[displayed[x]].description);
                time[x].SetText(getTimeForTraining(business.trainingList[displayed[x]].cost));
                buttons[x].interactable = true;
            }
        }
    }

    //suffles the training that is displayed
    public void suffle()
    {
        displayed = new int[] { -1, -1, -1, -1 };
        if (player.playerEmployee.trainingAvailible.Count < 5)
        {
            for (short x = 0; x < player.playerEmployee.trainingAvailible.Count; x++)
            {
                displayed[x] = player.playerEmployee.trainingAvailible[x];
            }
        }
        else
        {
            for (short x = 0; x < 4; x++)
            {
                displayed[x] = getOne();
            }
        }
    }

    //method to generate a training that is not being displayed
    private int getOne()
    {
        int intSelect = Random.Range(0, player.playerEmployee.trainingAvailible.Count);
        intSelect = player.playerEmployee.trainingAvailible[intSelect];
        bool isIn = false;
        for (short x = 0; x < 4; x++)
        {
            if (intSelect == displayed[x])
            {
                isIn = true;
            }
        }

        if (isIn == true)
        {
            intSelect = getOne();
        }
        return intSelect;
    }

    //add the training when you hit the add button
    public void addTraining(int place)
    {
        int[] tempArray = new int[2];
        tempArray[0] = displayed[place];
        tempArray[1] = business.trainingList[displayed[place]].cost;
        order tempOrder = new order(business.trainingList[displayed[place]].name,tempArray,false);
        business.workStations[3].orders.Add(tempOrder);
        if (business.trainingList[displayed[place]].follower != -1)
        {
            player.playerEmployee.trainingAvailible.Add(business.trainingList[displayed[place]].follower);
        }
        player.playerEmployee.trainingAvailible.Remove(displayed[place]);
        suffle();
    }
}