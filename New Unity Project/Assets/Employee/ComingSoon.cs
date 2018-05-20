using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComingSoon : MonoBehaviour {

    public GameObject pannel;

    private int time;

    private void Start()
    {
        time = 200;
    }

    // Update is called once per frame
    void Update () {
        time = time - 1;
        if (time == 0){
            time = 200;
            pannel.SetActive(false);
        }
	}
}
