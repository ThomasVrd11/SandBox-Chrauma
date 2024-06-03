using UnityEngine;
using Cinemachine;
using System.Collections;

public class SmoothCinemachineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera perspectiveCamera;
    public CinemachineVirtualCamera orthographicCamera;
    public float transitionDuration = 1.0f;

    private Camera mainCamera;
    private bool isOrthographic = true;

    void Start()
    {
        mainCamera = Camera.main;

    }
	private void OnTriggerEnter(Collider other) {
		if(other.name == "Player" && isOrthographic)
		{
			SwitchCam(isOrthographic);
		}
	}
	private void SwitchCam(bool isOrthographic)
	{
		if(isOrthographic)
		{
			perspectiveCamera.gameObject.SetActive(true);
			orthographicCamera.gameObject.SetActive(false);
		}
		else
		{
			perspectiveCamera.gameObject.SetActive(false);
			orthographicCamera.gameObject.SetActive(true);
		}
	}

}
