var delay:float=0;
private var delayTime:float=0;
var fadeInTime:float=0.1;
var stayTime:float=1;
var fadeOutTime:float=0.7;
var myColor:Color=Color(0.5, 0.5, 0.5, 0.5);
var maxAlpha:float;

private var timeElapsed:float=0;
private var timeElapsedLast:float=0;
private var percent:float;


function Start ()
{
maxAlpha=myColor.a;
GetComponent.<Renderer>().material.SetColor ("_TintColor", Color(myColor.r, myColor.g, myColor.b,0));
if(fadeInTime<0.01) fadeInTime=0.01; //hack to avoid division with zero
percent=(timeElapsed/fadeInTime) * maxAlpha;

}


function Update () {
GetComponent.<Renderer>().material.SetColor ("_TintColor", Color(myColor.r, myColor.g, myColor.b,0));
delayTime+=Time.deltaTime;
	if (delayTime>delay)
		{
		timeElapsed+=Time.deltaTime;
		
		
			if(timeElapsed<=fadeInTime) //fade in
				{
				percent=(timeElapsed/fadeInTime) * maxAlpha;
				GetComponent.<Renderer>().material.SetColor ("_TintColor", Color(myColor.r, myColor.g, myColor.b, percent));
				}
			
			if((timeElapsed>fadeInTime)&&(timeElapsed<fadeInTime+stayTime)) //set the normal color
				{
				GetComponent.<Renderer>().material.SetColor ("_TintColor", Color(myColor.r, myColor.g, myColor.b, maxAlpha));
				}
		
			if(timeElapsed>=fadeInTime+stayTime&&timeElapsed<=fadeInTime+stayTime+fadeOutTime) //fade out
				{
				timeElapsedLast+=Time.deltaTime;
				percent=maxAlpha-((timeElapsedLast/fadeOutTime)*maxAlpha);
				if (percent<0) percent=0;
				GetComponent.<Renderer>().material.SetColor ("_TintColor", Color(myColor.r, myColor.g, myColor.b, percent));
				}
		
		}

//if (delayTime>=delay+fadeInTime+stayTime+fadeOutTime) renderer.material.SetColor ("_TintColor", Color(myColor.r, myColor.g, myColor.b,0));

}

