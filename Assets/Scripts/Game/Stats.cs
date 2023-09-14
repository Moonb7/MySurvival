using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField]
    private float startValue;

    public List<float> values = new List<float>();

    public void SetValue(float value)
    {
        startValue = value;
    }

    public float GetValue()
    {
        float result = startValue;

        foreach (var value in values)
        {
            result += value;
        }

        return result;
    }

    public void AddValue(float value)
    {
        if (value == 0)
            return;

        values.Add(value);
    }

    public void RemoveValue(float value)
    {
        if (value == 0)
            return;

        values.Remove(value);
    }
}
