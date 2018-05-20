using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player")]
public class PlayerInfo : ScriptableObject
{
    public int level = 1;
    public int stage = 1;
    public int sector;
    public int money = 0;
    public int percentEffect = 10;
    public int[] focus = new int[] {0, 0, 0, 0};
    public int[] skillPercent = new int[] { 100, 100, 100, 100, 100 };
    public int[] skillNeeded = new int[] { 10, 10, 10, 10, 10 };
    public float universeControl = 0;
    public float decreaseAmount = 0.1F;
    public float[] skillPoints = new float[] { 0, 0, 0, 0, 0 };
    public bool savedGame = false;
    public Business business;
    public employee playerEmployee;

    public void reset()
    {
        //this.stage = 1;
        this.money = 0;
        this.skillPercent = new int[] { this.level * 100, this.level * 100, this.level * 100, this.level * 100, this.level * 100 };
        this.skillNeeded = new int[] { 10, 10, 10, 10, 10 };
        this.universeControl = 0;
        this.skillPoints = new float[] { 0, 0, 0, 0, 0 };
        this.savedGame = true;
        this.focus = new int[] { 0, 0, 0, 0};
        this.percentEffect = 10;
        this.playerEmployee = new employee("player", 0, business.wage * this.level);
        this.focus[this.playerEmployee.workingIn] = 10;
    }
}
