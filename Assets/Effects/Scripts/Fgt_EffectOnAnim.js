#pragma strict

var onThisAnim:AnimationClip;
var here:GameObject;
var delay:float=0.2;  // this is the maximum value of delay
private var actualDelay:float;  //this is where we keep the actual delay, reduced by time
private var animName:String;
private var time:float;
private var changeCooldown:float=0.2;
private var played:boolean=false;

var writeHere:GUIText;

var database:GameObject[];
private var currentEffectNo:int;

function Start () {

actualDelay=delay;
currentEffectNo=database.length-1;
if (writeHere) writeHere.text=currentEffectNo.ToString()+" "+database[currentEffectNo].name;


}

function Update () {
if (changeCooldown>0) changeCooldown-=Time.deltaTime;

animName=onThisAnim.name;


if (GetComponent.<Animation>().IsPlaying(animName) && played == false)  // if the animation is running, and we didn't played the anim yet
	{
	actualDelay-=Time.deltaTime;
	if (actualDelay <= 0)  // delay was done, time to play the effect
		{
		actualDelay=delay; // resetting the delay to its default value
		time = 0;			//technical value, to prevent re-playing the effect until the end of the anim
		played = true;		//prevent to play it multiple times
		var effect:GameObject = Instantiate(database[currentEffectNo], here.transform.position, here.transform.rotation); //creating the effect
		effect.transform.parent = here.transform; // transforming to its target
		}
	
	}


if (time<onThisAnim.length)  // we reset the time when needed
	{
	time+=Time.deltaTime;
	}
	else 
	{
	played=false;
	time=0;
	}


if (Input.GetKeyDown(KeyCode.UpArrow) && changeCooldown<=0)
{
changeCooldown+=0.25;
currentEffectNo+=1;
if (currentEffectNo>=database.length) currentEffectNo=0;
if (writeHere) writeHere.text=currentEffectNo.ToString()+" "+database[currentEffectNo].name;
}


if (Input.GetKeyDown(KeyCode.DownArrow) && changeCooldown<=0)
{
changeCooldown+=0.25;
currentEffectNo-=1;
if (currentEffectNo<0) currentEffectNo=database.length-1;
if (writeHere) writeHere.text=currentEffectNo.ToString()+" "+database[currentEffectNo].name;
}



}