using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The usage of this pool system is, 
///  create a variable in the spawner with the class in the generic,        private Pool<EnemyBehaviour> enemyPool;
///  initialize before the usage, ex, in the start or awake,                enemyPool.Create();
///  get a pooled object using Get.                                         var instance = enemyPool.Get();
/// </summary>
[System.Serializable]
public class Pool<T> where T : Component
{
    // Variables
    [SerializeField]
    private T m_poolable;

    [SerializeField]
    private Transform m_parent;

    private List<T> m_pool;

    // Methods
    public void Create(bool destroyChilds = true)
    {
        m_pool = new List<T>();

        if (destroyChilds)
            ClearLayoutChilds();
    }

    public T Get() => Get(Vector3.zero, Quaternion.identity);
    public T Get(Vector3 position) => Get(position, Quaternion.identity);
    public T Get(Vector3 position, Quaternion rotation)
    {
        T instance = null;

        foreach (var item in m_pool)
        {
            if (item.gameObject.activeSelf)
                continue;

            instance = item;
            instance.gameObject.SetActive(true);
            break;
        }

        if (instance == null)
        {
            instance = Object.Instantiate(m_poolable, m_parent);
            m_pool.Add(instance);
        }

        instance.transform.position = position;
        instance.transform.rotation = rotation;

        return instance;
    }

    public void DisableAll()
    {
        foreach (var item in m_pool)
            item.gameObject.SetActive(false);
    }

    public void DestroyAll()
    {
        foreach (var item in m_pool)
            Object.Destroy(item.gameObject);

        m_pool.Clear();
    }

    public void ClearLayoutChilds()
    {
        if (m_parent == null)
            return;

        for (int i = 0; i < m_parent.childCount; i++)
        {
            Object.Destroy(m_parent.GetChild(i).gameObject);
        }
    }
}
