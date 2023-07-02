using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNewInputController : MonoBehaviour
{
    public static Animator animator;
    private CharacterController characterController;
    private GameObject mainCamera;
    private PlayerInput playerInput;

    // 애니메이션 스피드 파라미터값
    private float blendSpeed;
    public float moveSpeed = 2f;
    public float sprintSpeed = 5.5f;
    private float speed = 0;

    public float rollSpeed = 8f;
    public static bool hasroll;
    private Vector3 rollVec;
    private Vector3 moveVec;

    // 점프 최대 높이
    public float jumpHeight = 1.2f;
    // 엔진기본값은 -9.81
    public float gravity = -15f;

    [Tooltip("속도 변화율 이동속도 및 speed애니메이션 파라미터 값을 Lerp함수로 구하는데 쓰인다")]
    public float speedChangeRate = 1000f;

    [Range(0.0f, 0.3f)] // 부드러운 변화에 걸리는 시간입니다. 이 값이 작을수록 빠르게 목표에 도달하게 됩니다.
    public float rotationSmoothTime = 0.12f; 
    private float ratationSmoothValue;


    [Tooltip("모든 축에서 카메라 위치 고정용")]
    public bool lockCameraPosition;

    [Tooltip("카메라가 따라갈 Camera Root 게임 오브젝트 설정")]
    public GameObject cinemachineCameraTarget;

    [Tooltip("카메라 로테이션 위치조절 Y")]
    public float topClamp = 70.0f;
    [Tooltip("카메라 로테이션 위치조절 Y")]
    public float fastenTopClamp = 25.0f;

    [Tooltip("카메라 로테이션 위치조절 Y")]
    public float bottomClamp = -30.0f;

    private float targetRotation;
    private float rotationVelocity; // 현재 각도의 변화 속도를 저장하기 위한 참조 변수입니다. 함수를 호출할 때 이 값이 업데이트되며, 함수 내부에서 사용됩니다.

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

        // 다시 초기화 하기위해 회전값 받아놓기
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
        // 입력이 있고 카메라 위치가 고정되지 않은 경우
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

            cameraRotation = new Vector2(lookRot.eulerAngles.y , lookRot.eulerAngles.x); // Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0); y가 x로 가고 x가 y로 가니깐 이런식으로 주는게 맞다.

            // 카메라 회전값에 제한을 주는 부분 추가
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
        // 약간의 버벅임이 있을수 있으니 이렇게 설정
        if (blendSpeed < 0.01f) blendSpeed = 0f;


        if (CanMove)
        {
            // 스피드값
            float inputMagnitude = InputManager.Instance.analogMovement ? InputManager.Instance.move.magnitude : 1.0f;
            speed = Mathf.Lerp(speed, targetSpeed * inputMagnitude,
                 speedChangeRate);

            // 구르기
            if (hasroll && animator.GetBool(AnimString.Instance.isGround))
            {
                // 굴렀을때 회전값 0
                rotationSmoothTime = 0;
                moveVec = rollVec;

                speed = Mathf.Lerp(speed, rollSpeed,
                 speedChangeRate);
            }
            else
            {
                // 회전변화 값 초기화
                rotationSmoothTime = ratationSmoothValue; // 변수하나 설정하기
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
        // 입력방향
        Vector3 inputDirection = new Vector3(InputManager.Instance.move.x, 0f, InputManager.Instance.move.y).normalized;

        

        if (InputManager.Instance.move != Vector2.zero && hasroll == false && CanMove) // 움직임임 값이 있을시 로테이션 
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                            mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                            rotationSmoothTime);

            // 플레이어가 카메라랑 같은 방향 회전
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        if (PlayerTargeting.fastentargeting && PlayerTargeting.targetEnemy && InputManager.Instance.sprintKey == false && hasroll == false) // 고정 하고 적이 있으면 그리고 달리기 키를 누르지 않으면
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

        // 주어진 회전 각도에 맞게 이동하거나 회전해야 하는 경우에 사용됩니다. 회전을 적용한 후, 전방 벡터를 이용하여 새로운 이동 방향을 결정할 수 있습니다.
        Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0) * Vector3.forward;

        moveVec = targetDirection.normalized * (CurrentSpeed() * Time.deltaTime) + 
                          new Vector3(0, verticalVelocity, 0) * Time.deltaTime;

        // 플레이어 이동
        characterController.Move(moveVec);

        // 애니
        
        animator.SetBool(AnimString.Instance.move, InputManager.Instance.move != Vector2.zero);
        animator.SetFloat(AnimString.Instance.speed, blendSpeed);

        // 고정 했을때의 애니매이션 앞뒤옆걸음 애니매이션 값을 주기위한거
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
        if (animator.GetBool(AnimString.Instance.isGround)) // 땅에 있으면
        {
            animator.SetBool(AnimString.Instance.jump, false);
            animator.SetBool(AnimString.Instance.freeFall, false);

            if (verticalVelocity < 0) // 혹시 값이 작으면
            {
                verticalVelocity = -2f;
            }

            // 점프
            if (InputManager.Instance.jumpKey && hasroll == false && CanMove) // 점프키를 누르고 구르지 않고 움직일수 있는 상태면
            {
                // 제곱근
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
