using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefabToPool;
    public int amountToPool;
    public Transform parentTransform;
    public List<GameObject> pooledObjects;

    private void Start()
    {
        if (parentTransform == null) parentTransform = transform;
        pooledObjects = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(prefabToPool, parentTransform);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy) return pooledObject;
        }

        return AddToPool();
    }

    private GameObject AddToPool()
    {
        Debug.Log("Insufficient " + prefabToPool + "in object pool, adding 1 more");
        GameObject addedObject = Instantiate(prefabToPool, parentTransform);
        addedObject.SetActive(false);
        pooledObjects.Add(addedObject);
        return addedObject;
    }
}
