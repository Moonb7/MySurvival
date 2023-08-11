using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : Singleton<InputManager>
{
    public Vector2 move;
    public Vector2 look;
    public float scrollWheel;

    [HideInInspector]
    public bool jumpKey;
    [HideInInspector]
    public bool sprintKey;
    [HideInInspector]
    public bool rollKey;
    [HideInInspector]
    public bool weapon1Key;
    [HideInInspector]
    public bool weapon2Key;
    [HideInInspector]
    public bool weapon3Key;

    // 직접 구한 값을 통해 할것인가
    public bool analogMovement;

    public bool cursorLocked = true;        // 커서상태를 물어보는것 
    public bool cursorInputForLook = true;  // 마우스의 값을 줄것인가
    
    public static float chargingEnergy = 0f;       // 차징에너지를 모을 변수
    private bool isCharging;                // 차징 중인지 체크
    private Coroutine chargingcoroutine;
    private GameObject chagingEff;
    private GameObject chargingFullEff;      

    public Transform weaponEquipPos;

    private void Start()
    {
        SetCursorState(cursorLocked);
    }

    public void OnMove(InputAction.CallbackContext context) // 움직임 값
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) // 마우스 값
    {
        if (cursorInputForLook)
        {
            look = context.ReadValue<Vector2>();
        }
    }

    public void OnJump(InputAction.CallbackContext context) // 점프 키
    {
        if (context.started)
        {
            jumpKey = true;
        }
        else if (context.canceled)
        {
            jumpKey = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context) // 달리기 키
    {
        if (context.performed)
        {
            sprintKey = true;
            PlayerController.animator.SetBool(AnimString.instance.sprint, true); // 상태 체크
        }
        else if (context.canceled)
        {
            sprintKey = false;
            PlayerController.animator.SetBool(AnimString.instance.sprint, false);
        }
    }

    public void OnRoll(InputAction.CallbackContext context) // 구르기키
    {
        if (context.performed)
        {
            if (context.interaction is PressInteraction)
            {
                rollKey = true;
            }
        }
        else if (context.canceled)
        {
            rollKey = false;
        }
    }
    
    public void OnScrollWheel(InputAction.CallbackContext context) // 마우스 스크롤값
    {
        scrollWheel = context.ReadValue<float>();
    }

    public void OnPause(InputAction.CallbackContext context) // 일시정지 
    {
        if (context.started)
        {
            FindAnyObjectByType<PauseUI>().togle();
        }
    }

    public void OnAttack(InputAction.CallbackContext context) // 기본 공격
    {
        if (WeaponManager.activeWeapon != null && WeaponManager.isChangeReady &&
            PlayerController.animator.GetBool(AnimString.Instance.isAttack) == false &&
            PlayerController.animator.GetBool(AnimString.Instance.isGround) && jumpKey == false && rollKey == false ) // 땅에 있는지체크 다른 행동을 취하고 있는지
        {
            if (context.performed) // 키를 누르고 있는 동안
            {
                isCharging = true;
                chagingEff = Instantiate(WeaponManager.activeWeapon.chargingEffect, this.transform);
                chagingEff.transform.localPosition = Vector3.zero;
                chagingEff.transform.localRotation = Quaternion.identity;

                if (chargingcoroutine != null)
                    StopCoroutine(chargingcoroutine);
                chargingcoroutine = StartCoroutine(Charging());
            }
            else if (chargingEnergy >= WeaponManager.activeWeapon.weaponScriptable.chargingEnergyTime) // 다모으고 키를 떼었을 때
            {
                WeaponManager.activeWeapon.ChargingAttack();
                isCharging = false;
                chargingEnergy = 0;
                WeaponManager.activeWeapon.weaponAudioSource.loop = false;
                Destroy(chagingEff, 1f);
                Destroy(chargingFullEff, 1.5f);
            }
            else if (context.canceled)
            {
                isCharging = false;
                chargingEnergy = 0;
                Debug.Log("취소");
                WeaponManager.activeWeapon.weaponAudioSource.loop = false;
                /*ParticleSystem particle = chagingEff.GetComponent<ParticleSystem>(); 갑작기 오류 발생
                if (particle != null)
                    particle.loop = false;*/

                Destroy(chagingEff, 1f);
            }

            if (context.interaction is PressInteraction) // 일반 공격
            {
                WeaponManager.activeWeapon.Attack();
            }
        }
    }

    IEnumerator Charging()
    {
        WeaponManager.activeWeapon.weaponAudioSource.clip = WeaponManager.activeWeapon.chargingSound;
        WeaponManager.activeWeapon.weaponAudioSource.loop= true;
        WeaponManager.activeWeapon.weaponAudioSource.Play();

        while (isCharging)
        {
            chargingEnergy += Time.deltaTime;
            Debug.Log("헐딩");
            yield return null;
            if (chargingEnergy >= WeaponManager.activeWeapon.weaponScriptable.chargingEnergyTime)
            {
                // 차징에너지가 다모였다는 효과 이펙트가 더화려해 진다던다 그런거
                chargingFullEff = Instantiate(WeaponManager.activeWeapon.chagingFullEff, weaponEquipPos);
                chargingFullEff.transform.localPosition = Vector3.zero;
                chargingFullEff.transform.localRotation = Quaternion.identity;
                WeaponManager.activeWeapon.weaponAudioSource.loop = false;
                yield break;
            }
        }
    }

    public void OnTargetting(InputAction.CallbackContext context) // 고정할 타켓 설정 자세한건 PlayerTargetting 참고
    {
        if (context.started)
        {
            PlayerTargeting.targetEnemy = PlayerTargeting.enemy; // 키를 누를 때마다 고정할 적을 갱신
            PlayerTargeting.fastentargeting = !PlayerTargeting.fastentargeting;
        }
    }
    public void OnWeapon1(InputAction.CallbackContext context)
    {
        if (context.started && !weapon1Key)
        {
            weapon1Key = true;
        }
        else if (context.canceled)
        {
            weapon1Key = false;
        }
    }
    public void OnWeapon2(InputAction.CallbackContext context)
    {

        if (context.started && !weapon2Key)
        {
            weapon2Key = true;
        }
        else if (context.canceled)
        {
            weapon2Key = false;
        }
    }
    public void OnSkill1(InputAction.CallbackContext context)
    {
        if (WeaponManager.activeWeapon != null && WeaponManager.isChangeReady &&
            PlayerController.animator.GetBool(AnimString.Instance.isAttack) == false &&
            PlayerController.animator.GetBool(AnimString.Instance.isGround) && jumpKey == false && rollKey == false && PlayerController.animator.GetBool(AnimString.Instance.canMove)) // 땅에 있는지체크 다른 행동을 취하고 있는지
        {
            if (context.performed)
            {
                if (WeaponManager.isSkill1Ready)
                {
                    WeaponManager.activeWeapon.Skill1();
                    WeaponManager.skill1CoolTimedown = 0;
                }
                else
                {
                    Debug.Log("스킬1 쿨타임 중이다.");
                }
            }
        }
    }

    public void OnSkill2(InputAction.CallbackContext context)
    {
        if (WeaponManager.activeWeapon != null && WeaponManager.isChangeReady &&
            PlayerController.animator.GetBool(AnimString.Instance.isAttack) == false &&
            PlayerController.animator.GetBool(AnimString.Instance.isGround) && jumpKey == false && rollKey == false && PlayerController.animator.GetBool(AnimString.Instance.canMove)) // 땅에 있는지체크 다른 행동을 취하고 있는지
        {
            if (context.performed)
            {
                if (WeaponManager.isSkill2Ready)
                {
                    WeaponManager.activeWeapon.Skill2();
                    WeaponManager.skill2CoolTimedown = 0;
                }
                else
                {
                    Debug.Log("스킬2 쿨타임 중이다.");
                }
            }
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

