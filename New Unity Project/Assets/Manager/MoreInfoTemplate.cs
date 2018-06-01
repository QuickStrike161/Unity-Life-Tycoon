using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoreInfoTemplate : MonoBehaviour
{
    /*
     * controls the moreInfo template so that you can select the image to get information about the workstation 
     */


    [SerializeField]
    public TMP_Text NameText;
    public TMP_Text PlaceText;
    public TMP_Text SelectedText;
    public Button button;
    public MoreInfoControl moreInfoControl;
    public PlayerInfo player;
    public employee employee;//will referance and employee or order that it is displaying
    public order order;
    public int place;// the location in the imageList
    public bool on;//if the button is active or not
    public TMP_ColorGradient[] colors;

    private Business business;

    //set up the needed information 
    public void setUp(int place, employee employee, bool on)
    {
        this.place = place;
        this.employee = employee;
        this.on = on;
        PlaceText.SetText(place + 1 + ".");
        NameText.SetText(employee.name);
        SelectedText.SetText("Select");
    }

    public void setUp(int place, order order, bool on)
    {
        this.place = place;
        this.order = order;
        this.on = on;
        PlaceText.SetText(place + 1 + ".");
        NameText.SetText(order.name);
        SelectedText.SetText("Select");
    }

    //tell more info control that the button was clicked
    public void onClick()
    {
        if (on == true)
        {
            moreInfoControl.changeSelected(place);
        }
    }

    //change the color if the button is selected
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
