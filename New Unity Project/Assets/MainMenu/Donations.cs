using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Donations : MonoBehaviour
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
        if (pageNumber < 10)
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
                TitleText.SetText("Donations");
                InfoText.SetText("Any names created by donators have to be appropriate for younger players and will be seen by everyone that plays the game. Also any donators here will receive my private email to directly contact me with any ideas that they would like to see in the game. Any titles are a one time thing and will never be given again in the game. All the following items will be given when the game is realise on steam.");
                PageText.SetText("1");
                break;
            case 2:
                TitleText.SetText("First Place");
                InfoText.SetText("Character level 40\n Antimatter title\n Listed in credits as 1st\n Name the first planet\n Name 4 countries\n Name and concept for 4 alien races\n Custom skin made for character\n First pick for character name");
                PageText.SetText("2");
                break;
            case 3:
                TitleText.SetText("Secound Place");
                InfoText.SetText("Character level 30\n Californium title\n Listed in credits as 2nd\n Name 3 countries\n Name and concept for 3 alien races\n Custom skin made for character\n Second pick for character name");
                PageText.SetText("3");
                break;
            case 4:
                TitleText.SetText("Third Place");
                InfoText.SetText("Character level 25\n Diamond title\n Listed in credits as 3rd\n Name 2 countries\n Name and concept for 2 alien races\n Custom skin made for character\n Third pick for character name");
                PageText.SetText("4");
                break;
            case 5:
                TitleText.SetText("Fourth to Tenth Place");
                InfoText.SetText("Character level 20\n Platinum title\n Listed in credits as Platinum \n Name 1 countries\n Name and concept for 1 alien races\n Top picks for character name");
                PageText.SetText("5");
                break;
            case 6:
                TitleText.SetText("11 - 25 Place");
                InfoText.SetText("Character level 15\n Gold title\n Listed in credits as Gold\n Top picks for character name");
                PageText.SetText("6");
                break;
            case 7:
                TitleText.SetText("26 - 50 Place");
                InfoText.SetText("Character level 10\n Silver title\n Listed in credits as Silver\n Top picks for character name");
                PageText.SetText("7");
                break;
            case 8:
                TitleText.SetText("51 - 100 Place");
                InfoText.SetText("Character level 5\n Bronze title\n Listed in credits as Bronze\n Top picks for character name");
                PageText.SetText("8");
                break;
            case 9:
                TitleText.SetText("First 10 Donators");
                InfoText.SetText("Character level 10\n Special thanks in credits\n Top picks for character name");
                PageText.SetText("9");
                break;
            case 10:
                TitleText.SetText("First 10 Donators");
                InfoText.SetText("Each Character level will take 2-4 weeks to complete making receiving these character levels a huge boost when the game is released.");
                PageText.SetText("10");
                break;
        }
    }
}
