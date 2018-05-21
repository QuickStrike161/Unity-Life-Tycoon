using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmployeeControl : MonoBehaviour {

    /*
     * this menu displays employee information for the player and allows the player to move the employee to a new work station
     */ 

    public TMP_ColorGradient regularColor;
    public TMP_ColorGradient highlitedColor;
    public TMP_Text[] titles;
    public TMP_Text[] shopDisplay;
    public TMP_Text[] trainingDisplay;
    public TMP_Text[] employeeText;
    public Button[] buttons;
    public TMP_InputField raiseAmount;

    public ManagerGame managerGame;
    public PlayerInfo player;

    private Business business;
    private int place;
    private int raiseFor;
    private float timeSec;

    // Use this for initialization
    void Start () {
        setUp();
    }

    //update the displayes based on the employee that has been selected
    public void setUp()
    {
        raiseFor = 0;
        business = player.business;
        place = managerGame.getSelectedEmployee();
        //make the title of the workstation that the employee is working in blue
        for (short x = 0; x < 4; x++)
        {
            if (business.employeesInfo[place].workingIn == x)
            {
                titles[x].colorGradientPreset = highlitedColor;
            }
            else
            {
                titles[x].colorGradientPreset = regularColor;
            }
        }
        titles[4].text = business.employeesInfo[place].name;
        employeeText[0].text = business.employeesInfo[place].happiness + "%";
        employeeText[1].text = getWage(business.employeesInfo[place].wage);
        employeeText[2].text = getTimeWorking(business.employeesInfo[place].timeWorking);
        employeeText[3].text = business.workStations[business.employeesInfo[place].wasWorkingIn].name;

        //set the amount in percent that the training and shop have been completed
        for (short x = 0; x < 4; x++)
        {
            titles[x].text = business.workStations[x].name;
            shopDisplay[x].text = "Shoping: " + getPercentDone(x, true) + "%";
            trainingDisplay[x].text = "Training: " + getPercentDone(x, false) + "%";
            if (business.workStations[x].EmployeeSpace == 0)
            {
                buttons[x].interactable = false;
            }

        }
    }

    //create a loop that is called ever secound
    void Update () {
        float tempTime = Time.fixedTime % 1;
        if (tempTime < timeSec)
        {
            employeeText[2].text = getTimeWorking(business.employeesInfo[place].timeWorking);
        }
        timeSec = tempTime;
    }

    //when the raise button has been click increase the employees salery, and increase there hapiness
    public void raise()
    {
        if (raiseFor == 0)
        {
            return;
        }

        business.employeesInfo[place].wage = business.employeesInfo[place].wage + (int)Mathf.Floor((business.employeesInfo[place].wage * raiseFor) / 100);
        employeeText[1].text = getWage(business.employeesInfo[place].wage);
        business.employeesInfo[place].happiness = business.employeesInfo[place].happiness + raiseFor;
        if (business.employeesInfo[place].happiness > 100)
        {
            business.employeesInfo[place].happiness = 100;
        }
        employeeText[0].text = business.employeesInfo[place].happiness + "%";
        managerGame.upDateEmployeeVisual(business.employeesInfo[place], true);
        raiseFor = 0;
        raiseAmount.text = "0";
        
    }

    //controls the percent raise that has been placed in the input feild is within the limits
    public void raiseChange(string amount)
    {
        if (amount.Length == 0)
        {
            return;
        }
        int temp = System.Int32.Parse(amount);
        if (temp > 100)
        {
            temp = 100;
        }

        if (temp == 0 && amount.Length > 1)
        {
            temp = 0;
        }
        raiseAmount.text = temp.ToString();
        raiseFor = temp;
    }
    
    //update when the employees workstation has been changed
    public void changeWorkStation(int station)
    {
        if (station == business.employeesInfo[place].workingIn)
        {
            return;
        }
        //change the color of the staion that he is working in now
        titles[station].colorGradientPreset = highlitedColor;
        titles[business.employeesInfo[place].workingIn].colorGradientPreset = regularColor;
        
        //set the employees new workstation and make the button interactable - false if there is no more space in that work station
        business.employeesInfo[place].workingIn = station;
        if (station != 3)
        {
            if (business.workStations[station].EmployeeSpace != -1)
            {
                business.workStations[station].EmployeeSpace = business.workStations[station].EmployeeSpace - 1;
                if (business.workStations[station].EmployeeSpace == 0)
                {
                    buttons[station].interactable = false;
                }
            }

            if (business.workStations[business.employeesInfo[place].wasWorkingIn].EmployeeSpace != -1)
            {
                if (business.workStations[business.employeesInfo[place].wasWorkingIn].EmployeeSpace == 0)
                {
                    buttons[business.employeesInfo[place].wasWorkingIn].interactable = true;
                }
                business.workStations[business.employeesInfo[place].wasWorkingIn].EmployeeSpace = business.workStations[business.employeesInfo[place].wasWorkingIn].EmployeeSpace + 1;
            }
            business.employeesInfo[place].wasWorkingIn = station;
        }
        employeeText[3].text = business.workStations[business.employeesInfo[place].wasWorkingIn].name;

        //let the managerGame know that the employee has been moved
        managerGame.changeEmployeeStation(place, station);
    }

    //return the percent of shop/training that has been finished for the selected employee
    private int getPercentDone(int station, bool focus)
    {
        if (focus == true)
        {
            float temp = business.employeesInfo[place].focusMultiplyer[station] - 1;
            if (station == 3)
            {
                return (int)Mathf.Floor((temp / 1F) * 100);
            }
            else
            {
                return (int)Mathf.Floor((temp / 3F) * 100);
            }
        }
        else
        {
            float temp = business.employeesInfo[place].timeMultiplyer[station] - 1;
            if (station == 3)
            {
                return (int)Mathf.Floor((temp / 1F) * 100);
            }
            else
            {
                return (int)Mathf.Floor((temp / 3F) * 100);
            }
        }
    }

    //return the string for the employee wage
    private string getWage(int wage)
    {
        //set up the string display for the wage
        var temp1 = Mathf.Floor(wage / 100);
        var temp2 = (wage % 10);
        var temp3 = (((wage - temp2) / 10) % 10);
        return "$" + temp1 + "." + temp3 + temp2;
    }

    //create a string to represent the time that an employee has been working 
    private string getTimeWorking(int time)
    {
        int tempYear = (int)Mathf.Floor(time / 8766);
        int temphours = time % 8766;
        int tempDays = (int)Mathf.Floor(temphours / 24);
        return tempYear + "y" + tempDays + "d";
    }
}
