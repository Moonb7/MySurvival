using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNewInputController : MonoBehaviour
{
    public static Animator animator;
    private CharacterController characterController;
    private GameObject mainCamera;
    private PlayerInput playerInput;

    // �ִϸ��̼� ���ǵ� �Ķ���Ͱ�
    private float blendSpeed;
    public float moveSpeed = 2f;
    public float sprintSpeed = 5.5f;
    private float speed = 0;

    public float rollSpeed = 8f;
    public static bool hasroll;
    private Vector3 rollVec;
    private Vector3 moveVec;

    // ���� �ִ� ����
    public float jumpHeight = 1.2f;
    // �����⺻���� -9.81
    public float gravity = -15f;

    [Tooltip("�ӵ� ��ȭ�� �̵��ӵ� �� speed�ִϸ��̼� �Ķ���� ���� Lerp�Լ��� ���ϴµ� ���δ�")]
    public float speedChangeRate = 1000f;

    [Range(0.0f, 0.3f)] // �ε巯�� ��ȭ�� �ɸ��� �ð��Դϴ�. �� ���� �������� ������ ��ǥ�� �����ϰ� �˴ϴ�.
    public float rotationSmoothTime = 0.12f; 
    private float ratationSmoothValue;


    [Tooltip("��� �࿡�� ī�޶� ��ġ ������")]
    public bool lockCameraPosition;

    [Tooltip("ī�޶� ���� Camera Root ���� ������Ʈ ����")]
    public GameObject cinemachineCameraTarget;

    [Tooltip("ī�޶� �����̼� ��ġ���� Y")]
    public float topClamp = 70.0f;
    [Tooltip("ī�޶� �����̼� ��ġ���� Y")]
    public float fastenTopClamp = 25.0f;

    [Tooltip("ī�޶� �����̼� ��ġ���� Y")]
    public float bottomClamp = -30.0f;

    private float targetRotation;
    private float rotationVelocity; // ���� ������ ��ȭ �ӵ��� �����ϱ� ���� ���� �����Դϴ�. �Լ��� ȣ���� �� �� ���� ������Ʈ�Ǹ�, �Լ� ���ο��� ���˴ϴ�.

    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    private const float threshold = 0.01f;

    private Quaternion beforCameraRotation; // 
    private Vector2 cameraRotation;
    private bool CanMove
    {
        get
        {
            return animator.GetBool("CanMove");
        }
    }

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return playerInput.currentControlScheme == "PC";
#else
            return false;
#endif
        }
    }

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // �ٽ� �ʱ�ȭ �ϱ����� ȸ���� �޾Ƴ���
        ratationSmoothValue = rotationSmoothTime;

        beforCameraRotation = cinemachineCameraTarget.transform.rotation;
        cameraRotation = beforCameraRotation.eulerAngles;
    }

    private void Update()
    {
        Move();
        JumpAndGravity();
        Roll();
    }

    private void LateUpdate()
    {
        CameraRotaion();
    }
    private void CameraRotaion()
    {        
        // �Է��� �ְ� ī�޶� ��ġ�� �������� ���� ���
        if (InputManager.Instance.look.sqrMagnitude >= threshold && !lockCameraPosition)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            cameraRotation.y += InputManager.Instance.look.y * deltaTimeMultiplier;
            cameraRotation.x += InputManager.Instance.look.x * deltaTimeMultiplier;

            cameraRotation.y = ClampAngle(cameraRotation.y, bottomClamp, topClamp);
            cameraRotation.x = ClampAngle(cameraRotation.x, float.MinValue, float.MaxValue);

            Quaternion rot = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);

            cinemachineCameraTarget.transform.rotation = rot;
            beforCameraRotation = cinemachineCameraTarget.transform.rotation;
        }
        else
        {
            cinemachineCameraTarget.transform.rotation = beforCameraRotation;
        }

        if (PlayerTargeting.fastentargeting && PlayerTargeting.targetEnemy)
        {
            lockCameraPosition = true;

            Vector3 tadir = PlayerTargeting.targetEnemy.transform.position - cinemachineCameraTarget.transform.position;

            cinemachineCameraTarget.transform.forward = tadir;
            Quaternion lookRot = cinemachineCameraTarget.transform.rotation;

            cameraRotation = new Vector2(lookRot.eulerAngles.y , lookRot.eulerAngles.x); // Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0); y�� x�� ���� x�� y�� ���ϱ� �̷������� �ִ°� �´�.

            // ī�޶� ȸ������ ������ �ִ� �κ� �߰�
            cameraRotation.y = ClampAngle(cameraRotation.y, bottomClamp, fastenTopClamp);
            cameraRotation.x = ClampAngle(cameraRotation.x, float.MinValue, float.MaxValue);

            Quaternion rot = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);
            cinemachineCameraTarget.transform.rotation = rot;


            beforCameraRotation = cinemachineCameraTarget.transform.rotation;
        }
        else
        {
            lockCameraPosition = false;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
        {
            lfAngle += 360f;
        }
        if (lfAngle > 360f)
        {
            lfAngle -= 360f; 
        }

        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    float CurrentSpeed()
    {
        float targetSpeed = InputManager.Instance.sprintKey ? sprintSpeed : moveSpeed;

        if (InputManager.Instance.move == Vector2.zero) targetSpeed = 0.0f;

        blendSpeed = Mathf.Lerp(blendSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        // �ణ�� �������� ������ ������ �̷��� ����
        if (blendSpeed < 0.01f) blendSpeed = 0f;


        if (CanMove)
        {
            // ���ǵ尪
            float inputMagnitude = InputManager.Instance.analogMovement ? InputManager.Instance.move.magnitude : 1.0f;
            speed = Mathf.Lerp(speed, targetSpeed * inputMagnitude,
                 speedChangeRate);

            // ������
            if (hasroll && animator.GetBool(AnimString.Instance.isGround))
            {
                // �������� ȸ���� 0
                rotationSmoothTime = 0;
                moveVec = rollVec;

                speed = Mathf.Lerp(speed, rollSpeed,
                 speedChangeRate);
            }
            else
            {
                // ȸ����ȭ �� �ʱ�ȭ
                rotationSmoothTime = ratationSmoothValue; // �����ϳ� �����ϱ�
            }

            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = 0;
        }
        return speed;
    }

    public void Move()
    {
        // �Է¹���
        Vector3 inputDirection = new Vector3(InputManager.Instance.move.x, 0f, InputManager.Instance.move.y).normalized;

        

        if (InputManager.Instance.move != Vector2.zero && hasroll == false && CanMove) // �������� ���� ������ �����̼� 
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                            mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                            rotationSmoothTime);

            // �÷��̾ ī�޶�� ���� ���� ȸ��
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        if (PlayerTargeting.fastentargeting && PlayerTargeting.targetEnemy && InputManager.Instance.sprintKey == false && hasroll == false) // ���� �ϰ� ���� ������ �׸��� �޸��� Ű�� ������ ������
        {
            //animator.SetLayerWeight(3, 1);
            animator.SetBool(AnimString.Instance.fasten, true);
            transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y,0);
        }
        else
        {
            animator.SetBool(AnimString.Instance.fasten, false);
            //animator.SetLayerWeight(3, 0);
        }

        // �־��� ȸ�� ������ �°� �̵��ϰų� ȸ���ؾ� �ϴ� ��쿡 ���˴ϴ�. ȸ���� ������ ��, ���� ���͸� �̿��Ͽ� ���ο� �̵� ������ ������ �� �ֽ��ϴ�.
        Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0) * Vector3.forward;

        moveVec = targetDirection.normalized * (CurrentSpeed() * Time.deltaTime) + 
                          new Vector3(0, verticalVelocity, 0) * Time.deltaTime;

        // �÷��̾� �̵�
        characterController.Move(moveVec);

        // �ִ�
        
        animator.SetBool(AnimString.Instance.move, InputManager.Instance.move != Vector2.zero);
        animator.SetFloat(AnimString.Instance.speed, blendSpeed);

        // ���� �������� �ִϸ��̼� �յڿ����� �ִϸ��̼� ���� �ֱ����Ѱ�
        animator.SetFloat(AnimString.Instance.X, InputManager.Instance.move.x);
        animator.SetFloat(AnimString.Instance.Y, InputManager.Instance.move.y);
    }

    

    private void Roll()
    {
        animator.SetBool(AnimString.Instance.roll, hasroll);

        if (hasroll)
            return;

        if (InputManager.Instance.rollKey && InputManager.Instance.move != Vector2.zero && animator.GetBool(AnimString.Instance.isGround) && CanMove)
        {
            hasroll = true;
            rollVec = moveVec;
        }
    }

    private void JumpAndGravity()
    {
        if (animator.GetBool(AnimString.Instance.isGround)) // ���� ������
        {
            animator.SetBool(AnimString.Instance.jump, false);
            animator.SetBool(AnimString.Instance.freeFall, false);

            if (verticalVelocity < 0) // Ȥ�� ���� ������
            {
                verticalVelocity = -2f;
            }

            // ����
            if (InputManager.Instance.jumpKey && hasroll == false && CanMove) // ����Ű�� ������ ������ �ʰ� �����ϼ� �ִ� ���¸�
            {
                // ������
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
                animator.SetBool(AnimString.Instance.jump, true);
            }
            else
            {
                InputManager.Instance.jumpKey = false;
            }
        }
        else
        {
            InputManager.Instance.jumpKey = false;
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }
}
