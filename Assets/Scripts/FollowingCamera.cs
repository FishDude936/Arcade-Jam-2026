using System;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float minXClamp = 0, maxXClamp = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            Destroy(this);
            return;
        }
        transform.position = new Vector3(Mathf.Clamp(target.position.x, minXClamp, maxXClamp), 0, transform.position.z);
    }
}
