using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Future : MonoBehaviour {

    public TMP_Text TitleText;
    public TMP_Text PageText;
    public TMP_Text InfoText;

    public Button MainMenu;
    public Button MoreInfo;

    public GameObject skills;
    public GameObject stages;
    public GameObject donations;
    public GameObject future;

    int pageNumber;

    // Use this for initialization
    void Start () {
        pageNumber = 1;
        updateInfo();
	}

    public void lastPage(){
        if (pageNumber > 1){
            pageNumber = pageNumber - 1;
            updateInfo();
        }
    }

    public void nextPage(){
        if (pageNumber < 17){
            pageNumber = pageNumber + 1;
            updateInfo();
        }
    }

    public void change(){
        if (pageNumber == 7){
            future.SetActive(false);
            skills.SetActive(true);
        }
        else if (pageNumber == 8){
            future.SetActive(false);
            stages.SetActive(true);
        }
        else{
            future.SetActive(false);
            donations.SetActive(true);
        }
    }

    public void updateInfo(){
        if (pageNumber == 7 || pageNumber == 8 || pageNumber == 17){
            MoreInfo.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(false);
        }
        else{
            MainMenu.gameObject.SetActive(true);
            MoreInfo.gameObject.SetActive(false);
        }
        switch(pageNumber){
            case 1:
                TitleText.SetText("Vision");
                InfoText.SetText("I see this game filling an area of games that have yet to be created. It takes building/ management games and builds on them to give players a new way of looking at these games. Current building games end and start in the same time frame, meaning that you spend all your time in one area of time and there is minimal growth and expansion. I want this game to give players the opportunity to incorporate both large amounts of time and exponential growth into there building games. ");
                PageText.SetText("1");
                break;
            case 2:
                TitleText.SetText("Vision");
                InfoText.SetText("This will be achieved by giving the player a multitude of technologies for the player to experience and incorporate into the game. The player will grow from an employee in a small business to enormous empire consisting of galaxies with all the responsibilities attached. In the future I plan to add multiplayer aspects into the game allowing you to compete/align with your friends in games that you create, or to test your skills against players around the world in events. ");
                PageText.SetText("2");
                break;
            case 3:
                TitleText.SetText("Game-Play");
                InfoText.SetText("This game at its core is about finances, because of this there is a variety of factors that will effect your character and what they can do. For example one of the main factors that will effect your character is population. Each county/region/city has a designated population that will restrict your growth. These amounts will change over time, as for population this amount will naturally increase over time, but there are ways that you can influence this change and you must use these.");
                PageText.SetText("3");
                break;
            case 4:
                TitleText.SetText("Game-Play");
                InfoText.SetText("The following are some of the information that you will have to take into consideration:\nPopulation\nAge Demographic\nTecnology\nRaw Resources amount/type\nNet Income\nLiving Costs\nProduction Capacity");
                PageText.SetText("4");
                break;
            case 5:
                TitleText.SetText("Game-Play");
                InfoText.SetText("All of these amounts will influence the decisions your character will have to make, but in the same way your character will be able to influence these amounts. To go back to population, once you have reached the mayor stage you can increase housing to increase your towns population to name one way. These stats will keep the game interesting and ultimately as these amounts will change every time you play, keep you on your feet for many games to come.");
                PageText.SetText("5");
                break;
            case 6:
                TitleText.SetText("Game-Play");
                InfoText.SetText("Something to note, for the purpose of reducing complicity resources will not run out but have a limit to the amount they can produce (abundance) represented by an amount per game second. This amount will increase with new technology and the innovative skill. I want this game to have many parts for the player to work with but to be a simplistic overview of what is actually needed in the real world equivalent. To keep the game interesting without making it frustrating.");
                PageText.SetText("6");
                break;
            case 7:
                TitleText.SetText("Skills");
                InfoText.SetText("Throughout the game there are five skills that your character will be developing. These skills have a variety of affects designed to make the Game faster with both the higher the stage you are in and the level of your character(your character level multiplies these skills) Also these skills give you points that you can spend within the game. For example every 100% in intelligence gives your character 1 reasurch point. Each sector effects the skills differently.");
                PageText.SetText("7");
                break;
            case 8:
                TitleText.SetText("Stages");
                InfoText.SetText("Employee\nManager\nBusiness Owner\nMayor\nPremier\nPresident\nGlobal Domination\nSystem Domination\nGalactic Domination\nUniversal Domination");
                PageText.SetText("8");
                break;
            case 9:
                TitleText.SetText("Training");
                InfoText.SetText("As your character progresses through the stages there will be far to much to control for one player. Because of this I added the \"Training Academy\", this academy will train individuals to control the diffrent stages of the game. This will be set up into the stages, and all stages under your current stage will be available. When you select a stage you can control the amount of people in this stage as well as how much training they receive.");
                PageText.SetText("9");
                break;
            case 10:
                TitleText.SetText("Training");
                InfoText.SetText("Basically you can hire people to do the jobs you use to do so that you don't have to. But this comes with a price, more training will make individuals more efficient but will also cost more, as well as individuals age over time and when they retire you will have to hire new people to take there place. Finally these people will want to be paid for there work, this amount will vary depending on the stage they control.");
                PageText.SetText("10");
                break;
            case 11:
                TitleText.SetText("Technology");
                InfoText.SetText("One of the important aspects of this game is the implementation of technology and the effects that they will have. The game begins with the technologies that already exist, but once your character reaches the President stage the research tab will be open creating new opportunities. As your character increases their intelligence skill each 100% will give them a research point that they can use to develop new  technologies.");
                PageText.SetText("11");
                break;
            case 12:
                TitleText.SetText("Technology");
                InfoText.SetText("As the game grows I want to get feedback from players on technologies that they want to see implemented within the reasurch tree. As time passes I want the technology tree to be large enough that a player may go through the game 10 times and never reasurch the same technology. This will include many sci-fi concepts we know well such as warp drives and terraforming as well as some technologies you have never heard of before.");
                PageText.SetText("12");
                break;
            case 13:
                TitleText.SetText("Map Making");
                InfoText.SetText("Map making is going to be one of my challenges for this game, Until the game has been released on steam I will create a premade world that the players will interact with. But for the final three stages the map must become much larger so I plan on creating a procegially generated world. This posses a problem as the player will be changing the map with development and tecnologies and these changes need to be remembered by the computer.");
                PageText.SetText("13");
                break;
            case 14:
                TitleText.SetText("Map Making");
                InfoText.SetText("Part of the appeal of this game is each stage builds upon the previous stages. This means that the created map has to be versatile enough such that a player can go from galaxies to buildings anywhere in the map. To achieve this there will be a limited amount of buildings, using this the game will keep track of the amounts of  buildings for income an other data and will only generate individual parts of the map when selected by the player.  ");
                PageText.SetText("14");
                break;
            case 15:
                TitleText.SetText("Game Development");
                InfoText.SetText("I want to apologise, I am a single person creating this game, so the creation process will take time, but with your patience and support I hope to make an amazing game in the future. I will try to break down for you some of the ideas and concepts that I have planned for the future. First I will release the Game in stages that you can find on page three. Each stage builds and adds new concepts and challenges for the player to experience.");
                PageText.SetText("15");
                break;
            case 16:
                TitleText.SetText("Game Development");
                InfoText.SetText("Once the game has reached Global domination stage I plan on releasing this game on steam for convenience of the players and the assecciblility. Once on steam I will continue to grow the game stage by stage. After the Final stage has been reached I will switch gears in the development and focus on adding new content to the stages to add depth to the game and start to develop a multiplayer application for the game.");
                PageText.SetText("16");
                break;
            case 17:
                TitleText.SetText("Donations");
                InfoText.SetText("As I have said before I am a single developer so your support is greatly appreciated. As a way of showing my gratitude for any donations I have created a reward system for the top 100 people, varying depending on there position as well as a reward for the first 10 people that donate more than 20$. For all donations greater than 20$ your information will be kept and when the game has been released on steam you will receive a free copy of the game including all future updates.");
                PageText.SetText("17");
                break;
        }
    }
}
