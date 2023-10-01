using UnityEngine;

public class WindArea : MonoBehaviour
{
    // Wind settings
    [Header("Wind Settings"), SerializeField]
    private Vector3 direction;
    [SerializeField]
    private float power;

    private void OnTriggerStay(Collider other)
    {
        other.attachedRigidbody.AddForce(direction * power, ForceMode.Force);
    }
}
