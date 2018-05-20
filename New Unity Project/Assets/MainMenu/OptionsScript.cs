using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScript : MonoBehaviour {

    int QValue;
    Resolution[] resolutions;

    public TMP_Dropdown ResolutionDropDown;
    public TMP_Dropdown QualityDropDown;
    public Toggle fullScreen;
    public ManagerGame managerGame;

    void Start(){
        resolutions = Screen.resolutions;
        ResolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentRes = 0;
        for(int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height){
                currentRes = i;
            }
        }

        ResolutionDropDown.AddOptions(options);
        ResolutionDropDown.value = currentRes;
        ResolutionDropDown.RefreshShownValue();

        QualityDropDown.value = QualitySettings.GetQualityLevel();
        QualityDropDown.RefreshShownValue();

        if (Screen.fullScreen == true){
            fullScreen.isOn = true;
        }
        else{
            fullScreen.isOn = false;
        }
    }

    public void setQuality(int value){

        QualitySettings.SetQualityLevel(value);
    }

    public void fullS(bool input){
        Screen.fullScreen = input;
    }

    public void setRes(int value){
        Resolution res = resolutions[value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

        if (managerGame.isActiveAndEnabled == true)
        {
            managerGame.changeEmployeeSize(-1);
        }
    }

}
