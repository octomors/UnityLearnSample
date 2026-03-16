using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DianaSampleScript : SampleScript
{
    [SerializeField] 
    [Tooltip("Объект, копии которого будут создаваться")]
    private GameObject prefab;

    [SerializeField] 
    [Min(1)]
    [Tooltip("Требуемое количество копий")]
    private int count = 3;

    [SerializeField] 
    [Min(1f)]
    [Tooltip("Дистанция между копиями")]
    private float step = 2f;

    public override void Use()
    {
        Vector3 startPos = transform.position;

        for (int i = 1; i <= count; i++)
        {
            Vector3 spawnPosition = startPos + transform.forward * step * i;
            Instantiate(prefab, spawnPosition, prefab.transform.rotation);
        }
    }
}
