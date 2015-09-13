using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoomGenerator : MonoBehaviour {
	//tiles
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject cornerTile;
	public GameObject doorTile;
	public int RandomSizeUpperBound = 8;
	public int RandomSizeLowerBound = 2;
	public bool irregularShapeOrNot = false;

	private GameObject room;

	//For showing in the editor
	[SerializeField]
	private IrregularShape irregularShapeType;


	public List<GameObject> roomTiles = new List<GameObject>();
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		
		if (Input.GetMouseButtonDown (0)) {
			GenerateRoom ();
		}
		if (Input.GetMouseButtonDown (1)) {
			DestoryRoom();
		}
	
	}

	public void GenerateRoom(){

		//instantiate here
		Room newRoom = new Room (transform.position, RandomSize(),RandomSize());

		//test
		room = new GameObject ("Room"+transform.position);
		room.transform.position = transform.position;

		//add
		if (irregularShapeOrNot == true) {

			irregularShapeType = RandomShape ();
			newRoom = irreguralize (newRoom, irregularShapeType);

		} else {

			newRoom = calculateTilePosition(newRoom);

		}



		//Generate floor
		foreach (Tile tile in newRoom.floorTiles) {
			GameObject temp = (GameObject)Instantiate (floorTile, new Vector3 (tile.x, 0, tile.z), Quaternion.identity);
			temp.transform.parent = room.transform;
			roomTiles.Add (temp);

		}
		//Generate wall
		foreach (Tile tile in newRoom.wallTiles) {
			GameObject temp = (GameObject)Instantiate(wallTile, new Vector3(tile.x,0,tile.z), Quaternion.Euler(0,tile.rotateAngle,0));
			temp.transform.parent = room.transform;
			roomTiles.Add(temp);
		}
		//Generate corner
		foreach (Tile tile in newRoom.cornerTiles) {
			GameObject temp = (GameObject)Instantiate(cornerTile, new Vector3(tile.x,0,tile.z), Quaternion.Euler(0,tile.rotateAngle,0));
			temp.transform.parent = room.transform;
			roomTiles.Add(temp);
		}



		return;
	}

	public void DestoryRoom(){
		foreach (GameObject tile in roomTiles) {
			Destroy(tile);
		}
		Destroy (room);
		roomTiles.Clear ();
		return;
	}

	//Generate Random Length & Width
	public int RandomSize(){

		//return an even number
		return UnityEngine.Random.Range (RandomSizeLowerBound,RandomSizeUpperBound)*2;

	}

	//Generate Random shape
	private IrregularShape RandomShape(){

		return (IrregularShape)(UnityEngine.Random.Range (0, Enum.GetNames (typeof(IrregularShape)).Length));
		//return IrregularShape.Lshape;
	}
	public int RandomSign(){

		if (UnityEngine.Random.value < 0.5f) {
			return 1;
		} else {
			return -1;
		}
	}

	public struct Tile{
		//1 for floor, 2 for wall, 3 for corner, 4 for door, 5 for inward corner, 6 for door, 7 for window
		public int tileType;
		public float x;
		public float z;
		public int rotateAngle;

		public Tile(int type, float posx, float posz, int rotateAng){
			tileType = type;
			x = posx;
			z = posz;
			rotateAngle = rotateAng;

		}

	}


	public class Room{
		public Vector3 centerPoint;
		public int lengthWithoutBound;//x
		public int widthWithoutBound;//z
		public List<Tile> wallTiles;
		public List<Tile> doorTiles;
		public List<Tile> floorTiles;
		public List<Tile> cornerTiles;
		public List<Tile> inwardTiles;
		public static bool irregularShapeOrNot1 = false;


		public Room(){}
		public Room(Vector3 center, int length, int width){
			centerPoint = center;
			lengthWithoutBound = length;
			widthWithoutBound = width;
			floorTiles = new List<Tile>();
			wallTiles = new List<Tile>();
			doorTiles = new List<Tile>();
			cornerTiles = new List<Tile>();
			inwardTiles = new List<Tile>();

			//irregularShapeOrNot1 = true;

			/*if(irregularShapeOrNot1 == false){
				calculateTilePosition();
			}*/
		}

		//should be a method to calculate every single tile position from length/width
		public void calculateTilePosition(){


				//four points from center point to edges
				int halfLengthLeft = (int)(centerPoint.x - lengthWithoutBound / 2) + 1;
				int halfLengthRight = (int)(centerPoint.x + lengthWithoutBound / 2) - 1;
				int halfWidthBot = (int)(centerPoint.z - widthWithoutBound / 2) + 1;
				int halfWidthTop = (int)(centerPoint.z + widthWithoutBound / 2) - 1;

				//!Floor first
				floorTiles.Clear ();
				for (int x = halfLengthLeft; x <= halfLengthRight; x = x+2) {
					for (int z = halfWidthBot; z <= halfWidthTop; z = z+2) {
						Debug.Log (z);
						Debug.Log (x);
						floorTiles.Add (new Tile (1, x, z, 0));
					}
				}

				//!Wall second
				wallTiles.Clear ();
				for (int x = halfLengthLeft; x <= halfLengthRight; x = x+2) {
					wallTiles.Add (new Tile (2, x, halfWidthBot - 2, 180));
					wallTiles.Add (new Tile (2, x, halfWidthTop + 2, 0));
				}
				for (int z = halfWidthBot; z <= halfWidthTop; z = z+2) {
					wallTiles.Add (new Tile (2, halfLengthLeft - 2, z, 270));
					wallTiles.Add (new Tile (2, halfLengthRight + 2, z, 90));
				}


				//!Corner third
				cornerTiles.Clear ();
				cornerTiles.Add (new Tile (3, halfLengthLeft - 1.5f, halfWidthBot - 1.5f, 90));
				cornerTiles.Add (new Tile (3, halfLengthRight + 1.5f, halfWidthBot - 1.5f, 0));
				cornerTiles.Add (new Tile (3, halfLengthLeft - 1.5f, halfWidthTop + 1.5f, 180));
				cornerTiles.Add (new Tile (3, halfLengthRight + 1.5f, halfWidthTop + 1.5f, 270));


			}


	}

	public Room calculateTilePosition(Room room){
		
		
		//four points from center point to edges
		int halfLengthLeft = (int)(room.centerPoint.x - room.lengthWithoutBound / 2) + 1;
		int halfLengthRight = (int)(room.centerPoint.x + room.lengthWithoutBound / 2) - 1;
		int halfWidthBot = (int)(room.centerPoint.z - room.widthWithoutBound / 2) + 1;
		int halfWidthTop = (int)(room.centerPoint.z + room.widthWithoutBound / 2) - 1;
		
		//!Floor first
		room.floorTiles.Clear ();
		for (int x = halfLengthLeft; x <= halfLengthRight; x = x+2) {
			for (int z = halfWidthBot; z <= halfWidthTop; z = z+2) {
				Debug.Log (z);
				Debug.Log (x);
				room.floorTiles.Add (new Tile (1, x, z, 0));
			}
		}
		
		//!Wall second
		room.wallTiles.Clear ();
		for (int x = halfLengthLeft; x <= halfLengthRight; x = x+2) {
			room.wallTiles.Add (new Tile (2, x, halfWidthBot - 2, 180));
			room.wallTiles.Add (new Tile (2, x, halfWidthTop + 2, 0));
		}
		for (int z = halfWidthBot; z <= halfWidthTop; z = z+2) {
			room.wallTiles.Add (new Tile (2, halfLengthLeft - 2, z, 270));
			room.wallTiles.Add (new Tile (2, halfLengthRight + 2, z, 90));
		}
		

		//!Corner third
		room.cornerTiles.Clear ();
		room.cornerTiles.Add (new Tile (3, halfLengthLeft - 1.5f, halfWidthBot - 1.5f, 90));
		room.cornerTiles.Add (new Tile (3, halfLengthRight + 1.5f, halfWidthBot - 1.5f, 0));
		room.cornerTiles.Add (new Tile (3, halfLengthLeft - 1.5f, halfWidthTop + 1.5f, 180));
		room.cornerTiles.Add (new Tile (3, halfLengthRight + 1.5f, halfWidthTop + 1.5f, 270));


		return room;
		
	}



	//using enum, three types
	public enum IrregularShape{
		Tshape,Lshape/*,Cross,Eshape*/

	}

	
	//using method to build irregularRoom
	public Room irreguralize(Room room, IrregularShape type){

		if (type == IrregularShape.Tshape) {
			room = TshapeGenerator (room);
		} else if (type == IrregularShape.Lshape) {
			room =LshapeGenerator (room);
		}/* else if (type == IrregularShape.Cross) {
			room = CrossGenerator (room);
		} else if (type == IrregularShape.Eshape) {
			room = EshapeGenerator(room);
		}*/

		return room;
	}

	public Room LshapeGenerator (Room room){
		int length2 = RandomSize ();
		int width2 = RandomSize ();

		int randomSign1 = RandomSign ();
		int randomSign2 = RandomSign ();



		Vector3 centerPoint1 = new Vector3 (room.centerPoint.x, room.centerPoint.y, room.centerPoint.z + randomSign1*(room.widthWithoutBound/2 - width2/2));
		//Vector3 centerPointC = new Vector3 (room.centerPoint.x +randomSign2*(length2/2 - room.lengthWithoutBound/2), room.centerPoint.y, room.centerPoint.z);

		Vector3 centerPoint2 = new Vector3(room.centerPoint.x +randomSign2*(room.lengthWithoutBound/2+length2/2) ,room.centerPoint.y, room.centerPoint.z);

		//Vector3 inwardPoint = new Vector3 (centerPoint1.x+(-1)*randomSign1*(room.lengthWithoutBound/2), centerPoint1.y, centerPointC.z+(-1)*randomSign2*length2/2);

		//!Floor first, 1st part of the room
		int halfLengthLeft1 = (int)(centerPoint1.x -  room.lengthWithoutBound/ 2)+1;
		int halfLengthRight1 = (int)(centerPoint1.x + room.lengthWithoutBound / 2)-1;
		int halfWidthBot1 = (int)(centerPoint1.z - room.widthWithoutBound / 2) + 1;
		int halfWidthTop1 = (int)(centerPoint1.z + room.widthWithoutBound / 2) - 1;

		room.floorTiles.Clear ();
		for (int x = halfLengthLeft1; x <= halfLengthRight1 ; x = x+2) {
			for (int z = halfWidthBot1; z <= halfWidthTop1 ; z = z+2) {
				Debug.Log(z);
				Debug.Log(x);
				room.floorTiles.Add(new Tile(1,x,z,0));
			}
		}

		//!Floor first, 2nd part of the room
		int halfLengthLeft2 = (int)(centerPoint2.x -  length2 / 2) + 1;
		int halfLengthRight2 = (int)(centerPoint2.x + length2 / 2) - 1;
		int halfWidthBot2 = (int)(centerPoint2.z - width2 / 2) + 1;
		int halfWidthTop2 = (int)(centerPoint2.z + width2 / 2) - 1;

		for (int x = halfLengthLeft2; x <= halfLengthRight2 ; x = x+2) {
			for (int z = halfWidthBot2; z <= halfWidthTop2 ; z = z+2) {
				Debug.Log(z);
				Debug.Log(x);
				room.floorTiles.Add(new Tile(1,x,z,0));
			}
		}

		//! Wall second
		room = wallGenerator (room);

	
		return room;
	}
	public Room TshapeGenerator(Room room){

		//fixed length/width
		room.lengthWithoutBound = 12;
		room.widthWithoutBound = 4;

		int length2 = 4;
		int width2 = 8;
		int sign = RandomSign ();

		Vector3 centerPoint1 = room.centerPoint;
		Vector3 centerPoint2 = new Vector3 (room.centerPoint.x, room.centerPoint.y, room.centerPoint.z+ sign*(width2/2+room.widthWithoutBound/2));

		int halfLengthLeft1 = (int)(centerPoint1.x -  room.lengthWithoutBound/ 2)+1;
		int halfLengthRight1 = (int)(centerPoint1.x + room.lengthWithoutBound / 2)-1;
		int halfWidthBot1 = (int)(centerPoint1.z - room.widthWithoutBound / 2) + 1;
		int halfWidthTop1 = (int)(centerPoint1.z + room.widthWithoutBound / 2) - 1;
		
		room.floorTiles.Clear ();
		for (int x = halfLengthLeft1; x <= halfLengthRight1 ; x = x+2) {
			for (int z = halfWidthBot1; z <= halfWidthTop1 ; z = z+2) {
				Debug.Log(z);
				Debug.Log(x);
				room.floorTiles.Add(new Tile(1,x,z,0));
			}
		}
		
		//!Floor first, 2nd part of the room
		int halfLengthLeft2 = (int)(centerPoint2.x -  length2 / 2) + 1;
		int halfLengthRight2 = (int)(centerPoint2.x + length2 / 2) - 1;
		int halfWidthBot2 = (int)(centerPoint2.z - width2 / 2) + 1;
		int halfWidthTop2 = (int)(centerPoint2.z + width2 / 2) - 1;
		
		for (int x = halfLengthLeft2; x <= halfLengthRight2 ; x = x+2) {
			for (int z = halfWidthBot2; z <= halfWidthTop2 ; z = z+2) {
				Debug.Log(z);
				Debug.Log(x);
				room.floorTiles.Add(new Tile(1,x,z,0));
			}
		}
		
		//! Wall second
		room.wallTiles.Clear ();
		room = wallGenerator (room);

		return room;
	}

	public Room EshapeGenerator(Room room){
		
		return null;
	}
	public Room CrossGenerator(Room room){


		return null;
	}

	
	public Room wallGenerator (Room room){
		int count = 0;


		foreach (Tile floortile in room.floorTiles) {
			bool noWallOnTop = false;
			bool noWallOnBot = false;
			bool noWallOnLeft = false;
			bool noWallOnRight = false;

			if((room.floorTiles.Exists(next => next.z == (floortile.z+2) && next.x == floortile.x) == false ) && room.wallTiles.Exists(next => next.x == floortile.x && next.z == floortile.z+2) == false){
				room.wallTiles.Add(new Tile(2,floortile.x,floortile.z+2,0));
				count++;
				noWallOnTop = true;
			}
			if((room.floorTiles.Exists(next => next.z == (floortile.z-2) && next.x == floortile.x) == false )&& room.wallTiles.Exists(next => next.x == floortile.x && next.z == floortile.z-2) == false){
				room.wallTiles.Add(new Tile(2,floortile.x,floortile.z-2,180));
				noWallOnBot = true;
				count++;
			}
			if((room.floorTiles.Exists(next => next.x == (floortile.x+2) && next.z == floortile.z) == false )&& room.wallTiles.Exists(next => next.x == floortile.x+2 && next.z == floortile.z) == false){
				room.wallTiles.Add(new Tile(2,floortile.x+2,floortile.z,90));
				noWallOnRight = true;
				count++;
			}
			if((room.floorTiles.Exists(next => next.x == (floortile.x-2) && next.z == floortile.z) == false )&& room.wallTiles.Exists(next => next.x == floortile.x-2 && next.z == floortile.z) == false){
				room.wallTiles.Add(new Tile(2,floortile.x-2,floortile.z,270));
				noWallOnLeft = true;
				count++;
			}

			if(noWallOnTop && noWallOnLeft){
				room.cornerTiles.Add(new Tile(3,floortile.x-1.5f,floortile.z+1.5f,180));
			}
			else if(noWallOnTop && noWallOnRight){
				room.cornerTiles.Add(new Tile(3,floortile.x+1.5f,floortile.z+1.5f,270));

			}
			else if(noWallOnBot && noWallOnLeft){
				room.cornerTiles.Add(new Tile(3,floortile.x-1.5f,floortile.z-1.5f,90));

			}
			else if(noWallOnBot && noWallOnRight){
				room.cornerTiles.Add(new Tile(3,floortile.x+1.5f,floortile.z-1.5f,0));
			}

		}

		Debug.LogError(room.centerPoint + "is" +count);
		return room;
	}

}
