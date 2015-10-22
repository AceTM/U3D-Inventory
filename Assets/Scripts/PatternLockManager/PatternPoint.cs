using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

public class PatternPoint : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler {
	public int pointId;
	public Vector2 pointPosition;
	public bool isSelected;

	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log ("Begining Drag object " + pointId);
		PatternLockManager.Instance.StartDrag(pointId);
		PatternLockManager.Instance.DrawAtPoint(transform.position);
		isSelected = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		PatternLockManager.Instance.DrawLine(new Vector3(eventData.position.x, eventData.position.y, 0));
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		foreach(int number in PatternLockManager.Instance.patternNumbers) {
			Debug.Log ("End point " + number);
		}

		PatternLockManager.Instance.ClearPoints();
		PatternLockManager.Instance.ClearLines();
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{

	}
		
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null && isSelected == false) {
			Debug.Log ("Oh crap, point object " + eventData.pointerDrag.GetComponent<PatternPoint>().pointId + " entered on object " + pointId);
			Debug.Log ("And position of this point is " + transform.position);
			Vector3 endPoint = new Vector3(transform.position.x, transform.position.y, 0);
			PatternLockManager.Instance.EnterPointDrag(pointId);
			PatternLockManager.Instance.DrawLine(endPoint);
			PatternLockManager.Instance.SetStartPoint(endPoint);
			PatternLockManager.Instance.DrawNextLine();
			isSelected = true;
		}
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{

	}

	public void OnDrop(PointerEventData eventData)
	{

	}
}
