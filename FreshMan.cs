using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreshMan : MonoBehaviour
{
    int Step = 0; //新手步骤
    Color CloseText= new Color(0, 0, 0, 0);   //关闭时的颜色
    Color OpenText = new Color(255, 255, 255, 255);   //显示时的颜色

    // Use this for initialization
    void Start ()
    {
        //开始关闭引导图片
        GameObject.Find("FreshMan/Story1").GetComponent<Image>().color = CloseText;
        GameObject.Find("FreshMan/Story2").GetComponent<Image>().color = CloseText;
        GameObject.Find("FreshMan/Story3").GetComponent<Image>().color = CloseText;
        GameObject.Find("FreshMan/Direction").GetComponent<Image>().color = CloseText;
        GameObject.Find("FreshMan/Rotation").GetComponent<Image>().color = CloseText;
        GameObject.Find("FreshMan/Back").GetComponent<Image>().color = CloseText;
        GameObject.Find("FreshMan/Ice").GetComponent<Image>().color = CloseText;
        GameObject.Find("FreshMan/Score").GetComponent<Image>().color = CloseText;
        GameObject.Find("FreshMan/StartYourTrip").GetComponent<Image>().color = CloseText;
        GUIManager.HideLevelClearUI();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (SystemInit.FreshMan == 1)
        {
            //新手须完成
            switch (Step)
            {
                case 0:GameObject.Find("FreshMan/Story1").GetComponent<Image>().color = OpenText;break;
                case 1:
                    {
                        GameObject.Find("FreshMan/Story1").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/Story2").GetComponent<Image>().color = OpenText;
                    } break;
                case 2:
                    {
                        GameObject.Find("FreshMan/Story2").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/Story3").GetComponent<Image>().color = OpenText;
                    }
                    break;
                case 3:
                    {
                        GameObject.Find("FreshMan/Story3").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/Ice").GetComponent<Image>().color = OpenText;
                        
                    }
                    break;
                case 4:
                    {
                        GameObject.Find("FreshMan/Ice").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/Direction").GetComponent<Image>().color = OpenText;
                        GUIManager.ShowHotUI();
                    }
                    break;
                case 5:
                    {
                        GameObject.Find("FreshMan/Direction").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/Rotation").GetComponent<Image>().color = OpenText;
                    }
                    break;
                case 6:
                    {
                        GameObject.Find("FreshMan/Rotation").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/Back").GetComponent<Image>().color = OpenText;
                    }
                    break;
                case 7:
                    {
                        GameObject.Find("FreshMan/Back").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/Score").GetComponent<Image>().color = OpenText;
                    }
                    break;
                case 8:
                    {
                        GameObject.Find("FreshMan/Score").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/StartYourTrip").GetComponent<Image>().color = OpenText;
                    }
                    break;
                case 9:
                    {
                        GameObject.Find("FreshMan/StartYourTrip").GetComponent<Image>().color = CloseText;
                        GameObject.Find("FreshMan/BackGround").GetComponent<Image>().color = CloseText;
                        SystemInit.FreshMan = 0;   //不再是新手
                        PlayerPrefs.SetInt("FreshMan", SystemInit.FreshMan);
                        GUIManager.ShowColdUI(); //显示冷操作界面
                    }
                    break;
            }
        }
        else
        {
            //非新手销毁引导界面
            GameObject.Find("FreshMan").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("FreshMan").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("FreshMan").GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void Skip()
    {
        Step++;
    }
}
