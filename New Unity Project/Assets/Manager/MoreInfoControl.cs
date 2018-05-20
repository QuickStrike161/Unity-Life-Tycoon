using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoreInfoControl : MonoBehaviour {

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
    private List<string> infoContainer = new List<string> { };
    private Business business;
    private List<Image> ImageList = new List<Image> { };
    private float[] storeInfo = new float[] { 0, 0, 0, 0 };

    // Use this for initialization
    void Start () {
        business = player.business;
        timeSec = 0;
        selected = 0;
	}
	
	// Update is called once per frame
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
    
    public void setUp(int station)
    {
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

                title[0].SetText(business.workStations[station].name);
                info[0].SetText("Employee's Working: " + ImageList.Count);
                

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

                title[0].SetText(business.workStations[station].name);
                info[0].SetText("Employee's Working: " + ImageList.Count);

                if (ImageList.Count == 0)
                {
                    title[1].SetText("None");
                    infoContainer.Add("No Employees Training");
                    changeSelectedSize();
                }
                else
                {
                    if (ImageList.Count < selected + 1)
                    {
                        selected = ImageList.Count - 1;
                    }
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

    public string removeEnd(string word)
    {
        word.ToLower();
        word = word.Remove(word.Length - 1);
        return word;
    }

    public void changeEmployeeSize()
    {
        firstVisual.rectTransform.sizeDelta = new Vector2(topControl[0].rect.width, 35);
    }

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


