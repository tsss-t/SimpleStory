using UnityEngine;
using System.Collections;
public enum TextType
{
    text,textArea,picture
}
public enum TextEffect
{
    foi,pictureIn,pictureOut
}
public class TalkText
{
    public TextType textType;
    public TextEffect textEffect;
    public string textInfo;

}
public class TextManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
