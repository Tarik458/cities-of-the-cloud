using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Popup : MonoBehaviour
{
    protected void Start() {
        GameManager.obj().m_mainCanvas.blocksRaycasts = false;
    }

    public void closePopup() {
        GameManager.obj().m_mainCanvas.blocksRaycasts = true;
        LeanPool.Despawn(gameObject);
    }
   
}
