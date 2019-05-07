using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //不用可以删除


public class Ship : MonoBehaviour
{

    public bool LightHit = false;  //并未照射飞船
    private int LightHitCount = 0;  //飞船被照射时间    

    //----------------分数------------------
    public int CurrentUsedIce = 0;                                //当前在使用的冰块数
    public float CurrentUsedTime;                                  //当前在使用的时间
    public int CurrentSpecialBonus = 0;                           //特殊奖励情况 黑洞 虫洞 击碎等等
    public int CurrentStoneHit = 0;                               //陨石撞击扣分
    public bool LevelClear = false;                                 //当前未通关
    float CurrentScore = 0;                                  //当前未得分
    float HighestScore = 0;                                  //最高得分
    float CTime = 0;
    float CIce = 0;
    float CBonus = 0;
    float CCrash = 0;
    float HTime = 0;
    float HIce = 0;
    float HBonus = 0;
    float HCrash = 0;

    // Use this for initialization
    void Start()
    {
        LevelClear = false;     //清空通关标志
        CurrentStoneHit = 0;    //状态清理

        GUIManager.HideLevelClearUI();         //隐藏通关界面

        if(SystemInit.FreshMan == 1)StoreScore(GlobalVariable.CurrentLevelSence); //新手需要写一次

        GetScore(GlobalVariable.CurrentLevelSence); //读档
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!LevelClear)  //未通关前
        {
            if (LightHit)              //飞船被照射
                LightHitCount++;
            else                        //未照射
                LightHitCount = 0;
            if (LightHitCount == 100) //照射超过150帧通关
            {
                LevelClear = true;
                CurrentUsedTime = Time.timeSinceLevelLoad; //通关时间
                GUIManager.ShowLevelClearUI();   //显示通关界面
            }
        }
        else //通关后
        {
            //----------计算通关得分-------------
            if (GlobalVariable.CurrentLevelSence == "Level1")
            {
                CTime = (100 - CurrentUsedTime * 0.1f) * 0.4f;    //计时
                CIce = (100 - CurrentUsedIce * 20) * 0.6f;        //冰块使用
                CBonus = 0;                                        //特殊奖励
                CCrash = CurrentStoneHit * 5;                     //碰撞减分
            }
            if (GlobalVariable.CurrentLevelSence == "Level2" || GlobalVariable.CurrentLevelSence == "Level3")
            {
                CTime = (100 - CurrentUsedTime * 0.1f) * 0.3f;    //计时
                CIce = (100 - CurrentUsedIce * 20) * 0.4f;        //冰块使用
                CBonus = (60 + CurrentSpecialBonus * 40) * 0.3f;  //特殊奖励
                CCrash = CurrentStoneHit * 5;                     //碰撞减分
            }
                

            //总得分
            CurrentScore = CTime + CIce + CBonus - CCrash;
            if (CurrentScore < 60) CurrentScore = 60;

            //-----------显示得分----------------
            GameObject.Find("LevelClear/Score").GetComponent<Text>().text = ((int)CurrentScore).ToString();
            GameObject.Find("LevelClear/CIce").GetComponent<Text>().text = ((int)CIce).ToString();
            GameObject.Find("LevelClear/CTime").GetComponent<Text>().text = ((int)CTime).ToString();
            GameObject.Find("LevelClear/CBonus").GetComponent<Text>().text = ((int)CBonus).ToString();
            GameObject.Find("LevelClear/CCrash").GetComponent<Text>().text = "- " + ((int)CCrash).ToString();

            GameObject.Find("LevelClear/Record").GetComponent<Text>().text = ((int)HighestScore).ToString();
            GameObject.Find("LevelClear/RIce").GetComponent<Text>().text = ((int)HIce).ToString();
            GameObject.Find("LevelClear/RTime").GetComponent<Text>().text = ((int)HTime).ToString();
            GameObject.Find("LevelClear/RBonus").GetComponent<Text>().text = ((int)HBonus).ToString();
            GameObject.Find("LevelClear/RCrash").GetComponent<Text>().text = "- " + ((int)HCrash).ToString();
            //数据存储
            if (CurrentScore > HighestScore) //刷新最高分
                StoreScore(GlobalVariable.CurrentLevelSence);
        }

    }

    //关卡得分存档
    public void StoreScore(string CurrentLevelSence)
    {
        switch (CurrentLevelSence)
        {
            case "Level1":
                {
                    PlayerPrefs.SetFloat("Level1HighestScore", CurrentScore);
                    PlayerPrefs.SetFloat("Level1HighestIce", CIce);
                    PlayerPrefs.SetFloat("Level1HighestTime", CTime);
                    PlayerPrefs.SetFloat("Level1HighestBonus", CBonus);
                    PlayerPrefs.SetFloat("Level1HighestCrash", CCrash);
                }
                break;
            case "Level2":
                {
                    PlayerPrefs.SetFloat("Level2HighestScore", CurrentScore);
                    PlayerPrefs.SetFloat("Level2HighestIce", CIce);
                    PlayerPrefs.SetFloat("Level2HighestTime", CTime);
                    PlayerPrefs.SetFloat("Level2HighestBonus", CBonus);
                    PlayerPrefs.SetFloat("Level2HighestCrash", CCrash);
                }
                break;
            case "Level3":
                {
                    PlayerPrefs.SetFloat("Level3HighestScore", CurrentScore);
                    PlayerPrefs.SetFloat("Level3HighestIce", CIce);
                    PlayerPrefs.SetFloat("Level3HighestTime", CTime);
                    PlayerPrefs.SetFloat("Level3HighestBonus", CBonus);
                    PlayerPrefs.SetFloat("Level3HighestCrash", CCrash);
                }
                break;
        }
    }
    //关卡取档
    public void GetScore(string CurrentLevelSence)
    {
        switch (CurrentLevelSence)
        {
            case "Level1":
                {
                    HighestScore = PlayerPrefs.GetFloat("Level1HighestScore"); //获取当前关最高分
                    HIce = PlayerPrefs.GetFloat("Level1HighestIce"); 
                    HTime = PlayerPrefs.GetFloat("Level1HighestTime"); 
                    HBonus = PlayerPrefs.GetFloat("Level1HighestBonus"); 
                    HCrash = PlayerPrefs.GetFloat("Level1HighestCrash"); 
                }
                break;
            case "Level2":
                {
                    HighestScore = PlayerPrefs.GetFloat("Level2HighestScore"); //获取当前关最高分
                    HIce = PlayerPrefs.GetFloat("Level2HighestIce");
                    HTime = PlayerPrefs.GetFloat("Level2HighestTime");
                    HBonus = PlayerPrefs.GetFloat("Level2HighestBonus");
                    HCrash = PlayerPrefs.GetFloat("Level2HighestCrash");
                }
                break;
            case "Level3":
                {
                    HighestScore = PlayerPrefs.GetFloat("Level3HighestScore"); //获取当前关最高分
                    HIce = PlayerPrefs.GetFloat("Level3HighestIce");
                    HTime = PlayerPrefs.GetFloat("Level3HighestTime");
                    HBonus = PlayerPrefs.GetFloat("Level3HighestBonus");
                    HCrash = PlayerPrefs.GetFloat("Level3HighestCrash");
                }
                break;
        }
       
    }
}
