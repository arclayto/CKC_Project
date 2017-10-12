using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatformController : MonoBehaviour {

	private StandardShaderUtils.BlendMode blendMode;
	private Material render;

	// Use this for initialization
	void Start () {
		render = gameObject.GetComponentInChildren<Renderer>().material;
		blendMode = StandardShaderUtils.BlendMode.Fade;
		StandardShaderUtils.ChangeRenderMode (render, blendMode);
	}
	
	// Update is called once per frame
	void Update () {
		Animator animator = transform.GetComponentInChildren<Animator>();
		if (animator.GetCurrentAnimatorStateInfo (0).nameHash == Animator.StringToHash ("Base Layer.RisingPlatformIdleTop")) {
			blendMode = StandardShaderUtils.BlendMode.Opaque;
			StandardShaderUtils.ChangeRenderMode (render, blendMode);
		} 
		else {
			blendMode = StandardShaderUtils.BlendMode.Fade;
			StandardShaderUtils.ChangeRenderMode (render, blendMode);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Animator animator = transform.GetComponentInChildren<Animator>();
			animator.SetTrigger("PlatformUp");
		}
	
	}

	void onTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player") {
			Animator animator = transform.GetComponentInChildren<Animator>();
			animator.SetTrigger("PlatformUp");
		}

	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Animator animator = transform.GetComponentInChildren<Animator>();
			animator.SetTrigger("PlatformDown");
		}
			
	}

}
