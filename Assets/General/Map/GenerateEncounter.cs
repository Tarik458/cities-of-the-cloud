using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEncounter : MonoBehaviour
{

    public List<encounter> mapGen;
    //public List<environment> mapGen2;



    [System.Serializable]
    public struct encounter
    {
        public int Distance; //Distance used to calculate ETA and resource usage.
        private int ETA; //FOr display purposes and resource calc.
        public int depth; //determines possible leviathan and ruin encounters and what can be harvested.
        public int time; //determines if the match takes place at day or night.
        public int weather; //weather options. rain will be replaced with snow or ice at low temperatures.
        public int tmptr; //temperature, low temperatures reduce efficiency of buildings or replace rain with snow and hail.
        public int wldplts; //food scavange multiplier.
        public int grndmats; //below-ground scavanging multiplier.
        public int windspeed; //wind speed that encounters start with. determines which direction you can move in with sails. 
        public int target; //determines what threat you will encounter based on prefabs.
        public int relicChance; //multiplier, determines chance of finding a relic.
        public int objective;
        public GenerateEncounter.environment hazardSpawns;



    }

    //[System.Serializable]
    public struct environment
    {
        public int spawn; //Determines what environment spawn will occure. Could be nothing, a ruin or couvering grass etc.
        public int position; //Spawn point of the object in question.

    }




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








        mapGen.Add(encounter);

    }
}
