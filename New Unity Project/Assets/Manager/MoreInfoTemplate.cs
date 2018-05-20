using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoreInfoTemplate : MonoBehaviour
{

    [SerializeField]
    public TMP_Text NameText;
    public TMP_Text PlaceText;
    public TMP_Text SelectedText;
    public Button button;
    public MoreInfoControl moreInfoControl;
    public PlayerInfo player;
    public employee employee;
    public order order;
    public int place;
    public bool on;
    public TMP_ColorGradient[] colors;

    private Business business;

    public void setUp(int place, employee employee, bool on)
    {
        this.place = place;
        this.employee = employee;
        this.on = on;
        PlaceText.SetText(place + 1 + ".");
        NameText.SetText(employee.name);
        SelectedText.SetText("Select");
    }

    public void onClick()
    {
        if (on == true)
        {
            moreInfoControl.changeSelected(place);
        }
    }

    public void setColor(bool selected)
    {
        if (on == true)
        {
            if (selected == true)
            {
                SelectedText.SetText("Selected");
                SelectedText.colorGradientPreset = colors[0];
            }
            else
            {
                SelectedText.SetText("Select");
                SelectedText.colorGradientPreset = colors[1];
            }
        }
        else
        {
            SelectedText.SetText("");
        }
    }
}
