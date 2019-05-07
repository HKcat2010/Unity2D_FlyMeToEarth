using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stone : MonoBehaviour
{
    public int LightHitCount = 0;                    //统计被照光数
    
    private const int StoneExplosionDegree = 150;    //爆炸热量限
    private int HeatDegree = 0;                      //当前陨石热量
    private Color StoneColor;                        //记录陨石色彩通道正常值
    private Color CurrentStoneColor;                //记录陨石色彩通道当前值
    private Animator StoneExplosionAnimator;       //陨石爆炸过场

	// Use this for initialization
	void Start ()
    {
       StoneExplosionAnimator = gameObject.GetComponent<Animator>();
        StoneColor = gameObject.GetComponent<SpriteRenderer>().color; //记录陨石当前色彩
        CurrentStoneColor = StoneColor; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LightHitCount > 2)
        {
            HeatDegree++;                       //加热 变红
            if (CurrentStoneColor.g > 0 && CurrentStoneColor.b > 0)
            {
                CurrentStoneColor.g -= 0.005f;
                CurrentStoneColor.b -= 0.005f;
            }
            gameObject.GetComponent<SpriteRenderer>().color = CurrentStoneColor; //石块
            
        }
        else if (HeatDegree > 0)
        {
            HeatDegree--;    //冷却至零度 还原色彩
            if (CurrentStoneColor.g < StoneColor.g && CurrentStoneColor.b < StoneColor.b)
            {
                CurrentStoneColor.g += 0.005f;
                CurrentStoneColor.b += 0.005f;
            }
        }
        if (HeatDegree > StoneExplosionDegree)
        {
            //石头爆炸
            CurrentStoneColor.a = 0;    //石块透明
            gameObject.GetComponent<SpriteRenderer>().color = CurrentStoneColor; 
            StoneExplosionAnimator.SetBool("StoneExplosion", true); //爆炸动画
            Destroy(this.gameObject);  //销毁陨石
        }

        LightHitCount = 0;  //刷新光照数量
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
            GameObject.Find("Ship").GetComponent<Ship>().CurrentStoneHit++;  //碰上冰块
    }
}
