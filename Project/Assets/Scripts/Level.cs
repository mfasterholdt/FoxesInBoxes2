using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Level : MonoBehaviour 
{	
	public GameObject[] tileList;
	public string fileName = "level01.txt";

	public Tile[,] tiles;
	Fox[] foxes;
	Box[] boxes;
	
	string[,] levelData;	
	public static int levelWidth;
	public static int levelHeight;
	
	float widthOffset;
	float heightOffset;
	
	public static int tileSize = 1;
	public static float gravity = -0.1f;
	
	public Tile dragging;
	public GridPos lastDragPos;
	
	void Start ()
	{		
		LoadLevel(fileName);		
		CreateLevel();
	}
	
	void LoadLevel(string name)
	{
		//Read file
	    StreamReader sr = new StreamReader(Application.dataPath + "/Levels/" + name);
	    string fileContents = sr.ReadToEnd();
	    sr.Close();
	 
		//Parse file
	    string[] lines = fileContents.Split("\n"[0]);
		levelHeight = lines.Length;
		levelWidth = (lines[0] as string).Length;
		
		levelData = new string[levelWidth,levelHeight];
		tiles = new Tile[levelWidth, levelHeight];
		
		for(int y = 0; y < levelHeight; y++)
		{
			string line = lines[y];
			
			for(int x = 0; x < levelWidth; x++) 
			{
				levelData[x,y] = line.Substring(x, 1);
			}
	    }
	}
	
	void CreateLevel()
	{
		widthOffset =  (tileSize - levelWidth) / 2f;
		heightOffset = (levelHeight - tileSize) / 2f;
		
		Camera.mainCamera.orthographicSize = levelHeight / 1.9f;
		
		for(int y = 0; y < levelHeight; y++)
		{	
			for(int x = 0; x < levelWidth; x++) 
			{
				int index = int.Parse(levelData[x,y]);
				
				CreateTile(index, x, y);
			}
	    }
		
		boxes = FindObjectsOfType(typeof(Box)) as Box[];
		foxes = FindObjectsOfType(typeof(Fox)) as Fox[];
	}
	
	void CreateTile(int index, int x, int y)
	{
		//Create tile
		Vector3 pos = new Vector3(x * tileSize, y * -tileSize, 0);
		pos.x += widthOffset;
		pos.y += heightOffset;
		
		GameObject newObj = Instantiate(tileList[index]) as GameObject;
		Tile newTile = newObj.GetComponent<Tile>();
		
		newObj.transform.parent = this.transform;
		newTile.Setup(pos, x, y, this);	
		
		
		if(newTile.type != Tile.Type.background)
		{
			//Create air behind boxes and foxes
			if(newTile.type == Tile.Type.box || newTile.type == Tile.Type.fox)
			{
				CreateTile(0, x, y);	
			}
		
			//Add to level
			tiles[x, y] = newTile;
		}
	}
	
	void Update()
	{
		for(int i = 0; i < boxes.Length; i++)
		{
			Box b = boxes[i];
			b.TileUpdate();
		}
		
		HandleMouse();
	}
	
	public bool GetTile(int x, int y)
	{
		//Check to fall
		Tile t = tiles[x, y];
		
		if(t == null)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	
	void HandleMouse()
	{	
		bool mouseDown = Input.GetMouseButtonDown(0);
		
		if(dragging != null)
		{
			//Move Object
			Vector3 pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);			
			pos.x = Mathf.Round(pos.x / tileSize);
			pos.y = Mathf.Round(pos.y / tileSize);
			pos.z = 0; 	
			
			GridPos currentPos = new GridPos( (int) pos.x + (levelWidth / 2), ((int)pos.y - (levelHeight / 2)) * -1);
			Tile tile = tiles[currentPos.x, currentPos.y];	
			
			//currentPos.x
			if(tile == null || tile.type == Tile.Type.background)
			{
				pos.x = currentPos.x - (levelWidth / 2);
				pos.y = (currentPos.y - (levelHeight / 2)) * -1;
				
				lastDragPos = currentPos;
				dragging.gameObject.transform.position = pos;
			}			

			//Drop object
			bool mouseUp = Input.GetMouseButtonUp(0);						
			
			if(mouseUp)	
			{
				dragging.Drop(lastDragPos);
			}
		}
		else
		{
			//Pick up object
			if(mouseDown)
			{	
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
				
				if (Physics.Raycast (ray, out hit))
				{  
					Tile tile = hit.collider.transform.parent.GetComponent<Tile>();
					
					if (tile) tile.Drag();
				}
			}
		}
	}
}
