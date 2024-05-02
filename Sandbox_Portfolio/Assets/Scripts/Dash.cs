using UnityEngine;

public class DashAbility : MonoBehaviour
{
    public float dashDistance = 5f;

    void Update()
    {
        if (Input.GetButtonDown("Dashing"))
        {
            Debug.Log("Dash");
            Dash();
        }
    }

    private void Dash()
    {
        Vector3 dashPosition = transform.position + transform.forward * dashDistance;
        Debug.Log("Dash Position: " + dashPosition);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, dashDistance))
        {
            dashPosition = hit.point;
        }

        transform.position = dashPosition;
    }
}
