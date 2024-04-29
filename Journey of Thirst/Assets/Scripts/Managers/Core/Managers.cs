using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _manager;
    static Managers Manager { get { Init(); return _manager; } }


    private static InputManager _input = new InputManager();
    public static InputManager Input { get { return _input;} }

    private void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Input.OnUpdate();
    }

    static void Init()
    {
        if(_manager == null)
        {
            GameObject go = GameObject.Find("@Manager");
            if(go == null)
            {
                go = new GameObject("@Manager");
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            _manager = go.GetComponent<Managers>();
        }
    }
}
