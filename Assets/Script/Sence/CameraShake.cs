using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isShake)
        {
            ShakeCamera();
        }
    }
    ///是否震动相机
    public bool isShake = false;

    /// <summary>
    /// 震动间隔
    /// </summary>
    public float ShakeTime = 1.0f;

    /// <summary>
    /// 震动增量
    /// </summary>
    public float ShakeDelta = 0.1f;

    /// <summary>
    /// 记录当前帧时间
    /// </summary>
    private float frameTime = 0.0f;

    /// <summary>
    /// FPS 
    /// </summary>
    private float fps = 20.0f;

    /// <summary>
    /// 从开始震动计时
    /// </summary>
    private float time = 0.0f;

    private float sharkup = 0.0f;

    /// <summary>
    /// 相机震动的方法
    /// </summary>
    private void ShakeCamera()
    {

        time += Time.deltaTime;

        if (ShakeTime > 0)
        {
            if (time < 5f)
            {
                if (sharkup < 1f)
                {
                    sharkup += 0.01f;
                }
            }
            else
            {
                if (sharkup - 0.01f > 0)
                {
                    sharkup -= 0.01f;
                }
                else
                {
                    GetComponent<Camera>().rect = new Rect(0, 0, 1.0f, 1.0f);
                    time = 0.0f;
                    isShake = false;
                }
            }

            ShakeTime -= Time.deltaTime;
            if (ShakeTime <= 0)
            {
                //重置相机
                GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                //重置震动间隔
                ShakeTime = 1.0f;
            }
            else
            {
                //为了使相机震动平稳而均匀设计一个帧时间
                frameTime += Time.deltaTime;
                //只有帧时间大于FPS时震动
                if (frameTime >= 1 / fps)
                {
                    //重置帧时间
                    frameTime = 0;
                    //随机调整相机矩形
                    GetComponent<Camera>().rect =
                        new Rect(0.1f * (-ShakeDelta + 2 * Random.value) * sharkup,
                        0.1f * (-ShakeDelta + 2 * Random.value) * sharkup
                        , 1.0f, 1.0f);
                }
            }
        }
    }

}

