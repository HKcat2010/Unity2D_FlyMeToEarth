using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugOut : MonoBehaviour
{
    public bool LightIn = false;

    public Ray2D IncidentLight = new Ray2D(); //射入虫洞光线

    public int ReflectionCount = 0;    //已反射次数

    private LineRenderer LightLine;    //虫洞划线器

    // Use this for initialization
    void Start ()
    {
        LightLine = gameObject.GetComponent<LineRenderer>(); //虫洞划线器

    }

    // Update is called once per frame
    void Update ()
    {
        int HitCount = 0;                                           //光线射中次数

        int RemainReflectionCount = GlobalVariable.LIGHT_MAX_NUM - ReflectionCount; //剩余折射次数

        Ray2D[] CheckRayLine = new Ray2D[RemainReflectionCount];      //剩余检测射线

        Vector3[] LightLineVertex = new Vector3[RemainReflectionCount + 1];  //折点坐标存储

        RaycastHit2D[] HitObject = new RaycastHit2D[RemainReflectionCount]; //击中物体 

        if (LightIn && RemainReflectionCount > 0)  //有光线射入且可以接着反射
        {
                //初始时刻光线 和虫洞 垂直
                if (gameObject.transform.eulerAngles.z == 90) CheckRayLine[0] = new Ray2D(gameObject.transform.position, new Vector2(0, 1));
                else if (gameObject.transform.eulerAngles.z == -90) CheckRayLine[0] = new Ray2D(gameObject.transform.position, new Vector2(0, -1));
                else CheckRayLine[0] = new Ray2D(gameObject.transform.position, new Vector2(-1, Mathf.Tan(-gameObject.transform.eulerAngles.z) * Mathf.Deg2Rad));
            //虫洞垂直射出线
            

            //--------------光线路径判断------------------
            for (int i = 0; i < RemainReflectionCount - 1; i++)
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
                   //再次进入虫洞
                    LightLineVertex[i + 1].x = HitObject[i].point.x;    //存储光线折点
                    LightLineVertex[i + 1].y = HitObject[i].point.y;
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
        }
        else
            RemainReflectionCount = 0;  //不允许反射

        //开始画光线
        LightLine.positionCount = RemainReflectionCount + 1;      //最大折点数 = 剩余最多折射次数 +1

        LightLineVertex[0] = gameObject.transform.position;  //初始点为虫洞

        for (int i = 0; i < LightLine.positionCount; i++)
            if (i >= HitCount) LightLineVertex[i] = LightLineVertex[HitCount];
        // 最后反射

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
        float Nx = HitObject.normal.x * 10;
        float Ny = HitObject.normal.y * 10;
        float Ix = IncidentLight.direction.x * 10;
        float Iy = IncidentLight.direction.y * 10;

        ReflectionLight = new Ray2D(ReflectionPoint, new Vector2(-2 * (Ix * Nx + Iy * Ny) * Nx + Ix, -2 * (Ix * Nx + Iy * Ny) * Ny + Iy));
        return ReflectionLight;
    }
}