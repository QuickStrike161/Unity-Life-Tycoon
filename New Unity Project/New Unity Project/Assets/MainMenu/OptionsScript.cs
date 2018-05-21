using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScript : MonoBehaviour {

    /*
     * This controls the options displays, updates the quality and full screen, need to add sound options
     * when a sound track has been added 
     */

    int QValue;
    Resolution[] resolutions;

    public TMP_Dropdown ResolutionDropDown;
    public TMP_Dropdown QualityDropDown;
    public Toggle fullScreen;
    public ManagerGame managerGame;

    void Start(){
        //set up the resolution options based on there computer graphics
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

        //set up the quality resolutions 
        QualityDropDown.value = QualitySettings.GetQualityLevel();
        QualityDropDown.RefreshShownValue();

        //toggle fullscreen on or off depending on there settings 
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

        if (managerGame == null)
        {
            return;
        }

        if (managerGame.isActiveAndEnabled == true)
        {
            managerGame.changeEmployeeSize(-1);
        }
    }

}
