using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MapBuildSwitch : MonoBehaviour
{


    public GameObject BuildMode;
    public GameObject MapMode;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SwitchToBuild()
    {
        MapMode.SetActive(false);
        BuildMode.SetActive(true);


    }


    public void SwitchToMap()
    {
        MapMode.SetActive(true);
        BuildMode.SetActive(false);


    }




}
