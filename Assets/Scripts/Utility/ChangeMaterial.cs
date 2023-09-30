using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public GameObject[] bodyParts;

    public Material baseMaterial;
    public Material spwanMaterial;
    public Material dissolveMaterial;
    private Material activeMaterial;

    [Range(-1, 1)] // 형태가 완전히 안보일 정도의 값
    public float minValue;
    [Range(-1, 1)] // 형태가 완전히 보일 정도의 값
    public float maxValue;
    [Range(-1, 1)] // 실제 형태가 값
    public float getValue;
    public float speed;

    private CharacterStats characterStats;
    private bool isSpwan;
    private bool isDissolve;

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        isSpwan = true;

        foreach (var part in bodyParts) 
        {
            var renderer = part.GetComponent<SkinnedMeshRenderer>();
            renderer.material = new Material(spwanMaterial);

            activeMaterial= renderer.material;
        }
        getValue = minValue;
    }
    void Update()
    {
        if (isSpwan)
        {
            SpawnObj();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var part in bodyParts)
            {
                part.GetComponent<SkinnedMeshRenderer>().material = spwanMaterial;
            }

            getValue = minValue;
            isSpwan = true;
        }

        if (characterStats.isDeath) // 죽으면 디졸브 머트리얼로 변경하여 값설정
        {
            DissolveObj();
        }
    }

    public void SpawnObj()
    {
        getValue += speed * Time.deltaTime;

        if(getValue < maxValue) // 현재 값이 maxValue 보다 작으면
        {
            activeMaterial.SetFloat("_Value", getValue);
        }
        else
        {
            activeMaterial.SetFloat("_Value", maxValue);

            foreach (var part in bodyParts)
            {
                var renderer = part.GetComponent<SkinnedMeshRenderer>();
                renderer.material = baseMaterial;

                activeMaterial = renderer.material;
            }

            isSpwan = false;
        }
    }

    private void DissolveObj() // 버그있다 얼굴이랑 칼 방패 는 사라지지 않음 각각의 머티리얼이 조정이 안된거 같다. 확인 해야 곘다.
    {
        if(isDissolve == false)
        {
            foreach (var part in bodyParts)
            {
                var renderer = part.GetComponent<SkinnedMeshRenderer>();
                renderer.material = new Material(dissolveMaterial);

                activeMaterial = renderer.material;
            }
            isDissolve = true;
        }
        
        getValue -= speed * Time.deltaTime;

        if(getValue > minValue) // 현재 값이  minValue 보다 크면
        {
            activeMaterial.SetFloat("_Value", getValue);
        }
        else
        {
            activeMaterial.SetFloat("_Value", minValue);
        }
    }
}
