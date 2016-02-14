private var startTime;
private var restSeconds : int;
private var roundedRestSeconds : int;
private var displaySeconds : int;
private var displayMinutes : int;


var countDownSeconds : int;


function Awake() {
    startTime = Time.time;
}


function OnGUI () {
    //make sure that your time is based on when this script was first called
    //instead of when your game started
    var guiTime = Time.time - startTime;
 	restSeconds = countDownSeconds - (guiTime);
 	//display messages or whatever here --&gt;do stuff based on your timer
 	if (restSeconds == 60) {
     	print ("One Minute Left");
 	}
 	if (restSeconds == 0) {
     	print ("Time is Over");
     	//do stuff here
 	}
 	//display the timer
 	roundedRestSeconds = Mathf.CeilToInt(restSeconds);
 	displaySeconds = roundedRestSeconds % 60;
 	displayMinutes = roundedRestSeconds / 60; 
 	text = String.Format ("{0:00}:{1:00}", displayMinutes, displaySeconds); 
 	GUI.Label (Rect (400, 25, 100, 30), text);
 }