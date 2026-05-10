using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] Transform target;
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
        transform.position = new Vector3(target.position.x, 0, transform.position.z);
    }
}
