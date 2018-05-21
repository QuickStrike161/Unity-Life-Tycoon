using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopEmployee : MonoBehaviour
{

    /*
     * this controls the employee shop for purchasing items and focus  
     */

    public TMP_Text[] infoText;
    public TMP_Dropdown[] areaFocus;
    public PlayerInfo player;
    public GameObject[] contentList;
    public Image trainingVisual;
    public RectTransform topControl;
    public MainControl mainControl;

    private int employee;
    private int focusArea1;
    private int focusArea2;
    private float timeSec;
    private Business business;
    private List<Image> ImageList = new List<Image> { };
    
    //set values and set up the two focus dropDowns 
    void Start()
    {
        business = player.business;
        foreach(TMP_Dropdown dropdown in areaFocus)
        {
            dropdown.ClearOptions();
            List<string> options = new List<string>();
            options.Add("All Stations");
            for (int x = 1; x < 5; x++)
            {
                options.Add(business.workStations[x - 1].name);
            }
            dropdown.AddOptions(options);
            dropdown.value = 0;
            dropdown.RefreshShownValue();
        }
        focusArea1 = 0;
        focusArea2 = 0;
        timeSec = 0;
    }

    //update the lists and turn on the item's buttons if there is enough money to purchase
    void Update()
    {
        //create a loop that is called ever secound
        float tempTime = Time.fixedTime % 1;
        if (tempTime < timeSec)
        {
            listVisualUpdate();
        }
        timeSec = tempTime;

        //update if the buttons are active
        foreach (Image image in ImageList)
        {
            image.GetComponent<EmployeeShopTemplate>().updateButton();
        }
    }

    //set up the visuals for the two item lists
    public void setUp(int employee)
    {
        //remove any images from before
        foreach (Image image in ImageList)
        {
            contentList[image.GetComponent<EmployeeShopTemplate>().location].transform.DetachChildren();
            Destroy(image.gameObject);
        }
        ImageList = new List<Image> { };

        //if the employee has not had the items availible added add them
        business = player.business;
        this.employee = employee;
        if (business.employeesInfo[employee].shopIteamsAvailible.Count == 0 && business.employeesInfo[employee].focusMultiplyer[0] == 1)
        {
            for (short x = 0; x < business.iteamList.Length; x++)
            {
                if (business.iteamList[x].active == true)
                {
                    business.employeesInfo[employee].shopIteamsAvailible.Add(x);
                }
            }
        }

        //add focus to the images if the employee has less than 50
        if (business.employeesInfo[employee].focus < 50)
        {
            Image visual1 = Instantiate(trainingVisual) as Image;
            visual1.GetComponent<EmployeeShopTemplate>().setUp(-1, 0, false);
            ImageList.Add(visual1);

            Image visual2 = Instantiate(trainingVisual) as Image;
            visual2.GetComponent<EmployeeShopTemplate>().setUp(-1, 1, false);
            ImageList.Add(visual2);
        }

        //add the iteams that are availible to both lists
        for (int x = 0; x < business.employeesInfo[employee].shopIteamsAvailible.Count; x++)
        {
            Image visual1 = Instantiate(trainingVisual) as Image;
            visual1.GetComponent<EmployeeShopTemplate>().setUp(business.employeesInfo[employee].shopIteamsAvailible[x], 0, false);
            ImageList.Add(visual1);

            Image visual2 = Instantiate(trainingVisual) as Image;
            visual2.GetComponent<EmployeeShopTemplate>().setUp(business.employeesInfo[employee].shopIteamsAvailible[x], 1, false);
            ImageList.Add(visual2);
        }
        listVisualUpdate();
    }

    //update the two lists visual so everything is up to date
    private void listVisualUpdate()
    {
        changeTrainingSize();
        foreach(Image image in ImageList)
        {
            //if the items are in the first list
            if (image.GetComponent<EmployeeShopTemplate>().location == 0)
            {
                //if there is no focus area for the first list make all visable
                if (focusArea1 == 0)
                {
                    image.transform.SetParent(contentList[0].transform, false);
                    image.gameObject.SetActive(true);
                }
                //if there is a focus area for the first list make the relavent images visable 
                else
                {
                    if (changesArea(image.GetComponent<EmployeeShopTemplate>().shopIteam, focusArea1 - 1) == true)
                    {
                        image.transform.SetParent(contentList[0].transform, false);
                        image.gameObject.SetActive(true);
                    }
                    else
                    {
                        image.transform.SetParent(contentList[2].transform, false);
                        image.gameObject.SetActive(false);
                    }
                }
            }
            //if the items are in the second list
            else
            {
                //if there is no focus area for the second list make all visable
                if (focusArea2 == 0)
                {
                    image.transform.SetParent(contentList[1].transform, false);
                    image.gameObject.SetActive(true);
                }
                //if there is a focus area for the second list make the relavent images visable 
                else
                {
                    if (changesArea(image.GetComponent<EmployeeShopTemplate>().shopIteam, focusArea2 - 1) == true)
                    {
                        image.transform.SetParent(contentList[1].transform, false);
                        image.gameObject.SetActive(true);
                    }
                    else
                    {
                        image.transform.SetParent(contentList[2].transform, false);
                        image.gameObject.SetActive(false);
                    }
                }
            }
        }
        updateInfo();
    }

    //update when an item has been purchased, add the benifits and remove from both lists, and remove the cost of the items
    public void clicked(int iteam)
    {
        //if it is focus add the focus
        if (iteam == -1)
        {
            business.employeesInfo[employee].focus = business.employeesInfo[employee].focus + 1;
            mainControl.spendMoney(50000);
        }
        //remove the item active any followers and add the benifits 
        else
        {
            business.employeesInfo[employee].shopIteamsAvailible.Remove(iteam);
            for (short x = 0; x < business.iteamList[iteam].affectAreas.Length; x++)
            {
                int tempInt = business.iteamList[iteam].affectAreas[x];
                business.employeesInfo[employee].focusMultiplyer[tempInt] = business.employeesInfo[employee].focusMultiplyer[tempInt] + (business.iteamList[iteam].affectAmount / 100F);
            }

            if (business.iteamList[iteam].follower != -1)
            {
                business.employeesInfo[employee].shopIteamsAvailible.Add(business.iteamList[iteam].follower);
            }
            mainControl.spendMoney(business.iteamList[iteam].cost);
        }
        setUp(employee);
    }

    //update the info at the bottom of the lists
    private void updateInfo()
    {
        infoText[0].SetText("Iteams Available: " + contentList[0].transform.childCount);
        infoText[1].SetText("Iteams Available: " + contentList[1].transform.childCount);
    }

    //change the size of the images to fit nicely in the lists
    public void changeTrainingSize()
    {
        for (short x = 0; x < ImageList.Count; x++)
        {
            ImageList[x].rectTransform.sizeDelta = new Vector2(topControl.rect.width, 61.2F);
        }
    }

    //update the focus area for the first list
    public void changeFocusArea1(int area)
    {
        this.focusArea1 = area;
        listVisualUpdate();
    }

    //update the focus area for the second list
    public void changeFocusArea2(int area)
    {
        this.focusArea2 = area;
        listVisualUpdate();
    }

    //return true if the item affects the area, else false
    private bool changesArea(int place, int area)
    {
        if (place == -1)
        {
            return true;
        }
        else
        {
            bool inArea = false;
            for (short x = 0; x < business.iteamList[place].affectAreas.Length; x++)
            {
                if (business.iteamList[place].affectAreas[x] == area)
                {
                    inArea = true;
                }
            }
            return inArea;
        }
    }
}
