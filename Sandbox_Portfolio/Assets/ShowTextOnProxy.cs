using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTextOnProxy : MonoBehaviour
{
    public GameObject player;
    public TMP_Text proximityText;
    public float detectionRadius = 4f;
    public string message = "Nothing to see here..";

    private bool isTextDisplayed = false;

    void Start()
    {
        proximityText.enabled = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance >= detectionRadius && !isTextDisplayed)
        {
            StartCoroutine(DisplayText());
        }
    }

    private IEnumerator DisplayText()
    {
    isTextDisplayed = true;
    proximityText.text = message;
    proximityText.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
    proximityText.enabled = true;

    yield return new WaitForSeconds(3f);

    proximityText.enabled = false;
    isTextDisplayed = false;
    }
}
