using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class MainMenuScript : MonoBehaviour
{
    public GameObject MenuActivator;
    public GameObject timelineOff;
    //public List<GameObject> menuButtons;
    public GameObject backButton;
    public GameObject Prologue;
    private GameObject Listbutton;
    public string SceneSwitch;


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
        SceneManager.LoadScene(SceneSwitch);
    }


    public void GoToMenu()
    {
        MenuActivator.SetActive(true);
        timelineOff.SetActive(false);
        Prologue.SetActive(false);
        backButton.SetActive(false);


        //foreach (GameObject Listbutton in menuButtons)
        //{
        //    Listbuton.SetActive(true);
        //}
        //backButton.SetActive(false);

    }


    public void GoToPrologue()
    {
        MenuActivator.SetActive(false);
        backButton.SetActive(true);
        Prologue.SetActive(true);


    }


    public void QuiteGameOption()
    {
        Application.Quit();
    }


    public void SceneSwitchToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }









}
