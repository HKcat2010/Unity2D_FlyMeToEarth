using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Preparing
}    
//游戏状态：进入关卡游戏中；离开关卡准备游戏

public enum ControllState
{
    Hot,
    Cold
}
//操作状态 ：挂起操作物体； 冷机操作屏幕；

public enum HotButton
{
    MoveUp,      
    MoveDown,
    MoveRight,
    MoveLeft,
    ClockWise,
    AntiClockWise,
    NoPressButton
}
//挂起时使用的按钮：左移，右移，上移，下移，顺时针旋转，逆时针旋转,未按下


public class GlobalVariable : MonoBehaviour
{

    public static string CurrentSence = "start"; //当前显示场景
    public static string CurrentLevelSence = CurrentSence; //调整游戏选项时使用
    public static float MainCameraPosition_z = -100;//相机纵坐标位置

    
    //当前状态
    public static GameState CurrentGameState = GameState.Preparing;        //当前游戏状态
    public static ControllState CurrentControllState = ControllState.Cold; //当前操作状态
    public static HotButton CurrentPressButton = HotButton.NoPressButton;  //热状态下按下的按键 
    public static GameObject Hot_Object = null;                            //热状态下操作的对象
    public static int LIGHT_MAX_NUM = 10;                                   //光线反射最大次数

    

}
