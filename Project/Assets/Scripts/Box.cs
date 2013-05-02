using UnityEngine;
using System.Collections;

public class Box : Tile 
{
	Vector3 velocity;
	Vector3 acceleration;
	
	delegate void State();
	State state;
	
	public float bounce = -0.25f;
	
	void Start () 
	{
		
	}
	
	public void TileUpdate () 
	{
		if(state != null)
		{
			state();
		}
		else 
		{
			if(!level.GetTile(pos.x, pos.y + 1))
			{
				StartFall();
			}
		}
	}
	
	void DraggingState()
	{
		
	}
	
	void FallState()
	{
		Vector3 p = this.transform.position;
		velocity.y += Level.gravity * Time.deltaTime;  
		
		if(p.y + velocity.y > moveTarget.y)
		{
			p += velocity;		
			this.transform.position = p;	
		}
		else
		{
			if(level.GetTile(pos.x, pos.y + 1))
			{
				this.transform.position = moveTarget;
				
				velocity *= bounce;				
			}
			else
			{
				StartFall();
			}
		}
	}
	
	void StartFall()
	{
		MoveTile(0, 1);		
		state = new State(FallState);
	}
	
	override public void Drag()
	{
		level.dragging = this;
		level.tiles[pos.x, pos.y] = null;						
		
		state = new State(DraggingState);
	}
	
	override public void Drop(GridPos drop)
	{
		pos = drop;
		
		velocity = Vector3.zero;
		level.dragging = null;
		
		level.tiles[pos.x, pos.y] = this;				
				
		state = null;
	}
}