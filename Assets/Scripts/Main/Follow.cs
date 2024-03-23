using UnityEngine;

public class Follow : MonoBehaviour
{
    // Settings
    [Header("Settings"), SerializeField]
    private Vector3 offset;
    [SerializeField]
    private bool followRotationY;

    // Other components
    [Space, Header("Other Components"), SerializeField]
    private Transform target;

    private void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, followRotationY ? target.eulerAngles.y : transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
