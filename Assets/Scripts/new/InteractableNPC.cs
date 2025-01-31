using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : Interactable
{
    private Animator animator;

    public override void Start()
	{
		base.Start();
        animator = GetComponent<Animator>();
        animator.SetTrigger("wave");
    }
    protected override void Interaction()
	{
		base.Interaction();
        print("Hello! Unfortunately I don't have a dialog system yet.");
        animator.SetTrigger("wave");


        //Start Dialogue System
    }
}
