using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBhv<T> : MonoBehaviour where T : MonoBehaviour
{
    // singleton instance
    public static T instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            //instance = new T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
