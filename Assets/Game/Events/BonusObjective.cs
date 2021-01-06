using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class BonusObjective : MonoBehaviour
{

    public string mainRewardType;
    public int rewardMin;
    public int rewardMax;

    public string secondaryReward;
    public int secondaryMin;
    public int secondaryMax;


    public Text m_previewSpotted;
    public string description;

    private bool canClose;


    public GameObject theButton;


    // Start is called before the first frame update
    void Start()
    {
        canClose = false;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void reward()
    {

        if (canClose == false)
        {
            m_previewSpotted.text = description; //Eventually will need to add the specific rewards gained.
                                                 //Apply random reward between min and max- or penalty, in some cases.
            theButton.SetActive(false);

        }
        else
        {


            //self.SetActive(false);


        }
        

    }

 


}
