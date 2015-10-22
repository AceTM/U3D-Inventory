using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternLockManager : MonoSingleton<PatternLockManager> {
	public GameObject[] patternPoints;

	public List<int> patternNumbers;

	public int lineNumber;
	public int lineWidth;
	public Vector3 startLinePoint;
	public GameObject dotPrefab;
	public GameObject lineParent;
	private RectTransform presentRectTransform;
	public List<GameObject> connectingLines;
		
	public int patternLines;

	void Start () 
	{
		if (patternPoints.Length > 9) {
			Destroy(this.gameObject);
			return;
		}

		for (int i = 0; i < patternPoints.Length; i++) {
			PatternPoint patternPoint = patternPoints[i].GetComponent<PatternPoint>();
			patternPoint.pointId = i;
			patternPoint.isSelected = false;
		}

		patternNumbers = new List<int>();
		connectingLines = new List<GameObject>();
	}

	public void CreatePatternGrid()
	{
		for (int i = 0; i < patternLines; i++) {

		}
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
		foreach(GameObject patternObject in patternPoints) {
			patternObject.GetComponent<PatternPoint>().isSelected = false;
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
		connectingLines.Clear();
	}
}
