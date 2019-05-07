using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//一关只有一个
public class LightSource : MonoBehaviour
{
    LineRenderer LightLine;
    private GameObject[] BugIn;                    //获取虫洞入口
    private GameObject[] BugOut;                   //获取虫洞出口
    //获取黑洞

    // Use this for initialization
    void Start()
    {
        LightLine = gameObject.GetComponent<LineRenderer>();                        //获取本光源划线器

        //获取虫洞入口
        BugIn = GameObject.FindGameObjectsWithTag("BugIn");

        //获取虫洞出口
        BugOut = GameObject.FindGameObjectsWithTag("BugOut");
    }

    // Update is called once per frame
    void Update()
    {

        int HitCount = 0;                                    //光线反射次数

        Ray2D[] CheckRayLine = new Ray2D[GlobalVariable.LIGHT_MAX_NUM];            //检测射线

        RaycastHit2D[] HitObject = new RaycastHit2D[GlobalVariable.LIGHT_MAX_NUM]; //击中的物体 

        Vector3[] LightLineVertex = new Vector3[GlobalVariable.LIGHT_MAX_NUM + 1];     //折点坐标存储 

        //初始时刻光线 和透镜垂直
        if (gameObject.transform.eulerAngles.z == 90) CheckRayLine[0] = new Ray2D(gameObject.transform.position, new Vector2(0, 1));
        else if (gameObject.transform.eulerAngles.z == -90) CheckRayLine[0] = new Ray2D(gameObject.transform.position, new Vector2(0, -1));
        else CheckRayLine[0] = new Ray2D(gameObject.transform.position, new Vector2(-1, Mathf.Tan(-gameObject.transform.eulerAngles.z * Mathf.Deg2Rad)));

        //--------------------//刷新状态----------------------
        if (BugIn.Length != 0)BugOut[0].GetComponent<BugOut>().LightIn = false;  //关卡内有虫洞才刷新虫洞状态，否则Null异常
        GameObject.Find("Ship").GetComponent<Ship>().LightHit = false; //未照射在飞船上
        GameObject.Find("Ship").GetComponent<Ship>().CurrentUsedIce = 0;
        GameObject.Find("Ship").GetComponent<Ship>().CurrentSpecialBonus = 0;
        

        //-------------------判断光线路径------------------
        for (int i = 0; i < GlobalVariable.LIGHT_MAX_NUM  - 1; i++)
        {
            HitObject[i] = Physics2D.Raycast(CheckRayLine[i].origin, CheckRayLine[i].direction);
            //碰撞检测

            HitCount++; //碰撞次数

            if (HitObject[i].transform.CompareTag("Bounder"))  //射向边界
            {
                LightLineVertex[i + 1].x = HitObject[i].point.x;            //存储光线折点
                LightLineVertex[i + 1].y = HitObject[i].point.y;
                break;                                     //停止继续搜寻
            }
            else if (HitObject[i].transform.CompareTag("Ice"))              //射向冰块
            {
                GameObject.Find("Ship").GetComponent<Ship>().CurrentUsedIce++;                            //使用冰块计数
                LightLineVertex[i + 1].x = HitObject[i].point.x;            //存储光线折点
                LightLineVertex[i + 1].y = HitObject[i].point.y;
                CheckRayLine[i + 1] = LightToIce(HitObject[i], CheckRayLine[i]);   //反射检测线更新
                continue;                                //继续搜寻
            }
            else if (HitObject[i].transform.CompareTag("Stone"))   //射向石块
            {
                LightLineVertex[i + 1].x = HitObject[i].point.x;            //存储光线折点
                LightLineVertex[i + 1].y = HitObject[i].point.y;
                HitObject[i].collider.GetComponent<Stone>().LightHitCount++;
                break;                                   //停止继续搜寻
            }
            else if (HitObject[i].transform.CompareTag("BugIn"))  //虫洞入口
            {
                GameObject.Find("BugOut").GetComponent<BugOut>().LightIn = true;
                GameObject.Find("BugOut").GetComponent<BugOut>().IncidentLight=CheckRayLine[i];  //光线射入虫洞
                GameObject.Find("BugOut").GetComponent<BugOut>().ReflectionCount = HitCount; //光线剩余反射次数
                LightLineVertex[i + 1].x = HitObject[i].point.x;    //存储光线折点
                LightLineVertex[i + 1].y = HitObject[i].point.y;
                GameObject.Find("Ship").GetComponent<Ship>().CurrentSpecialBonus++;       //特殊奖励
                break;                                        //停止继续搜寻
            }
            else if (HitObject[i].transform.CompareTag("Ship"))  //射向船上 
            {
                HitObject[i].collider.GetComponent<Ship>().LightHit = true; //确实照射在飞船上
                LightLineVertex[i + 1].x = HitObject[i].point.x;            //存储光线折点
                LightLineVertex[i + 1].y = HitObject[i].point.y;
                break;                                   //停止继续搜寻
            }
            else if (HitObject[i].transform.CompareTag("BlackHole"))  //射向黑洞
            {
                LightLineVertex[i + 1].x = HitObject[i].point.x;            //存储光线折点
                LightLineVertex[i + 1].y = HitObject[i].point.y;
                GameObject.Find("Ship").GetComponent<Ship>().CurrentSpecialBonus++;         //特殊奖励
                break;                                   //停止继续搜寻
            }
            break;
        }

        //开始画光线
        LightLine.positionCount = GlobalVariable.LIGHT_MAX_NUM + 1;      //最大折点数 = 最大光线数+1

        LightLineVertex[0] = gameObject.transform.position;  //初始点为光源

        for (int i = 0; i < LightLine.positionCount; i++)
            if (i >= HitCount) LightLineVertex[i] = LightLineVertex[HitCount];//光线折点坐标更新为最后的撞击点
        
        LightLine.SetPositions(LightLineVertex);
    }

    /// <summary>
    /// 冰块反射光
    /// </summary>
    /// <param name="HitObject">反射物体</param>
    /// <param name="IncidentLight">入射光</param>
    /// <returns>反射光</returns>
    public Ray2D LightToIce(RaycastHit2D HitObject, Ray2D IncidentLight)
    {
        Ray2D ReflectionLight = new Ray2D(); //反射光线
        Vector2 ReflectionPoint = new Vector2(HitObject.point.x - IncidentLight.direction.normalized.x * 1,
                                              HitObject.point.y - IncidentLight.direction.normalized.y * 1);
        //偏移反射出发点以防止出发点重合RigidBody

        //向量计算对称向量
        float Nx = HitObject.normal.x *10;
        float Ny = HitObject.normal.y *10;
        float Ix = IncidentLight.direction.x *10;
        float Iy = IncidentLight.direction.y *10;

        ReflectionLight = new Ray2D(ReflectionPoint, new Vector2(-2 * (Ix * Nx + Iy * Ny) * Nx + Ix, -2 * (Ix * Nx + Iy * Ny) * Ny + Iy));
        return ReflectionLight;
    }
}
