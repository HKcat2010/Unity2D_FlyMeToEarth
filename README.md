# Unity2D_FlyMeToEarth
腾讯游戏学院2018年秋季班作品
为了维护美工和策划的汗水，传上来也大，所以这里就只提供脚本了，菜鸡如我你们也不会看的(T.T)
<b>后续还有玩家自定义地图，积分排名，不定期关卡挑战等方式</b>

GUIManager 是用户界面逻辑

GloabalVariable 是变量存储

SystemInit是系统初始化 {读档到全局数据类;新手判定;构建不会被销毁的object;将全局数据挂载在object上;}

FreshMen 是新手引导 {引导全关闭{};新手过程{steps;不再是新手(标志位);}}

PlayerControl 是手势识别<br>
{<br>
相机位置左边界=背景左边界+相机视角宽度/2;<br>
相机位置右边界，相机位置上边界，相机位置下边界;<br>
每帧刷新检测;<br>
{<br>
   if(没有物体被选中){手势识别有效,手指1坐标-手指2坐标=>放大缩小(坐标距离变化趋势);缩放限幅；手指坐标刷新}}<br>
   if(双击物体){选中对象(双击间隔判定);}<br>
   if(双击背景){释放被选中物体;}<br>
}<br>

Bug 是虫洞逻辑

Stone 陨石的行为逻辑

IceManager 冰块的行为逻辑<br>
![image](https://github.com/HKcat2010/MyPhoto/blob/master/FlymeToEarthP1.PNG)<br>
LightSource 划线器<br>
{<br>
初始化光源方向;<br>
switch()<br>
{<br>
case 边界：不划线;break;<br>
case 冰块：冰块使用个数++;<br>
          新建反射光线，出射点需要有偏移避免“直接撞上刚体”{<br>
          {<br>
          光源设定为反射点();<br>
          反射向量计算：<br>
          ![image](https://github.com/HKcat2010/MyPhoto/blob/master/FlymeToEarthP2.PNG)
          <br>
          }<br>
          划线;break;<br>
case 石块：不划线;break;<br>
case 虫洞：入口记录，出口新建光源;break;<br>
case 飞船：不划线，记录状态;break;<br>
case 黑洞：不划线;break;<br>
}<br>
}<br>

Ship 是飞船(通关检测)逻辑<br>
{<br>
if(飞船被稳定照射150帧)<br>
{<br>
通关计算得分(100-w1用时-w2道具浪费-w3道具损坏+w4特殊奖励)；<br>
跳转下一关卡;<br>
}<br>
}<br>


