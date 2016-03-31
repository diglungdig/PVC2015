@script ExecuteInEditMode()
// Scroll main texture based on time
import UnityEngine.UI;

var scrollSpeed = 1.0;
var MainoffsetX = 0.0;
var MainoffsetY = 0.0;

var UseCustomTex = false;
var CustomTexName = "";

function Update () 
{
    var offset = Time.time * scrollSpeed;
    if(UseCustomTex){
	GetComponent.<Image>().material.SetTextureOffset (CustomTexName, Vector2(MainoffsetX*offset, MainoffsetY*offset));
    }
    else{
        GetComponent.<Image>().material.SetTextureOffset ("_MainTex", Vector2(MainoffsetX*offset, MainoffsetY*offset));
    
    }
}