using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int initSize;
    }

    public List<Pool> pools;
    private Dictionary<string, GameObject> prefabDict;
    public Dictionary<string, Queue<GameObject>> poolDict;

    #region Unity
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        InitPoolDicts();
    }

    //private void Start()
    //{
    //    InitPoolDicts();
    //}
    #endregion

    #region Object Pool Methods
    private void InitPoolDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            prefabDict.Add(pool.tag, pool.prefab);

            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < pool.initSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }

            poolDict.Add(pool.tag, poolQueue);
        }
    }

    public GameObject GetObject(string tag, Vector2 pos)
    {
        if(!poolDict.ContainsKey(tag))
        {
            Debug.Log(tag + " pool is not exist.");
            return null;
        }

        GameObject obj = null;
        if(poolDict[tag].Count < 1)
        {
            // Create object to pool
            obj = Instantiate(prefabDict[tag]);
        }
        else
        {
            obj = poolDict[tag].Dequeue();
        }

        obj.transform.position = pos;

        obj.SetActive(true); // 초기화할 때 잔상이 남을 수 있음

        return obj;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.Log(tag + " pool is not exist.");
            return;
        }

        poolDict[tag].Enqueue(obj);
        obj.SetActive(false);
    }
    #endregion

}
