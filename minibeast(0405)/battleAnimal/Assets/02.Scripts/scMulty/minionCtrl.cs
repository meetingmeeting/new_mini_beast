﻿using UnityEngine;
using System.Collections;

public class minionCtrl : MonoBehaviour {
	private Transform minionTr;
	public Transform playerTr;

	public bool isMove;
	private Transform[] point;
	public Vector3 dest;
	public Vector3 target;
	public Vector3 syncTarget;

	private int idx;
	private int speed;

	public enum MinionState{idle,trace,attack,die};
	public MinionState minionState;
	public float traceDist;
	public float attackDist;

	private bool isDie;
	private bool isPlayer;
	private bool isTrace;

	public float dist;

	private bool moveKey;
	private bool traceKey;

	public bool isMaster;

	// Use this for initialization
	void Start () {

		traceDist = 5.0f;
		attackDist = 2.0f;

		moveKey = true;
		traceKey = false;

		minionState = MinionState.idle;

		isMove = false;		
		isDie = false;
		isPlayer = false;
		isTrace = false;

		idx = 1;
		speed = 2;
		minionTr = gameObject.GetComponent<Transform> ();

		int number = int.Parse(gameObject.name[2]+"");
		Debug.Log (number);

		if (number % 3 == 0) {
			point = GameObject.Find ("redMovePoints/route1").GetComponentsInChildren<Transform> ();
		} else if (number % 3 == 1) {
			point = GameObject.Find ("redMovePoints/route2").GetComponentsInChildren<Transform> ();
		} else if (number % 3 == 2) {
			point = GameObject.Find ("redMovePoints/route3").GetComponentsInChildren<Transform> ();
		}

		syncTarget = dest = point [idx].position;

		if (isMaster) {
			StartCoroutine (checkPlayer ());
			StartCoroutine (this.CheckMonsterState ());
		}
	}

	// Update is called once per frame
	void Update () {
		if (isMaster) {
			if (moveKey) {
				moveKey = false;

				move ();
			}
			if (traceKey) {
				traceKey = false;
				trace ();
			}
		}

		if (isMove){
			minionTr.LookAt(dest);
			if (dest != minionTr.position) {
				float step = speed * Time.deltaTime;
				minionTr.position = Vector3.MoveTowards (minionTr.position, dest, step);
			} else {
				if(isMaster){
					if (idx < point.Length - 1){
						syncTarget = dest = point [++idx].position;
						moveKey=true;
					}
				}
			}		
		}

		if (isTrace) {
			syncTarget = target = playerTr.position;
			minionTr.LookAt(target);
			float step = speed * Time.deltaTime;
			minionTr.position = Vector3.MoveTowards (minionTr.position, target, step);
		}
	}

	public void move(){
		isMove = true;
		isTrace = false;
	}

	public void trace(){		
		isMove=false;
		isTrace = true;
	}

	IEnumerator checkPlayer(){
		yield return new WaitForSeconds (0.2f);
		if (GameObject.FindWithTag ("Player") != null) {
			playerTr = GameObject.FindWithTag ("Player").GetComponent<Transform>();
			isPlayer = true;
			yield return null;
		}
	}

	IEnumerator CheckMonsterState(){
		while (!isDie) {
			yield return new WaitForSeconds(0.2f);
			if(isPlayer){
				dist = Vector3.Distance(playerTr.position,minionTr.position);
			}else{
				dist = 1000.0f;
			}

			if(dist<=attackDist){
				minionState = MinionState.attack;
			}
			else if(dist<=traceDist)
			{
				if(isTrace==false)
					traceKey = true;
			}
			else
			{
				if(isMove==false)
					moveKey = true;
			}
		}
	}
}
