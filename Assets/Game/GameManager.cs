using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Pool;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    //Ensure singleton
    static GameManager s_this = null;
    public static GameManager obj() {
        return s_this;
    }
    private void Awake() {
        if(s_this != null) Destroy(this);        
        s_this = this;
    }

    //UI objects
    public CanvasGroup m_mainCanvas;
    public Canvas m_buildCanvas;
    public Transform m_popupParent;
    public GameObject m_map;
    Image m_uiCover;

    //Cloud graphics
    public List<ParticleSystem> m_clouds;
    public GameObject snowSfx;
    public GameObject rainSfx;
    public GameObject fogSfx;
    public GameObject iceSfx;

    //Events
    [BoxGroup ("Events")]
    public GameObject m_defaultEvent;
    [BoxGroup("Events")]
    public GameObject m_combatPlaceholder;

    EncounterManager.encounter m_curEncounter;

    void Start()
    {
        m_uiCover = m_mainCanvas.transform.Find("Cover").GetComponent<Image>();
        m_uiCover.CrossFadeAlpha(1f, 0f, true);
        m_uiCover.gameObject.SetActive(true);

        //Load city

        //Load encounter
        StartCoroutine(startEncounter());
    }

    public void nextEncounter(EncounterManager.encounter next) {
        m_curEncounter = next;
        m_map.SetActive(false);

        StartCoroutine(nextEncounter());
    }
    IEnumerator nextEncounter() {
        //play animation
        float animLength = 1f;
        m_uiCover.CrossFadeAlpha(0f, 0f, true);
        m_uiCover.gameObject.SetActive(true);
        m_uiCover.CrossFadeAlpha(1f, animLength, false);
        yield return new WaitForSeconds(animLength);

        StartCoroutine(startEncounter());
    }
    IEnumerator startEncounter() {
        //setup city + UI

        //setup environment
        //Color col = Color.red;
        //switch (m_curEncounter.weather) {
        //    case EncounterManager.weather.RAIN:
        //        col = Color.green;
        //        break;
        //    case EncounterManager.weather.SNOW:
        //        col = Color.white;
        //        break;
        //    case EncounterManager.weather.ICE:
        //        col = Color.cyan;
        //        break;
        //    default:
        //        col = Color.blue;
        //        break;
        //}
        //foreach(ParticleSystem i in m_clouds) {
        //    i.startColor = col;
        //    i.time = 0;
        //}


        //SwitchWeather
        rainSfx.SetActive(false);
        fogSfx.SetActive(false);
        snowSfx.SetActive(false);
        iceSfx.SetActive(false);
        switch (m_curEncounter.weather)
        {
            case EncounterManager.weather.RAIN:
                rainSfx.SetActive(true);
                break;
            case EncounterManager.weather.SNOW:
                snowSfx.SetActive(true);
                break;
            case EncounterManager.weather.ICE:
                iceSfx.SetActive(true);
                break;
            case EncounterManager.weather.FOG:
                fogSfx.SetActive(true);
                break;
            default:
                rainSfx.SetActive(false);
                break;
        }










        //play animation
        float animLength = 1f;
        m_uiCover.CrossFadeAlpha(0f, animLength, false);
        yield return new WaitForSeconds(animLength);
        //finished
        m_uiCover.gameObject.SetActive(false);

        //combat?
        if (m_curEncounter.isCombat) {
            LeanPool.Spawn(m_combatPlaceholder, m_popupParent);
        }
        //event
        else {
            if (m_curEncounter.nonCombatEvent != null) {
                LeanPool.Spawn(m_curEncounter.nonCombatEvent, m_popupParent);
            }
            else {
                LeanPool.Spawn(m_defaultEvent, m_popupParent);
            }
        }
    }

    //UI functions
    public void toggleMap() {
        m_map.SetActive(!m_map.activeSelf);
    }    
}
