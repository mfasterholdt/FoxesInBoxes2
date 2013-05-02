using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public GameObject visuals;
	
	public enum Type{fox, box, environemnt, background};
	public Type type;

	public int depth;
	public Level level;
		
	public GridPos pos;
	public Vector3 moveTarget;
	
	public void Setup(Vector3 p, int x, int y, Level level)
	{
		pos = new GridPos(x, y);		
		this.level = level;
		
		p.z = depth;
		this.transform.position = p;
	}
	
	public void MoveTile(int xOffset, int yOffset)
	{
		level.tiles[pos.x, pos.y] = null;
		
		pos.x += xOffset;
		pos.y += yOffset;
		
		moveTarget = this.transform.position;
		
		moveTarget.x -= Level.tileSize * xOffset;
		moveTarget.x = Mathf.Round(moveTarget.x);
		
		moveTarget.y -= Level.tileSize * yOffset;
		moveTarget.y = Mathf.Round(moveTarget.y);
		
		level.tiles[pos.x, pos.y] = this;
		
	}
	
	virtual public void Drop(GridPos drop)
	{
	}
	
	virtual public void Drag()
	{
	}
}
