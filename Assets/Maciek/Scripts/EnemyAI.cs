﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour {

    public float life = 100;
    public bool staticSpeed = false;
    public float speed = 400f;
    public float nextWaypointDistance = 3f;
    public Animator animator;
    public EnemyType enemyType;
    public GameObject noticedIcon;
    public GameObject spawnDust;
    public List<GameObject> weapons = new List<GameObject>();

    private GameObject player;
    private GameObject weapon;
    private GameObject weaponRender;
    private Vector2 target;
    private Vector2 startingPos;
    private State state = State.Roaming;
    private float staticRandom;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool canChangePos = true;
    Seeker seeker;
    Rigidbody2D rb;

    private enum State
    {
        Roaming,
        ChaseTarget,
        Rest,
    }

    public enum EnemyType
    {
        BigFish,
        TallGuy,
        Sneaky,
    }

    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        weaponRender = gameObject.transform.Find("WeaponRender").gameObject;
        weapon = Instantiate(weapons[Random.Range(0, weapons.Count)],weaponRender.transform.position,Quaternion.identity);
        weapon.transform.SetParent(weaponRender.transform);
        weapon.GetComponent<WeaponInteract>().friendly = false;
        staticRandom = Random.Range(3f,6f);
        InvokeRepeating("UpdatePath", 0.11f, 0.5f);

        if (spawnDust != null) Instantiate(spawnDust, transform.position, Quaternion.identity);

        if (!staticSpeed) {

            switch (enemyType) {
                case EnemyType.BigFish:
                    speed = Random.Range(400,500);
                    break;
                case EnemyType.TallGuy:
                    speed = Random.Range(500, 600);
                    break;
                case EnemyType.Sneaky:
                    speed = Random.Range(600, 700);
                    break;
            }

        }
    }

    void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(rb.position, target, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate() {

        if (player == null) {
            player = GameObject.Find("Player(Clone)");
        }

        switch (state) {
            default:
            case State.Roaming:
                FindTarget();
                if (canChangePos) {
                    target = GetRoamingPos();
                    canChangePos = false;
                    StartCoroutine(WaitTime(Random.Range(3,11)));
                }
                break;
            case State.ChaseTarget:
                if (weapon.GetComponent<WeaponInteract>().IsGun) {
                    target = LerpByDistance(player.transform.position, gameObject.transform.position, staticRandom);
                    if (Vector2.Distance(transform.position, player.transform.position) <= 5) {
                        weapon.GetComponent<WeaponInteract>().aimAtPlayer = true;
                    }
                    
                    if (Vector2.Distance(transform.position, player.transform.position) <= 5) {
                        weapon.GetComponent<WeaponInteract>().Shoot();
                    }
                }

                else target = player.transform.position;

                break;
            case State.Rest:

                break;
        }
        MoveTo();
    }

    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float x) {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }

    public void TargetPlayer() {
        state = State.ChaseTarget;
        StartCoroutine(NoticePlayer(1.5f));
    }

    public void TakeDamage(float dmg) {
        life -= dmg;
        Debug.Log(life);
        if (life <= 0) {
            StartCoroutine(die());
        }
    }

    private IEnumerator die() {
        state = State.Rest;
        transform.rotation = new Quaternion(0, 0, -90,0);
        Destroy(weapon);
        yield return new WaitForSeconds(1);
        animator.SetTrigger("Die");
        transform.Find("MinimapIcon").gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator WaitTime(int time) {
        yield return new WaitForSeconds(time);
        canChangePos = true;
    }
    private IEnumerator NoticePlayer(float time) {
        noticedIcon.SetActive(true);
        yield return new WaitForSeconds(time);
        noticedIcon.SetActive(false);
    }
    private void MoveTo() {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        }
        else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        rb.AddForce(force);
        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    private Vector2 GetRoamingPos() {
        Vector2 randDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        return startingPos + randDir * Random.Range(5f,6f);
    }

    private void FindTarget() {
        if (Vector2.Distance(transform.position, player.transform.position) < Random.Range(9.5f,11f)) {
            //player in range
            TargetPlayer();
        }
    }

    

}
