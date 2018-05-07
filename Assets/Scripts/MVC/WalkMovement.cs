using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : Movement {

	protected override bool ExpandSearch(Tile from, Tile to){

		// Check height and skip if it's too high
		if ((Mathf.Abs(from.height - to.height) > jumpHeight)){
			return false;
		}

		// Also skip tiles occupied by enemy or objects
		if (to.content != null){
			return false;
		}

		// Add anything else that could stop movement

		// shoot out result now
		return base.ExpandSearch(from, to);
	}

	public override IEnumerator Traverse (Tile tile){
		unit.Place(tile);

		// Build list of waypoints
		List<Tile> targets = new List<Tile>();

		while(tile != null){
			targets.Insert(0, tile);
			tile = tile.prev;
		}

		// Move  to each waypoint
		for (int i = 1; i < targets.Count; i++){
			Tile from = targets[i-1];
			Tile to = targets[i];

			Directions dir = from.GetDirection(to);

			if(unit.dir != dir){
				yield return StartCoroutine(Turn(dir));
			}

			if (from.height == to.height){
				yield return StartCoroutine(Walk(to));
			}
			else{
				yield return StartCoroutine(Jump(to));
			}
		}

		yield return null;
	}

	// Change this later probably
	IEnumerator Walk(Tile target){
		Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);

		Tweener t2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0), 
			tweener.easingControl.duration / 2f, 
			EasingEquations.EaseOutQuad);

		while (tweener != null){
			yield return null;
		}
	}

	// Change this later probably
	IEnumerator Jump (Tile to){
		Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);

  		Tweener t2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0), 
  			tweener.easingControl.duration / 2f, 
  			EasingEquations.EaseOutQuad);

  		t2.easingControl.loopCount = 1;
  		t2.easingControl.loopType = EasingControl.LoopType.PingPong;

  		while (tweener != null){
    		yield return null;
  		}
	}
}
