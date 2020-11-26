using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;





public class MainMenuScript : MonoBehaviour
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

    public void startGame()
    {
        SceneManager.LoadScene("Cloud World");
    }


    public void GoToMenu()
    {
        activator.SetActive(true);
        timelineOff.SetActive(false);
    }

}
