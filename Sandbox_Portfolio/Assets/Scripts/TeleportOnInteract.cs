using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeleportOnInteract : MonoBehaviour
{
    // * Variables for the teleportation
    [SerializeField] Transform receivingPortal;
    public bool isTeleporting = false;
    private bool playerIsInTrigger = false;


    void Start()
    {}
    // * I setup input to G because Geleportation

    void Update()
    {
        if (playerIsInTrigger && Input.GetKeyDown(KeyCode.G) && !isTeleporting)
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            playerIsInTrigger = true;
        }
    }
    // * Hello nico <3
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger");
            playerIsInTrigger = false;
        }
    }
    // * TP logic
    private IEnumerator TeleportPlayer()
    {
        isTeleporting = true;
        GameObject player = GameObject.FindWithTag("Player");
        CharacterController characterController = player.GetComponent<CharacterController>();

        Quaternion portalRotationDifference = receivingPortal.rotation * Quaternion.Inverse(transform.rotation);
        portalRotationDifference *= Quaternion.Euler(0f, 180f, 0f);

        Vector3 positionOffset = player.transform.position - transform.position;
        positionOffset = portalRotationDifference * positionOffset;
        Vector3 newPosition = receivingPortal.position + positionOffset;

        characterController.enabled = false;
        player.transform.SetPositionAndRotation(newPosition, player.transform.rotation * portalRotationDifference);
        characterController.enabled = true;
        isTeleporting = false;
        yield return null;
    }
}
