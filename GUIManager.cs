using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    //bool NeedHelp = false;//不需要帮助
    static Color CloseColor = new Color(0, 0, 0, 0);   //关闭时的颜色
    static Color OpenColor = new Color(255, 255, 255, 100);   //关闭时的颜色

    void Start()
    {
        if(GlobalVariable.CurrentSence == "Level3")
                  DisEndStory();                            //第三关不显示故事
    }

    //void Update()
    //{
    //    //帮助界面 
    //    if (GlobalVariable.CurrentSence == "Level2" || GlobalVariable.CurrentSence == "Level3")
    //    {
    //        if (NeedHelp)
    //        {
    //            //GameObject.Find("CanvasCold/Skip").GetComponent<Text>();//.color = Color.red;
    //            //GameObject.Find("CanvasCold/Hint").GetComponent<Image>().color = OpenText;
    //            //GameObject.Find("CanvasCold/Skip").GetComponent<Button>().interactable = true;

    //        }
    //        else
    //        {
    //            //GameObject.Find("CanvasCold/Skip").GetComponent<Image>().color = CloseText;
    //            //GameObject.Find("CanvasCold/Hint").GetComponent<Image>().color = CloseText;
    //            //GameObject.Find("CanvasCold/Skip").GetComponent<Button>().interactable = false;

    //            //GameObject.Find("CanvasCold/DisHelp/Text").GetComponent<Text>().color = CloseText;
    //        }
    //    }

    //}

    //选择关卡
    public void Level_Choose()
    {
        SceneManager.LoadSceneAsync("Level_Choose");
        GlobalVariable.CurrentSence = "Level_Choose";
        //GlobalVariable.CurrentGameState = GameState.Preparing;  //刷新游戏状态 准备中
        //if (SystemInit.FreshMan == 1)  //新手不显示二三关
        //{
        //    GameObject.Find("Canvas/2").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //    GameObject.Find("Canvas/3").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //    GameObject.Find("Canvas/2").GetComponent<Button>().interactable = false;
        //    GameObject.Find("Canvas/3").GetComponent<Button>().interactable = false;
        //}
        return;
    }
        
    //游戏性选项
    public void Setting_Choose()
    {
        SceneManager.LoadSceneAsync("Setting");
        GlobalVariable.CurrentSence = "Setting";
        return;
    }

    //退出
    public void Quit()
    {
        Application.Quit(); //退出
    }
    //返回
    public void Back()
    {
        if (GlobalVariable.CurrentSence == "Level_Choose")
        {
            SceneManager.LoadSceneAsync("start");
            GlobalVariable.CurrentSence = "start";
            GlobalVariable.CurrentGameState = GameState.Preparing;  //刷新游戏状态 准备中
            return;
        }

        if (GlobalVariable.CurrentSence == "Level1")
        {
            SceneManager.LoadSceneAsync("Level_Choose");
            GlobalVariable.CurrentSence = "Level_Choose";
            GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态 游戏中
            return;
        }
        if (GlobalVariable.CurrentSence == "Level2")
        {
            SceneManager.LoadSceneAsync("Level_Choose");
            GlobalVariable.CurrentSence = "Level_Choose";
            GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态 游戏中
            return;
        }
        if (GlobalVariable.CurrentSence == "Level3")
        {
            SceneManager.LoadSceneAsync("Level_Choose");
            GlobalVariable.CurrentSence = "Level_Choose";
            GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态 游戏中
            return;
        }

        if (GlobalVariable.CurrentSence == "Setting")      
        {
            if (GlobalVariable.CurrentGameState == GameState.Playing)
            {
                if (GlobalVariable.CurrentLevelSence == "Level1")
                {
                    SceneManager.LoadSceneAsync("Level1");
                    GlobalVariable.CurrentSence = "Level1";
                    GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态 
                    return;
                }
                else if (GlobalVariable.CurrentLevelSence == "Level2")
                {
                    SceneManager.LoadSceneAsync("Level2");
                    GlobalVariable.CurrentSence = "Level2";
                    GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态 
                    return;
                }
                else if (GlobalVariable.CurrentLevelSence == "Level3")
                {
                    SceneManager.LoadSceneAsync("Level3");
                    GlobalVariable.CurrentSence = "Level3";
                    GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态 
                    return;
                }
            }
            else
            {
                SceneManager.LoadSceneAsync("Level_Choose");
                GlobalVariable.CurrentSence = "Level_Choose";
                GlobalVariable.CurrentGameState = GameState.Preparing;  //刷新游戏状态 准备中
                return;
            }
        }
    }


    //关卡跳转
    public void Level1()
    {
        GlobalVariable.CurrentSence = "Level1";
        GlobalVariable.CurrentLevelSence = "Level1";
        SceneManager.LoadSceneAsync("Level1");
        GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态
        GlobalVariable.CurrentControllState = ControllState.Cold; //刷新操作状态
        return;
    }
    public void Level2()
    {
        GlobalVariable.CurrentSence = "Level2";
        SceneManager.LoadSceneAsync("Level2");
        GlobalVariable.CurrentLevelSence = "Level2";
        GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态
        GlobalVariable.CurrentControllState = ControllState.Cold; //刷新操作状态
        return;
    }
    public void Level3()
    {
        GlobalVariable.CurrentSence = "Level3";
        SceneManager.LoadSceneAsync("Level3");
        GlobalVariable.CurrentLevelSence = "Level3";
        GlobalVariable.CurrentGameState = GameState.Playing;  //刷新游戏状态
        GlobalVariable.CurrentControllState = ControllState.Cold; //刷新操作状态
        return;
    }
    
    /// <summary>
    /// 最终关卡故事使能
    /// </summary>
    public void EndStory()
    {
        HideLevelClearUI();   //隐藏通关提示
        GlobalVariable.CurrentGameState = GameState.Preparing;  //刷新游戏状态
        GlobalVariable.CurrentControllState = ControllState.Cold; //刷新操作状态
        //显示故事
        GameObject.Find("EndStory").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("EndStory").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("EndStory").GetComponent<CanvasGroup>().blocksRaycasts = true;
        return;
    }
    /// <summary>
    /// 最终关卡故事不能
    /// </summary>
    public void DisEndStory()
    {
        GameObject.Find("EndStory").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EndStory").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("EndStory").GetComponent<CanvasGroup>().blocksRaycasts = false;
        return;
    }

    public void NextLevel()
    {
        switch (GlobalVariable.CurrentLevelSence)
        {
            case "Level1": Level2(); break;
            case "Level2": Level3(); break;
            case "Level3": EndStory(); break;
        }
    }
    ///// <summary>
    ///// 帮助界面
    ///// </summary>
    //public void Help()
    //{
    //    NeedHelp = true; //需要帮助
    //}
    //public void DisHelp()
    //{
    //    NeedHelp = false; //不需要帮助
    //}

    /// <summary>
    /// HotUI挂起时，按下的按键
    /// </summary>
    /// <returns>修改全局变量</returns>
    public void TargetMoveUp()
    {
        GlobalVariable.CurrentPressButton = HotButton.MoveUp;
    }
    public void TargetMoveDown()
    {
        GlobalVariable.CurrentPressButton = HotButton.MoveDown;
    }
    public void TargetMoveLeft()
    {
        GlobalVariable.CurrentPressButton = HotButton.MoveLeft;
    }
    public void TargetMoveRight()
    {
        GlobalVariable.CurrentPressButton = HotButton.MoveRight;
    }
    public void TargetRotateClockWise()
    {
        GlobalVariable.CurrentPressButton = HotButton.ClockWise;
    }
    public void TargetRotateAntiClockWise()
    {
        GlobalVariable.CurrentPressButton = HotButton.AntiClockWise;
    }
    public void Button_Loose() //无按键
    {
        GlobalVariable.CurrentPressButton = HotButton.NoPressButton;
    }

    /// <summary>
    /// 显示挂起UI
    /// </summary>
    public static void ShowHotUI()
    {
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().blocksRaycasts = true;
        //开启光标
        GameObject.Find("Curse").GetComponent<SpriteRenderer>().color = OpenColor;
    }
    /// <summary>
    /// 显示冷机UI
    /// </summary>
    public static void ShowColdUI()
    {
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().blocksRaycasts = true;
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().blocksRaycasts = false;
        //关闭光标
        GameObject.Find("Curse").GetComponent<SpriteRenderer>().color = CloseColor;
    }

    /// <summary>
    /// 隐藏通关界面
    /// </summary>
    public static void HideLevelClearUI()
    {
        GameObject.Find("LevelClear").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("LevelClear").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("LevelClear").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    /// <summary>
    /// 显示通关界面
    /// </summary>
    public static void ShowLevelClearUI()
    {
        GameObject.Find("LevelClear").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("LevelClear").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("LevelClear").GetComponent<CanvasGroup>().blocksRaycasts = true;

        //隐藏其他界面
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("CanvasHot").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("CanvasCold").GetComponent<CanvasGroup>().blocksRaycasts = false;
        //关闭光标
        GameObject.Find("Curse").GetComponent<SpriteRenderer>().color = CloseColor;
    }

    
    /// <summary>
    /// HotUI防误触检测
    /// </summary>
    /// <param name="TouchPosition">手指Touch[0]的坐标</param>
    /// <returns>触摸HotUI为真</returns>
    public static bool HotUI_Touch_Check(Vector2 TouchPosition)
    {
        if (TouchPosition.x < 220 && TouchPosition.y < 190) return true; //左边方向键区
        if (TouchPosition.x > 600 && TouchPosition.y < 190) return true; //右边旋转键区
        return false;
    }
}
