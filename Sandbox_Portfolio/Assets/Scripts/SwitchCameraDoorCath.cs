using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchCameraDoorCath : MonoBehaviour
{	[SerializeField] Camera mainCamera;
	[SerializeField] CinemachineVirtualCamera vcplayer;
	[SerializeField] CinemachineVirtualCamera vcdoor;
	[SerializeField] CinemachineBrain cinemachineBrain;
	public float transitionDuration = 1.0f;
	private bool isOrthographic = false;

	private void Start() {
        vcdoor.Priority = 1;
        vcplayer.Priority = 0;	
	}

	private void OnTriggerEnter(Collider other) {
		if(other.name == "Player") {
			SwitchToPerspective();
		}
	}    public void SwitchToOrthographic()
    {
        if (!isOrthographic)
        {
            StartCoroutine(SmoothSwitch(true));
        }
    }

    public void SwitchToPerspective()
    {
        if (isOrthographic)
        {
            StartCoroutine(SmoothSwitch(false));
        }
    }

    private IEnumerator SmoothSwitch(bool toOrthographic)
    {
        float elapsedTime = 0f;
        float startFOV = mainCamera.fieldOfView;
        float endFOV = toOrthographic ? 0f : 60f; // Adjust end FOV for perspective as needed
        float startOrthoSize = mainCamera.orthographicSize;
        float endOrthoSize = toOrthographic ? 5f : 0f; // Adjust end size for orthographic as needed

        // Adjust virtual camera priorities
        if (toOrthographic)
        {
            vcplayer.Priority = 10;
            vcdoor.Priority = 5;
        }
        else
        {
            vcdoor.Priority = 10;
            vcplayer.Priority = 5;
        }

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            if (toOrthographic)
            {
                mainCamera.fieldOfView = Mathf.Lerp(startFOV, endFOV, t);
                mainCamera.orthographicSize = Mathf.Lerp(startOrthoSize, endOrthoSize, t);
            }
            else
            {
                mainCamera.fieldOfView = Mathf.Lerp(startFOV, endFOV, t);
                mainCamera.orthographicSize = Mathf.Lerp(startOrthoSize, endOrthoSize, t);
            }

            yield return null;
        }

        // Ensure final settings are applied
        mainCamera.orthographic = toOrthographic;
        mainCamera.fieldOfView = toOrthographic ? 0f : 60f; // Adjust final FOV as needed
        mainCamera.orthographicSize = toOrthographic ? 5f : 0f; // Adjust final orthographic size as needed
        isOrthographic = toOrthographic;
    }
}
