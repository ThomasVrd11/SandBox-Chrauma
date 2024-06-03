using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.AI;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera perspectiveCamera;
    public CinemachineVirtualCamera orthographicCamera;
	public CinemachineVirtualCamera cameraDoor2;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private NavMeshAgent navAgentPlayer;
	[SerializeField] private GameObject waypoint1;
	private bool hasBeenTriggered = false;
    public float time = 5.0f;

    private Camera mainCamera;
    private bool isOrthographic = true;

    void Start()
    {
        mainCamera = Camera.main;

    }
	private void OnTriggerEnter(Collider other) {
		if(other.name == "Player" && isOrthographic && !hasBeenTriggered)
		{
			SwitchCam(isOrthographic);
			StartCoroutine(StopPlayerForASec(time));
			hasBeenTriggered = true;
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
			cameraDoor2.gameObject.SetActive(false);
			orthographicCamera.gameObject.SetActive(true);
		}
	}
	IEnumerator StopPlayerForASec(float timeStopped){
		characterController.enabled = false;
		navAgentPlayer.enabled = true;
		navAgentPlayer.SetDestination(waypoint1.transform.position);
		StartCoroutine(AnimateDoorCamera());
		yield return new WaitForSeconds(timeStopped);
		characterController.enabled = true;
		navAgentPlayer.enabled = false;
		orthographicCamera.m_Lens.NearClipPlane = -7.9f;
		SwitchCam(false);
	}
	IEnumerator AnimateDoorCamera()
	{
		yield return new WaitForSeconds(2);
		perspectiveCamera.gameObject.SetActive(false);
		cameraDoor2.gameObject.SetActive(true);

	}

}
