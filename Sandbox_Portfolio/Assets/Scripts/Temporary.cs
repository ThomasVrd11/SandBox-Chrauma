using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getDestroyed());
    }
    IEnumerator getDestroyed(){
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

}
