using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour {

	public int range;
	public int jumpHeight;
	protected Unit unit;
	protected Transform jumper;

	protected virtual void Awake(){
		unit = GetComponent<Unit>();
		jumper = transform.FindChild("Jumper");
	}

	protected virtual void Filter (List<Tile> tiles){
		for (int i = tiles.Count - 1; i >= 0; i--){
			if (tiles[i].content != null){
				tiles.RemoveAt(i);
			}
		}
	}

	protected virtual IEnumerator Turn (Directions dir){
		TransformLocalEulerTweener t = 
		(TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

		// Stuff for going between North and West so it's not retarded 
		if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f)){
    		t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
		}
  		else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0)){
    		t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
  		}

  		unit.dir = dir;
  
  		while (t != null){
    		yield return null;
  		}

	}

	public virtual List<Tile> GetTilesInRanger(Board board){
		List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
		Filter(retValue);
		return retValue;
	}

	protected virtual bool ExpandSearch(Tile from, Tile to){
		return (from.distance + 1) <= range;
	}

	// Animation stuff while walking, leaving for subclasses
	public abstract IEnumerator Traverse (Tile tile);

}
