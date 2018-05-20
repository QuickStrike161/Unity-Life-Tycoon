using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Stages : MonoBehaviour
{
    /*
     * this code controls the stage dispaly, updates the dispal when the left or right arrows are clicked,
     * depending on the value of pageNumber a diffrent page will be displayed
     */

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
        if (pageNumber < 21)
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
                TitleText.SetText("Stages");
                InfoText.SetText("In order to prevent the game from becoming boring/grindy each time the player reaches the required stage(see page 2) the player can choose to be reborn into a new reality gaining their character one level. (As the game develops there will be other ways to gain levels) This level will multiply there skills making the next reality faster, for example if a player is level 10 the game will be at least 10 times faster than when they first played. Note the stages for Black Market sector are very different than the other sectors");
                PageText.SetText("1");
                break;
            case 2:
                TitleText.SetText("Stages");
                InfoText.SetText("Employee\nManager\nBusiness Owner\nMayor(level 1)\nPremier(level 2)\nPresident(level 3)\nGlobal Domination(level 4,5)\nSystem Domination(level 6,7)\nGalactic Domination(level 8,9,10)\nUniversal Domination(rest)");
                PageText.SetText("2");
                break;
            case 3:
                TitleText.SetText("Employee");
                InfoText.SetText("This is the first and easiest stage of the game. This stage allows the player to experience the building blocks of the game thus enabling them to better control and grow there empire. During the first game this stage will take approximately an hour, but this will quickly diminish with increased player levels. (At level 10 it will take 6 min)");
                PageText.SetText("3");
                break;
            case 4:
                TitleText.SetText("Employee");
                InfoText.SetText("In this stage the player will be placed into a business within the sector of there choosing and do the \"grunt work\" of the business such as producing goods, mining or engaging with customers. Success in this sector is achieved by serving/producing/creating the required amount of goods. Although simple this stage will still challenge the player by requiring them to work at the various aspects of there business to achieve efficiency. When in this stage your character will make a set wage that they will use to purchase training for themselves to increase productivity.");
                PageText.SetText("4");
                break;
            case 5:
                TitleText.SetText("Manager");
                InfoText.SetText("In the second stage your character has been promoted to a manager. Now in addition to being able to work as an employee you are now in control of hiring employee's, managing merchandise, maintaining the business grounds, and customer relations. As in the first stage this looks different in each of the different sectors.");
                PageText.SetText("5");
                break;
            case 6:
                TitleText.SetText("Manager");
                InfoText.SetText("Success in this stage will be achieved when after you purchase your first business. You do not have to purchase the business you are currently working in but it is recommended because it will be discounted to you. In this stage your character will make a percentage of the profits, this amount is set at 20%.");
                PageText.SetText("6");
                break;
            case 7:
                TitleText.SetText("Business Owner");
                InfoText.SetText("After you purchase your first business the game changes. For the first two stages your character has been very restricted as to what they can do, that is no longer the case you are now in full control of your business and you can do whatever you want with it. You will have to create new strategies for efficiency in order to increase your profit and influence and there are a variety of new paths available to your character.");
                PageText.SetText("7");
                break;
            case 8:
                TitleText.SetText("Business Owner");
                InfoText.SetText("Success in this stage will be achieved when your character has been elected for mayor. You can affect influence(votes) in a number of ways: Your employees will vote for you depending on their happiness, depending on your amount of customers and customer relations, and through your campaign platform. This will take time and you will have to own much of the town in order to become the mayor, meaning that larger towns will take longer to reach this stage.");
                PageText.SetText("8");
                break;
            case 9:
                TitleText.SetText("Mayor");
                InfoText.SetText("Once you have been elected the game will change once again. In addition to running your businesses you must run/maintain your city. You are responsible for city panning which includes designating town zones, road/path construction and government services(schools, hospitals, fire stations). As now you own many businesses within the town it is up to you how you will use your new powers to increase/expand your businesses.");
                PageText.SetText("9");
                break;
            case 10:
                TitleText.SetText("Mayor");
                InfoText.SetText("Success in this stage is achieved when you have been elected as premier(Secretary of state) of the region that you are the mayor in. (Note it is possible to be the mayor of more than one city) Being elected requires influence of the region, influence is gain in the same way as for mayor as well mayor rating will influence as well. At this stage of the game it is possible to purchase towns thus becoming the mayor of them without influence but this is very pricy.");
                PageText.SetText("10");
                break;
            case 11:
                TitleText.SetText("Premier");
                InfoText.SetText("Once you have been elected as premier there will be a few new changes. You are now responsible for infrastructure within your region. Meaning you will now create roads and major building projects within your region. You are also responsible for the development of natural resources, which means that you can now develop new businesses within the extraction sector. You are also responsible for the creation of power which a new area of income.");
                PageText.SetText("11");
                break;
            case 12:
                TitleText.SetText("Premier");
                InfoText.SetText("Success in this stage is achieved when you have been elected as president(Prime-Minister) of the country you are premier in. (Note it is possible to be the premier of more than one region) Being elected requires influence over the country, influence is gain in the same way as for mayor stage as well as your development rating. At this stage of the game it is possible to purchase regions thus becoming the premier of them without influence but this is very pricy.");
                PageText.SetText("12");
                break;
            case 13:
                TitleText.SetText("President");
                InfoText.SetText("Once you have been elected as president you will have to control more than just your finances and influence. The reasurch menu will now be open to you and you will have to be careful to spend your reasurch points wisely. You will also be able to create and control an army as well as maintaining international relations with other countries. You are also able to change tax as well as influence tariffs(tax on imported/exported goods).");
                PageText.SetText("13");
                break;
            case 14:
                TitleText.SetText("President");
                InfoText.SetText("Success in this stage will be achieved when you have full control of all the countries of the world. There are a variety of ways you can gain control of a new country you can do it the same way as you have by owning most of the country and being elected. But you can also gain control through force as well as international relations.");
                PageText.SetText("14");
                break;
            case 15:
                TitleText.SetText("President");
                InfoText.SetText("You will have to learn balance as you will have to maintain the worlds view of you. Some countries you will have to be take with force, but if you are to aggressive the world will see you as a tyrant and will join forces against you. Thus you will have to learn timing and have alliances so that if war comes your will be able to win. Note: just because you are in control of a county does not mean that you own it, to own a country you must purchase all of its businesses.");
                PageText.SetText("15");
                break;
            case 16:
                TitleText.SetText("Global Domination");
                InfoText.SetText("You are now in control of your home planet, but you will soon realize if you want to continue to grow you will have to move beyond the reaches of your world. You must reasurch new tecnologies and start new projects to begin expanding your empire to the stars. You must explore your solar system and you will soon find that the planets around you contain valuable resources that you must claim and extract for your future growth and expansion.");
                PageText.SetText("16");
                break;
            case 17:
                TitleText.SetText("Global Domination");
                InfoText.SetText("Success in this will be achieved when you have claimed all of the planets within your solar system. You can claim a baron planet in a variety of ways, exploration(explore the required amount of the planet with satellites and probes), population(Grow and sustain the required population on the planet), development(set up extraction facilities producing and transporting the required amount of goods to your home planet).");
                PageText.SetText("17");
                break;
            case 18:
                TitleText.SetText("Rest");
                InfoText.SetText("I have not worked out all the details for the last three stages of the game, much of it may change as I work through the other stages of the game. Here are a few of the concepts that I am planning on adding. Continued growth and expansion of your empire and the challenges of space, the discovery and transition to a universal currency. New tecnology and the creation of mega projects. Galactic wars waged between massive civilizations spanning years and galaxies.");
                PageText.SetText("18");
                break;
            case 19:
                TitleText.SetText("Rest");
                InfoText.SetText("There are going to be three types of planets within the game baron, habitable, habited. Most of the planets that you will encounter will be baron planets, this means that the atmosphere does not support human life, some of these planets will still have unintelligent lifeforms living there. Habitable planets will be much rarer, they will occur once in 5-10 solar systems. These planets have no intelligent life but have an environment suitable to humans.");
                PageText.SetText("19");
                break;
            case 20:
                TitleText.SetText("Rest");
                InfoText.SetText("The final type of planets are habited plannets. These will occur a few times in every galaxy, the habitants of these plannets will have varying levels of development and tecnology as well as the growth of there empire, you will have to conquer or influence these people in order to achieve your final goal. These civilizations will be very diffrent then yours and relations with them will unlock new reasurch and buildings. Also some habited plannets will not be habitable for humans.");
                PageText.SetText("20");
                break;
            case 21:
                TitleText.SetText("Rest");
                InfoText.SetText("For this part of the game the entire map will not be discovered, you will have to send out ships and probes into the far reaches of space. These probes will slowly reveal parts of the map to you. The only part of space that is discovered for you is your initial solar system. Finally depending on the tecnologies you choose to discover terraforming will become possible for plannets within the correct distance from the sun to create new habitable plannets.");
                PageText.SetText("21");
                break;
        }
    }
}
