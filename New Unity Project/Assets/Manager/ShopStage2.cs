using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopStage2 : MonoBehaviour
{
    public TMP_Text[] infoText;
    public TMP_Dropdown areaFocus;
    public PlayerInfo player;
    public GameObject[] contentList;
    public Image shopVisual;
    public RectTransform topControl;
    public MainControl mainControl;
    public TMP_InputField nameEnter;
    public ManagerGame managerGame;
    public Button[] buttons;
    
    private int focusArea;
    private string nameForUse;
    private Business business;
    private List<Image> ImageList = new List<Image> { };
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
        areaFocus.RefreshShownValue();

        focusArea = 0;
        
        if (player.playerEmployee.shopIteamsAvailible.Count == 0 && player.playerEmployee.focusMultiplyer[0] == 1)
        {
            for (short x = 0; x < business.iteamList.Length; x++)
            {
                if (business.iteamList[x].active == true)
                {
                    player.playerEmployee.shopIteamsAvailible.Add(x);
                }
            }
        }
        setUp();
    }

    public void setUp()
    {
        foreach (Image image in ImageList)
        {
            contentList[image.GetComponent<EmployeeShopTemplate>().location].transform.DetachChildren();
            Destroy(image.gameObject);
        }
        ImageList = new List<Image> { };

        if (player.playerEmployee.focus < 110)
        {
            Image visual = Instantiate(shopVisual) as Image;
            visual.GetComponent<EmployeeShopTemplate>().setUp(-1, 0, true);
            ImageList.Add(visual);
        }

        for (int x = 0; x < player.playerEmployee.shopIteamsAvailible.Count; x++)
        {
            Image visual = Instantiate(shopVisual) as Image;
            visual.GetComponent<EmployeeShopTemplate>().setUp(player.playerEmployee.shopIteamsAvailible[x], 0, true);
            ImageList.Add(visual);
        }
        listVisualUpdate();
        nameForUse = mainControl.getName();
        hireVisualUpdate();
    }

    void Update()
    {
        hireVisualUpdate();
        //update if the buttons are active
        foreach (Image image in ImageList)
        {
            image.GetComponent<EmployeeShopTemplate>().updateButton();
        }
    }

    private void listVisualUpdate()
    {
        changeTrainingSize();
        foreach (Image image in ImageList)
        {
            if (focusArea == 0)
            {
                image.transform.SetParent(contentList[0].transform, false);
                image.gameObject.SetActive(true);
                image.GetComponent<EmployeeShopTemplate>().location = 0;
            }
            else
            {
                if (changesArea(image.GetComponent<EmployeeShopTemplate>().shopIteam, focusArea - 1) == true)
                {
                    image.transform.SetParent(contentList[0].transform, false);
                    image.gameObject.SetActive(true);
                    image.GetComponent<EmployeeShopTemplate>().location = 0;
                }
                else
                {
                    image.transform.SetParent(contentList[1].transform, false);
                    image.gameObject.SetActive(false);
                    image.GetComponent<EmployeeShopTemplate>().location = 1;
                }
            }
        }
        updateInfo();
    }

    private void hireVisualUpdate()
    {
        infoText[1].SetText("Hire: " + nameForUse);
        if (player.money < 0)
        {
            buttons[2].interactable = false;
        }
        else
        {
            buttons[2].interactable = true;
        }
    }

    public void changeName(string name)
    {
        if (name.Length == 0)
        {
            nameForUse = mainControl.getName();
        }
        else
        {
            nameForUse = name;
        }
        hireVisualUpdate();
    }

    public void clicked(int iteam)
    {
        if (iteam == -1)
        {
            player.playerEmployee.focus = player.playerEmployee.focus + 1;
            mainControl.spendMoney(50000);
        }
        else
        {
            player.playerEmployee.shopIteamsAvailible.Remove(iteam);
            for (short x = 0; x < business.iteamList[iteam].affectAreas.Length; x++)
            {
                int tempInt = business.iteamList[iteam].affectAreas[x];
                player.playerEmployee.focusMultiplyer[tempInt] = player.playerEmployee.focusMultiplyer[tempInt] + (business.iteamList[iteam].affectAmount / 100F);
            }

            if (business.iteamList[iteam].follower != -1)
            {
                player.playerEmployee.shopIteamsAvailible.Add(business.iteamList[iteam].follower);
            }
            mainControl.spendMoney(business.iteamList[iteam].cost);
        }
        setUp();
    }

    private void updateInfo()
    {
        infoText[2].SetText("Past Items Available: " + contentList[0].transform.childCount);
    }

    public void changeTrainingSize()
    {
        for (short x = 0; x < ImageList.Count; x++)
        {
            ImageList[x].rectTransform.sizeDelta = new Vector2(topControl.rect.width, 61.2F);
        }
    }

    public void changeFocusArea(int area)
    {
        this.focusArea = area;
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

    public void hireEmployee()
    {
        int place = -1;
        for(short x = 0; x < business.workStations.Length; x++)
        {
            if (business.workStations[x].EmployeeSpace > 0 && place == -1)
            {
                place = x;
            }
            else if (business.workStations[x].EmployeeSpace == -1 && place == -1)
            {
                place = x;
            }
        }
        managerGame.newEmployee(nameForUse, -1, place);
        nameForUse = mainControl.getName();
        nameEnter.text = "";
    }
}
