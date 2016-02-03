using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;

public class Test : EditorWindow
{
    public int a;
    [MenuItem("Model/Make")]
    static void DoCreateAnimationAssets()
    {
        Test window = (Test)GetWindow(typeof(Test));

        window.Show();
        ////创建animationController文件，保存在Assets路径下
        //AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath("Assets/animation.controller");
        ////得到它的Layer， 默认layer为base 你可以去拓展
        //AnimatorControllerLayer layer = animatorController.layers[0];
        ////把动画文件保存在我们创建的AnimationController中
        //AddStateTransition("Assets/Enemy/Flower Monster/Animations/FlowerMonster@idle.FBX", layer);
        //AddStateTransition("Assets/Enemy/Flower Monster/Animations/FlowerMonster@attack01.FBX", layer);
        //AddStateTransition("Assets/Enemy/Flower Monster/Animations/FlowerMonster@walk.FBX", layer);
    }
    void OnGUI()
    {
        
        if (GUI.Button(new Rect(10, 10, 200, 20), "Create"))
        {
            Debug.Log("hello world");

        }
       
    }
    //private static void AddStateTransition(string path, AnimatorControllerLayer layer)
    //{
    //    AnimatorStateMachine sm = layer.stateMachine;
    //    //根据动画文件读取它的AnimationClip对象
    //    AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;

    //    //取出动画名子 添加到state里面
    //    AnimatorState state = sm.AddState(newClip.name);

    //    //把state添加在layer里面
    //    AnimatorStateTransition trans = sm.AddAnyStateTransition(state);

    //}
}