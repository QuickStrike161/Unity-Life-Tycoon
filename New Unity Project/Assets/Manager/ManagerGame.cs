using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerGame : MonoBehaviour {

    /*
     * this is the main control for the second stage of the game, it controls the workstations,
     * and the work that employees do within the stations
     */ 

    public GameObject[] contentList;
    public RectTransform[] topControl;
    public Image employeeVisual;
    public PlayerInfo player;
    public GameObject employeeBackGround;
    public EmployeeControl employeeControl;
    public MoreInfoControl moreInfoControl;
    public TMP_Text[] workTitles;
    public TMP_Text[] workInfo;
    public TMP_Text[] businessInfo;
    public TMP_Dropdown runDropdown;
    public GameObject trainingMenu;
    public GameObject trainingMenuEmployee;
    public GameObject shopMenu;
    public GameObject shopMenuEmployee;
    public GameObject menu;
    public GameObject runMenu;
    public GameObject moreInfoBackground;
    public MainControl mainControl;

    private int currentlySelected;
    private int needsCustomers;
    private bool maintain;
    private bool timeAm_Pm;
    private float timeSec;
    private float timeAmount;
    private Business business;
    private GameObject employeeTempate;
    private List<Image> buttons = new List<Image> {};
    // Use this for initialization
    void Start () {
        business = player.business;

        //recreate the employees from the saved data
        for (short x = 0; x < business.employeesInfo.Count; x++)
        {
            newEmployee(business.employeesInfo[x].name, x, business.employeesInfo[x].workingIn);
        }
        currentlySelected = -1;

        //set up the diffrent displayes
        businessInfo[0].SetText(business.servingName);
        businessInfo[1].SetText(business.customerServed.ToString());
        businessInfo[2].SetText(getWage(player.playerEmployee.wage));
        businessInfo[3].SetText(business.happiness + "%");
        businessInfo[4].SetText(player.percentEffect + "%");
        for (short x = 0; x < 4; x++)
        {
            business.workStations[x].progressText = workInfo[x];
            upDateAmount(x,null);
            if (business.workStations[x].type == 2 || business.workStations[x].type == 3)
            {
                needsCustomers = x;
            }
        }
        upDateTitles();

        //set up the work background
        runDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int x = 0; x < 4; x++)
        {
            options.Add(business.workStations[x].name);
        }
        runDropdown.AddOptions(options);
        runDropdown.value = player.playerEmployee.workingIn;
        runDropdown.RefreshShownValue();
        businessInfo[5].SetText("0:00 AM");
        timeAm_Pm = false;
        timeAmount = 0;
        
        //for the menuIteam's make a connection between the ingredients and the ingredients that the menuIteams need
        foreach (menuIteams menuIteam in business.businessList[0].menuIteams)
        {
            menuIteam.ingredientsList = new ingredients[menuIteam.ingredients.Length];
            for(short x = 0; x < menuIteam.ingredients.Length; x++)
            {
                foreach (ingredients ingredient in business.ingredientList)
                {
                    if (ingredient.name == menuIteam.ingredients[x])
                    {
                        menuIteam.ingredientsList[x] = ingredient;
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //this controles the time diplay as well as ever 24 seconds updates the add customers funtion
        float tempTime = Time.fixedTime % 12;
        int tempHours = (int)Mathf.Floor(tempTime);
        int tempSec = (int)Mathf.Floor((tempTime - tempHours) * 60);
        tempHours = tempHours + 1;
        if (tempTime < timeAmount)
        {
            if (timeAm_Pm == true)
            {
                timeAm_Pm = false;
            }
            else
            {
                timeAm_Pm = true;
                this.addCustomers(business.workStations[needsCustomers]);
            }
        }
        string temp = "AM";
        if (timeAm_Pm == true)
        {
            temp = "AM";
        }
        else
        {
            temp = "PM";
        }
        string tempSecTime = "00";
        if (tempSec < 10)
        {
            tempSecTime = "0" + tempSec.ToString();
        }
        else
        {
            tempSecTime = tempSec.ToString();
        }
        businessInfo[5].SetText(tempHours + ":" + tempSecTime + temp);
        businessInfo[6].SetText(tempHours + ":" + tempSecTime + temp);
        timeAmount = tempTime;

        //create a loop that is called ever secound
        tempTime = Time.fixedTime % 1;
        if (tempTime < timeSec)
        {
            businessInfo[2].SetText(getWage(player.playerEmployee.wage));
            employeeEverSecond();
        }
        timeSec = tempTime;

        //run the players work if there are less than 10 employees
        if (business.employeesInfo.Count < 10)
        {

        }
        
        //run the employees
        for (short x = 0; x < business.employeesInfo.Count; x++)
        {
            updateStations(business.employeesInfo[x].workingIn, business.workStations[business.employeesInfo[x].workingIn], business.employeesInfo[x], player.decreaseAmount, x);
        }
        //run any maintain workstations 
        maintainBusiness();

    }

    //run any maintain workStaions 
    private void maintainBusiness()
    {
        for (short x = 0; x < 4; x++)
        {
            //int tempFocus;
            int tempNumber;
            float tempProgress;
            workStation useThis = business.workStations[x];
            switch (business.workStations[x].type)
            {
                case 4:
                    //calculate progress since last update and add to the exsisting progress
                    tempProgress = (business.customerIncrease * Time.deltaTime) / useThis.timeMain;
                    tempProgress = useThis.orders[0].progress + tempProgress;

                    //the store is fully dirty or unmaintained
                    if (useThis.orders[0].progress == 1 && useThis.orders[0].wants[0] == 100)
                    {
                        return;
                    }

                    if (tempProgress >= 1)
                    {
                        // makes a new number to be the amount of times the bar was filled
                        tempNumber = (int)Mathf.Floor(tempProgress);

                        //make new variable to hold the progress less than 1
                        tempProgress = tempProgress - tempNumber;

                        //if the amount if full stop at full
                        if (useThis.orders[0].wants[0] == 100)
                        {
                            tempProgress = 1;
                        }
                        else
                        {
                            //add the amount and update happiness
                            useThis.orders[0].wants[0] = useThis.orders[0].wants[0] + tempNumber;
                            business.negativeHappiness[x] = useThis.orders[0].wants[0];
                            if (business.negativeHappiness[x] > 50)
                            {
                                business.negativeHappiness[x] = 50;
                            }
                            updateHappiness();
                        }
                        upDateAmount(x, null);
                    }

                    //update the displays of the employees that are working in this staion 
                    foreach(employee tempEmployee in business.employeesInfo)
                    {
                        if(tempEmployee.workingIn == x)
                        {
                            tempEmployee.task.progress = tempProgress;
                            upDateAmount(x, tempEmployee);
                        }
                    }
                    useThis.orders[0].progress = tempProgress;
                    break;
            }
        }
    }

    //update the employees every second for there wage,time working
    public void employeeEverSecond()
    {
        for (short x = 0; x < business.employeesInfo.Count; x++)
        {
            //take the employees wage from the players money
            mainControl.spendMoney(business.employeesInfo[x].wage);
            //update the employees time working
            business.employeesInfo[x].timeWorking = business.employeesInfo[x].timeWorking + 1;
            if (business.employeesInfo[x].timeTillUnhappiness == 0)
            {
                business.employeesInfo[x].happiness = business.employeesInfo[x].happiness - 1;
                upDateEmployeeVisual(business.employeesInfo[x], true);
                if (business.employeesInfo[x].wage >= (business.wage * 3) - business.wage / 10)
                {
                    business.employeesInfo[x].timeTillUnhappiness = 8760;
                }
                else
                {
                    float temp = 1 - (business.employeesInfo[x].wage / (business.wage * 3F));
                    business.employeesInfo[x].timeTillUnhappiness = (int)Mathf.Ceil(240/temp);
                }
            }
            business.employeesInfo[x].timeTillUnhappiness = business.employeesInfo[x].timeTillUnhappiness - 1;
        }
    }

    //set the size of the employee template so that it fits nicely in the lists
    public void changeEmployeeSize(int place)
    {
        if (place == -1)
        {
            for(short x = 0; x < buttons.Count; x++)
            {
                buttons[x].rectTransform.sizeDelta = new Vector2(topControl[business.employeesInfo[x].workingIn].rect.width, 65);
            }
        }
        else
        {
            buttons[place].rectTransform.sizeDelta = new Vector2(topControl[business.employeesInfo[place].workingIn].rect.width, 65);
        }
    }

    //change the station that an employee is working in 
    public void changeEmployeeStation(int place, int station)
    {
        buttons[place].transform.SetParent(contentList[station].transform, false);
        changeEmployeeSize(place);
        upDateTitles();
    }

    //update the information at the bottom of the lists depending on the station type
    private void upDateAmount(int place, employee employee)
    {
        //update the display
        if (employee == null)
        {
            if (business.workStations[place].type < 2)
            {
                business.workStations[place].progressText.SetText(business.workStations[place].workName + " " + business.workStations[place].orders[0].wants[1]);
            }
            else if (business.workStations[place].type < 4)
            {
                business.workStations[place].progressText.SetText(business.workStations[place].workName + " " + business.workStations[place].orders.Count);
            }
            else if (business.workStations[place].type == 7)
            {
                business.workStations[place].progressText.SetText("Training Info");
            }
            else
            {
                business.workStations[place].progressText.SetText(business.workStations[place].workName + " " + business.workStations[place].orders[0].wants[0] + "%");
            }
        }
        else
        {
            if (business.workStations[place].type < 2)
            {
                business.workStations[place].progressText.SetText(business.workStations[place].workName + " " + business.workStations[place].orders[0].wants[1]);
            }
            else if (business.workStations[place].type < 4)
            {
                business.workStations[place].progressText.SetText(business.workStations[place].workName + " " + business.workStations[place].orders.Count);
            }
            else if (business.workStations[place].type == 7)
            {
                business.workStations[place].progressText.SetText("Training Info");
            }
            else
            {
                business.workStations[place].progressText.SetText(business.workStations[place].workName + " " + business.workStations[place].orders[0].wants[0] + "%");
            }
            upDateEmployeeVisual(employee, true);
        }
    }

    //create or recreate an employee's visual and add them into the game 
    public void newEmployee(string name, int place, int location)
    {
        //if the employee has not been created before
        if (place == -1)
        {
            //if there is no name selected create a name
            string nameFor = name;
            if (name == "")
            {
                nameFor = mainControl.getName();
            }
            business.employeesInfo.Add(new employee(nameFor, location, business.wage));
            place = business.employeesInfo.Count - 1;

            //update the workstation space
            if (business.workStations[location].EmployeeSpace != -1)
            {
                business.workStations[location].EmployeeSpace = business.workStations[location].EmployeeSpace - 1;
            }
        }

        //set up the images
        Image button = Instantiate(employeeVisual) as Image;
        buttons.Add(button);
        button.gameObject.SetActive(true);
        button.GetComponent<EmployeeTemplate>().setUp(business, place);
        button.transform.SetParent(contentList[location].transform, false);
        upDateEmployeeVisual(business.employeesInfo[place], true);
        upDateTitles();
        changeEmployeeSize(place);
    }

    //select an employee and open the employee menu
    public void selectEmployee(int place)
    {
        currentlySelected = place;
        employeeBackGround.SetActive(true);
        employeeBackGround.GetComponent<EmployeeControl>().setUp();
        runMenu.SetActive(true);
    }

    //unselect an employee and close the employee menu
    public void backFromSelectEmployee()
    {
        if (currentlySelected == -1)
        {
            moreInfoSelect(-1);
        }
        else
        {
            currentlySelected = -1;
            employeeBackGround.SetActive(false);
            runMenu.SetActive(false);
            shopMenuEmployee.SetActive(false);
            trainingMenuEmployee.SetActive(false);
        }
    }

    //return the employee that is currently selected
    public int getSelectedEmployee()
    {
        return currentlySelected;
    }

    //update the employee visual within the lists
    public void upDateEmployeeVisual(employee employee, bool all)
    {
        if (all == true)
        {
            employee.nameText.text = employee.name;
            employee.infoText.text = getWage(employee.wage) + " : " + employee.happiness + "%";
        }
        if (employee.task != null)
        {
            employee.slider.value = employee.task.progress;
        }
        else
        {
            employee.slider.value = 0;
        }
    }

    //update the titles for the workstations
    private void upDateTitles()
    {
        for (short x = 0; x < 4; x++)
        {
            if (business.workStations[x].EmployeeSpace == -1)
            {
                workTitles[x].text = business.workStations[x].name + " " + contentList[x].transform.childCount;
            }
            else
            {
                workTitles[x].text = business.workStations[x].name + " " + contentList[x].transform.childCount + "/" + business.workStations[x].spaceDefault;
            }
        }
    }

    //return the string for a wage
    private string getWage(int wage)
    {
        //set up the string display for the wage
        var temp1 = Mathf.Floor(wage / 100);
        var temp2 = (wage % 10);
        var temp3 = (((wage - temp2) / 10) % 10);
        return "$" + temp1 + "." + temp3 + temp2;
    }

    //change the workstation that the player is running
    public void changeAreaOfWork(int workArea)
    {
        //update when the player stats working in a new section
        runDropdown.value = workArea;
        player.focus[player.playerEmployee.workingIn] = player.focus[player.playerEmployee.workingIn] - 10;
        player.focus[workArea] = player.focus[workArea] + 10;
        if (workArea == 3)
        {
            player.playerEmployee.workingIn = workArea;
        }
        else
        {
            player.playerEmployee.workingIn = workArea;
            player.playerEmployee.wasWorkingIn = workArea;
        }
    }
    
    //toggle the training menues
    public void training()
    {
        //toggle the training background
        if (currentlySelected == -1)
        {
            if (trainingMenu.activeInHierarchy == true)
            {
                trainingMenu.SetActive(false);
            }
            else
            {
                moreInfoBackground.SetActive(false);
                trainingMenu.SetActive(true);
            }
        }
        else
        {
            if (trainingMenuEmployee.activeInHierarchy == true)
            {
                shopMenuEmployee.SetActive(false);
                trainingMenuEmployee.SetActive(false);
                employeeControl.setUp();
            }
            else
            {
                shopMenuEmployee.SetActive(false);
                trainingMenuEmployee.SetActive(true);
                trainingMenuEmployee.GetComponent<TrainingEmployee>().setUp(currentlySelected);
            }
        }
    }

    //toggle the shoping menues
    public void shoping()
    {
        //toggle the shoping background
        if (currentlySelected == -1)
        {
            if (shopMenu.activeInHierarchy == true)
            {
                shopMenu.SetActive(false);
            }
            else
            {
                moreInfoBackground.SetActive(false);
                shopMenu.SetActive(true);
            }
        }
        else
        {
            if (shopMenuEmployee.activeInHierarchy == true)
            {
                shopMenuEmployee.SetActive(false);
                trainingMenuEmployee.SetActive(false);
                employeeControl.setUp();
            }
            else
            {
                shopMenuEmployee.SetActive(true);
                trainingMenuEmployee.SetActive(false);
                shopMenuEmployee.GetComponent<ShopEmployee>().setUp(currentlySelected);
            }
        }
    }

    //toggle the menu
    public void menuChange()
    {
        //toggle the menu background
        if (menu.activeInHierarchy == true)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }

    //update happiness and add customers to the workstation that works with customers
    private void addCustomers(workStation useThis)
    {
        //if happiness is greater than 90% increase the amount of customers that come per day/ if happiness is lower that 90 decrease
        if (business.happiness > 90)
        {
            int temp = (int)(business.happiness) - 90;
            business.customerIncrease = business.customerIncrease * (1 + (((float)temp / 100) * player.level));
        }
        else if (business.happiness < 90)
        {
            int temp = Mathf.Abs((int)(business.happiness) - 90);
            business.customerIncrease = business.customerIncrease * (1 - (((float)temp / 100) * player.level));
            if (business.customerIncrease < 2)
            {
                business.customerIncrease = 2;
            }
        }

        //update the hapiness based on customers waiting
        int percent = (int)Mathf.Floor(useThis.orders.Count / 20);
        if (percent < 1)
        {
            business.negativeHappiness[needsCustomers] = business.negativeHappiness[needsCustomers] - 1;
            if (business.negativeHappiness[needsCustomers] < 0)
            {
                business.negativeHappiness[needsCustomers] = 0;
            }
        }
        else
        {
            business.negativeHappiness[needsCustomers] = business.negativeHappiness[needsCustomers] + percent;
            if (business.negativeHappiness[needsCustomers] > 50)
            {
                business.negativeHappiness[needsCustomers] = 50;
            }
        }
        updateHappiness();

        int tempCustomers = (int)Mathf.Round(business.customerIncrease);
        //update the customers waiting
        for (int x = 0; x < tempCustomers; x++)
        {
            int[] want = new int[] { 0 };
            int ID = Random.Range(0, 3);
            wants want1 = new wants(ID);
            want1.GetWant();
            order order = new order(mainControl.getName(), want1, false);
            useThis.orders.Add(order);
        }
        upDateAmount(needsCustomers, null);
    }

    //update the happiness of the business and happiness diplay
    private void updateHappiness()
    {
        //calculate the current happiness of the business and change the display
        int amount = 0;
        for (short x = 1; x < 5; x++)
        {
            amount = amount + business.negativeHappiness[x];
        }
        if (amount > 100)
        {
            amount = 100;
        }
        business.happiness = 100 - amount;
        businessInfo[3].SetText(business.happiness + "%");
    }
    
    //run the work stations for each employee - needs work
    private void updateStations(int station, workStation useThis, employee employee, float decreaseAmount, int place)
    {
        int tempFocus;
        int tempNumber;
        float tempProgress;
        switch (useThis.type)
        {
            case 0:
                /*
                //creates the amount of progress in the last update taking into account the focus multiplyer
                float tempProgress0 = (focus * employee.focusMultiplyer[place] * Time.deltaTime * (player.skillPercent[0]/100F) * decreaseAmount) / (useThis.timeMain / employee.timeMultiplyer[place]);

                //adds the progresses together to get the current progress
                tempProgress0 = tempProgress0 + employee.progress;

                if (tempProgress0 >= 1)
                {
                    // makes a new number to be the amount of times the bar was filled
                    int tempNumber0 = (int)Mathf.Floor(tempProgress0);

                    //make new variable to hold the progress less than 1
                    tempProgress0 = tempProgress0 - tempNumber0;
                    useThis.amountWaiting = useThis.amountWaiting + tempNumber0;
                    upDateAmount(place, employee);
                }
                employee.slider.value = tempProgress0;
                employee.progress = tempProgress0;
                break;
            case 1:
                //if there are no supply and progress
                if (employee.progress == 0 && business.workStations[useThis.extra[0]].amountWaiting == 0)
                {
                    return;
                }

                //creates the amount of progress in the last update taking into account the focus multiplyer
                float tempProgress1 = (focus * employee.focusMultiplyer[place] * Time.deltaTime * (player.skillPercent[0] / 100F) * decreaseAmount) / (useThis.timeMain / employee.timeMultiplyer[place]);

                //adds the progresses together to get the current progress
                tempProgress1 = tempProgress1 + employee.progress;

                //take away one used object since the bar has no progress
                if (employee.progress == 0)
                {
                    business.workStations[useThis.extra[0]].amountWaiting = business.workStations[useThis.extra[0]].amountWaiting - 1;
                }

                //if the progress is greater than one
                if (tempProgress1 >= 1)
                {
                    // makes a new number to be the amount of times the bar was filled
                    int tempNumber1 = (int)Mathf.Floor(tempProgress1);

                    //make new variable to hold the progress less than 1
                    tempProgress1 = tempProgress1 - tempNumber1;
                    int amountToUse1;
                    //if it went though more times then there are used amount
                    if (tempNumber1 > business.workStations[useThis.extra[0]].amountWaiting)
                    {
                        amountToUse1 = business.workStations[useThis.extra[0]].amountWaiting + 1;
                        business.workStations[useThis.extra[0]].amountWaiting = business.workStations[useThis.extra[0]].amountWaiting - (amountToUse1 - 1);
                        tempProgress1 = 0;
                    }
                    //if the amount it went though was the correct amount
                    else
                    {
                        amountToUse1 = tempNumber1;
                        business.workStations[useThis.extra[0]].amountWaiting = business.workStations[useThis.extra[0]].amountWaiting - amountToUse1;
                    }
                    useThis.amountWaiting = useThis.amountWaiting + amountToUse1;
                }
                employee.slider.value = tempProgress1;
                employee.progress = tempProgress1;
                upDateAmount(place,employee);
                upDateAmount(useThis.extra[0],employee);
                */
                break;
            case 2:
                /*
                //if there are no supply and progress
                if (employee.progress == 0 && business.workStations[useThis.extra[0]].amountWaiting == 0)
                {
                    return;
                }

                //if there are no customers and progress
                if (employee.progress == 0 && useThis.amountWaiting == 0)
                {
                    return;
                }

                //creates the amount of progress in the last update taking into account the focus multiplyer
                float tempProgress2 = (focus * employee.focusMultiplyer[place] * Time.deltaTime * (player.skillPercent[0] / 100F) * decreaseAmount) / (useThis.timeMain / employee.timeMultiplyer[place]);

                //adds the progresses together to get the current progress
                tempProgress2 = tempProgress2 + employee.progress;

                //take away one of the customers and used object since the bar has no progress
                if (employee.progress == 0)
                {
                    business.workStations[useThis.extra[0]].amountWaiting = business.workStations[useThis.extra[0]].amountWaiting - 1;
                    useThis.amountWaiting = useThis.amountWaiting - 1;
                }

                //if the progress is greater than one
                if (tempProgress2 >= 1)
                {
                    // makes a new number to be the amount of times the bar was filled
                    int tempNumber2 = (int)Mathf.Floor(tempProgress2);

                    //make new variable to hold the progress less than 1
                    tempProgress2 = tempProgress2 - tempNumber2;
                    int amountToUse2;
                    //if it went though more times then there are customers
                    if (tempNumber2 > useThis.amountWaiting)
                    {
                        amountToUse2 = useThis.amountWaiting;
                        addToCustomers(amountToUse2 + 1);
                        tempProgress2 = 0;
                    }
                    //if it went though more times then there are used amount
                    else if (tempNumber2 > business.workStations[useThis.extra[0]].amountWaiting)
                    {
                        amountToUse2 = business.workStations[useThis.extra[0]].amountWaiting;
                        addToCustomers(amountToUse2 + 1);
                        tempProgress2 = 0;
                    }
                    //if the amount it went though was the correct amount
                    else
                    {
                        amountToUse2 = tempNumber2;
                        addToCustomers(tempNumber2);
                    }
                    useThis.amountWaiting = useThis.amountWaiting - amountToUse2;
                    business.workStations[useThis.extra[0]].amountWaiting = business.workStations[useThis.extra[0]].amountWaiting - amountToUse2;
                }
                employee.slider.value = tempProgress2;
                employee.progress = tempProgress2;
                upDateAmount(place,employee);
                upDateAmount(useThis.extra[0],employee);
                */
                break;
            case 3:
                /*
                //if there are no customers and progress
                if (useThis.progress == 0 && useThis.amountWaiting == 0)
                {
                    return;
                }

                //creates the amount of progress in the last update taking into account the focus multiplyer
                float tempProgress3 = (focus * employee.focusMultiplyer[place] * Time.deltaTime * (player.skillPercent[0] / 100F) * decreaseAmount) / (useThis.timeMain / employee.timeMultiplyer[place]);

                //adds the progresses together to get the current progress
                tempProgress3 = tempProgress3 + employee.progress;

                //take away one of the customers and used object since the bar has no progress
                if (employee.progress == 0)
                {
                    useThis.amountWaiting = useThis.amountWaiting - 1;
                }

                //if the progress is greater than one
                if (tempProgress3 >= 1)
                {
                    // makes a new number to be the amount of times the bar was filled
                    int tempNumber3 = (int)Mathf.Floor(tempProgress3);

                    //make new variable to hold the progress less than 1
                    tempProgress3 = tempProgress3 - tempNumber3;
                    int amountToUse3;
                    //if it went though more times then there are customers
                    if (tempNumber3 > useThis.amountWaiting)
                    {
                        amountToUse3 = useThis.amountWaiting;
                        addToCustomers(amountToUse3 + 1);
                        tempProgress3 = 0;
                    }
                    //if the amount it went though was the correct amount
                    else
                    {
                        amountToUse3 = tempNumber3;
                        addToCustomers(tempNumber3);
                    }
                    useThis.amountWaiting = useThis.amountWaiting - amountToUse3;
                }
                employee.slider.value = tempProgress3;
                employee.progress = tempProgress3;
                upDateAmount(place,employee);
                */
                break;
            case 4:
                if (employee.task == null)
                {
                    employee.task = useThis.orders[0];
                }
                else if (employee.task.name != useThis.orders[0].name)
                {
                    employee.task = useThis.orders[0];
                }

                //set the amount of focus that the employee has
                tempFocus = employee.focus;
                
                //creates the amount of progress in the last update taking into account the focus multiplyer
                tempProgress = ((tempFocus * employee.focusMultiplyer[station] * Time.deltaTime) / (useThis.timeMain / employee.timeMultiplyer[station])) * decreaseAmount * (player.skillPercent[0] / 100F);
                tempProgress = tempProgress * getWorkForHappiness(employee);
                if (player.playerEmployee.workingIn == station)
                {
                    tempProgress = tempProgress * ((100 + player.percentEffect) / 100F);
                }
                //adds the progresses together to get the current progress
                tempProgress = useThis.orders[0].progress - tempProgress;

                //the store is fully clean or maintained
                if (useThis.orders[0].progress == 0 && useThis.orders[0].wants[0] == 0)
                {
                    return;
                }

                if (tempProgress < 0)
                {
                    tempProgress = Mathf.Abs(tempProgress);
                    // makes a new number to be the amount of times the bar was filled
                    tempNumber = (int)Mathf.Floor(tempProgress) + 1;

                    //make new variable to hold the progress less than 1
                    tempProgress = tempProgress % 1;
                    tempProgress = 1 - tempProgress;

                    //if the amount is full stop at full
                    if (useThis.orders[0].wants[0] == 0)
                    {
                        tempProgress = 0;
                    }
                    else
                    {
                        //subtract the amount and update happiness
                        useThis.orders[0].wants[0] = useThis.orders[0].wants[0] - tempNumber;
                        business.negativeHappiness[station] = useThis.orders[0].wants[0];
                        if (business.negativeHappiness[station] < 0)
                        {
                            business.negativeHappiness[station] = 0;
                        }
                        updateHappiness();
                    }
                }
                employee.task.progress = tempProgress;
                break;
            case 5:
                /*
                //calculate progress since last update and add to the exsisting progress
                float tempProgress5 = (business.customerIncrease * Time.deltaTime) / (useThis.extra[0] / player.timeMultiplyer[place]);
                tempProgress5 = useThis.progress - tempProgress5;
                int tempNumber5 = 0;

                if (useThis.running == true)
                {
                    //add focuse if it is being run by player
                    int tempFocus5 = useThis.focus;
                    if (business.isRunning == place)
                    {
                        tempFocus5 = tempFocus5 + 10;
                    }

                    //update the progress based on the amount of work that is being done
                    tempProgress5 = tempProgress5 + ((tempFocus5 * player.focusMultiplyer[place] * Time.deltaTime) / (useThis.timeMain / player.timeMultiplyer[place]));
                }

                //the store is fully stocked or maintained
                if (useThis.progress == 1 && useThis.amountWaiting == 100 && tempProgress5 < 0)
                {
                    return;
                }

                //the store is fully unstocked or unmaintained
                if (useThis.progress == 0 && useThis.amountWaiting == 0 && tempProgress5 > 0)
                {
                    return;
                }

                if (tempProgress5 >= 1)
                {
                    // makes a new number to be the amount of times the bar was filled
                    tempNumber5 = (int)Mathf.Floor(tempProgress5);

                    //make new variable to hold the progress less than 1
                    tempProgress5 = tempProgress5 - tempNumber5;

                    //if the amount is full stop at full
                    if (useThis.amountWaiting == 100)
                    {
                        tempProgress5 = 1;
                    }
                    else
                    {
                        //add the amount and update happiness
                        useThis.amountWaiting = useThis.amountWaiting + tempNumber5;
                        business.negativeHappiness[place] = 100 - useThis.amountWaiting;
                        if (business.negativeHappiness[place] < 0)
                        {
                            business.negativeHappiness[place] = 0;
                        }
                        updateHappiness();
                    }
                    upDateAmount(place);
                }

                if (tempProgress5 < 0)
                {
                    tempProgress5 = Mathf.Abs(tempProgress5);
                    // makes a new number to be the amount of times the bar was filled
                    tempNumber5 = (int)Mathf.Floor(tempProgress5) + 1;

                    //make new variable to hold the progress less than 1
                    tempProgress5 = tempProgress5 % 1;
                    tempProgress5 = 1 - tempProgress5;

                    //if the amount if full stop at full
                    if (useThis.amountWaiting == 0)
                    {
                        tempProgress5 = 0;
                    }
                    else
                    {
                        //subtract the amount and update happiness
                        useThis.amountWaiting = useThis.amountWaiting - tempNumber5;
                        business.negativeHappiness[place] = 100 - useThis.amountWaiting;
                        if (business.negativeHappiness[place] > 50)
                        {
                            business.negativeHappiness[place] = 50;
                        }
                        updateHappiness();
                    }
                    upDateAmount(place);
                }
                useThis.slider.value = tempProgress5;
                useThis.progress = tempProgress5;
                */
                break;
            case 6:
                /*
                //if the store is fully supplied/maintained
                if (useThis.progress == 1 && useThis.amountWaiting == 100)
                {
                    return;
                }

                //if there are no goods waiting
                if (useThis.extra[2] == 0 && business.workStations[useThis.extra[0]].amountWaiting == 0)
                {
                    return;
                }

                //add 10 to the focus amount if this workplace is being run
                int tempFocus6 = useThis.focus;
                if (business.isRunning == place)
                {
                    tempFocus6 = tempFocus6 + 10;
                }

                //creates the amount of progress in the last update taking into account the multiplyers
                float thisProgress6 = (tempFocus6 * player.focusMultiplyer[place] * Time.deltaTime) / (useThis.timeMain / player.timeMultiplyer[place]);
                //adds the progresses together to get the current progress
                float tempProgress6 = thisProgress6 + useThis.progress;
                int tempNumber6 = 0;

                if (thisProgress6 > (float)useThis.extra[2] / 1000000)
                {
                    tempProgress6 = thisProgress6 - ((float)useThis.extra[2] / 1000000);
                    // makes a new number to be the amount of times the bar was filled
                    tempNumber6 = (int)Mathf.Ceil(tempProgress6);

                    if (tempNumber6 > business.workStations[useThis.extra[0]].amountWaiting)
                    {
                        tempNumber6 = business.workStations[useThis.extra[0]].amountWaiting;
                        useThis.extra[2] = useThis.extra[2] + (1000000 * tempNumber6);
                        tempProgress6 = useThis.progress + ((float)useThis.extra[2] / 1000000F);
                        useThis.extra[2] = 0;
                    }
                    else
                    {
                        business.workStations[useThis.extra[0]].amountWaiting = business.workStations[useThis.extra[0]].amountWaiting - tempNumber6;
                        tempProgress6 = useThis.progress + thisProgress6;
                        useThis.extra[2] = (useThis.extra[2] + (1000000 * tempNumber6)) - (int)(thisProgress6 * 1000000F);
                    }
                }
                else
                {
                    useThis.extra[2] = useThis.extra[2] - (int)(thisProgress6 * 1000000);
                }

                if (tempProgress6 >= 1)
                {
                    // makes a new number to be the amount of times the bar was filled
                    tempNumber6 = (int)Mathf.Floor(tempProgress6);

                    //make new variable to hold the progress less than 1
                    tempProgress6 = tempProgress6 - tempNumber6;
                    useThis.amountWaiting = useThis.amountWaiting + tempNumber6;
                    setHappinessForUses(useThis.amountWaiting);
                    if (useThis.amountWaiting > 100)
                    {
                        useThis.extra[2] = useThis.extra[2] + (int)(tempProgress6 * 1000000F);
                        tempProgress6 = 1;
                        useThis.amountWaiting = 100;
                    }
                }
                useThis.slider.value = tempProgress6;
                useThis.progress = tempProgress6;
                upDateAmount(place);
                upDateAmount(useThis.extra[0]);
                */
                break;
            case 7:
                if (employee.trainingQ.Count == 0)
                {
                    return;
                }

                //set the amount of focus that the employee has
                tempFocus = employee.focus;

                //creates the amount of progress in the last update taking into account the focus multiplyer
                tempProgress = ((tempFocus * employee.focusMultiplyer[station] * Time.deltaTime) / (employee.trainingQ[0].wants[1] / employee.timeMultiplyer[station])) * decreaseAmount * (player.skillPercent[0] / 100F);
                //adds the progresses together to get the current progress
                tempProgress = tempProgress * getWorkForHappiness(employee);
                if (player.playerEmployee.workingIn == station)
                {
                    tempProgress = tempProgress * ((100 + player.percentEffect) / 100F);
                }
                tempProgress = tempProgress + employee.trainingQ[0].progress;

                if (tempProgress > 1)
                {
                    int iteam = employee.trainingQ[0].wants[0];
                    for (short x = 0; x < business.iteamList[iteam].affectAreas.Length; x++)
                    {
                        int tempInt = business.iteamList[iteam].affectAreas[x];
                        employee.timeMultiplyer[tempInt] = employee.timeMultiplyer[tempInt] + (business.iteamList[iteam].affectAmount / 100F);
                    }

                    employee.trainingQ.RemoveAt(0);
                    if (currentlySelected == -1)
                    {
                        //do nothing
                    }
                    else if (business.employeesInfo[currentlySelected] == employee)
                    {
                        trainingMenuEmployee.GetComponent<TrainingEmployee>().setUp(currentlySelected);
                    }
                    tempProgress = tempProgress - 1;
                }

                if (employee.trainingQ.Count != 0)
                {
                    employee.slider.value = tempProgress;
                    employee.trainingQ[0].progress = tempProgress;
                }
                else
                {
                    employee.slider.value = 0;
                    if (business.workStations[employee.wasWorkingIn].EmployeeSpace != 0)
                    {
                        
                        changeEmployeeStation(place, employee.wasWorkingIn);
                        employee.workingIn = employee.wasWorkingIn;
                    }
                }
                break;
        }
    }

    //this is run when a customer has been served updates the served amount and adds skill points to main control
    private void addToCustomers(int amount)
    {
        //add the amount of new customers to the amount already there
        business.customerServed = business.customerServed + amount;
        //update the display
        businessInfo[1].SetText(business.customerServed.ToString());

        //if there is a work station that takes good for every customer served update it with the amount of customers just served
        /*if (business.usesGoods != 0)
        {
            float tempProgress = business.workStations[business.usesGoods].progress - ((float)business.workStations[business.usesGoods].extra[1] / 100 * amount);

            if (tempProgress < 0)
            {
                tempProgress = Mathf.Abs(tempProgress);
                // makes a new number to be the amount of times the bar was filled
                int tempNumber = (int)Mathf.Floor(tempProgress) + 1;

                //make new variable to hold the progress less than 1
                tempProgress = tempProgress % 1;
                tempProgress = 1 - tempProgress;

                business.workStations[business.usesGoods].amountWaiting = business.workStations[business.usesGoods].amountWaiting - tempNumber;
                setHappinessForUses(business.workStations[business.usesGoods].amountWaiting);
            }
            upDateAmount(business.usesGoods);
            business.workStations[business.usesGoods].slider.value = tempProgress;
            business.workStations[business.usesGoods].progress = tempProgress;
        }*/

        //add points for skills to main control 
        mainControl.addToSkillsAmounts(amount, business.sector);
    }

    //toggle moreInfo menu
    public void moreInfoSelect(int station)
    {
        if (station == -1)
        {
            moreInfoBackground.SetActive(false);
            runMenu.SetActive(false);
        }
        else
        {
            moreInfoBackground.SetActive(true);
            runMenu.SetActive(true);
            moreInfoControl.setUp(station);
        }
    }

    //set the work of an employee based on there hapiness 
    public float getWorkForHappiness(employee employee)
    {
        float temp = employee.happiness + 10;
        if (temp > 100)
        {
            temp = 100;
        }
        return temp / 100F;
    }

    //extra code dont know if we will need
    /*

    private void setHappinessForUses(int amount)
    {
        //used only with type 6 sets the negative happiness for this station
        if (amount >= 90)
        {
            business.negativeHappiness[business.usesGoods] = 0;
        }
        else
        {
            business.negativeHappiness[business.usesGoods] = 90 - amount;
            if (business.negativeHappiness[business.usesGoods] > 50)
            {
                business.negativeHappiness[business.usesGoods] = 50;
            }
        }
        updateHappiness();
    }

    public void selectionChange(int workArea){
        //update when the player stats working in a new section
        RunDropdown.value = workArea;
        workArea = workArea + 1;
        business = player.business;
        if (business.workStations[business.isRunning].running == true){
            if (business.workStations[business.isRunning].focus == 0){
                business.workStations[business.isRunning].running = false;
            }
        }
        business.isRunning = workArea;
        business.workStations[workArea].running = true;
    }

    public void upDateWage(){
        //update the wage when changes occure
        BusinessInfo[2].SetText(getWage((business.wage * player.skillPercent[3] / 100)));
    }

    private string getWage(int wage){
        //set up the string display for the wage
        var temp1 = Mathf.Floor(wage/100);
        var temp2 = (wage % 10);
        var temp3 = (((wage - temp2) / 10) % 10);
        return "$" + temp1 + "." + temp3 + temp2;
    }

    public void training(){
        //toggle the training background
        if (trainingMenu.active == true){
            trainingMenu.SetActive(false);
        }
        else{
            trainingMenu.SetActive(true);
        }
    }

    public void shoping()
    {
        //toggle the shoping background
        if (shopMenu.active == true)
        {
            shopMenu.SetActive(false);
        }
        else
        {
            shopMenu.SetActive(true);
        }
    }

    public void menuChange()
    {
        //toggle the menu background
        if (menu.active == true)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }

    public void firstFocusChange(string text)
    {
        changeFocusFor(1, text);
    }

    public void secondFocusChange(string text)
    {
        changeFocusFor(2, text);
    }

    public void thirdFocusChange(string text)
    {
        changeFocusFor(3, text);
    }

    public void fourthFocusChange(string text)
    {
        changeFocusFor(4, text);
    }

    private void changeFocusFor(int place, string text){
        //if there is no text set the amount to 0
        if (text.Length == 0){
            business.workStations[place].focuseInput.text = "0";
        }
        else{
            int temp = System.Int32.Parse(text);
            business.workStations[place].focus = 0;
            int amount = getFocusA();
            //if the amount is negative change it to positive
            if (temp < 0){
                temp = Mathf.Abs(temp);
            }
            //if the amount is multiple zeros change the amount to a single 0
            if (temp == 0){
                business.workStations[place].focuseInput.text = "0";
            }
            //if the amount inputed is more that the amount availible change the value to amount availible
            else if (temp > amount){
                business.workStations[place].focuseInput.text = amount.ToString();
                business.workStations[place].focus = amount;
            }
            //set the amount to the input text
            else{
                business.workStations[place].focuseInput.text = temp.ToString();
                business.workStations[place].focus = temp;
            }
        }

        if (business.workStations[place].focus == 0){
            if (business.isRunning != place){
                business.workStations[place].running = false;
            }
            else{
                business.workStations[place].running = true;
            }
        }
        else{
            business.workStations[place].running = true;
        }
    }

    private int getFocusA(){
        //return the focus that is availible
        int temp = business.workStations[1].focus + business.workStations[2].focus + business.workStations[3].focus + business.workStations[4].focus;
        return business.focusT - temp;
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void SaveGame(){

    }
    */
}
