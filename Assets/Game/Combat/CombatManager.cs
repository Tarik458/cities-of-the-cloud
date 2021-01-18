using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CombatManager : MonoBehaviour
{
    public Camera m_combatCam;
    Camera m_mainCam;

    public GameObject m_UI;
    public Text m_rangeText;

    bool m_combatActive = false;

    //Range    
    float m_curRange;
    float m_targetRangePlayer;
    float m_targetRangeEnemy;    
    float m_maxSpeedPlayer;
    float m_maxSpeedEnemy;    
    float m_accelPlayer;
    float m_accelEnemy;
    float m_speedPlayer = 0f;
    float m_speedEnemy = 0f;
    float m_targetSpeedPlayer = 0f;
    float m_targetSpeedEnemy = 0f;
    [TitleGroup ("Range")]
    public Text m_targetRangeText;
    [TitleGroup("Range")]
    public Slider m_targetRangeSlider;
    [TitleGroup("Range")]
    public Text m_playerSpeedText;
    [TitleGroup("Range")]
    public Text m_enemySpeedText;
    public void sliderSetTargetRange() {
        switch (m_targetRangeSlider.value) {
            case 0: setTargetRange(0f);
                break;
            case 1: setTargetRange(100f);
                break;
            case 2:
                setTargetRange(250f);
                break;
            case 3:
                setTargetRange(500f);
                break;
            case 4:
                setTargetRange(800f);
                break;
            case 5:
                setTargetRange(1000f);
                break;
        }
    }
    public void setTargetRange(float target, bool setSlider = false) {
        if (setSlider) {
            switch (target) {
                case 0f: m_targetRangeSlider.SetValueWithoutNotify(0f);
                    break;
                case 100f:
                    m_targetRangeSlider.SetValueWithoutNotify(1f);
                    break;
                case 250f:
                    m_targetRangeSlider.SetValueWithoutNotify(2f);
                    break;
                case 500f:
                    m_targetRangeSlider.SetValueWithoutNotify(3f);
                    break;
                case 800f:
                    m_targetRangeSlider.SetValueWithoutNotify(4f);
                    break;
                case 1000f:
                    m_targetRangeSlider.SetValueWithoutNotify(5f);
                    break;
                default:
                    m_targetRangeSlider.SetValueWithoutNotify(0f);
                    setTargetRange(0f);
                    return;                    
            }
        }

        m_targetRangePlayer = target;
        if (m_targetRangePlayer != 0) m_targetRangeText.text = m_targetRangePlayer.ToString("n2") + "m";
        else m_targetRangeText.text = "RAM";
    }

    private void Start() {
        m_mainCam = Camera.main;

        //disable by default
        m_combatCam.gameObject.SetActive(false);
        m_UI.SetActive(false);

        //Range
        m_curRange = 1000f;
        m_targetRangePlayer = 800f;
        m_targetRangeEnemy = 200f;        
        m_maxSpeedPlayer = 10f;
        m_maxSpeedEnemy = 8f;
        m_accelPlayer = 1f;
        m_accelEnemy = 0.6f;
        setTargetRange(m_targetRangePlayer, true);
    }
    public void StartCombat() {
        //switch camera (will use Cinemachine later. Simply disable/enable for now.)
        //also adjust camera position for different city sizes
        m_mainCam.enabled = false;
        m_combatCam.gameObject.SetActive(true);
        m_combatCam.enabled = true;

        //switch UI
        GameManager.obj().m_mainCanvas.gameObject.SetActive(false);
        m_UI.SetActive(true);

        m_combatActive = true;
    }
    void updateRange() {
        if (m_curRange < m_targetRangePlayer) m_targetSpeedPlayer = m_maxSpeedPlayer;
        else m_targetSpeedPlayer = -m_maxSpeedPlayer;
        if (m_curRange < m_targetRangeEnemy) m_targetSpeedEnemy = m_maxSpeedEnemy;
        else m_targetSpeedEnemy = -m_maxSpeedEnemy;

        if (m_speedPlayer != m_targetSpeedPlayer) {
            m_speedPlayer = Mathf.Lerp(m_speedPlayer, m_targetSpeedPlayer, m_accelPlayer * Time.deltaTime);
        }
        if (m_speedEnemy != m_targetSpeedEnemy) {
            m_speedEnemy = Mathf.Lerp(m_speedEnemy, m_targetSpeedEnemy, m_accelEnemy * Time.deltaTime);
        }

        m_curRange += m_speedEnemy * Time.deltaTime;
        m_curRange += m_speedPlayer * Time.deltaTime;
    }
    void updateUI() {
        m_rangeText.text = m_curRange.ToString("n2") + "m";

        m_playerSpeedText.text = "Player Speed: " + m_speedPlayer.ToString("n2");
        m_enemySpeedText.text = "Enemy Speed: " + m_speedEnemy.ToString("n2");
    }
    private void Update() {
        if (m_combatActive) {
            updateRange();

            updateUI();
        }
    }
}
