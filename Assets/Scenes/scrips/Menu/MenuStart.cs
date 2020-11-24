using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public GameObject activator;
    public GameObject timelineOff;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void GoToMenu()
    {
        activator.SetActive(true);
        timelineOff.SetActive(false);
    }
}
