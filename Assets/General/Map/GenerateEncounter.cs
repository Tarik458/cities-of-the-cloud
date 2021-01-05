using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class GenerateEncounter : MonoBehaviour
{

    public List<encounter> mapGen;
    //public List<environment> mapGen2;



    [System.Serializable]
    public struct encounter
    {
        public int Distance; //Distance used to calculate ETA and resource usage. ETA (Days) = Distance(KM) / 50
        public int depth; //determines possible leviathan and ruin encounters and what can be harvested.
        public int time; //determines if the match takes place at day or night.
        public int weather; //weather options. rain will be replaced with snow or ice at low temperatures.
        public int tmptr; //temperature, low temperatures reduce efficiency of buildings or replace rain with snow and hail.
        public float wldplts; //food scavange multiplier.
        public float grndmats; //below-ground scavanging multiplier.
        public float windspeed; //wind speed that encounters start with. determines which direction you can move in with sails. 
        public int target; //determines what threat you will encounter based on prefabs.
        public float relicChance; //multiplier, determines chance of finding a relic.
        public int objective;
        //public GenerateEncounter.environment hazardSpawns;
        public List<int> hazardSpawns;
        public List<int> hazardPos;


    }



    public List<int> ev1;
    public List<int> ev2;



    public int readListNum;





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Generate()
    {

        //var envGen = new environment();

        //envGen.GetType().GetField("spawn").SetValueDirect(__makeref(envGen), 0);
        //System.Console.WriteLine(envGen.spawn); //Prints 5

        //envGen.GetType().GetField("position").SetValueDirect(__makeref(envGen), 0);
        //System.Console.WriteLine(envGen.position); //Prints 5


        ev1.Add(0);
        ev2.Add(0);







        var newIcon = new encounter();



        newIcon.GetType().GetField("wldplts").SetValueDirect(__makeref(newIcon), 1.0);
        System.Console.WriteLine(newIcon.wldplts); //Prints 5

        newIcon.GetType().GetField("grndmats").SetValueDirect(__makeref(newIcon), 1.0);
        System.Console.WriteLine(newIcon.grndmats); //Prints 5

        newIcon.GetType().GetField("depth").SetValueDirect(__makeref(newIcon), 1000);
        System.Console.WriteLine(newIcon.depth); //Prints 5

        newIcon.GetType().GetField("Distance").SetValueDirect(__makeref(newIcon), 200);
        System.Console.WriteLine(newIcon.Distance); //Prints 5

        newIcon.GetType().GetField("time").SetValueDirect(__makeref(newIcon), 6);
        System.Console.WriteLine(newIcon.time); //Prints 5

        newIcon.GetType().GetField("weather").SetValueDirect(__makeref(newIcon), 5);
        System.Console.WriteLine(newIcon.weather); //Prints 5

        newIcon.GetType().GetField("tmptr").SetValueDirect(__makeref(newIcon), 5);
        System.Console.WriteLine(newIcon.tmptr); //Prints 5

        newIcon.GetType().GetField("windspeed").SetValueDirect(__makeref(newIcon), 15.0);
        System.Console.WriteLine(newIcon.windspeed); //Prints 5

        newIcon.GetType().GetField("target").SetValueDirect(__makeref(newIcon), 0);
        System.Console.WriteLine(newIcon.target); //Prints 5

        newIcon.GetType().GetField("relicChance").SetValueDirect(__makeref(newIcon), 0.0);
        System.Console.WriteLine(newIcon.relicChance); //Prints 5

        newIcon.GetType().GetField("objective").SetValueDirect(__makeref(newIcon), 0);
        System.Console.WriteLine(newIcon.objective); //Prints 5

        newIcon.GetType().GetField("hazardSpawns").SetValueDirect(__makeref(newIcon), ev1);
        System.Console.WriteLine(newIcon.hazardSpawns); //Prints 5

        newIcon.GetType().GetField("hazardPos").SetValueDirect(__makeref(newIcon), ev2);
        System.Console.WriteLine(newIcon.hazardPos); //Prints 5





        mapGen.Add(newIcon);

    }


    public void SetButton1()

    {
        setPreview(0, false);
    }

    public void SetButton2()

    {
        setPreview(1, false);
    }

    public void SetButton3()

    {
        setPreview(2, false);
    }

    public void SetButton4()

    {
        setPreview(3, false);
    }

    public void SetButton5()

    {
        setPreview(4, false);
    }

    public void SetButton6()

    {
        setPreview(5, false);
    }


    public void setPreview(int iconNumber, bool cityScouting) //Eventually replace cityscouting with a percentage chance for the scout to succeed, based on number of scouting options the city has. 
    {


        var printTime = new int();
        var printCost = new int();
        var printWeather = new int();
        var printDepth = new int();
        var printSightings = new int();




        //switch (readListNum)



        printDepth = mapGen[iconNumber].depth;


        printTime = mapGen[iconNumber].Distance; //Find a way to divide by 50.


        printCost = 25; //Must eventually be "people*time" or something.


        printWeather = mapGen[iconNumber].weather;


        if (cityScouting == true) //Eventually needs a chance to randomly select one of them.
        {

            printSightings = mapGen[iconNumber].target;


            //newIcon.GetType().GetField("objective")


            //newIcon.GetType().GetField("hazardSpawns")



        }
        else
        {
            printSightings = 256;
        }


        
        GameObject.Find("Time").GetComponent<Text>().text = printTime.ToString();

        GameObject.Find("Cost").GetComponent<Text>().text = printCost.ToString();
        
        GameObject.Find("Dep").GetComponent<Text>().text = printDepth.ToString();

        GameObject.Find("weather").GetComponent<Text>().text = printWeather.ToString();




        

        switch (printSightings)
        {
            case 5:
                GameObject.Find("Spotted").GetComponent<Text>().text = "Immense vegetation.";
                break;
            case 4:
                GameObject.Find("Spotted").GetComponent<Text>().text = "Wreackage";
                break;
            case 3:
                GameObject.Find("Spotted").GetComponent<Text>().text = "Ruins";
                break;
            case 2:
                GameObject.Find("Spotted").GetComponent<Text>().text = "Destrigulls";
                break;
            case 1:
                GameObject.Find("Spotted").GetComponent<Text>().text = "Leviathans";
                break;
            case 0:
                GameObject.Find("Spotted").GetComponent<Text>().text = "City";
                break;
            default:
                GameObject.Find("Spotted").GetComponent<Text>().text = "Requires scouting";
                break;
        }






        switch (printWeather)
        {
            case 3:
                GameObject.Find("Weath").GetComponent<Text>().text = "Stormy";
                break;
            case 2:
                GameObject.Find("Weath").GetComponent<Text>().text = "Rainy";
                break;
            case 1:
                GameObject.Find("Weath").GetComponent<Text>().text = "Foggy";
                break;
            default:
                GameObject.Find("Weath").GetComponent<Text>().text = "Sunny";
                break;
        }






    }


}
