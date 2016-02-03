var Gradient : Gradient;
var timeMultiplier:float=1;

private var curColor:Color;
private var time:float=0;



function Start ()
{

GetComponent.<Renderer>().material.SetColor ("_TintColor", Color(0, 0, 0, 0));

}


function Update () {
time+=Time.deltaTime*timeMultiplier;
curColor=Gradient.Evaluate(time);


GetComponent.<Renderer>().material.SetColor ("_TintColor", curColor);
}

