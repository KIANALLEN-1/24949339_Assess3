using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private Tween activeTween;
    public Vector3[] waypoints;
    public float speed = 5.0f;
    private float snapDistance = 0.2f;
    public int currentWaypoint = 0;
    private Animator _pacAnimator;
    private Transform targetObject;
    private AudioSource pacMove;
    
    void Awake()
    {
        pacMove = GetComponent<AudioSource>();
        _pacAnimator = GetComponent<Animator>();
        targetObject = GetComponent<Transform>();
        waypoints = new Vector3[4];
        float distance = 16f; 

        waypoints[0] = transform.position + Vector3.right * distance; 
        waypoints[1] = waypoints[0] + Vector3.down * distance/5*4; 
        waypoints[2] = waypoints[1] + Vector3.left * distance; 
        waypoints[3] = waypoints[2] + Vector3.up * distance/5*4; 
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (TweenExists(targetObject))
        {
            return false;
        }
        else
        {
            float distance = Vector3.Distance(startPos, endPos);
            duration = distance / speed;
            activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
            return true;
        }
    }

    public bool TweenExists(Transform target)
    {
        if (activeTween != null && activeTween.Target == target)
        {
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTween == null)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = waypoints[currentWaypoint];
            float duration = Vector3.Distance(startPos, endPos) / speed;
            activeTween = new Tween(transform, startPos, endPos, Time.time, duration);

            if (!pacMove.isPlaying)
            {
                pacMove.Play();
            }
        }

        if (activeTween != null)
        {
            float timeFraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
            timeFraction = Mathf.Clamp01(timeFraction);

            transform.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, timeFraction);
            
            if (!pacMove.isPlaying)
            {
                pacMove.Play();
            }
            if (Vector3.Distance(transform.position, activeTween.EndPos) < snapDistance)
            {
                transform.position = activeTween.EndPos;
                activeTween = null;

                currentWaypoint++;

                // if (currentWaypoint >= waypoints.Length)
                // {
                //     currentWaypoint = 0;
                // }
                pacMove.Pause();

            }
        }
    }
}
