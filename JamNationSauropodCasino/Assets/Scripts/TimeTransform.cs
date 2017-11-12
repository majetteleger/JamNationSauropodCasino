using System;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using System.Reflection;

//DESC: DISPLAY
//TODO: [~] revisit translation ticks: draw at parent instead?
//TODO: [~] try and give translation ticks a distance as well
//DESC: USAGE
//TODO: [ ] reinstate loop enum feature
//DESC: DESIGN
//TODO: [ ] revisit trs traverse job: merge ping pong, snap curve handles?
//TODO: [ ] refactor rotation/translation draw and merge into one
//TODO: [ ] refactor get direction to make shorter and fancier
//DESC: BEAUTY
//TODO: [ ] make custom inspector
//DESC: EXTENSION
//TODO: [ ] mode that has the cam follow selected object's scope
//TODO: [ ] mode that averages the cam so to keep objects screen aligned
using UnityEngine.Events;

public class TrsTraverseJob {
	public float traverse = 0f;
	public int traverseLoopNum = -1;
	public bool isStack = true;
	public bool isPingPong = false;
	public float traverseCurvesPower = 1f;
	public AnimationCurve traversePingCurve;
	public AnimationCurve traversePongCurve;
	public Vector2 range;

	public TrsTraverseJob (
		float aTraverse,
		int aTraverseLoopNum,
		bool aIsStack,
		bool aIsPingPong,
		float aTraverseCurvesPower,
		AnimationCurve aTraversePingCurve,
		AnimationCurve aTraversePongCurve,
		Vector2 aRange){
		traverse = aTraverse;
		traverseLoopNum = aTraverseLoopNum;
		isStack = aIsStack;
		isPingPong = aIsPingPong;
		traverseCurvesPower = aTraverseCurvesPower;
		traversePingCurve = aTraversePingCurve;
		traversePongCurve = aTraversePongCurve;
		range = aRange;
	}
	public bool IsAtPong(float t){
		return (t % 2f > 1f);
	}

	public float CurveEval(AnimationCurve curve, float t){
		float value = t;
		bool funkyMode = false;

		if (traverseCurvesPower <= 1f) {
			traverseCurvesPower = 1f;
		}

		if (funkyMode) {
			for (int i = 1; i <= Mathf.RoundToInt(traverseCurvesPower); i++) {
				value = curve.keys.Length > 0 ? curve.Evaluate (value) : value;
			}
		} else {
			value = curve.keys.Length > 0 ? curve.Evaluate (value) : value;

			//bug workaround: somehow gets above 1f in evaluation, and somehow 1.00004 causes NaN
			value = Mathf.Round(value *1000f)/1000f;

			float valueSign = 1f;
			float absValue = Mathf.Abs(value);
			if(absValue != 0f){
				valueSign = value /absValue;
			}
			value = 1f-Mathf.Pow(1f-absValue, traverseCurvesPower);
			value *= valueSign;


			if(float.IsNaN(value)){
				Debug.Log ("isNaN: " +absValue +" ^ "+traverseCurvesPower +"(t= " +t +")");
				value = 0f;
			}
		}
		return value;
	}
	private float RangeFloat(float value, float oldMin, float oldMax, float newMin, float newMax){
		return (((value- oldMin) /(oldMax -oldMin)) *(newMax-newMin)) +newMin;
	}
	public TrsTraverseResult GetResult(){

		float t = traverse;

		int tPingCount = (int)Mathf.Floor((t +1f) /2f);
		int tPongCount = (int)Mathf.Floor((t +0f) /2f);			

		tPingCount = isPingPong ? tPingCount : tPingCount +tPongCount;
		tPongCount = isPingPong ? tPongCount : 0;

		tPingCount = isStack ? tPingCount : 0;
		tPongCount = isStack ? tPongCount : 0;

		Vector2 pingOffsetBounds = new Vector2(CurveEval(traversePingCurve, 0f), CurveEval(traversePingCurve, 1f));
		Vector2 pongOffsetBounds = new Vector2(CurveEval(traversePongCurve, 0f), CurveEval(traversePongCurve, 1f));
		
		float pingOffsetRange = pingOffsetBounds.y -pingOffsetBounds.x;
		float pongOffsetRange = pongOffsetBounds.y -pongOffsetBounds.x;

		float pingOffset = (float)tPingCount *pingOffsetRange;
		float pongOffset = (float)tPongCount *pongOffsetRange;
		
		float mt = t %1f;
		float mtOffset = CurveEval(isPingPong && IsAtPong(t) ? traversePongCurve : traversePingCurve, mt);

		float offset = 0f;
		offset += pingOffset;
		offset += pongOffset;
		offset += mtOffset;

		float rangedOffset = RangeFloat(offset, 0f, 1f, range.x, range.y);
		int sideNum = isPingPong ? IsAtPong(t) ? 1 : -1 : 0;

		return new TrsTraverseResult(rangedOffset, sideNum);
	}
}

public class TrsTraverseResult {
	public float rangedOffset = 0f;
	public int sideNum = 0;
	public TrsTraverseResult(float aRangedOffset, int aSideNum){
		rangedOffset = aRangedOffset;
		sideNum = aSideNum;
	}
}


[Serializable]
public class EventTrigger
{
    public float Traverse = -1f;
    public UnityEvent Event = new UnityEvent();
    private float _lastFlooredMasterTraverse = -1f;

    public void Process(float flooredMasterTraverse, float unitMasterTraverse, float traverseOffset)
    {
        float traverse = Traverse + traverseOffset;
        float unitTraverse = Mathf.Repeat(traverse, 1f);
        if (unitMasterTraverse > unitTraverse && flooredMasterTraverse > _lastFlooredMasterTraverse)
        {
            Event.Invoke();
            _lastFlooredMasterTraverse = flooredMasterTraverse;
        }
    }
}

public class TimeTransform : MonoBehaviour {
	
	public enum operationEnum { TRANSLATE, ROTATE_RELATIVE, ROTATE };
	public operationEnum operation = operationEnum.TRANSLATE;
	
	public enum axisEnum { X, Y, Z };
	public axisEnum axis = axisEnum.Z;

	public Transform referenceTrs;

	public Vector2 valueRange = new Vector2(-1f, 1f);

	public float deltaTraverse = 1f;

	public float traverse = 0f;
	public float initTraverse = 0f;
	
	private enum traverseLoopEnum { INFINITE, NUM }
	private traverseLoopEnum traverseLoop = traverseLoopEnum.INFINITE;
	private int traverseLoopNum = 1;

	public enum traverseOffsetEnum { STACK, RESET }
	public traverseOffsetEnum traverseOffset = traverseOffsetEnum.STACK;

	public enum traverseSchemeEnum { PING, PINGPONG }
	public traverseSchemeEnum traverseScheme = traverseSchemeEnum.PINGPONG;

	public float traverseCurvesPower = 1f;

	public AnimationCurve traversePingCurve = new AnimationCurve();
	public AnimationCurve traversePongCurve = new AnimationCurve();

	public float traverseDrawSpan = 1.2f;

    public List<EventTrigger> LaunchEventTriggers = new List<EventTrigger>();
    public List<EventTrigger> DetonateEventTriggers = new List<EventTrigger>();

    private bool _canLaunch;
    private float _launchOffset;
    public void Launch()
    {
        _canLaunch = true;
        enabled = true;
        _launchOffset = traverse;
    }

    private bool _canDetonate;
    private float _detonateOffset;
    public void Detonate()
    {
        _canDetonate = true;
        _detonateOffset = traverse;
    }


	private AnimationCurve RoundCurveKey(AnimationCurve curve){
		int keysLength = curve.keys.Length;
		Keyframe[] keys = new Keyframe[keysLength];
		for(int i = 0; i < keysLength; i++){
			Keyframe key = curve.keys[i];
			key.time = Mathf.Round(key.time *10f) /10f;
			key.value = Mathf.Round(key.value *10f) /10f;
			keys[i] = key;
		}
		curve.keys = keys;
		return curve;
	}
	private void RoundCurveKeys(){
		traversePingCurve = RoundCurveKey(traversePingCurve);
		traversePongCurve = RoundCurveKey(traversePongCurve);
	}

    public bool Repeats;
	private void Traverse(){

		if(valueRange.x == valueRange.y){return;}

		float timedDeltaTraverse = deltaTraverse *Time.deltaTime;
		traverse += timedDeltaTraverse;

        if (!Repeats) traverse = traverse > 1f ? 1f : traverse;
	}

	private float prevValue = 0f;
	public float Apply(){
		
		//skip
		if(valueRange.x == valueRange.y){return 0f;}

		//refactor
		TrsTraverseJob trsTraverseJob = new TrsTraverseJob(
			traverse +initTraverse*0.01f,
			traverseLoop == traverseLoopEnum.INFINITE || traverseLoopNum < 0 ? -1 : traverseLoopNum,
			traverseOffset == traverseOffsetEnum.STACK,
			traverseScheme == traverseSchemeEnum.PINGPONG,
			traverseCurvesPower,
			traversePingCurve,
			traversePongCurve,
			valueRange);
		TrsTraverseResult trsTraverseResult = trsTraverseJob.GetResult();
		float value = trsTraverseResult.rangedOffset;

		//apply
		if(operation == operationEnum.TRANSLATE){

			Vector3 pos = transform.localPosition;
			pos.x = axis == axisEnum.X ? value : pos.x;
			pos.y = axis == axisEnum.Y ? value : pos.y;
			pos.z = axis == axisEnum.Z ? value : pos.z;
			transform.localPosition = pos;

		}else if(operation == operationEnum.ROTATE_RELATIVE){

			if(referenceTrs!=null){
				transform.position=referenceTrs.position;
				transform.rotation=referenceTrs.rotation;
				
				transform.rotation = Quaternion.Euler(fDir *value) *transform.rotation;
				return value;
			}

			float valueOffset = value -prevValue;
			transform.rotation = Quaternion.Euler(fDir *valueOffset) *transform.rotation;
			prevValue = value;

		}else if(operation == operationEnum.ROTATE){

			Vector3 euler = transform.localEulerAngles;

			value = float.IsNaN(value) ? 0f : value;

			euler.x = axis == axisEnum.X ? value : euler.x;
			euler.y = axis == axisEnum.Y ? value : euler.y;
			euler.z = axis == axisEnum.Z ? value : euler.z;

			transform.localEulerAngles = euler;
		}

		//return
		return value;
	}

	public Vector2 DrawTicks(float value){
		
		Vector2 levelRange = new Vector2();


		//range
		float rangeMagnitude = valueRange.y -valueRange.x;

		
		//refactor
		float tStep = 0.1f;
		float tMin = traverse -traverseDrawSpan +initTraverse*0.01f;
		tMin = tMin < 0f? 0f: tMin;
		float tMax = traverse +initTraverse*0.01f;
		tMin = Mathf.Round (tMin /tStep) *tStep;
		tMax = Mathf.Round (tMax /tStep) *tStep;


		List<float> drawValueList = new List<float>();
		List<int> sideNumList = new List<int>();
		for(float t = tMin; t <= tMax; t += tStep){
			t = Mathf.Round (t /tStep) *tStep;
			TrsTraverseJob trsTraverseJob = new TrsTraverseJob(
				t,
				traverseLoop == traverseLoopEnum.INFINITE || traverseLoopNum < 0 ? -1 : traverseLoopNum,
				traverseOffset == traverseOffsetEnum.STACK,
				traverseScheme == traverseSchemeEnum.PINGPONG,
				traverseCurvesPower,
				traversePingCurve,
				traversePongCurve,
				valueRange);
			TrsTraverseResult trsTraverseResult = trsTraverseJob.GetResult();
			
			float drawValue = trsTraverseResult.rangedOffset;
			drawValueList.Add (drawValue);

			int sideNum = trsTraverseResult.sideNum;
			sideNum = t %1f == 0f ? 0 : sideNum;
			sideNumList.Add (sideNum);
		}
		if(operation == operationEnum.TRANSLATE){

			//draw
			for(int i = 0; i < drawValueList.Count; i++){
				
				int ii = i -1 < 0 ? i : i -1;
				int ij = i;
				int ik = i +1 >= drawValueList.Count ? i : i +1;
				
				float iValue = drawValueList[ii];
				float jValue = drawValueList[ij];
				float kValue = drawValueList[ik];
				
				float ijkValue = 0f;
				ijkValue += jValue -iValue;
				ijkValue += kValue -jValue;
				ijkValue /= (ii != ij && ij != ik) ? 2f : 1f;
				
				float ijkRadius = RangeFloat(Mathf.Abs(ijkValue), 0f, 1f, 0.05f, 1f);
				
				//ping on one side, pong on the other
				int sideNum = sideNumList[i];
				float innerRadiusFactor = sideNum == -1 ? 0f : rodRadius;
				float outerRadiusFactor = sideNum == 1 ? 0f : rodRadius;
				
				
				// point ticks at editor camera	
				Vector3 camRelPos = camPos -transform.position;
				Vector3 rCamDir = Vector3.Cross(camRelPos, fDir).normalized;

				// offset-from-current value
				float jOffsetValue = jValue -value;

				Vector3 jfDir = fDir *jOffsetValue;

				float baseRadius = 0f;
				float radiusTimeInvFactor = 1f;//deltaTraverse;//0.01f;//
				float radius = ijkRadius /(radiusTimeInvFactor *Mathf.Abs(valueRange.y -valueRange.x));
				radius = RangeFloat(radius, 0f, 1f, 0.25f, 1f);
				
				float innerRadius = baseRadius -innerRadiusFactor *radius;
				float outerRadius = baseRadius +outerRadiusFactor *radius;
				
				Vector3 jInnerPos = transform.position;
				Vector3 jOuterPos = transform.position;
				
				jInnerPos += jfDir;
				jOuterPos += jfDir;

				jInnerPos += rCamDir *innerRadius;
				jOuterPos += rCamDir *outerRadius;
				
				//fade away from cursor
				float traverseAlphaFactor = (float)i /(float)drawValueList.Count;
				
				//draw tick
				Debug.DrawLine(jInnerPos, jOuterPos, color *new Color(1f, 1f, 1f, tickAlphaFactor *traverseAlphaFactor));
				
				//draw axis
				if(iValue == jValue){
					continue;
				}
				float iOffsetValue = iValue -value;
				
				Vector3 ifDir = fDir *iOffsetValue;

				levelRange.x = iOffsetValue < levelRange.x ? iOffsetValue : levelRange.x;
				levelRange.y = iOffsetValue > levelRange.y ? iOffsetValue : levelRange.y;
				
				levelRange.x = jOffsetValue < levelRange.x ? jOffsetValue : levelRange.x;
				levelRange.y = jOffsetValue > levelRange.y ? jOffsetValue : levelRange.y;

			}


		}else if(operation == operationEnum.ROTATE){

			//draw
			for(int i = 0; i < drawValueList.Count; i++){
				
				int ii = i -1 < 0 ? i : i -1;
				int ij = i;
				int ik = i +1 >= drawValueList.Count ? i : i +1;
				
				float iValue = drawValueList[ii];
				float jValue = drawValueList[ij];
				float kValue = drawValueList[ik];
				
				float ijkValue = 0f;
				ijkValue += jValue -iValue;
				ijkValue += kValue -jValue;
				ijkValue /= (ii != ij && ij != ik) ? 2f : 1f;
				
				float ijkRadius = RangeFloat(Mathf.Abs(ijkValue), 0f, 1f, 0.05f, 1f);
				
				//ping on one side, pong on the other
				int sideNum = sideNumList[i];
				float innerRadiusFactor = sideNum == -1 ? 0f : rodRadius;
				float outerRadiusFactor = sideNum == 1 ? 0f : rodRadius;

				
				// offset-from-current value
				float jOffsetValue = jValue -value;
				float jAngle = jOffsetValue /180f *Mathf.PI;

				Vector3 jrDir = rDir *Mathf.Cos(jAngle);
				Vector3 juDir = uDir *Mathf.Sin(jAngle);



				float baseRadius = 1f;
				float radiusTimeInvFactor = 1f;//deltaTraverse;//0.01f;//deltaTraverse/traverse +initTra;
				float radius = ijkRadius /(radiusTimeInvFactor *Mathf.Abs(valueRange.y -valueRange.x));
				radius = RangeFloat(radius, 0f, 1f, 0.25f, 1f);

				if(simpleDraw){
					baseRadius = 0f;
				}

				float innerRadius = baseRadius -innerRadiusFactor *radius;
				float outerRadius = baseRadius +outerRadiusFactor *radius;

				
				Vector3 jInnerPos = transform.position;
				Vector3 jOuterPos = transform.position;

				jInnerPos += jrDir *innerRadius;
				jInnerPos += juDir *innerRadius;

				jOuterPos += jrDir *outerRadius;
				jOuterPos += juDir *outerRadius;
				

				//fade away from cursor
				float traverseAlphaFactor = (float)i /(float)drawValueList.Count;
				
				//draw tick
				Debug.DrawLine(jInnerPos, jOuterPos, color *new Color(1f, 1f, 1f, tickAlphaFactor *traverseAlphaFactor));

				//draw axis
				if(simpleDraw){
					continue;
				}
				if(iValue == jValue){
					continue;
				}
				
				float iOffsetValue = iValue -value;
				float iAngle = iOffsetValue /180f *Mathf.PI;

				Vector3 irDir = rDir *Mathf.Cos(iAngle);
				Vector3 iuDir = uDir *Mathf.Sin(iAngle);

				Vector3 iPos = transform.position;
				iPos += irDir *baseRadius;
				iPos += iuDir *baseRadius;
				
				Vector3 jPos = transform.position;
				jPos += jrDir *baseRadius;
				jPos += juDir *baseRadius;

				Debug.DrawLine(iPos, jPos, color *new Color(1f, 1f, 1f, tickAxisAlphaFactor *traverseAlphaFactor));
			}
		}
		return levelRange;
	}
	public List<float> outFloatList = new List<float>();
	public List<string> outStringList = new List<string>();
	//move

	//range func
	private float RangeFloat(float value, float oldMin, float oldMax, float newMin, float newMax){
		return (((value- oldMin) /(oldMax -oldMin)) *(newMax-newMin)) +newMin;
	}

	//draw
	public bool draw = true;
	public bool simpleDraw = true;
	public float rodRadius = 0.5f;
	public float meshRadius = 0.5f;
	private float prevMeshRadius = 0.5f;
	public float meshAlphaFactor = 0.1f;
	public float tickAxisAlphaFactor = 0.05f;
	public float tickAlphaFactor = 0.1f;
	public float rodAlphaFactor = 0.1f;
	public bool rodIsPretty = true;
	public Color color = new Color(1f, 1f, 1f, 1f);

	public void DrawAxis(Vector2[] levelRanges){
		Vector2 levelRange = new Vector2();
		for(int i = 0; i < levelRanges.Length; i++){
			levelRange.x = levelRanges[i].x < levelRange.x ? levelRanges[i].x : levelRange.x;
			levelRange.y = levelRanges[i].y > levelRange.y ? levelRanges[i].y : levelRange.y;
		}
		
		Vector3 mPos = transform.position +fDir *levelRange.x;
		Vector3 nPos = transform.position +fDir *levelRange.y;
		Debug.DrawLine(mPos, nPos, color *new Color(1f, 1f, 1f, tickAxisAlphaFactor));
	}

	public int outInt = 0;
	private Vector2 DrawChildRods(){
		Vector2 levelRange = new Vector2();
		foreach(Transform childTrs in transform){

			Vector3 childRelPos = childTrs.position -transform.position;
			float fChildLevel = Vector3.Dot (fDir, childRelPos);

			Vector3 nPos = transform.position +fDir *fChildLevel;
			Vector3 oPos = childTrs.position;

			levelRange.x = fChildLevel < levelRange.x ? fChildLevel : levelRange.x;
			levelRange.y = fChildLevel > levelRange.y ? fChildLevel : levelRange.y;

			float timeTransformAlphaFactor = 1f;
			TimeTransform timeTransform = childTrs.gameObject.GetComponent<TimeTransform>();
			if(timeTransform != null && timeTransform.enabled == true){
				timeTransformAlphaFactor = 0.5f;
			}
			timeTransformAlphaFactor = 0.5f;
			
			//rod
			if(rodIsPretty){
				nPos=transform.position;
			}
			Debug.DrawLine(nPos, oPos, color *new Color(1f, 1f, 1f, rodAlphaFactor *timeTransformAlphaFactor));
		}
		return levelRange;
	}

	private Vector3 parentTrsDir(Transform trs, Vector3 dir){
		return trs.parent ? trs.parent.TransformDirection(dir) : dir;
	}
	private Vector3 parentInvTrsDir(Transform trs, Vector3 dir){
		return trs.parent ? trs.parent.InverseTransformDirection(dir) : dir;
	}
	private Vector3 parentTrsPos(Transform trs, Vector3 pos){
		return trs.parent ? trs.parent.TransformPoint(pos) : pos;
	}
	private Vector3 parentInvTrsPos(Transform trs, Vector3 pos){
		return trs.parent ? trs.parent.InverseTransformPoint(pos) : pos;
	}

	//get axes
	private Vector3 rPreLocalDir = new Vector3();
	private Vector3 uPreLocalDir = new Vector3();
	private Vector3 fPreLocalDir = new Vector3();
	
	private Vector3 rDir = new Vector3();
	private Vector3 uDir = new Vector3();
	private Vector3 fDir = new Vector3();

	private void GetDirections(){

		//scope
		rPreLocalDir = Vector3.right;
		uPreLocalDir = Vector3.up;
		fPreLocalDir = Vector3.forward;

		if(transform.parent){
			rDir = transform.parent.TransformDirection(rPreLocalDir);
			uDir = transform.parent.TransformDirection(uPreLocalDir);
			fDir = transform.parent.TransformDirection(fPreLocalDir);
		}else{
			rDir = rPreLocalDir;
			uDir = uPreLocalDir;
			fDir = fPreLocalDir;
		}

		//axis
		Vector3 rAxisLocalDirection = new Vector3();
		Vector3 uAxisLocalDirection = new Vector3();
		Vector3 fAxisLocalDirection = new Vector3();
		
		Vector3 rAxisDirection = new Vector3();
		Vector3 uAxisDirection = new Vector3();
		Vector3 fAxisDirection = new Vector3();

		if(axis == axisEnum.Z){
			rAxisLocalDirection = rPreLocalDir;
			uAxisLocalDirection = uPreLocalDir;
			fAxisLocalDirection = fPreLocalDir;

			rAxisDirection = rDir;
			uAxisDirection = uDir;
			fAxisDirection = fDir;
		}else if(axis == axisEnum.Y){
			rAxisLocalDirection = fPreLocalDir;
			uAxisLocalDirection = rPreLocalDir;
			fAxisLocalDirection = uPreLocalDir;

			rAxisDirection = fDir;
			uAxisDirection = rDir;
			fAxisDirection = uDir;
		}
		else if(axis == axisEnum.X){
			rAxisLocalDirection = uPreLocalDir;
			uAxisLocalDirection = fPreLocalDir;
			fAxisLocalDirection = rPreLocalDir;
			
			rAxisDirection = uDir;
			uAxisDirection = fDir;
			fAxisDirection = rDir;
		}

		uPreLocalDir = uAxisLocalDirection;
		fPreLocalDir = fAxisLocalDirection;
		rPreLocalDir = rAxisLocalDirection;
		
		uDir = uAxisDirection;
		fDir = fAxisDirection;
		rDir = rAxisDirection;
	}

	public static SceneView GetActiveSceneView() {
		//Return the focused window, if it is a SceneView
		if(EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType() == typeof(SceneView)){
			return (SceneView)EditorWindow.focusedWindow;
		}
		//Otherwise return the first available SceneView
		ArrayList temp = SceneView.sceneViews;
	    if (temp.Count < 1) return null;
		return (SceneView)temp[0];
	}
	private Camera editorCamera;
	private Vector3 camPos = new Vector3();

	//update
	void Start () {
		transformPos = transform.position;
	}


	void Update () {

		if(operation == operationEnum.TRANSLATE){
			simpleDraw = true;

		}else{
			simpleDraw = false;
		}

		//use editor camera to project ticks
		SceneView editorSceneView = GetActiveSceneView();
	    if (editorSceneView != null)
	    {
            editorCamera = editorSceneView.camera;
            if (editorCamera)
            {
                camPos = editorCamera.transform.position;
            } 
	    }

		//fix curves
		RoundCurveKeys();



		//fix live rotation
		bool isHandleOffset = false;
		{
			System.Type type = typeof(Tools);
			PropertyInfo propertyInfo = type.GetProperty("get", BindingFlags.NonPublic | BindingFlags.Static);
			object propertyInfoValue = propertyInfo.GetValue(null, null);
			Tools currentTools = (Tools)propertyInfoValue;
			
			object valueObject = new object();

			//zero handle offset
			valueObject = new Vector3();
			{
				FieldInfo fieldInfo = type.GetField("handleOffset", BindingFlags.Static | BindingFlags.NonPublic);
				if(fieldInfo != null){
					fieldInfo.SetValue(currentTools, valueObject);
				}
			}

			//check if handle is actually offset

			if(!float.IsInfinity(Tools.handlePosition.x)){
				if(Selection.activeTransform == transform || Selection.activeTransform.IsChildOf(transform)){
					if(Selection.activeTransform.position != Tools.handlePosition){
						valueObject = Tools.handlePosition -Selection.activeTransform.position;
						valueObject = Selection.activeTransform.position -Tools.handlePosition;
						isHandleOffset = true;
					}
				}
			}

			//save
			if(Tools.current == Tool.Move){
				transformPos = transform.position;
			}

			//set handle offset
			if(isHandleOffset && applyHandleOffset){
				FieldInfo fieldInfo = type.GetField("handleOffset", BindingFlags.Static | BindingFlags.NonPublic);
				if(fieldInfo != null){
					fieldInfo.SetValue(currentTools, valueObject);
				}

				if(Tools.current == Tool.Rotate){
					transform.position = transformPos;
				}
			}else{
				transformPos = transform.position;
			}
		}

		deltaTraverse = Mathf.Clamp(deltaTraverse, 0f, 1f);

		//apply
		GetDirections();
		float value = Apply();
		Vector2 ticksLevelRange = DrawTicks(value);
		Vector2 childRodsLevelRange = DrawChildRods();
		DrawAxis(new Vector2[2]{ticksLevelRange, childRodsLevelRange});

		Traverse();

        if (_canLaunch || _canDetonate)
        {
            float flooredTraverse = Mathf.Floor(traverse);
            float unitTraverse = Mathf.Repeat(traverse, 1f);

            //launch
	        if (_canLaunch)
            {
                foreach (EventTrigger eventTrigger in LaunchEventTriggers)
                {
                    eventTrigger.Process(flooredTraverse, unitTraverse, _launchOffset);
                }
	        }

            //detonate
	        if (_canDetonate)
            {
                foreach (EventTrigger eventTrigger in DetonateEventTriggers)
                {
                    eventTrigger.Process(flooredTraverse, unitTraverse, _detonateOffset);
                } 
	        }
	    }

	    //temp trs and traverse reset
		if(reset){
			transform.localPosition = new Vector3();
			transform.localRotation = new Quaternion();
			traverse = 0f;
			reset = false;
		}
		if(resetTranslate){
			transform.localPosition = new Vector3();
			traverse = 0f;
			resetTranslate = false;
		}
		if(resetRotate){
			transform.localRotation = new Quaternion();
			transform.localEulerAngles = new Vector3();
			traverse = 0f;
			resetRotate = false;
		}

	}
	private bool applyHandleOffset = true;
	private Vector3 transformPos = new Vector3();
	public bool resetTranslate = false;
	public bool resetRotate = false;
	public bool reset = false;
}
