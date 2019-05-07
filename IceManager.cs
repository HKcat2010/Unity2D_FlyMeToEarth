using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Ice 类通用管理 只挂载一个Ice对象下 
/// </summary>
public class IceManager : MonoBehaviour
{
    GameObject[] IceArray;
    //遍历所有Ice类

    // Use this for initialization
    void Start ()
    {
        IceArray = GameObject.FindGameObjectsWithTag("Ice");

        for (int i = 0; i < IceArray.Length; i++)
        {
            IceArray[i].GetComponent<Rigidbody2D>().gravityScale = 0;  //重力设0
            IceArray[i].GetComponent<Rigidbody2D>().simulated = true;  //开启模拟
            IceArray[i].GetComponent<Rigidbody2D>().mass = 1;          //质量1
        }
        //初始化冰块参数
    }
	
	// Update is called once per frame
	void Update ()
    {
        //冰块仅在通关前可操作
        if (!GameObject.Find("Ship").GetComponent<Ship>().LevelClear)
        {
            //---------电脑调试--------------
            {
                Vector3 ObjectNewPosition = IceArray[0].transform.position;
                if (Input.GetKey(KeyCode.W)) ObjectNewPosition.y += 0.1f;
                else if (Input.GetKey(KeyCode.S)) ObjectNewPosition.y += -0.1f;
                else if (Input.GetKey(KeyCode.A)) ObjectNewPosition.x += -0.1f;
                else if (Input.GetKey(KeyCode.D)) ObjectNewPosition.x += 0.1f;
                else if (Input.GetKey(KeyCode.Q)) IceArray[0].transform.Rotate(0, 0, -1);
                else if (Input.GetKey(KeyCode.E)) IceArray[0].transform.Rotate(0, 0, 1);
                IceArray[0].transform.position = ObjectNewPosition;
            }
            //--------------------------------------------------------------
            //无挂起UI按键按下
            if (GlobalVariable.CurrentPressButton == HotButton.NoPressButton)
            {
                if (Camera.main.GetComponent<PlayerControl>().Double_Click_Check())
                {
                    //检测到双击事件 
                    for (int i = 0; i < IceArray.Length; i++)
                    {
                        if (PlayerControl.Touch_2D_Position_Check(Input.touches[0].position, IceArray[i].GetComponent<Rigidbody2D>().position, 15))
                        {
                            GlobalVariable.CurrentControllState = ControllState.Hot;  //双击冰块挂起
                            GlobalVariable.Hot_Object = IceArray[i];          //存储当前操作对象
                            GUIManager.ShowHotUI();                                  //切换热UI界面
                            break;                                            //跳出遍历
                        }
                        GlobalVariable.CurrentControllState = ControllState.Cold;
                    }
                    if (GlobalVariable.CurrentControllState == ControllState.Cold)   //没有选中冰块  双击背景释放
                        GUIManager.ShowColdUI();                                //切换冷UI界面
                    GameObject.Find("Curse").transform.position = GlobalVariable.Hot_Object.GetComponent<Rigidbody2D>().transform.position;
                }
            }
            else //有挂起UI按键按下
            {
                Vector3 ObjectNewPosition = GlobalVariable.Hot_Object.GetComponent<Rigidbody2D>().transform.position;
                GameObject.Find("Curse").transform.position = GlobalVariable.Hot_Object.GetComponent<Rigidbody2D>().transform.position;
                //光标移动至选中冰晶上
                switch (GlobalVariable.CurrentPressButton)
                {
                    //根据相应按键修改当前值
                    case HotButton.MoveUp: ObjectNewPosition.y += 0.1f; break;
                    case HotButton.MoveDown: ObjectNewPosition.y += -0.1f; break;
                    case HotButton.MoveLeft: ObjectNewPosition.x += -0.1f; break;
                    case HotButton.MoveRight: ObjectNewPosition.x += 0.1f; break;
                    case HotButton.ClockWise: GlobalVariable.Hot_Object.GetComponent<Rigidbody2D>().transform.Rotate(0, 0, -0.3f); break;
                    case HotButton.AntiClockWise: GlobalVariable.Hot_Object.GetComponent<Rigidbody2D>().transform.Rotate(0, 0, 0.3f); break;
                }
                GlobalVariable.Hot_Object.GetComponent<Rigidbody2D>().transform.position = ObjectNewPosition;
            }
        }
    }
}
