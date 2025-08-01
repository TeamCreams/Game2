﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_FrontMover : MonoBehaviour 
{
    public Transform pivot;
    public ParticleSystem effect;
    public float speed = 15f;
    public float drug = 1f;
    public float repeatingTime = 1f;

    public float startSpeed = 0f;

    void Start()
    {
        InvokeRepeating("StartAgain", 0f, repeatingTime);
        effect.Play();
        startSpeed = speed;
    }

    void StartAgain()
    {
        startSpeed = speed;
        transform.position = pivot.position;
    }

    void Update()
    {
        startSpeed = startSpeed * drug;
        transform.position += transform.forward * (startSpeed * Time.deltaTime);
    }
}
