using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;





public class SkipIntro : MonoBehaviour
{

    //public List<PlayableDirector> playableDirectors;
    //public List<TimelineAsset> timelines;




    public TimelineAsset TmLne; //Timeline here.
    public PlayableDirector timelineHere;
    public string skpky; //skip key here.
    private bool opening;
    private bool keyPressed;
    public GameObject skipText;
    private bool skipNow;




    // Start is called before the first frame update
    void Start()
    {
        opening = true;
        keyPressed = false;
        skipNow = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(skpky) && opening == true && timelineHere.time > 3)
        //{
            //skip the opening
        //    opening = false;
        //    timelineHere.time = 56;
        //}

        if (Input.anyKey && keyPressed == false)
        {
            if (skipNow == true)
            {
                //skip the opening
                opening = false;
                timelineHere.time = 56;
                skipText.SetActive(false);
            }
            else
            {
                keyPressed = true;
                skipNow = true;
                skipText.SetActive(true);
            }


        }


        if (Input.anyKey == false && skipNow == true)
        {
            keyPressed = false;
        }




    }
}
