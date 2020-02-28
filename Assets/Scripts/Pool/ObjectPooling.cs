using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pool/ObjectPooling")]
public class ObjectPooling
{
    List<PoolObject> objects;
    Transform objectsParent;

    public void Initialize(int count, PoolObject sample, Transform objects_parent)
    {
        objects = new List<PoolObject>(); //инициализируем List
        objectsParent = objects_parent; //инициализируем локальную переменную для последующего использования
        for (int i = 0; i < count; i++)
        {
            AddObject(sample, objects_parent); //создаем объекты до указанного количества
        }
    }

    void AddObject(PoolObject sample, Transform objects_parent)
    {
        GameObject temp = GameObject.Instantiate(sample.gameObject);
        temp.name = sample.name;
        temp.transform.SetParent(objects_parent);
        objects.Add(temp.GetComponent<PoolObject>());
        /*if (temp.GetComponent<Animator>())
            temp.GetComponent<Animator>().StartPlayback();*/
        temp.SetActive(false);
    }

    public PoolObject GetObject()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].gameObject.activeInHierarchy)
            {
                return objects[i];
            }
        }
        AddObject(objects[0], objectsParent);
        return objects[objects.Count - 1];
    }
}
