using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Business", menuName = "Business/Business")]
public class Business : ScriptableObject
{

    /*
     * This is the main object that we will be using, I want to create lots of diffrent businesses that will
     * make up the towns and cities. This containes the information that you need to know about the business
     */

    public new string name;
    public string servingName;
    public int sector;
    public int customerServed = 0;
    public int customerNeeded;
    public int wage;
    public int happiness = 100;
    public int storage;
    public int storageDefault;
    public int type;
    public int menuIteamsAmount;
    public int menuIteamsAmountDefault;
    public int[] negativeHappiness = new int[] {0,0,0,0,0};
    public int[] wageIncrease = new int[2];
    public float customerIncrease;
    public float customerIncreaseDefault;
    public businessTypes businessType;
    public businessTypes[] businessList;
    public workStation[] workStations = new workStation[4];
    public List <employee> employeesInfo = new List<employee> {};
    public shopIteam[] iteamList;
    public trainingIteam[] trainingList;
    public List <ingredients> ingredientList;

    public void reset() {
        this.customerServed = 0;
        this.happiness = 100;
        this.storage = this.storageDefault;
        this.negativeHappiness = new int[] { 0, 0, 0, 0, 0 };
        this.wageIncrease[0] = this.wageIncrease[1];
        this.customerIncrease = this.customerIncreaseDefault;
        this.employeesInfo = new List<employee> {};
        this.menuIteamsAmount = this.menuIteamsAmountDefault;

        for (short x = 0; x < businessList.Length; x++)
        {
            businessList[x].reset();
        }

        for (short x = 0; x < workStations.Length; x++)
        {
            workStations[x].reset();
        }
    }
}

//create a varriety of types of businesses within the area so for FastFood you could have MacDonalds/KFC/DairyQueen...
[System.Serializable]
public class businessTypes
{
    public string name;
    public int percentIntrest;
    public int percentIntrestDefault;
    public menuIteams[] menuIteams;

    public void reset()
    {
        this.percentIntrest = this.percentIntrestDefault;
        for (short x = 0; x < menuIteams.Length; x++)
        {
            menuIteams[x].reset();
        }
    }
}

//create the menuIteams for each business type
[System.Serializable]
public class menuIteams
{
    public string name;
    public string[] ingredients;
    public int percentIntrest;
    public int percentIntrestDefault;
    public int price;
    public int time;
    public int type;
    public ingredients[] ingredientsList;

    public void reset()
    {
        this.percentIntrest = this.percentIntrestDefault;
        for (short x = 0; x < ingredientsList.Length; x++)
        {
            ingredientsList[x].reset();
        }
    }
}

//create the ingredients that are needed for each menuIteam, this list is stored within the business and the menuIteams referance locations
[System.Serializable]
public class ingredients
{
    public string name;
    public int price;
    public float stored;
    public float amountPerUse;

    public void reset()
    {
        this.stored = 0;
    }
}

//create the information needed about each employee working within the business
[System.Serializable]
public class employee
{
    public string name;
    public float[] focusMultiplyer = new float[] {1, 1, 1, 1 };
    public float[] timeMultiplyer = new float[] {1, 1, 1, 1 };
    public int wage;
    public int workingIn;
    public int wasWorkingIn;
    public int happiness = 100;
    public int timeWorking = 0;
    public int timeTillUnhappiness = 360;
    public int focus = 10;
    public int multiplyerFocus;
    public List <int> shopIteamsAvailible;
    public List <int> trainingAvailible;
    public List <order> trainingQ;
    public order task;
    public TMP_Text infoText;
    public TMP_Text nameText;
    public Slider slider;

    public employee(string name, int location, int wage)
    {
        this.name = name;
        this.workingIn = location;
        this.wasWorkingIn = location;
        this.wage = wage;
        this.multiplyerFocus = -1;
        this.task = null;
        shopIteamsAvailible = new List<int> { };
        trainingAvailible = new List<int> { };
        trainingQ = new List<order> { };
    }
}

//this adds variety to the businesses allowing them to pick from the workStation types depening on the business
[System.Serializable]
public class workStation
{
    public string name;
    public string workName;
    public int type;
    public int timeMain;
    public int EmployeeSpace;
    public int spaceDefault;
    public int[] extra;
    public List <order> orders = new List<order> {};
    public Slider slider;
    public TMP_Text titleText;
    public TMP_Text progressText;
    public TMP_InputField focuseInput;

    public void reset()
    {
        this.EmployeeSpace = this.spaceDefault;
        this.orders = new List<order> {};
        if (this.type < 2)
        {
            int[] temp = new int[2] {0,0};
            order order = new order(this.workName, temp, false);
            this.orders.Add(order);
        }
        else if (this.type < 4)
        {
            //this.orders = new order[0];
        }
        else
        {
            int[] temp;
            if (this.type == 4)
            {
                temp = new int[1] {0};
            }
            else if (this.type == 7)
            {
                return;
            }
            else
            {
                temp = new int[1] {100};
            }
            order tempOrder = new order(this.workName, temp,false);
            this.orders.Add(tempOrder);
            if (this.type != 4)
            {
                this.orders[0].progress = 1;
            }
        }
    }
}

//what the cusomer wants from the workStation or what the workStation contains 
[System.Serializable]
public class order
{
    public string name;
    public int[] wants;
    public float progress;
    public bool taken;

    public order(string name, int[] wants, bool taken)
    {
        this.name = name;
        this.wants = wants;
        this.progress = 0;
        this.taken = taken;
    }
}

//iteams used in the shop menu's makes the player/employees faster
[System.Serializable]
public class trainingIteam
{
    public string name;
    public string description;
    public bool active;
    public int cost;
    public int affectAmount;
    public int follower = -1;
    public int[] affectAreas;
}

//training used in the training menu's makes the player/employees faster
[System.Serializable]
public class shopIteam
{
    public string name;
    public string description;
    public bool active;
    public int cost;
    public int affectAmount;
    public int follower = -1;
    public int[] affectAreas;
}