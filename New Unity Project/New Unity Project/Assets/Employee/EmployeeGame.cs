using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmployeeGame : MonoBehaviour {

    /*
     * this is the main control for the first stage of the game. Controls the 4 work stations that will be used
     * by the player in this stage of the game. Some of the workStations(updateStations) need to be created
     */

    //create variables to hold the diffrent diplays
    public TMP_Text[] BusinessInfo;
    public TMP_Text[] titles;
    public TMP_Text[] subInfo;
    public TMP_Dropdown RunDropdown;
    public TMP_InputField[] FocusInput;
    public Slider[] Sliders;

    public GameObject trainingMenu;
    public GameObject shopMenu;
    public GameObject menu;
    public TrainingStage1 trainingCS;
    public MainControl mainControl;
    public PlayerInfo player;

    private bool timeAm_Pm;
    private int needsCustomers;
    private float timeAmount;
    private Business business;

    // Use this for initialization
    void Start () {
        //get the business object that is being run
        business = player.business;

        //set up the diffrent displayes
        BusinessInfo[0].SetText(business.servingName);
        BusinessInfo[1].SetText(business.customerServed + "/" + business.customerNeeded);
        BusinessInfo[2].SetText(getWage((player.playerEmployee.wage * player.skillPercent[3] / 100)));
        BusinessInfo[3].SetText(business.happiness + "%");
        for (int x = 0; x < 4; x++){
            business.workStations[x].titleText = titles[x];
            business.workStations[x].progressText = subInfo[x];
            business.workStations[x].slider = Sliders[x];
            business.workStations[x].focuseInput = FocusInput[x];
            business.workStations[x].titleText.SetText(business.workStations[x].name);
            upDateAmount(x);
            if (business.workStations[x].type == 2 || business.workStations[x].type == 3)
            {
                needsCustomers = x;
            }
        }

        //set up the work background
        RunDropdown.ClearOptions();
        List<string> options = new List<string>();
        for(int x = 0; x < 4; x++){
            options.Add(business.workStations[x].name);
        }
        RunDropdown.AddOptions(options);
        RunDropdown.value = player.playerEmployee.workingIn;
        RunDropdown.RefreshShownValue();
        BusinessInfo[4].SetText("0:00 AM");
        timeAm_Pm = false;
        timeAmount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //this controles the time diplay as well as ever 24 seconds updates the add customers funtion
        //customers are added at the begining of ever day depending on the business.customerIncrease
        float tempTime = Time.fixedTime % 12;
        int tempHours = (int)Mathf.Floor(tempTime);
        int tempSec = (int)Mathf.Floor((tempTime - tempHours) * 60);
        tempHours = tempHours + 1;
        if(tempTime < timeAmount){
            if (timeAm_Pm == true){
                timeAm_Pm = false;
            }
            else{
                timeAm_Pm = true;
                this.addCustomers(business.workStations[needsCustomers]);
            }
        }
        string temp = "AM";
        if (timeAm_Pm == true){
            temp = "AM";
        }
        else{
            temp = "PM";
        }
        string tempSecTime = "00";
        if (tempSec < 10){
            tempSecTime = "0" + tempSec.ToString();
        }
        else{
            tempSecTime = tempSec.ToString();
        }
        BusinessInfo[4].SetText(tempHours + ":" + tempSecTime + temp);
        timeAmount = tempTime;

        //update any workstations that deteriorate over time
        maintainBusiness();

        //update any work stations that the player is working in 
        for (short x = 0; x < 4; x++)
        {
            if (player.focus[x] > 0)
            {
                updateStations(x, business.workStations[x]);
            }
        }

        //update the display for the training work station
        if (business.workStations[3].orders.Count == 0)
        {
            business.workStations[3].progressText.SetText("None: --:--");
        }
        else if (player.focus[3] == 0)
        {
            business.workStations[3].progressText.SetText(business.workStations[3].orders[0].name + ": --:--");
        }
        else
        {
            int amount = (int)Mathf.Floor(business.workStations[3].orders[0].wants[1] * (1 - business.workStations[3].orders[0].progress));
            business.workStations[3].progressText.SetText(business.workStations[3].orders[0].name + ": " + trainingCS.getTimeForTraining(amount));
        }
    }

    //updates the businesses that deteriorate over time
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

                        //if the amount is full stop at full
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
                        upDateAmount(x);
                    }
                    useThis.slider.value = tempProgress;
                    useThis.orders[0].progress = tempProgress;
                    break;
            }
        }
    }

    //adds customers once a day(every 24 sec)
    private void addCustomers(workStation useThis){
        //if happiness is greater than 90% increase the amount of customers that come per day/ if happiness is lower that 90 decrease
        if (business.happiness > 90){
            int temp = (int)(business.happiness) - 90;
            business.customerIncrease = business.customerIncrease * (1 + (((float)temp / 100) * player.level));
        }
        else if (business.happiness < 90)
        {
            int temp = Mathf.Abs((int)(business.happiness) - 90);
            business.customerIncrease = business.customerIncrease * (1 - (((float)temp / 100) * player.level));
            if (business.customerIncrease < 2){
                business.customerIncrease = 2;
            }
        }

        //update the hapiness based on customers waiting
        int percent = (int)Mathf.Floor(useThis.orders.Count / 20);
        if (percent == 0){
            business.negativeHappiness[needsCustomers] = business.negativeHappiness[needsCustomers] - 1;
            if (business.negativeHappiness[needsCustomers] < 0){
                business.negativeHappiness[needsCustomers] = 0;
            }
        }
        else{
            business.negativeHappiness[needsCustomers] = business.negativeHappiness[needsCustomers] + percent;
            if(business.negativeHappiness[needsCustomers] > 50){
                business.negativeHappiness[needsCustomers] = 50;
            }
        }
        updateHappiness();

        int tempCustomers = (int)Mathf.Round(business.customerIncrease);
        //update the customers waiting
        for (int x = 0; x < tempCustomers; x++)
        {
            int[] want = new int[] { 0 };
            order order = new order(mainControl.getName(), want, false);
            useThis.orders.Add(order);
        }
        upDateAmount(needsCustomers);
    }
    
    //this is the template that runs each workstation depending on there type
    private void updateStations(int place, workStation useThis){
        int tempFocus;
        int tempNumber;
        float tempProgress;
        switch (useThis.type){
            case 0:
                //set the amount of focus that the player has in this work station
                tempFocus = player.focus[place];

                //creates the amount of progress in the last update taking into account the focus multiplyer
                tempProgress = (tempFocus * player.playerEmployee.focusMultiplyer[place] * Time.deltaTime) / (useThis.timeMain / player.playerEmployee.timeMultiplyer[place]);
                //adds the progresses together to get the current progress
                tempProgress = tempProgress + useThis.orders[0].progress;

                if (tempProgress >= 1){
                    // makes a new number to be the amount of times the bar was filled
                    tempNumber = (int)Mathf.Floor(tempProgress);

                    //make new variable to hold the progress less than 1
                    tempProgress = tempProgress - tempNumber;
                    useThis.orders[0].wants[1] = useThis.orders[0].wants[1] + tempNumber;
                    upDateAmount(place);
                }
                useThis.slider.value = tempProgress;
                useThis.orders[0].progress = tempProgress;
                break;
            case 1:
                //if there are no supply and progress
                if (useThis.orders[0].progress == 0 && business.workStations[useThis.extra[0]].orders[0].wants[1] == 0)
                {
                    return;
                }

                //set the amount of focus that the player has in this station
                tempFocus = player.focus[place];

                //creates the amount of progress in the last update taking into account the focus multiplyer
                tempProgress = (tempFocus * player.playerEmployee.focusMultiplyer[place] * Time.deltaTime) / (useThis.timeMain / player.playerEmployee.timeMultiplyer[place]);
                //adds the progresses together to get the current progress

                tempProgress = tempProgress + useThis.orders[0].progress;

                //for orders if the taken value is false it means that the products for that order have not been removed
                if (useThis.orders[0].taken == false)
                {
                    if (business.workStations[useThis.extra[0]].orders[0].wants[1] == 0)
                    {
                        tempProgress = 0;
                    }
                    else
                    {
                        business.workStations[useThis.extra[0]].orders[0].wants[1] = business.workStations[useThis.extra[0]].orders[0].wants[1] - 1;
                        useThis.orders[0].taken = true;
                    }
                }

                //if the progress is greater than one
                if (tempProgress >= 1)
                {
                    // makes a new number to be the amount of times the bar was filled
                    tempNumber = (int)Mathf.Floor(tempProgress);

                    //make new variable to hold the progress less than 1
                    tempProgress = tempProgress - tempNumber;

                    //this code is not very nice and i will redo it the future, basically if it went though more than once take for each time through
                    useThis.orders[0].wants[1] = useThis.orders[0].wants[1] + 1;
                
                    if (business.workStations[useThis.extra[0]].orders[0].wants[1] == 0)
                    {
                        tempProgress = 0;
                        useThis.orders[0].taken = false;
                    }
                    else
                    {
                        business.workStations[useThis.extra[0]].orders[0].wants[1] = business.workStations[useThis.extra[0]].orders[0].wants[1] - 1;
                        useThis.orders[0].taken = true;
                    }

                    if (tempNumber == 2)
                    {
                        if (business.workStations[useThis.extra[0]].orders[0].wants[1] == 0)
                        {
                            tempProgress = 0;
                            useThis.orders[0].taken = false;
                        }
                        else
                        {
                            business.workStations[useThis.extra[0]].orders[0].wants[1] = business.workStations[useThis.extra[0]].orders[0].wants[1] - 1;
                            useThis.orders[0].wants[1] = useThis.orders[0].wants[1] + 1;
                        }
                    }
                }
                useThis.slider.value = tempProgress;
                useThis.orders[0].progress = tempProgress;
                upDateAmount(place);
                upDateAmount(useThis.extra[0]);
                break;
            case 2:
                //if there are no customers and progress
                if (useThis.orders.Count == 0)
                {
                    return;
                }

                //if there are no supply and progress
                if (useThis.orders[0].progress == 0 && business.workStations[useThis.extra[0]].orders[0].wants[1] == 0)
                {
                    return;
                }

                //set the amount of focus that the player has in this station
                tempFocus = player.focus[place];

                //creates the amount of progress in the last update taking into account the focus multiplyer
                tempProgress = (tempFocus * player.playerEmployee.focusMultiplyer[place] * Time.deltaTime) / (useThis.timeMain / player.playerEmployee.timeMultiplyer[place]);
                //adds the progresses together to get the current progress
                tempProgress = tempProgress + useThis.orders[0].progress;

                //take away one of the customers and used object since the bar has no progress
                if (useThis.orders[0].taken == false)
                {
                    if (business.workStations[useThis.extra[0]].orders[0].wants[1] == 0)
                    {
                        tempProgress = 0;
                    }
                    else
                    {
                        business.workStations[useThis.extra[0]].orders[0].wants[1] = business.workStations[useThis.extra[0]].orders[0].wants[1] - 1;
                        useThis.orders[0].taken = true;
                    }
                }

                //if the progress is greater than one
                if (tempProgress >= 1){
                    // makes a new number to be the amount of times the bar was filled
                    tempNumber = (int)Mathf.Floor(tempProgress);

                    //make new variable to hold the progress less than 1
                    tempProgress = tempProgress - tempNumber;

                    useThis.orders.RemoveAt(0);
                    addToCustomers(1);

                    //this code is not very nice and i will redo it the future, basically if it went though more than once take for each time through
                    if (useThis.orders.Count == 0)
                    {
                        tempProgress = 0;
                    }
                    else if (business.workStations[useThis.extra[0]].orders[0].wants[1] == 0)
                    {
                        tempProgress = 0;
                        useThis.orders[0].taken = false;
                    }
                    else
                    {
                        business.workStations[useThis.extra[0]].orders[0].wants[1] = business.workStations[useThis.extra[0]].orders[0].wants[1] - 1;
                        useThis.orders[0].taken = true;
                    }

                    if (tempNumber == 2)
                    {
                        if (useThis.orders.Count == 0)
                        {
                            tempProgress = 0;
                        }
                        else if (business.workStations[useThis.extra[0]].orders[0].wants[1] == 0)
                        {
                            tempProgress = 0;
                            useThis.orders[0].taken = false;
                        }
                        else
                        {
                            business.workStations[useThis.extra[0]].orders[0].wants[1] = business.workStations[useThis.extra[0]].orders[0].wants[1] - 1;
                            useThis.orders.RemoveAt(0);
                            addToCustomers(1);
                        }
                    }
                }
                useThis.slider.value = tempProgress;
                if (useThis.orders.Count > 0)
                {
                    useThis.orders[0].progress = tempProgress;
                }
                upDateAmount(place);
                upDateAmount(useThis.extra[0]);
                break;
            case 3:
                /*
                //if there are no customers and progress
                if (useThis.progress == 0 && useThis.amountWaiting == 0)
                {
                    return;
                }

                //add 10 to the focus amount if this workplace is being run
                int tempFocus3 = useThis.focus;
                if (business.isRunning == place)
                {
                    tempFocus3 = tempFocus3 + 10;
                }

                //creates the amount of progress in the last update taking into account the focus multiplyer
                float tempProgress3 = (tempFocus3 * player.focusMultiplyer[place] * Time.deltaTime) / (useThis.timeMain / player.timeMultiplyer[place]);
                //adds the progresses together to get the current progress
                tempProgress3 = tempProgress3 + useThis.progress;

                //take away one of the customers and used object since the bar has no progress
                if (useThis.progress == 0)
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
                useThis.slider.value = tempProgress3;
                useThis.progress = tempProgress3;
                upDateAmount(place);
                */
                break;
            case 4:
                //set the amount of focus that the player has
                tempFocus = player.focus[place];

                //creates the amount of progress in the last update taking into account the focus multiplyer
                tempProgress = (tempFocus * player.playerEmployee.focusMultiplyer[place] * Time.deltaTime) / (useThis.timeMain / player.playerEmployee.timeMultiplyer[place]);
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
                        business.negativeHappiness[place] = useThis.orders[0].wants[0];
                        if (business.negativeHappiness[place] < 0)
                        {
                            business.negativeHappiness[place] = 0;
                        }
                        updateHappiness();
                    }
                    upDateAmount(place);
                }
                useThis.slider.value = tempProgress;
                useThis.orders[0].progress = tempProgress;
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
                if(useThis.orders.Count == 0)
                {
                    return;
                }

                //set the amount of focus that the player has in this station 
                tempFocus = player.focus[place];

                //creates the amount of progress in the last update taking into account the focus multiplyer
                tempProgress = (tempFocus * player.playerEmployee.focusMultiplyer[place] * Time.deltaTime) / (useThis.orders[0].wants[1] / player.playerEmployee.timeMultiplyer[place]);
                //adds the progresses together to get the current progress
                tempProgress = tempProgress + useThis.orders[0].progress;

                //when the current training has been finished
                if (tempProgress > 1)
                {
                    trainingCS.finishTraining();
                    useThis.orders.RemoveAt(0);
                    tempProgress = tempProgress - 1;
                }

                //if there is training add the progress to the next training
                if (useThis.orders.Count != 0)
                {
                    useThis.slider.value = tempProgress;
                    useThis.orders[0].progress = tempProgress;
                }
                //if there is no more training
                else
                {
                    useThis.slider.value = 0;
                    selectionChange(player.playerEmployee.wasWorkingIn);
                }
                break;
        }
    }
    
    //set the happiness of the business
    private void updateHappiness(){
        //calculate the current happiness of the business and change the display
        int amount = 0;
        for (short x = 0; x < 4; x++){
            amount = amount + business.negativeHappiness[x]; 
        }
        if (amount > 100){
            amount = 100;
        }
        business.happiness = 100 - amount;
        BusinessInfo[3].SetText(business.happiness + "%");
    }

    //update the text next to the progress bar
    private void upDateAmount(int place){
        //how this text is displayed depends on the workstation type, go through the workStations that we are using and update
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
            if (business.workStations[3].orders.Count == 0)
            {
                business.workStations[3].progressText.SetText("None: --:--");
            }
            else if (player.focus[3] == 0)
            {
                business.workStations[3].progressText.SetText(business.workStations[3].orders[0].name + ": --:--");
            }
            else
            {
                int amount = (int)Mathf.Floor(business.workStations[3].orders[0].wants[1] * (1 - business.workStations[3].orders[0].progress));
                business.workStations[3].progressText.SetText(business.workStations[3].orders[0].name + ": " + trainingCS.getTimeForTraining(amount));
            }
        }
        else{
            business.workStations[place].progressText.SetText(business.workStations[place].workName + " " + business.workStations[place].orders[0].wants[0] + "%");
        }
    }
    /*
    private void setHappinessForUses(int amount)
    {
        //used only with type 6 sets the negative happiness for this station
        if (amount >= 90)
        {
            business.negativeHappiness[usesGoods] = 0;
        }
        else
        {
            business.negativeHappiness[usesGoods] = 90 - amount;
            if (business.negativeHappiness[usesGoods] > 50)
            {
                business.negativeHappiness[usesGoods] = 50;
            }
        }
        updateHappiness();
    }
    */
    //whenever you serve a customer add them to the amount
    private void addToCustomers(int amount){
        //add the amount of new customers to the amount already there
        business.customerServed = business.customerServed + amount;
        //update the display
        BusinessInfo[1].SetText(business.customerServed + "/" + business.customerNeeded);

        /*
        //if there is a work station that takes good for every customer served update it with the amount of customers just served
        if (usesGoods != 0)
        {
            float tempProgress = business.workStations[usesGoods].progress - ((float)business.workStations[usesGoods].extra[1] / 100 * amount);

            if (tempProgress < 0)
            {
                tempProgress = Mathf.Abs(tempProgress);
                // makes a new number to be the amount of times the bar was filled
                int tempNumber = (int)Mathf.Floor(tempProgress) + 1;

                //make new variable to hold the progress less than 1
                tempProgress = tempProgress % 1;
                tempProgress = 1 - tempProgress;

                business.workStations[usesGoods].amountWaiting = business.workStations[usesGoods].amountWaiting - tempNumber;
                setHappinessForUses(business.workStations[usesGoods].amountWaiting);
            }
            upDateAmount(usesGoods);
            business.workStations[usesGoods].slider.value = tempProgress;
            business.workStations[usesGoods].progress = tempProgress;
        }
        */
        //update the wage when the required amount of customers have been served
        if (business.customerServed > business.wageIncrease[0]){
            player.playerEmployee.wage = (int)Mathf.Floor(player.playerEmployee.wage * 1.1F);
            business.wageIncrease[0] = business.wageIncrease[0] + business.wageIncrease[1];
        }
        BusinessInfo[2].SetText(getWage((player.playerEmployee.wage * player.skillPercent[3] / 100)));

        //add the cusomer to the skill points 
        mainControl.addToSkillsAmounts(amount, business.sector);
    }

    //update when the player stats working in a new section
    public void selectionChange(int workArea){
        RunDropdown.value = workArea;
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

    //update the wage when changes occure
    public void upDateWage(){
        BusinessInfo[2].SetText(getWage(player.playerEmployee.wage));
    }

    //set the display for money
    private string getWage(int wage){
        //set up the string display for the wage
        var temp1 = Mathf.Floor(wage/100);
        var temp2 = (wage % 10);
        var temp3 = (((wage - temp2) / 10) % 10);
        return "$" + temp1 + "." + temp3 + temp2;
    }

    //toggle the training background
    public void training(){
        if (trainingMenu.activeInHierarchy == true){
            trainingMenu.SetActive(false);
        }
        else{
            trainingMenu.SetActive(true);
        }
    }

    //toggle the shoping background
    public void shoping()
    {
        if (shopMenu.activeInHierarchy == true)
        {
            shopMenu.SetActive(false);
        }
        else
        {
            shopMenu.SetActive(true);
        }
    }

    //toggle the menu background
    public void menuChange()
    {
        if (menu.activeInHierarchy == true)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }

    //the inputs for the input boxes for focus 
    public void firstFocusChange(string text)
    {
        changeFocusFor(0, text);
    }

    public void secondFocusChange(string text)
    {
        changeFocusFor(1, text);
    }

    public void thirdFocusChange(string text)
    {
        changeFocusFor(2, text);
    }

    public void fourthFocusChange(string text)
    {
        changeFocusFor(3, text);
    }

    //control that what the player is entering into the focus boxes is correct
    private void changeFocusFor(int place, string text){
        //if there is no text set the amount to 0
        int add = 0;
        if (player.playerEmployee.workingIn == place)
        {
            add = 10;
        }

        if (text.Length == 0){
            business.workStations[place].focuseInput.text = "0";
        }
        else{
            int temp = System.Int32.Parse(text);
            player.focus[place] = 0;
            int amount = getFocusA();
            if (player.playerEmployee.workingIn == place)
            {
                amount = amount - 10;
            }
            //if the amount is negative change it to positive
            if (temp < 0){
                temp = Mathf.Abs(temp);
            }
            //if the amount is multiple zeros change the amount to a single 0
            if (temp == 0){
                business.workStations[place].focuseInput.text = "0";
                player.focus[place] = 0 + add;
            }
            //if the amount inputed is more that the amount availible change the value to amount availible
            else if (temp > amount){
                business.workStations[place].focuseInput.text = amount.ToString();
                player.focus[place] = amount + add;
            }
            //set the amount to the input text
            else{
                business.workStations[place].focuseInput.text = temp.ToString();
                player.focus[place] = temp + add;
            }
        }
    }

    //return the focus that is not being used
    private int getFocusA(){
        //return the focus that is availible
        int temp = player.focus[0] + player.focus[1] + player.focus[2] + player.focus[3];
        return player.playerEmployee.focus - temp;
    }

    //end the game
    public void QuitGame(){
        Application.Quit();
    }

    //save the game
    public void SaveGame(){

    }
}
