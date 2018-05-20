using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmployeeShopTemplate : MonoBehaviour
{

    [SerializeField]
    public TMP_Text NameText;
    public TMP_Text InfoText;
    public TMP_Text buttonText;
    public Button button;
    public ShopEmployee shopEmployee;
    public ShopStage2 shopStage2;
    public PlayerInfo player;
    public MainControl mainControl;
    public int shopIteam;
    public int location;
    public int cost;
    public bool playerSelected;

    private Business business;

    public void setUp(int shopIteam, int location, bool playerSelected)
    {
        business = player.business;
        this.playerSelected = playerSelected;
        this.shopIteam = shopIteam;
        this.location = location;
        if (shopIteam == -1)
        {
            NameText.SetText("Focus");
            InfoText.SetText("Makes employees faster");
            cost = 50000;
        }
        else
        {
            NameText.SetText(business.iteamList[shopIteam].name);
            InfoText.SetText(business.iteamList[shopIteam].description);
            cost = business.iteamList[shopIteam].cost;
        }
        updateButton();
    }

    public void onClick()
    {
        if (playerSelected == true)
        {
            shopStage2.clicked(shopIteam);
        }
        else
        {
            shopEmployee.clicked(shopIteam);
        }
    }

    public void updateButton()
    {
        if (shopIteam == -1)
        {
            buttonText.SetText("Purchase: " + getWage(cost));
        }
        else
        {
            buttonText.SetText("Purchase: " + getWage(cost));
        }
        if (mainControl.getMoney() > cost)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    private string getWage(int wage)
    {
        //set up the string display for the wage
        var temp1 = Mathf.Floor(wage / 100);
        var temp2 = (wage % 10);
        var temp3 = (((wage - temp2) / 10) % 10);
        return "$" + temp1 + "." + temp3 + temp2;
    }
}
