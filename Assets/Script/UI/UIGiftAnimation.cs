using UnityEngine;
using System.Collections;

public class UIGiftAnimation : MonoBehaviour
{
    GameObject bagButton;

    public Transform target;
    // Use this for initialization
    void Start()
    {
        this.transform.position= this.transform.position;

        bagButton = GameObject.FindGameObjectWithTag(Tags.UIRoot).transform.Find("ToolBar/Menu/BagButton").gameObject;
        Camera gameCamera = NGUITools.FindCameraForLayer(target.gameObject.layer);
        Camera uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);

        transform.position = uiCamera.ViewportToWorldPoint(gameCamera.WorldToViewportPoint(target.position));

        transform.position = new Vector3(transform.position.x, transform.position.y, bagButton.transform.position.z);

        iTween.MoveTo(this.gameObject, iTween.Hash("time", 1f, "easeType", iTween.EaseType.easeInOutCubic, "position", bagButton.transform.position-new Vector3(0,0,0.1f), "oncomplete", "ScaleToBig"));
        iTween.ScaleTo(this.gameObject, iTween.Hash("time", 0.2f, "scale", new Vector3(100, 100, 100)));
    }

    void ScaleToBig()
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("time", 0.2f, "scale", new Vector3(150, 150, 150), "easeType", iTween.EaseType.easeInOutQuad,"oncomplete", "ScaleToSmall"));

    }
    void ScaleToSmall()
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("time", 0.3f, "scale", new Vector3(1, 1, 1), "easetype", iTween.EaseType.easeInQuart, "oncomplete", "Diestroy"));
        this.GetComponent<AudioSource>().Play();
    }
    void Diestroy()
    {
        StartCoroutine(delayDistroy());
    }

    IEnumerator delayDistroy()
    {
        yield return new WaitForSeconds(2f);
        UIGiftManager._instans.DestoryGift(this.gameObject);

    }

}
