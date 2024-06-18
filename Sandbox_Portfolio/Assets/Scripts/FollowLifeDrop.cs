using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLifeDrop : MonoBehaviour
{
    public Transform Target;
    public float MinModifier = 7;
    public float MaxModifier = 10;

    Vector3 _velocity = Vector3.zero;
    bool _isFollowing = false;

    public void StartFollowing()
    {
        _isFollowing = true;
    }

    void Update()
    {
        if (_isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, Random.Range(MinModifier, MaxModifier));
        }
    }
}