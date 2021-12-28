using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton 單例模式
//確保生成對象只有一個實例
//開發中希望某個類別只有一個實例化物件
//此Script即用來新增單例實例的登記接口

public class GameManagerSingleton
{
    private GameObject gameObject;

    //單例
    private static GameManagerSingleton m_Instance;
    //接口
    public static GameManagerSingleton Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameManagerSingleton();
                m_Instance.gameObject = new GameObject("GameManager");
                m_Instance.gameObject.AddComponent<InputController>();
            }
            return m_Instance;
        }
    }

    private InputController m_InputController;
    public InputController InputController
    {
        get
        {
            if (m_InputController == null)
            {
                m_InputController = gameObject.GetComponent<InputController>();
            }
            return m_InputController;
        }
    }
}
