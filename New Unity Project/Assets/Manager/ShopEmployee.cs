using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopEmployee : MonoBehaviour
{
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

    public void setUp(int employee)
    {
        foreach (Image image in ImageList)
        {
            contentList[image.GetComponent<EmployeeShopTemplate>().location].transform.DetachChildren();
            Destroy(image.gameObject);
        }
        ImageList = new List<Image> { };

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

        if (business.employeesInfo[employee].focus < 50)
        {
            Image visual1 = Instantiate(trainingVisual) as Image;
            visual1.GetComponent<EmployeeShopTemplate>().setUp(-1, 0, false);
            ImageList.Add(visual1);

            Image visual2 = Instantiate(trainingVisual) as Image;
            visual2.GetComponent<EmployeeShopTemplate>().setUp(-1, 1, false);
            ImageList.Add(visual2);
        }

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

    private void listVisualUpdate()
    {
        changeTrainingSize();
        foreach(Image image in ImageList)
        {
            if (image.GetComponent<EmployeeShopTemplate>().location == 0)
            {
                if (focusArea1 == 0)
                {
                    image.transform.SetParent(contentList[0].transform, false);
                    image.gameObject.SetActive(true);
                }
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
            else
            {
                if (focusArea2 == 0)
                {
                    image.transform.SetParent(contentList[1].transform, false);
                    image.gameObject.SetActive(true);
                }
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

    public void clicked(int iteam)
    {
        if (iteam == -1)
        {
            business.employeesInfo[employee].focus = business.employeesInfo[employee].focus + 1;
            mainControl.spendMoney(50000);
        }
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

    private void updateInfo()
    {
        infoText[0].SetText("Iteams Available: " + contentList[0].transform.childCount);
        infoText[1].SetText("Iteams Available: " + contentList[1].transform.childCount);
    }

    public void changeTrainingSize()
    {
        for (short x = 0; x < ImageList.Count; x++)
        {
            ImageList[x].rectTransform.sizeDelta = new Vector2(topControl.rect.width, 61.2F);
        }
    }

    public void changeFocusArea1(int area)
    {
        this.focusArea1 = area;
        listVisualUpdate();
    }

    public void changeFocusArea2(int area)
    {
        this.focusArea2 = area;
        listVisualUpdate();
    }

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
