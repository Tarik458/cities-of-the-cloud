using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MenuStart : MonoBehaviour
{

    public string skpky; //skip key here.

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(skpky))
        {
            //skip the opening
            SceneManager.LoadScene("MainMenu");

        }
    }



    public void SceneSwitchToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
