using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorActivate : MonoBehaviour
{
    [SerializeField]GameObject colorToActivate;
    [SerializeField]GameObject R;
    [SerializeField]GameObject G;
    [SerializeField]GameObject B;
    [SerializeField] Volume volume;
    private ColorAdjustments colorAdjustments;


    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out colorAdjustments);
    }

    IEnumerator  ColorTheWorld()
    {
        Debug.Log("checking");
        if (R.activeInHierarchy && G.activeInHierarchy && B.activeInHierarchy)
        {
        Debug.Log("yay");
            float currentTime = 0f;
            while (currentTime <= 2.5f)
            {
                colorAdjustments.saturation.value = Mathf.Lerp(-100, 0, currentTime / 2.5f);
                currentTime += Time.deltaTime;
                yield return null;
            }

        }
    }

    private void OnTriggerEnter(Collider other) {
        colorToActivate.SetActive(true);
        StartCoroutine(ColorTheWorld());
        Destroy(this.gameObject,3);
    }
}
