using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    float Left_Limit, Right_Limit, Up_Limit, Down_Limit;
    // Use this for initialization
    void Start()
    {
        Input.multiTouchEnabled = true;    //开启多点触碰
        GUIManager.ShowColdUI();                   //切换冷UI界面
        GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize = 100; //初始最大视野
    }

    // Update is called once per frame
    void Update()
    {
        //设置相机边界
        Left_Limit = GameObject.Find("BounderLeft").transform.position.x + Camera.main.orthographicSize*2f;
        Right_Limit = GameObject.Find("BounderRight").transform.position.x - Camera.main.orthographicSize*2f;
        Up_Limit = GameObject.Find("BounderUp").transform.position.y - Camera.main.orthographicSize * 2f;
        Down_Limit = GameObject.Find("BounderDown").transform.position.y + Camera.main.orthographicSize*2f;
        Enlarge_Operation(); //屏幕放大检测
        Slide_Operation();  //单指滑动检测
    }

    /// <summary>
    /// 单指滑动检测
    /// </summary>
    public void Slide_Operation()
    {
        if (Input.touchCount == 1)     //单指触控
        {
            Touch Finger0 = Input.touches[0]; //第0个手指

            if (GlobalVariable.CurrentControllState == ControllState.Hot && GUIManager.HotUI_Touch_Check(Input.touches[0].position)) 
                //热状态下 触碰GUI无效
                return;
            else
            {
                //滑动结束 
                if (Finger0.phase == TouchPhase.Ended) return;

                //滑动开始
                if (Finger0.phase == TouchPhase.Began) return;

                //滑动中
                if (Finger0.phase == TouchPhase.Moved) //手指在移动
                {
                    //摄像机位移限幅
                    Vector3 CameraNewPosition = Camera.main.transform.position;

                    CameraNewPosition += new Vector3(-Finger0.deltaPosition.x / 5, -Finger0.deltaPosition.y / 5, 0);
                    
                    if (CameraNewPosition.x < Left_Limit) CameraNewPosition.x = Left_Limit;
                    if (CameraNewPosition.x > Right_Limit) CameraNewPosition.x = Right_Limit;
                    if (CameraNewPosition.y < Down_Limit) CameraNewPosition.y = Down_Limit;
                    if (CameraNewPosition.y > Up_Limit) CameraNewPosition.y = Up_Limit;

                    CameraNewPosition.z = GlobalVariable.MainCameraPosition_z;

                    Camera.main.transform.position = CameraNewPosition;
                    //刷新触点位置
                }
            }    
        }
    }

    /// <summary>
    /// 双击确认函数
    /// </summary>
    /// <returns>返回确认标志</returns>
    private bool Double_click_first = false;      //双击第一击状态
    private int Double_click_cnt = 0;             //双击间隔计数存
    private bool Double_click_send = false;       //双击发送确认
    private const int Double_click_interval = 8;  //双击间隔帧      
    public bool Double_Click_Check()
    {
        //有单点触控输入
        if (Input.touchCount == 1) 
        {
            //第一击检测
            if (Double_click_first == false) Double_click_first = true;

            if (Double_click_first == true  && Double_click_cnt > 2 && Double_click_cnt < Double_click_interval && Double_click_send == false)
            {
                //松开后 ?帧内第二击 

                //Hot状态下 未触碰GUI区 才是有效双击
                if (GlobalVariable.CurrentControllState == ControllState.Hot && GUIManager.HotUI_Touch_Check(Input.touches[0].position))
                    return false;
                else
                {
                    Double_click_send = true; //确认发送
                    return true;
                }
            }
        }
        //手指松开无触点
        if (Input.touchCount == 0)
        {
            //双击松开后刷新状态
            if (Double_click_send == true)
            {
                Double_click_first = false;
                Double_click_send = false;
                Double_click_cnt = 0;
            } 

            //第一击后开始计时
            if (Double_click_first == true) Double_click_cnt++;

            //一击 帧后仍未第二击
            if (Double_click_cnt >= Double_click_interval) 
            {
                //标志位状态刷新
                Double_click_send = false;
                Double_click_first = false;
                Double_click_cnt = 0;
            }
        }
        return false; //无双击事件
    }


    /// <summary>
    /// 屏幕放大
    /// </summary>
    private  Vector2 EnlargePosition0= new Vector2();     //保存触点1
    private  Vector2 EnlargePosition1= new Vector2();     //保存触点2
    private Vector3 CameraNewPosition = new Vector3();     //保存相机新目标位置
    public void Enlarge_Operation()
    {
        if (Input.touchCount == 2)     //两指触控
        {
            Touch Finger0 = Input.touches[0]; //第0个手指
            Touch Finger1 = Input.touches[1]; //第1个手指

            //滑动结束
            if (Finger0.phase == TouchPhase.Ended || Finger1.phase == TouchPhase.Ended) return;

            //滑动开始 记录第一触点
            if (Finger0.phase == TouchPhase.Began || Finger1.phase == TouchPhase.Began)
            {
                EnlargePosition0= Finger0.position;
                EnlargePosition1= Finger1.position;
                //更新相机目标位置
                Vector3 TempPosition = (Finger0.position + Finger1.position) / 2;
                CameraNewPosition = Camera.main.ScreenToWorldPoint(new Vector3(TempPosition.x, TempPosition.y, -GlobalVariable.MainCameraPosition_z));
                CameraNewPosition.z = GlobalVariable.MainCameraPosition_z;
                //摄像机位移限幅
                if (CameraNewPosition.x < Left_Limit) CameraNewPosition.x = Left_Limit;
                if (CameraNewPosition.x > Right_Limit) CameraNewPosition.x = Right_Limit;
                if (CameraNewPosition.y < Down_Limit) CameraNewPosition.y = Down_Limit;
                if (CameraNewPosition.y > Up_Limit) CameraNewPosition.y = Up_Limit;
                return;
            }

            //滑动中
            if (Finger0.phase == TouchPhase.Moved && Finger1.phase == TouchPhase.Moved) //两手指在移动
            {
                //上一帧两点间距
                float oldDistance = Vector2.Distance(EnlargePosition0, EnlargePosition1);

                //当前帧两点间距
                float newDistance = Vector2.Distance(Finger0.position, Finger1.position);
                
                //触点距离增大
                if (newDistance > oldDistance)
                {
                    //摄像机视角非目标位置时 移动 
                    if (Camera.main.transform.position != CameraNewPosition)
                        Camera.main.transform.position += (CameraNewPosition - Camera.main.transform.position) / 200; 
                    //数字越大镜头移动越慢
                    
                    // 视野减小 限幅
                    if (GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize > 40)
                    {
                        if ((newDistance - oldDistance) / 2 > 10) //放大限速
                            GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize -= 10;
                        else
                            GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize -= (newDistance - oldDistance)/2;
                    }
                }

                //触点距离减小
                if (newDistance < oldDistance)
                {
                    //摄像机视角非目标位置时 移动 
                    if (Camera.main.transform.position != CameraNewPosition)
                        Camera.main.transform.position += (CameraNewPosition - Camera.main.transform.position) / 200;
                    //数字越大镜头移动越慢
                    
                    // 视野增大 限幅
                    if (GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize < 100)
                    {
                        if ((oldDistance - newDistance) / 2 > 10) //缩小限速
                            GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize += 10;
                        else
                            GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize += (oldDistance - newDistance)/2;
                    }

                }
                //刷新触点位置
                EnlargePosition0= Finger0.position;
                EnlargePosition1= Finger1.position;
            }
        }
    }


    /// <summary>
    /// 触点位置检测
    /// </summary>
    /// <param name="TouchPosition">触点屏幕坐标2D</param>
    /// <param name="TargetPosition">待检测世界坐标3D</param>
    /// <param name="DeltaDistance">允许偏差范围</param>
    /// <returns>BOOL触点重合检测点</returns>
    public static bool Touch_2D_Position_Check(Vector2 TouchPosition, Vector3 TargetPosition, float DeltaDistance)
    {
        Vector3 TouchToWorld = Camera.main.ScreenToWorldPoint(new Vector3(TouchPosition.x, TouchPosition.y, -GlobalVariable.MainCameraPosition_z));
        Vector2 Temp1 = new Vector2(TouchToWorld.x, TouchToWorld.y);
        Vector2 Temp2 = new Vector2(TargetPosition.x, TargetPosition.y);
        if (Vector2.Distance(Temp1, Temp2) <= DeltaDistance)
            return true;
        else
            return false;
    }
}
