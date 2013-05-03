using UnityEngine;
using System.Collections;

public class DynamicTile : Tile 
{
	public delegate void State();
	public State state;
	
	public Vector3 moveTarget;

	public float bounce = -0.25f;
	
	protected Vector3 velocity;
	protected Vector3 acceleration;
	
	override public void Setup(Vector3 p, int x, int y, Level level)
	{
		base.Setup(p, x, y, level);
		
		SetIdleState();
	}
	
	//States
	virtual public void SetDraggingState() { state = new State(DraggingState); }
	virtual public void DraggingState() {	}
	
	virtual public void SetIdleState() { state = new State(IdleState); }
	virtual public void IdleState() {	}
	
	virtual public void SetFallState() { state = new State(FallState); }
	virtual public void FallState() {	}
	
	public void TileUpdate () 
	{
		if(state != null)
		{
			state();
		}
	}
	
	public void MoveTile(int xOffset, int yOffset)
	{
		level.tiles[pos.x, pos.y] = null;
		
		pos.x += xOffset;
		pos.y += yOffset;
		
		moveTarget = this.transform.position;
		
		moveTarget.x += Level.tileSize * xOffset;
		moveTarget.x = Mathf.Round(moveTarget.x);
		
		moveTarget.y -= Level.tileSize * yOffset;
		moveTarget.y = Mathf.Round(moveTarget.y);
		
		level.tiles[pos.x, pos.y] = this;
	}
	
	public void StartFall()
	{
		MoveTile(0, 1);		
		
		SetFallState();
	}
	
	//Picking up
	virtual public void Drag()
	{
		level.dragging = this;
		level.tiles[pos.x, pos.y] = null;						
		
		SetDraggingState();
	}
	
	virtual public void Drop(GridPos drop)
	{
		pos = drop;
		
		velocity = Vector3.zero;
		level.dragging = null;
		
		level.tiles[pos.x, pos.y] = this;				
				
		SetIdleState();
	}
}
