using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//初始化对象
public class SystemInit : MonoBehaviour
{
    public static GlobalVariable GameData;    //游戏数据对象 

    public static int FreshMan = 1;       //新手标志 1 

    void Start()
    {
        if (PlayerPrefs.HasKey("FreshMan")) //检测是否完成新手引导
            FreshMan = PlayerPrefs.GetInt("FreshMan");

        GameObject SystemObject = new GameObject("SystemObject");
        //构造一个不被销毁的系统对象  // MonoBehaviour派生类不允许使用DontDestroyOnLoad

        DontDestroyOnLoad(SystemObject);
        //确保系统对象不被销毁

        GameData = SystemObject.AddComponent<GlobalVariable>(); 
        //将数据对象挂载在不会消失的系统对象上
    }
}
