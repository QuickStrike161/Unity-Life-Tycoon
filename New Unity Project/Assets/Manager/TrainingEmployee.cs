using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainingEmployee : MonoBehaviour
{

    /*
     * This script controls the training for employees, allows you to add/remove training as well as select
     * a focus area for the training(choose the workStation it effects)
     */

    public TMP_Text[] infoText;
    public TMP_Dropdown areaFocus;
    public PlayerInfo player;
    public GameObject[] contentList;
    public Image trainingVisual;
    public RectTransform topControl;

    private int employee;
    private int focusArea;
    private float timeSec;
    private Business business;
    private List<Image> ImageList = new List<Image> { };

    //set up variables and update the dropDown option for focus area
    void Start()
    {
        business = player.business;
        areaFocus.ClearOptions();
        List<string> options = new List<string>();
        options.Add("All Stations");
        for (int x = 1; x < 5; x++)
        {
            options.Add(business.workStations[x - 1].name);
        }
        areaFocus.AddOptions(options);
        areaFocus.value = 0;
        this.focusArea = 0;
        areaFocus.RefreshShownValue();
        timeSec = 0;
    }

    //create a loop that is called ever secound
    void Update()
    {
        float tempTime = Time.fixedTime % 1;
        if (tempTime < timeSec)
        {
            listVisualUpdate();
        }
        timeSec = tempTime;
    }

    //set up the lists visual for training
    public void setUp(int employee)
    {
        //remove any images that are there from previous use
        foreach (Image image in ImageList)
        {
            contentList[image.GetComponent<EmployeeTrainingTemplate>().location].transform.DetachChildren();
            Destroy(image.gameObject);
        }
        ImageList = new List<Image> { };

        //check to see if the employee has had the availible training set up if not add active trainings to availible trainings 
        business = player.business;
        this.employee = employee;
        if (business.employeesInfo[employee].trainingAvailible.Count == 0 && business.employeesInfo[employee].timeMultiplyer[0] == 1 && business.employeesInfo[employee].trainingQ.Count == 0)
        {
            for (short x = 0; x < business.trainingList.Length; x++)
            {
                if (business.trainingList[x].active == true)
                {
                    business.employeesInfo[employee].trainingAvailible.Add(x);
                }
            }
        }

        //create the templates for any training in the availible training
        for (short x = 0; x < business.employeesInfo[employee].trainingAvailible.Count; x++)
        {
            Image visual = Instantiate(trainingVisual) as Image;
            visual.GetComponent<EmployeeTrainingTemplate>().setUp(ImageList.Count, business.employeesInfo[employee].trainingAvailible[x],0);
            ImageList.Add(visual);
        }

        //create the templates for any training in the trainingQ
        foreach(order order in business.employeesInfo[employee].trainingQ)
        {
            Image visual = Instantiate(trainingVisual) as Image;
            visual.GetComponent<EmployeeTrainingTemplate>().setUp(ImageList.Count, order.wants[0], 1);
            ImageList.Add(visual);
        }

        //update visual
        listVisualUpdate();
    }

    //return the time that the current training will take based on how fast it is currently running (min:sec) or (hour:min)
    public string getTimeForTraining(int amount)
    {
        int tempFocus = business.employeesInfo[employee].focus;

        if (tempFocus == 0)
        {
            return "--:--";
        }
        else
        {
            if(player.playerEmployee.workingIn == 3)
            {
                tempFocus = (int)Mathf.Ceil(amount / (tempFocus * (player.skillPercent[0] / 100F) * business.employeesInfo[employee].focusMultiplyer[3] * business.employeesInfo[employee].timeMultiplyer[3] * player.decreaseAmount * (1 + (player.percentEffect / 100F))));
            }
            else
            {
                tempFocus = (int)Mathf.Ceil(amount / (tempFocus * (player.skillPercent[0] / 100F) * business.employeesInfo[employee].focusMultiplyer[3] * business.employeesInfo[employee].timeMultiplyer[3] * player.decreaseAmount));
            }

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

    //set up the list visual and makes sure that it is up to date
    private void listVisualUpdate()
    {
        changeTrainingSize();
        for (short x = 0; x < ImageList.Count; x++)
        {
            //if the image is not active then keep it invisable
            if (ImageList[x].GetComponent<EmployeeTrainingTemplate>().active == false)
            {
                ImageList[x].transform.SetParent(contentList[2].transform, false);
                ImageList[x].gameObject.SetActive(false);
            }
            else if (ImageList[x].GetComponent<EmployeeTrainingTemplate>().location == 0)
            {
                //if there is not focus area more images to the first list and make them visable
                if (focusArea == 0)
                {
                    ImageList[x].transform.SetParent(contentList[0].transform, false);
                    ImageList[x].gameObject.SetActive(true);
                }
                //if there is a focus area check if the training is in it, if not make image invisable, otherwise make it visable
                else
                {
                    if (changesArea(ImageList[x].GetComponent<EmployeeTrainingTemplate>().training, focusArea - 1) == true)
                    {
                        ImageList[x].transform.SetParent(contentList[0].transform, false);
                        ImageList[x].gameObject.SetActive(true);
                    }
                    else
                    {
                        ImageList[x].transform.SetParent(contentList[2].transform, false);
                        ImageList[x].gameObject.SetActive(false);
                    }
                }
            }
            //for all training in the trainingQ set them visable in the second list
            else if (ImageList[x].GetComponent<EmployeeTrainingTemplate>().location == 1)
            {
                ImageList[x].transform.SetParent(contentList[1].transform, false);
                ImageList[x].gameObject.SetActive(true);
            }
            //make any other images invisable
            else
            {
                ImageList[x].transform.SetParent(contentList[2].transform, false);
                ImageList[x].gameObject.SetActive(false);
            }

            //if the training is currently being worked update the time
            if (isFirst(ImageList[x].GetComponent<EmployeeTrainingTemplate>().training) == true)
            {
                ImageList[x].GetComponent<EmployeeTrainingTemplate>().updateButton(true);
            }
            else
            {
                ImageList[x].GetComponent<EmployeeTrainingTemplate>().updateButton(false);
            }
            updateInfo();
        }
    }

    //when a training button has been clicked and needs to be added/removed
    public void clicked(int place, int location, int training)
    {
        //if the image is in the first list add it to the trainingQ and add it to the second list
        if (location == 0)
        {
            ImageList[place].GetComponent<EmployeeTrainingTemplate>().location = 1;
            //if the training has a follower set it active and add it to the employees availible 
            if (business.trainingList[training].follower != -1)
            {
                bool tempBool = false;
                int tempTraining = business.trainingList[training].follower;
                int tempPlace = 0;
                for (short x = 0; x < ImageList.Count; x++)
                {
                    if (ImageList[x].GetComponent<EmployeeTrainingTemplate>().training == tempTraining)
                    {
                        tempBool = true;
                        tempPlace = x;
                    }
                }

                if (tempBool == false)
                {
                    business.employeesInfo[employee].trainingAvailible.Add(tempTraining);
                    Image visual = Instantiate(trainingVisual) as Image;
                    visual.GetComponent<EmployeeTrainingTemplate>().setUp(ImageList.Count, tempTraining, 0);
                    ImageList.Add(visual);
                }
                else
                {
                    ImageList[tempPlace].GetComponent<EmployeeTrainingTemplate>().location = 0;
                    ImageList[tempPlace].GetComponent<EmployeeTrainingTemplate>().active = true;
                }
            }

            int[] tempArray = new int[2];
            tempArray[0] = training;
            tempArray[1] = business.trainingList[training].cost;
            order tempOrder = new order(business.trainingList[training].name, tempArray, false);
            business.employeesInfo[employee].trainingQ.Add(tempOrder);
            business.employeesInfo[employee].trainingAvailible.Remove(training);
        }
        //if the image is in the second list remove it from the trainingQ and deal with any followers to put them back in there correct place
        else
        {
            ImageList[place].GetComponent<EmployeeTrainingTemplate>().location = 0;
            if (business.trainingList[training].follower != -1)
            {
                bool tempBool = false;
                int tempTraining = business.trainingList[training].follower;
                int tempPlace = 0;
                for (short x = 0; x < ImageList.Count; x++)
                {
                    if (ImageList[x].GetComponent<EmployeeTrainingTemplate>().training == tempTraining)
                    {
                        tempBool = true;
                        tempPlace = x;
                    }
                }

                if (tempBool == false)
                {
                    Debug.Log("this should not get here there should always be the training created");
                }
                else
                {
                    if (ImageList[tempPlace].GetComponent<EmployeeTrainingTemplate>().location == 1)
                    {
                        order tempOrder1 = null;
                        for (short x = 0; x < business.employeesInfo[employee].trainingQ.Count; x++)
                        {
                            if (business.employeesInfo[employee].trainingQ[x].name == business.trainingList[tempTraining].name)
                            {
                                tempOrder1 = business.employeesInfo[employee].trainingQ[x];
                            }
                        }
                        business.employeesInfo[employee].trainingQ.Remove(tempOrder1);
                    }
                    else
                    {
                        business.employeesInfo[employee].trainingAvailible.Remove(tempTraining);
                    }
                    ImageList[tempPlace].GetComponent<EmployeeTrainingTemplate>().location = 2;
                    ImageList[tempPlace].GetComponent<EmployeeTrainingTemplate>().active = false;
                }
            }

            //remove training from the trainingQ 
            order tempOrder2 = null;
            for (short x = 0; x < business.employeesInfo[employee].trainingQ.Count; x++)
            {
                if (business.employeesInfo[employee].trainingQ[x].name == business.trainingList[training].name)
                {
                    tempOrder2 = business.employeesInfo[employee].trainingQ[x];
                }
            }
            business.employeesInfo[employee].trainingQ.Remove(tempOrder2);
            business.employeesInfo[employee].trainingAvailible.Add(training);
        }
        listVisualUpdate();
    }

    //update the information at the bottom of the lists
    private void updateInfo()
    {
        infoText[0].SetText("Training Available: " + contentList[0].transform.childCount);
        infoText[1].SetText("Training In Selected: " + contentList[1].transform.childCount);
    }

    //set the size of the images so that they fit nicely in the lists 
    public void changeTrainingSize()
    {
        for (short x = 0; x < ImageList.Count; x++)
        {
            ImageList[x].rectTransform.sizeDelta = new Vector2(topControl.rect.width, 61.2F);
        }
    }

    //change to a new focus area and update the lists
    public void changeFocusArea(int area)
    {
        this.focusArea = area;
        listVisualUpdate();
    }
    
    //return true if this training effects the focus area, else false
    private bool changesArea(int place, int area)
    {
        bool inArea = false;
        for (short x = 0; x < business.trainingList[place].affectAreas.Length; x++)
        {
            if (business.trainingList[place].affectAreas[x] == area)
            {
                inArea = true;
            }
        }
        return inArea;
    }

    //get the currently selected employee from the managerGame
    public int getEmployee()
    {
        return employee;
    }

    //return true if the training is being learned, else false
    public bool isFirst(int place)
    {
        if (business.employeesInfo[employee].trainingQ.Count != 0)
        {
            if (business.employeesInfo[employee].trainingQ[0].wants[0] == place)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
