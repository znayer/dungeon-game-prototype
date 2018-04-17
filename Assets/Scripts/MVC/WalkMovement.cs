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

	public overried IEnumerator Traverse (Tile tile){
		
	}
}
