using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    //UI objects
    public Text m_previewTime;
    public Text m_previewCost;
    public Text m_previewDepth;
    public Text m_previewWeather;
    public Text m_previewSpotted;
    public GameObject m_confirmBox;

    [System.Serializable]
    public struct encounter
    {
        public bool isCombat;   //Whether this is a combat encounter
        /* Combat parameters go here */
        public GameObject nonCombatEvent;   //Popup that appears if this is not a combat encounter

        public bool scouting; //Whether the encounter has been scouted
        public int distance; //Distance used to calculate ETA and resource usage. ETA (Days) = Distance(KM) / 50
        public int depth; //determines possible leviathan and ruin encounters and what can be harvested.
        public int time; //determines if the match takes place at day or night.
        public weather weather; //weather options. rain will be replaced with snow or ice at low temperatures.
        public int tmptr; //temperature, low temperatures reduce efficiency of buildings or replace rain with snow and hail.
        public float wldplts; //food scavange multiplier.
        public float grndmats; //below-ground scavanging multiplier.
        public float windspeed; //wind speed that encounters start with. determines which direction you can move in with sails. 
        public int target; //determines what threat you will encounter based on prefabs.
        public float relicChance; //multiplier, determines chance of finding a relic.
        public int objective;

        public List<int> hazardSpawns;
        public List<int> hazardPos;
    }

    public enum weather {
        NONE,
        RAIN,
        SNOW,
        ICE,
        FOG,

        TOTAL
    }
    public List<encounter> m_encounters;
    //What is this stuff for?
    public List<int> ev1;
    public List<int> ev2;
    public int readListNum;

    int m_nextEncounterIndex = 0;

    private void Start() {
        GenerateEncounters();
    }

    public void GenerateEncounters() {
        m_encounters.Clear();
        for (int i = 0; i < 6; i++){
            Generate();
        }
    }
    void Generate()
    {
        ev1.Add(0);
        ev2.Add(0);

        encounter encounter;

        encounter.isCombat = Random.Range(0,1) == 0;
        encounter.nonCombatEvent = GameManager.obj().m_defaultEvent;
        encounter.scouting = true;
        encounter.wldplts = 1f;
        encounter.grndmats = 1f;
        encounter.depth = 1000;
        encounter.distance = 200;
        encounter.time = Random.Range(3,10);
        encounter.weather = (weather)Random.Range(1, (int)weather.TOTAL);
        encounter.tmptr = 5;
        encounter.windspeed = 15f;
        encounter.target = 0;
        encounter.relicChance = 0f;
        encounter.objective = 0;
        encounter.hazardSpawns = ev1;
        encounter.hazardPos = ev2;

        /*Debug.Log(encounter.wldplts);
        Debug.Log(encounter.grndmats);
        Debug.Log(encounter.depth);
        Debug.Log(encounter.distance);
        Debug.Log(encounter.time);
        Debug.Log(encounter.weather);
        Debug.Log(encounter.tmptr);
        Debug.Log(encounter.windspeed);
        Debug.Log(encounter.target);
        Debug.Log(encounter.relicChance);
        Debug.Log(encounter.objective);
        Debug.Log(encounter.hazardSpawns);
        Debug.Log(encounter.hazardPos);*/

        m_encounters.Add(encounter);
    }

    //UI functions
    public void setPreview(int index) //Eventually replace cityscouting with a percentage chance for the scout to succeed, based on number of scouting options the city has. 
    {
        encounter encounter = m_encounters[index];

        int sightings;
        if (encounter.scouting == true) //Eventually needs a chance to randomly select one of them.
        {
            sightings = encounter.target;
        }
        else
        {
            sightings = 256;
        }

        m_previewTime.text = encounter.time.ToString();
        m_previewCost.text = encounter.distance.ToString();
        m_previewDepth.text = encounter.depth.ToString();

        switch (sightings)
        {
            case 5:
                m_previewSpotted.text = "Immense vegetation.";
                break;
            case 4:
                m_previewSpotted.text = "Wreckage";
                break;
            case 3:
                m_previewSpotted.text = "Ruins";
                break;
            case 2:
                m_previewSpotted.text = "Destrigulls";
                break;
            case 1:
                m_previewSpotted.text = "Leviathans";
                break;
            case 0:
                m_previewSpotted.text = "City";
                break;
            default:
                m_previewSpotted.text = "Requires scouting";
                break;
        }

        switch (encounter.weather)
        {
            case weather.RAIN:
                m_previewWeather.text = "Rainy";
                break;
            case weather.SNOW:
                m_previewWeather.text = "Snowy";
                break;
            case weather.ICE:
                m_previewWeather.text = "Icy";
                break;
            default:
                m_previewWeather.text = "Sunny";
                break;
        }
    }
    public void setNextEncounter(int index) {
        m_nextEncounterIndex = index;
    }
    public void toggleConfirmBox() {
        m_confirmBox.SetActive(!m_confirmBox.activeSelf);
    }
    public void nextEncounter() {
        m_confirmBox.SetActive(false);
        GameManager.obj().nextEncounter(m_encounters[m_nextEncounterIndex]);
    }
}
