using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoreInfoControl : MonoBehaviour {

    /*
     * this is used in every work station to give the player more information abou the work station,
     * such as planning the creation of goods, maintaining the business, and displaying customer orders
     * needs to have the rest of the workstation types added
     */

    public TMP_Text[] title;
    public TMP_Text[] info;
    public PlayerInfo player;
    public GameObject content;
    public Image firstVisual;
    public RectTransform[] topControl;
    public GameObject[] infoSetUp;

    private int type;
    private int station;
    private int selected;
    private float timeSec;
    private bool updateVisual;
    private List<string> infoContainer = new List<string> { };//used to set up the string lists do infoContainer.add("words");
    private Business business;
    private List<Image> ImageList = new List<Image> { };
    private float[] storeInfo = new float[] { 0, 0, 0, 0 };

    // Use this for initialization
    void Start () {
        business = player.business;
        timeSec = 0;
        selected = 0;
	}

    //create a loop that is called ever secound used to update the information
    void Update () {
        //create a loop that is called ever secound
        float tempTime = Time.fixedTime % 1;
        if (tempTime < timeSec)
        {
            changeEmployeeSize();
            if (updateVisual == true)
            {
                setUp(station);
            }
            else
            {

            }
        }
        timeSec = tempTime;
    }
    
    //set up the displays diffrently depending on the workStaion type
    public void setUp(int station)
    {
        //reset so that it can be setup again 
        business = player.business;
        type = business.workStations[station].type;
        this.station = station;
        infoContainer = new List<string> { };
        foreach(GameObject back in infoSetUp)
        {
            back.SetActive(false);
        }

        foreach (Image image in ImageList)
        {
            content.transform.DetachChildren();
            Destroy(image.gameObject);
        }
        ImageList = new List<Image> { };
        changeEmployeeSize();

        switch (type)
        {
            case 4:
                updateVisual = true;
                infoSetUp[0].SetActive(true);
                storeInfo = new float[] { 0, 0, 0, 0 };
                setUpStoreInfo(station);

                //set up an image for each employee that is working in the station 
                foreach (employee employee in business.employeesInfo)
                {
                    if (employee.workingIn == station)
                    {
                        Image visual = Instantiate(firstVisual) as Image;
                        visual.GetComponent<MoreInfoTemplate>().setUp(ImageList.Count, employee, false);
                        ImageList.Add(visual);
                        visual.transform.SetParent(content.transform, false);
                        visual.gameObject.SetActive(true);
                    }
                }

                //set the text for title and info
                title[0].SetText(business.workStations[station].name);
                info[0].SetText("Employee's Working: " + ImageList.Count);
                
                //set up the information that will displayed for this work station
                title[1].SetText(business.workStations[station].name);
                infoContainer.Add("");
                infoContainer.Add(business.workStations[station].orders[0].name);
                infoContainer.Add(storeInfo[3].ToString("f3") + "%");
                infoContainer.Add("");
                infoContainer.Add("Decay per second:");
                infoContainer.Add(storeInfo[0].ToString("f3") + "%");
                infoContainer.Add("");
                infoContainer.Add("Maintain per second:");
                infoContainer.Add(storeInfo[1].ToString("f3") + "%");
                infoContainer.Add("");
                infoContainer.Add("Change per second:");
                infoContainer.Add(storeInfo[2].ToString("f3") + "%");
                changeSelectedSize();

                foreach (Image image in ImageList)
                {
                    image.GetComponent<MoreInfoTemplate>().setColor(false);
                }
                break;
            case 7:
                updateVisual = true;
                infoSetUp[0].SetActive(true);

                //set up an image for each employee that is working in the station 
                foreach (employee employee in business.employeesInfo)
                {
                    if (employee.workingIn == station)
                    {
                        Image visual = Instantiate(firstVisual) as Image;
                        visual.GetComponent<MoreInfoTemplate>().setUp(ImageList.Count,employee, true);
                        ImageList.Add(visual);
                        visual.transform.SetParent(content.transform, false);
                        visual.gameObject.SetActive(true);
                    }
                }

                //set the text for the title and info
                title[0].SetText(business.workStations[station].name);
                info[0].SetText("Employee's Working: " + ImageList.Count);

                //if there are no employees in this staion 
                if (ImageList.Count == 0)
                {
                    title[1].SetText("None");
                    infoContainer.Add("No Employees Training");
                    changeSelectedSize();
                }
                else
                {
                    //set the employee that is selected if the last selected employee was removed
                    if (ImageList.Count < selected + 1)
                    {
                        selected = ImageList.Count - 1;
                    }

                    //if there is no training for the selected employee
                    employee temp = ImageList[selected].GetComponent<MoreInfoTemplate>().employee;
                    title[1].SetText(temp.name);
                    if (temp.trainingQ.Count == 0)
                    {
                        infoContainer.Add("No Training");
                        infoContainer.Add("You can click on the employee's");
                        infoContainer.Add("name in the previous menu");
                        infoContainer.Add("to open the employee control");
                        changeSelectedSize();
                    }
                    else
                    {
                        //for each training in the selected employees trainingQ set up the dispaly 
                        foreach(order tempOrder in temp.trainingQ)
                        {
                            infoContainer.Add(tempOrder.name + ":");
                            infoContainer.Add(business.trainingList[tempOrder.wants[0]].description + ",");
                            infoContainer.Add("Adds " + business.trainingList[tempOrder.wants[0]].affectAmount + "% to:");
                            foreach (int affects in business.trainingList[tempOrder.wants[0]].affectAreas)
                            {
                                infoContainer.Add(removeEnd(business.workStations[affects].name));
                            }
                            infoContainer.Add("");
                        }
                        changeSelectedSize();
                    }
                }

                //set the image that is selected button to blue
                foreach(Image image in ImageList)
                {
                    image.GetComponent<MoreInfoTemplate>().setColor(false);
                }
                if (ImageList.Count != 0)
                {
                    ImageList[selected].GetComponent<MoreInfoTemplate>().setColor(true);
                }
                break;
        }
    }

    //remove the last character of a string
    public string removeEnd(string word)
    {
        word.ToLower();
        word = word.Remove(word.Length - 1);
        return word;
    }

    //set the size of the template in the first list
    public void changeEmployeeSize()
    {
        firstVisual.rectTransform.sizeDelta = new Vector2(topControl[0].rect.width, 35);
    }

    //if the text scroll is being used set the length of the text and then place in the strings that were added to infoContainer
    public void changeSelectedSize()
    {
        info[1].rectTransform.sizeDelta = new Vector2(topControl[1].rect.width, infoContainer.Count * 18.5F);
        string tempString = "";
        foreach(string temp in infoContainer)
        {
            tempString = tempString + temp + "\n";
        }
        info[1].SetText(tempString);
    }

    //update when the selected is changed 
    public void changeSelected(int place)
    {
        selected = place;
        if (updateVisual == true)
        {
            setUp(station);
        }
        else
        {

        }
    }

    //set up the amounts that the store is being maintained by
    public void setUpStoreInfo(int station)
    {
        workStation useThis = business.workStations[station];

        //calculate the filth created since created since last tick
        storeInfo[0] = business.customerIncrease / useThis.timeMain;

        foreach (employee employee in business.employeesInfo)
        {
            if (employee.workingIn == station)
            {
                //creates the amount of progress in the last update taking into account the focus multiplyer
                float progress = ((employee.focus * employee.focusMultiplyer[station]) / (useThis.timeMain / employee.timeMultiplyer[station])) * player.decreaseAmount * (player.skillPercent[0] / 100F);
                progress = progress * getWorkForHappiness(employee);
                if (player.playerEmployee.workingIn == station)
                {
                    progress = progress * ((100 + player.percentEffect) / 100F);
                }
                storeInfo[1] = storeInfo[1] + progress;
            }
        }
        storeInfo[2] = storeInfo[1] - storeInfo[0];
        storeInfo[3] = useThis.orders[0].wants[0] + useThis.orders[0].progress;
    }

    //update the employees productivity based on there happiness
    public float getWorkForHappiness(employee employee)
    {
        float temp = employee.happiness + 10;
        if (temp > 100)
        {
            temp = 100;
        }
        return temp / 100F;
    }
}


