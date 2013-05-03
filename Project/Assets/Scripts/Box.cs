using UnityEngine;
using System.Collections;

public class Box : DynamicTile 
{
	
	override public void IdleState()
	{
		//Ground check		
		Tile t = level.GetTile(pos.x, pos.y + 1);
			
		if(t == null)
		{
			StartFall();
		}
		else if(t.type == Type.fox)
		{
			//***Fox check
			StartFall();
		}
	}
	
	override public void FallState()
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
			Tile t = level.GetTile(pos.x, pos.y + 1);
			
			if(t != null && t.type != Tile.Type.fox)
			{
				this.transform.position = moveTarget;
				
				velocity *= bounce;				
			}
			else
			{
				SetIdleState();
			}
		}
	}
}