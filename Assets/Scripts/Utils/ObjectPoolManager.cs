using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UnitedSolution
{
    public class ObjectPoolManager : SingletonBehaviour<ObjectPoolManager>
    {
        public GameObject[] prefabs;
        public List<Pool> poolList = new List<Pool>();

        protected override void Awake()
        {
            base.Awake();

            if (prefabs != null)
            {
                foreach (GameObject prefab in prefabs)
                {
                    New(prefab); //cache prefabs
                }
            }
        }

        public static Transform Spawn(Transform objT)
        {
            return Spawn(objT.gameObject, Vector3.zero, Quaternion.identity).transform;
        }
        public static Transform Spawn(Transform objT, Vector3 pos, Quaternion rot)
        {
            return Instance._Spawn(objT.gameObject, pos, rot).transform;
        }

        public static GameObject Spawn(GameObject obj)
        {
            return Spawn(obj, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject obj, Transform parentTransform)
        {
            GameObject go = Spawn(obj, Vector3.zero, Quaternion.identity);
            go.transform.parent = parentTransform;
            return go;
        }

        internal static Mesh Spawn(Mesh mesh)
        {
            throw new NotImplementedException();
        }

        public static GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot)
        {
            return Instance._Spawn(obj, pos, rot);
        }
        public GameObject _Spawn(GameObject obj, Vector3 pos, Quaternion rot)
        {
            if (obj == null)
            {
                Debug.Log("NullReferenceException: obj unspecified");
                return null;
            }

            int ID = GetPoolID(obj);

            if (ID == -1) ID = _New(obj);

            return poolList[ID].Spawn(pos, rot);
        }
        static IEnumerator RemovalDelay(GameObject go, float delay) { yield return new WaitForSeconds(delay); Unspawn(go); }
        public static void Unspawn(GameObject objT, float delay) { Instance.StartCoroutine(RemovalDelay(objT, delay)); }
        public static void Unspawn(Transform objT) { Instance._Unspawn(objT.gameObject); }
        public static void Unspawn(GameObject obj) { Instance._Unspawn(obj); }
        public void _Unspawn(GameObject obj)
        {
            if (obj == null || !obj.activeSelf)
            {
                return;
            }

            for (int i = 0; i < poolList.Count; i++)
            {
                if (poolList[i].Unspawn(obj)) return;
            }

            obj.SetActive(false);
            Destroy(obj);
            Debug.LogError("Cannot unspawn the object properly, using force to destroy. Have you cached this object in OPM before?");
        }


        public static int New(Transform objT, int count = 5) { return Instance._New(objT.gameObject, count); }
        public static int New(GameObject obj, int count = 5) { return Instance._New(obj, count); }
        public int _New(GameObject obj, int count = 5)
        {
            int ID = GetPoolID(obj);
            if (ID != -1)
            {
                poolList[ID].MatchObjectCount(count);
            }
            else
            {
                Pool pool = new Pool()
                {
                    prefab = obj
                };
                pool.MatchObjectCount(count);
                poolList.Add(pool);
                ID = poolList.Count - 1;
            }
            return ID;
        }

        int GetPoolID(GameObject obj)
        {
            for (int i = 0; i < poolList.Count; i++)
            {
                if (poolList[i].prefab == obj) return i;
            }
            return -1;
        }

        public static void ClearAll()
        {
            for (int i = 0; i < Instance.poolList.Count; i++) Instance.poolList[i].Clear();
            Instance.poolList = new List<Pool>();
        }

        public static Transform GetOPMTransform()
        {
            return Instance.transform;
        }
    }
}