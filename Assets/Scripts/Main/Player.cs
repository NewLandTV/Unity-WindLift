using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // FPS
    [Header("FPS"), SerializeField]
    private TextMeshProUGUI fpsText;

    // Move speed
    [Space, Header("Move Speed"), SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float currentSpeed;

    private Vector3 moveDirection;

    // Rotation
    [Space, Header("Rotation"), SerializeField]
    private float rotationSensitivity;
    [SerializeField]
    private float limitCameraRotationX;

    private Vector2 rotation;

    // Jump
    [Space, Header("Jump"), SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravityScale;

    // Forward roll
    [Space, Header("Forward Roll"), SerializeField]
    private float moveForwardRollDistance;
    [SerializeField]
    private float forwardRollCooldownTime;
    private float currentForwardRollCooldownTime;

    // Flags
    private bool isRun;
    private bool isJump;
    private bool isForwardRolling;

    // Components
    private new Transform transform;
    private new Rigidbody rigidbody;
    private new CapsuleCollider collider;

    // Other components
    [Space, Header("Other Components"), SerializeField]
    private new Transform camera;

    // Coroutines
    private Coroutine forwardRollCoroutine;

    private void Awake()
    {
        // Get components
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        // Setup variables
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        ToggleCursor();
        ToggleFPS();

        if (isForwardRolling)
        {
            return;
        }

        TryRun();
        CameraRotation();
        CharacterRotation();
    }

    private void FixedUpdate()
    {
        if (isForwardRolling)
        {
            return;
        }

        Move();
        Jump();
        ForwardRoll();
    }

    private void ToggleCursor()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.lockState = (Cursor.visible = !Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void ToggleFPS()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            fpsText.gameObject.SetActive(!fpsText.gameObject.activeSelf);
        }

        if (!fpsText.gameObject.activeSelf)
        {
            return;
        }

        fpsText.text = $"{1 / Time.deltaTime:f1}";
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isRun = true;
            currentSpeed = runSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isRun = true;
            currentSpeed = walkSpeed;
        }
    }

    private void CameraRotation()
    {
        rotation.x -= Input.GetAxisRaw("Mouse Y") * rotationSensitivity;

        rotation.x = Mathf.Clamp(rotation.x, -limitCameraRotationX, limitCameraRotationX);

        camera.rotation = Quaternion.Euler(new Vector3(rotation.x, camera.eulerAngles.y, camera.eulerAngles.z));
    }

    private void CharacterRotation()
    {
        rotation.y += Input.GetAxisRaw("Mouse X") * rotationSensitivity;

        transform.rotation = Quaternion.Euler(transform.up * rotation.y);
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized * currentSpeed * Time.fixedDeltaTime;

        rigidbody.MovePosition(rigidbody.position + moveDirection);
    }

    private void Jump()
    {
        if (rigidbody.velocity.y < 0f && Physics.SphereCast(transform.position, collider.radius * 0.95f, Vector3.down, out RaycastHit hit, collider.height * 0.28f) && !hit.collider.CompareTag("Character"))
        {
            isJump = false;
        }
        else if (rigidbody.velocity.y > 0f)
        {
            isJump = true;

            rigidbody.AddForce(Vector3.down * gravityScale * Time.fixedDeltaTime);
        }

        if (!Input.GetKeyDown(KeyCode.Space) || isJump)
        {
            return;
        }

        isJump = true;

        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ForwardRoll()
    {
        if (!Input.GetKeyDown(KeyCode.R) || isJump || currentForwardRollCooldownTime > 0f)
        {
            return;
        }

        isForwardRolling = true;

        forwardRollCoroutine = StartCoroutine(ForwardRollCoroutine());
    }

    private IEnumerator ForwardRollCoroutine()
    {
        Vector3 start = rigidbody.position;
        Vector3 end = rigidbody.position + transform.forward * moveForwardRollDistance;
        Quaternion cameraOrigin = camera.rotation;

        float timer = 0f;

        while (timer < 1f)
        {
            camera.Rotate(Vector3.right, 360f * Time.fixedDeltaTime);

            rigidbody.MovePosition(Vector3.Lerp(start, end, timer));

            timer += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        camera.rotation = cameraOrigin;
        currentForwardRollCooldownTime = forwardRollCooldownTime;
        isForwardRolling = false;

        while (currentForwardRollCooldownTime > 0f)
        {
            currentForwardRollCooldownTime -= Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }
}
