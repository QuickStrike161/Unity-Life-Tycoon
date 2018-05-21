using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmployeeTrainingTemplate : MonoBehaviour {

    /*
     * this controls the training template
     */

    [SerializeField]
    public TMP_Text NameText;
    public TMP_Text InfoText;
    public TMP_Text buttonText;
    public Button button;
    public TrainingEmployee trainingEmployee;
    public PlayerInfo player;
    public int training;//the training that it represents
    public int place;//the place in the imageList
    public int location;//the location in the three content lists
    public bool active;//if the training can be used

    private Business business;

    //add the needed information
    public void setUp(int place, int training, int location)
    {
        business = player.business;
        this.place = place;
        this.training = training;
        this.location = location;
        this.active = true;
        NameText.SetText(business.trainingList[training].name);
        InfoText.SetText(business.trainingList[training].description);
        updateButton(false);
    }

    //pass information to trainingEmployee on click 
    public void onClick()
    {
        trainingEmployee.clicked(this.place, this.location, this.training);
    }

    //if the training is being run special will be true and ajust the time, other wise get the time that it will take for the training and add it to the buttons 
    public void updateButton(bool special)
    {
        if (special == true)
        {
            buttonText.SetText("Remove: " + trainingEmployee.getTimeForTraining((int)(business.trainingList[training].cost * (1F - business.employeesInfo[trainingEmployee.getEmployee()].trainingQ[0].progress))));
        }
        else if (location == 0)
        {
            buttonText.SetText("Add: " + trainingEmployee.getTimeForTraining(business.trainingList[training].cost));
        }
        else
        {
            buttonText.SetText("Remove: " + trainingEmployee.getTimeForTraining(business.trainingList[training].cost));
        }
    }
}
