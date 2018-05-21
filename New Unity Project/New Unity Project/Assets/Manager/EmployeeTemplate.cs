using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmployeeTemplate : MonoBehaviour {

    /*
     * this controls the employee visual in the workstations lists
     */

    [SerializeField]
    public Slider Slider;
    public TMP_Text NameText;
    public TMP_Text InfoText;
    public ManagerGame managerGame;
    private int place;//where the employee is located in the business employee list

    //set up the needed information and add the text and slider to the business employeeInfo
    public void setUp(Business business, int place)
    {
        this.place = place;
        business.employeesInfo[place].infoText = InfoText;
        business.employeesInfo[place].nameText = NameText;
        business.employeesInfo[place].slider = Slider;
    }

    //update when an employee was clicked on
    public void onClick()
    {
        managerGame.selectEmployee(this.place);
    }
}
