using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmployeeTrainingTemplate : MonoBehaviour {

    [SerializeField]
    public TMP_Text NameText;
    public TMP_Text InfoText;
    public TMP_Text buttonText;
    public Button button;
    public TrainingEmployee trainingEmployee;
    public PlayerInfo player;
    public int training;
    public int place;
    public int location;
    public bool active;

    private Business business;

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

    public void onClick()
    {
        trainingEmployee.clicked(this.place, this.location, this.training);
    }

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
