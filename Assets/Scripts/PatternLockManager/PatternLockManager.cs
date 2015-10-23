using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PatternLockManager : MonoSingleton<PatternLockManager> {

	[HideInInspector]public List<GameObject> pointList;
	public int patternRows;
	private int pointNumber;
	public GameObject pointPrefab;
	public GameObject pointParent;

	[HideInInspector]public List<int> patternNumbers;
	[HideInInspector]public int lineNumber;
	[HideInInspector]public Vector3 startLinePoint;
	[HideInInspector]public List<GameObject> connectingLines;
	public int lineWidth;
	public GameObject dotPrefab;
	public GameObject lineParent;
	private RectTransform presentRectTransform;
	
	void Start () 
	{
		patternNumbers = new List<int>();
		connectingLines = new List<GameObject>();
		pointList = new List<GameObject>();

		CreatePatternGrid();
	}

	public void CreatePatternGrid()
	{
		if (pointPrefab == null || pointParent == null) {
			//TODO Destroy the game, ahaha
			Debug.LogError("Setup not completed, critical error!");
		}

		//TODO PointLayout needs to be rewritten if this is to be used again
		GridLayoutGroup pointLayout = pointParent.GetComponent<GridLayoutGroup>();
		pointLayout.spacing = new Vector2(400 / patternRows - (5 * patternRows + 4), 400 / patternRows - (5 * patternRows + 4));

		for (int x = 0; x < patternRows; x++) {
			for (int y = 0; y < patternRows; y++) {
				GameObject pointObject = Instantiate(pointPrefab, pointPrefab.transform.position, pointPrefab.transform.rotation) as GameObject;
				pointObject.transform.SetParent(pointParent.transform);
				pointList.Add(pointObject);
				pointNumber = pointList.Count - 1;
				pointObject.name = string.Format("Point" + pointNumber.ToString()); 
				pointObject.GetComponent<PatternPoint>().pointId = pointNumber;
				pointObject.GetComponent<PatternPoint>().pointPosition = new Vector2(x, y);
			}
		}
	}

	public bool IsUnsignedPointExistBetweenLine(Vector2 startPoint, Vector2 endPoint)
	{
		//TODO Someone help me refactor this shit
		int largerX = 0;
		int smallerX = 0;
		int largerY = 0;
		int smallerY = 0;
		if (startPoint.x != endPoint.x && startPoint.y != endPoint.y) {
			if (startPoint.x != endPoint.x) {
				if (startPoint.x > endPoint.x) {
					largerX = (int)startPoint.x;
					smallerX = (int)endPoint.x;
				}
				else {
					largerX = (int)endPoint.x;
					smallerX = (int)startPoint.x;
				}
			}
			else if (startPoint.y != startPoint.y) {
				if (startPoint.y > endPoint.y) {
					largerY = (int)startPoint.y;
					smallerY = (int)endPoint.y;
				}
				else {
					largerY = (int)endPoint.y;
					smallerY = (int)startPoint.y;
				}
			}
		}

		return false;
	}

	public List<GameObject> GetObjectsBetweenPoints(Vector2 startPoint, Vector3 endPoint) 
	{
		return null;
	}

	public GameObject GetPointByCoordination(Vector2 coordinate)
	{
		if (pointList == null || pointList.Count == 0) {
			return null;
		}
		foreach(GameObject pointObject in pointList) {
			if (pointObject.GetComponent<PatternPoint>().pointPosition == coordinate) {
				return pointObject;
			}
		}
		return null;
	}

	public void StartDrag(int startPoint)
	{
		patternNumbers.Add(startPoint);
	}

	public void EnterPointDrag(int enterPoint)
	{
		if(patternNumbers.Exists(element => element == enterPoint) == false) {
			patternNumbers.Add(enterPoint);
		}
	}

	public void ComparePoints(List<int> comparePoints, List<int> toComparePoints)
	{

	}

	public void ClearPoints()
	{
		patternNumbers.Clear();
		foreach(GameObject pointtObject in pointList) {
			pointtObject.GetComponent<PatternPoint>().isSelected = false;
		}
	}

	public void DrawAtPoint(Vector3 startPoint)
	{
		GameObject startLine = Instantiate(dotPrefab, startPoint, dotPrefab.transform.rotation) as GameObject;
		startLine.transform.SetParent(lineParent.transform);
		connectingLines.Add(startLine);
		lineNumber = connectingLines.Count;
		startLine.name = string.Format("Line" + lineNumber.ToString());
		startLinePoint = startPoint;
		presentRectTransform = startLine.GetComponent<RectTransform>();
	}

	public void DrawNextLine()
	{
		GameObject startLine = Instantiate(dotPrefab, startLinePoint, dotPrefab.transform.rotation) as GameObject;
		startLine.transform.SetParent(lineParent.transform);
		connectingLines.Add(startLine);
		lineNumber = connectingLines.Count;
		startLine.name = string.Format("Line" + lineNumber.ToString());
		presentRectTransform = startLine.GetComponent<RectTransform>();
	}

	public void SetStartPoint(Vector3 startPoint)
	{
		startLinePoint = startPoint;
	}

	public void DrawLine(Vector3 pointB)
	{
		Vector3 differenceVector = pointB - startLinePoint;
		
		presentRectTransform.sizeDelta = new Vector2( differenceVector.magnitude, lineWidth);
		presentRectTransform.pivot = new Vector2(0, 0.5f);
		presentRectTransform.position = startLinePoint;
		float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
		presentRectTransform.rotation = Quaternion.Euler(0,0, angle);
	}

	public void ClearLines()
	{
		foreach(GameObject lineObject in connectingLines) {
			Destroy(lineObject);
		}
		foreach(GameObject pointObject in pointList) {
			pointObject.GetComponent<Image>().color = Color.white;
		}
		connectingLines.Clear();
	}

	public void MarkPoint(int id) {
		if (pointList != null || pointList.Count != 0) {
			pointList[id].GetComponent<Image>().color = new Color32(64, 132, 242, 100);
		}
	}
}
