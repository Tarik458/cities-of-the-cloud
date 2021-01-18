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
    public GameObject snowSFX;
    public GameObject rainSFX;
    public GameObject fogSFX;
    public GameObject iceSFX;
    public GameObject stormSFX;

    //Events
    [BoxGroup ("Events")]
    public GameObject m_defaultEvent;
    [BoxGroup("Events")]
    public GameObject m_combatPlaceholder;

    EncounterManager.encounter m_curEncounter;

    //Other
    public CombatManager m_combat;

    //Debug
    [TitleGroup ("Debug")]
    public bool m_startCombat = false;

    void Start()
    {
        m_uiCover = m_mainCanvas.transform.Find("Cover").GetComponent<Image>();
        m_uiCover.CrossFadeAlpha(1f, 0f, true);
        m_uiCover.gameObject.SetActive(true);

        if (m_combat == null) m_combat = GameObject.FindObjectOfType<CombatManager>();

        //Load city

        //Debug
        m_curEncounter.isCombat = m_startCombat;
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
        rainSFX.SetActive(false);
        fogSFX.SetActive(false);
        snowSFX.SetActive(false);
        iceSFX.SetActive(false);
        stormSFX.SetActive(false);
        switch (m_curEncounter.weather)
        {
            case EncounterManager.weather.RAIN:
                if (m_curEncounter.tmptr < -10)
                {
                    iceSFX.SetActive(true);
                }
                else if (m_curEncounter.tmptr < 0)
                {
                    snowSFX.SetActive(true);
                }
                else
                {
                    rainSFX.SetActive(true);
                }
                break;
            case EncounterManager.weather.CLOUD:
                //snowSFX.SetActive(true);
                break;
            case EncounterManager.weather.STORM:
                stormSFX.SetActive(true);
                break;
            case EncounterManager.weather.FOG:
                fogSFX.SetActive(true);
                break;
            default:
                rainSFX.SetActive(false);
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
            //LeanPool.Spawn(m_combatPlaceholder, m_popupParent);
            m_combat.StartCombat();
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
