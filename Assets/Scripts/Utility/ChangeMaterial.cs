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

    [Range(-1, 1)] // ���°� ������ �Ⱥ��� ������ ��
    public float minValue;
    [Range(-1, 1)] // ���°� ������ ���� ������ ��
    public float maxValue;
    [Range(-1, 1)] // ���� ���°� ��
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

        if (characterStats.isDeath) // ������ ������ ��Ʈ����� �����Ͽ� ������
        {
            DissolveObj();
        }
    }

    public void SpawnObj()
    {
        getValue += speed * Time.deltaTime;

        if(getValue < maxValue) // ���� ���� maxValue ���� ������
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

    private void DissolveObj() // �����ִ� ���̶� Į ���� �� ������� ���� ������ ��Ƽ������ ������ �ȵȰ� ����. Ȯ�� �ؾ� �ٴ�.
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

        if(getValue > minValue) // ���� ����  minValue ���� ũ��
        {
            activeMaterial.SetFloat("_Value", getValue);
        }
        else
        {
            activeMaterial.SetFloat("_Value", minValue);
        }
    }
}
