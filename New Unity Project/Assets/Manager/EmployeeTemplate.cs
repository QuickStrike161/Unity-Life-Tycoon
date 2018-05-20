using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmployeeTemplate : MonoBehaviour {

    [SerializeField]
    public Slider Slider;
    public TMP_Text NameText;
    public TMP_Text InfoText;
    public ManagerGame managerGame;
    private int place;

    public void setUp(Business business, int place)
    {
        this.place = place;
        business.employeesInfo[place].infoText = InfoText;
        business.employeesInfo[place].nameText = NameText;
        business.employeesInfo[place].slider = Slider;
    }

    public void onClick()
    {
        managerGame.selectEmployee(this.place);
    }
}
