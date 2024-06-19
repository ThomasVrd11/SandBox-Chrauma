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
	[SerializeField] float lookAroundTime;
	[SerializeField] Renderer ghostRenderer;
	[SerializeField] CharacterControls playerControls;
	[SerializeField] TutorialTriggers tutorialTriggers;
	private Vector3 posLook1 = new Vector3(-129.06f, 23.90f, -16.44f);
	private Vector3 posLook2 = new Vector3(-123f, 19f, -15f);

    void Start()
	{
		StartCoroutine(Initialize());
	}

	private IEnumerator Initialize()
	{
		yield return null;
		introCam.gameObject.SetActive(true);
		mainCam.gameObject.SetActive(false);
		characterDisplay[0].SetActive(false);
		characterDisplay[1].SetActive(false);
		characterDisplay[2].SetActive(true);

		playerControls.enabled = false;
		StartCoroutine(IntroSceneStart());
	}
	IEnumerator WhatHappened()
	{
		yield return null;
	}

	IEnumerator IntroSceneStart()
	{
		yield return new WaitForSeconds(1.5f);
		introCam.gameObject.SetActive(false);
		mainCam.gameObject.SetActive(true);
		yield return new WaitForSeconds(2f);
		yield return StartCoroutine(Apparition());
		lookAround(lookingAroundTarget.transform.position);
		yield return new WaitForSeconds(lookAroundTime);
		lookAround(posLook2);
		yield return new WaitForSeconds(lookAroundTime);
		lookAround(posLook1);
		yield return new WaitForSeconds(0.5f);
		lilGhostAnimator.SetTrigger("Surprised");
		yield return new WaitForSeconds(0.7f);
		playerControls.enabled = true;
		tutorialTriggers.StartMovTuto();
		yield return null;
	}

    IEnumerator Apparition()
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        float dissolveTime = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(0, 1, elapsedTime / dissolveTime);

            ghostRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_Dissolve", dissolveValue);
            ghostRenderer.SetPropertyBlock(propBlock);

            yield return null;
        }
        ghostRenderer.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_Dissolve", 1);
        ghostRenderer.SetPropertyBlock(propBlock);
    }
	private void lookAround(Vector3 pos)
	{
		lookingAroundTarget.transform.position = pos;
		player.transform.LookAt(lookingAroundTarget.transform.position);
	}

}
