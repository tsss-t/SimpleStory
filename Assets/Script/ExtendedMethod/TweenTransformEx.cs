/** 
    TransformPostion的扩展: 
     1.可以根据游戏屏幕分辨率播放从A到B动画 eg. UI从屏幕外左边移动到屏幕中英 
     2.TweenTransformExEditor.cs中对编辑器进行定制，实现了功能自动化处理，节省开发时间 
      
     Added by Teng. 
 **/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TweenTransform))]
public class TweenTransformEx : MonoBehaviour
{
    public GameObject FromAnchor;
    public GameObject ToAnchor;
}