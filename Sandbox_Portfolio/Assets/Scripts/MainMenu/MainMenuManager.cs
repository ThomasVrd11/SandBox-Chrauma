using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField]float duration = 5.0f;
    private GameObject floor;
    private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        floor = this.gameObject;
    }

    private void Start()
    {
        if (volume.profile.TryGet(out colorAdjustments))
        {
            StartCoroutine(ChangeSaturation());
        }
    }

    // Update is called once per frame
    void Update()
    {
        floor.transform.Rotate(new Vector3(0, -1, 0) * 4 * Time.deltaTime);
    }

    IEnumerator ChangeSaturation()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            float currentTime = 0f;
            while (currentTime <= duration)
            {
                colorAdjustments.saturation.value = Mathf.Lerp(-100, 0, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            }
            while (currentTime >= 0)
            {
                colorAdjustments.saturation.value = Mathf.Lerp(-100, 0, currentTime / duration);
                currentTime -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
