using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{

    public TMP_Text TitleText;
    public TMP_Text PageText;
    public TMP_Text InfoText;

    int pageNumber;

    // Use this for initialization
    void Start()
    {
        pageNumber = 1;
        updateInfo();
    }

    public void lastPage()
    {
        if (pageNumber > 1)
        {
            pageNumber = pageNumber - 1;
            updateInfo();
        }
    }

    public void nextPage()
    {
        if (pageNumber < 7)
        {
            pageNumber = pageNumber + 1;
            updateInfo();
        }
    }

    public void updateInfo()
    {
        switch (pageNumber)
        {
            case 1:
                TitleText.SetText("Skills");
                InfoText.SetText("The skills are:\nManagement(employee speed)\nInnovative(goods / resources created)\nIntelligence(The speed of training / research)\nQuality(value of goods / resources)\nStrategic(battle rating)");
                PageText.SetText("1");
                break;
            case 2:
                TitleText.SetText("Skills");
                InfoText.SetText("The next pages breakdown how the different sectors affect the skills. For example Management(2) means that this sector effects the management skill and has a multiplier of 2. So if you are in this sector your workers will be more efficient.");
                PageText.SetText("2");
                break;
            case 3:
                TitleText.SetText("Service:");
                InfoText.SetText("Management(2)\nIntelligence(1)\nQuality(0.5)\nStrategic(0.5)");
                PageText.SetText("3");
                break;
            case 4:
                TitleText.SetText("Extraction:");
                InfoText.SetText("Innovative(2)\nQuality(1)\nManagement(0.5)\nStrategic(0.5)");
                PageText.SetText("4");
                break;
            case 5:
                TitleText.SetText("Processing:");
                InfoText.SetText("Quality(2)\nInnovative(1)\nManagement(0.5)\nStrategic(0.5)");
                PageText.SetText("5");
                break;
            case 6:
                TitleText.SetText("Black Market:");
                InfoText.SetText("Strategic(3)\nIntelligence(2)\nManagement(1)\nQuality(0.5)");
                PageText.SetText("6");
                break;
            case 7:
                TitleText.SetText("Skills");
                InfoText.SetText("Because of these amounts, your character will have to expand to the different sectors of the game at higher stages so that they can become well-versed in all of the skills.");
                PageText.SetText("7");
                break;
        }
    }
}
