using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class IntroManager : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera introCam;
	[SerializeField] CinemachineVirtualCamera mainCam;
	[SerializeField] Canvas Tutorials;
	[SerializeField] GameObject lookingAroundTarget;
	[SerializeField] Animator lilGhostAnimator;
	[SerializeField] GameObject[] characterDisplay;
	//0-reaper body 1-scythe 2-lilghost body

	[SerializeField] GameObject player;

    void Start()
    {
		introCam.gameObject.SetActive(true);
		mainCam.gameObject.SetActive(false);
		characterDisplay[0].SetActive(false);
		characterDisplay[1].SetActive(false);
		characterDisplay[2].SetActive(true);
        StartCoroutine(IntroSceneStart());
		player.GetComponent<CharacterControls>().enabled = false;
    }

	IEnumerator IntroSceneStart()
	{
		yield return new WaitForSeconds(1.5f);
		introCam.gameObject.SetActive(false);
		mainCam.gameObject.SetActive(true);
		yield return new WaitForSeconds(2f);
		player.transform.LookAt(lookingAroundTarget.transform.position);
		yield return null;	
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
