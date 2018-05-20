using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopStage1 : MonoBehaviour
{
    public TMP_Text[] titles;
    public TMP_Text[] info;
    public TMP_Text[] priceText;
    public Button[] purchase;
    public TMP_InputField focusInput;

    public MainControl mainControl;
    public PlayerInfo player;

    private int focusSelected;
    public int[] displayed;
    private int[] price;
    private Business business;

    // Use this for initialization
    void Start () {
        business = player.business;
        focusSelected = 0;
        displayed = new int[] { -1, -1, -1 };
        price = new int[] { 0, 0, 0, 0 };
        setUp();
        updateFocus("0");
        updateShopDisplay();
    }

    //updates the buttons to useable if there is enough money to purchase the iteam
    private void Update(){
        for (short x = 0; x < 4; x++){
            updatePurchaseButton(x);
        }
    }

    //when someone purchases an iteam remove money and add a new iteam
    public void purchasedClicked(int pressed){
        mainControl.spendMoney(price[pressed]);
        if (pressed == 3){
        player.playerEmployee.focus = player.playerEmployee.focus + focusSelected;
            updateFocus(focusSelected.ToString());
        }
        else{
            int tempNumber = displayed[pressed];
            shopIteam tempIteam = business.iteamList[tempNumber];
            if (tempIteam.follower != -1)
            {
                player.playerEmployee.shopIteamsAvailible.Add(tempIteam.follower);
            }
            for (short x = 0; x < tempIteam.affectAreas.Length; x++)
            {
                player.playerEmployee.focusMultiplyer[tempIteam.affectAreas[x]] = player.playerEmployee.focusMultiplyer[tempIteam.affectAreas[x]] + (tempIteam.affectAmount / 100F);
            }
            player.playerEmployee.shopIteamsAvailible.Remove(displayed[pressed]);
            suffle();
        }

    }

    //set up the availible trainings as well as the display
    private void setUp()
    {
        if (player.playerEmployee.focusMultiplyer[0] == 1 && player.playerEmployee.shopIteamsAvailible.Count == 0)
        {
            for (short x = 0; x < business.iteamList.Length; x++)
            {
                if (business.iteamList[x].active == true)
                {
                    player.playerEmployee.shopIteamsAvailible.Add(x);
                }
            }
        }
        suffle();
    }

    //update the displayes for the training background 
    private void updateShopDisplay()
    {
        for (short x = 0; x < displayed.Length; x++)
        {
            if (displayed[x] == -1)
            {
                titles[x].SetText("None");
                info[x].SetText("out of training");
                priceText[x].SetText("--:--");
                purchase[x].interactable = false;
                price[x] = 0;
            }
            else
            {
                titles[x].SetText(business.iteamList[displayed[x]].name);
                info[x].SetText(business.iteamList[displayed[x]].description);
                priceText[x].SetText(priceSetup(business.iteamList[displayed[x]].cost));
            }
        }
    }

    //suffles the training that is displayed
    public void suffle()
    {
        displayed = new int[] { -1, -1, -1 };
        if (player.playerEmployee.shopIteamsAvailible.Count < 4)
        {
            for (short x = 0; x < player.playerEmployee.shopIteamsAvailible.Count; x++)
            {
                displayed[x] = player.playerEmployee.shopIteamsAvailible[x];
                price[x] = business.iteamList[displayed[x]].cost;
            }
        }
        else
        {
            for (short x = 0; x < 3; x++)
            {
                displayed[x] = getOne();
                price[x] = business.iteamList[displayed[x]].cost;
            }
        }
        updateShopDisplay();
    }

    //method to generate a training that is not being displayed
    private int getOne()
    {
        int intSelect = Random.Range(0, player.playerEmployee.shopIteamsAvailible.Count);
        intSelect = player.playerEmployee.shopIteamsAvailible[intSelect];
        bool isIn = false;
        for (short x = 0; x < 3; x++)
        {
            if (intSelect == displayed[x])
            {
                isIn = true;
            }
        }

        if (isIn == true)
        {
            intSelect = getOne();
        }
        return intSelect;
    }

    //updates the buttons to useable if there is enough money to purchase the iteam
    private void updatePurchaseButton(int place){
        if (price[place] == 0){
            purchase[place].interactable = false;
            return;
        }
        if (price[place] > mainControl.getMoney()){
            purchase[place].interactable = false;
        }
        else{
            purchase[place].interactable = true;
        }
    }

    //checks to see if there is enough focus avalible for the purchase, if there is update the cost and the display
    public void updateFocus(string value){
        //get the focus that the player can still buy
        int availible = 110 - player.playerEmployee.focus;

        //if you cant but more focus the set purchase amount to 0
        if (availible == 0){
            focusInput.text = "0";
            focusSelected = 0;
            price[3] = 0;
            priceText[3].SetText(priceSetup(0));
            updatePurchaseButton(3);
            return;
        }

        //if there is not text in the input set the value to 0
        if (value.Length == 0){
            focusInput.text = "0";
            return;
        }

        //change the input to an int and check if the value is less than 0 if it is make the value positive
        int temp = System.Int32.Parse(value);
        if (temp < 0){
            temp = Mathf.Abs(temp);
        }

        //if the amount in the input is greater that the amount of focus that can be purchased
        if (temp > availible){
            focusInput.text = availible.ToString();
            focusSelected = availible;
            price[3] = availible * 50000;
            priceText[3].SetText(priceSetup(availible * 50000));
            updatePurchaseButton(3);
        }
        else{
            focusInput.text = temp.ToString();
            focusSelected = temp;
            price[3] = temp * 50000;
            priceText[3].SetText(priceSetup(temp * 50000));
            updatePurchaseButton(3);
        }
    }

    //return the displayed amount of money for iteams
    private string priceSetup(int amount){
        var temp1 = Mathf.Floor(amount / 100);
        var temp2 = (amount % 10);
        var temp3 = (((amount - temp2) / 10) % 10);
        return "$" + temp1 + "." + temp3 + temp2;
    }
}
