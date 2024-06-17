using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TutorialTriggers : MonoBehaviour
{
    [SerializeField] GameObject fallPreventPos;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject movementTuto;
    [SerializeField] GameObject dashTuto;
    [SerializeField] GameObject attackTuto;
    private bool isTutoPaused;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player")
        {
            if(gameObject.name == "DashTutoCube")
            {
                StartDashTuto();
            }
            if(gameObject.name == "BridgeHole")
            {
                CharacterController playerCC = other.gameObject.GetComponent<CharacterController>();
                playerCC.enabled = false;
                other.transform.SetPositionAndRotation(fallPreventPos.transform.position,fallPreventPos.transform.rotation);
                playerCC.enabled = true;
            }

        }
    }
    public void Resume()
    {
        isTutoPaused = false;
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        if(gameObject.name == "DashTutoCube" || gameObject.name == "AttackTutoCube")
        {
            gameObject.SetActive(false);
        }
    }
    public void Pause()
    {
        isTutoPaused = true;
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
    }

    public void StartMovTuto()
    {
        movementTuto.SetActive(true);
        Pause();
    }
    public void StartDashTuto()
    {
        dashTuto.SetActive(true);
        Pause();
    }
}