using UnityEngine;
using System.Collections;

public class UICoinAnimation : MonoBehaviour
{

    GameObject bagButton;
    public int sizeScale;
    public Transform target;
    // Use this for initialization
    void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        float randomX = Random.value - 0.5f;
        Random.seed = System.DateTime.Now.Millisecond + 1;
        float randomY = Random.value - 0.5f;

        Animation anim = this.GetComponentInChildren<Animation>();
        foreach (AnimationState state in anim)
        {
            Random.seed = System.DateTime.Now.Millisecond + 2;
            state.speed = Random.value * 4 - 2;
            Random.seed = System.DateTime.Now.Millisecond + 3;
            state.time = Random.value;

        }


        this.transform.position = this.transform.position;

        bagButton = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("ToolBar/Menu/BagButton").gameObject;
        Camera gameCamera = NGUITools.FindCameraForLayer(target.gameObject.layer);
        Camera uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);

        transform.position = uiCamera.ViewportToWorldPoint(gameCamera.WorldToViewportPoint(target.position));

        iTween.ScaleTo(this.gameObject, iTween.Hash("time", 0.3f, "scale", new Vector3(sizeScale, sizeScale, sizeScale)));


        iTween.MoveTo(this.gameObject, iTween.Hash("time", 0.3f, "easeType", iTween.EaseType.easeInOutCubic, "position", new Vector3(transform.position.x, transform.position.y, bagButton.transform.position.z) + new Vector3(randomX * 0.2f, randomY * 0.2f), "oncomplete", "MoveToBagIcon"));

    }

    void MoveToBagIcon()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("time", 1f + Random.value * 0.2, "easeType", iTween.EaseType.easeInOutCubic, "position", bagButton.transform.position - new Vector3(0, 0, 0.1f), "oncomplete", "ScaleToSmall"));

    }
    void ScaleToSmall()
    {
        this.GetComponent<AudioSource>().Play();
        iTween.ScaleTo(this.gameObject, iTween.Hash("time", 0.1f, "scale", new Vector3(1, 1, 1), "easetype", iTween.EaseType.easeInQuart, "oncomplete", "Diestroy"));

    }
    void Diestroy()
    {
        StartCoroutine(delayDistroy());
    }

    IEnumerator delayDistroy()
    {
        yield return new WaitForSeconds(2f);
        UICoinManager._instance.DestoryCoin(this.gameObject);

    }
}
