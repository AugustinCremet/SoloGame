using System.Collections;
using System.Collections.Generic;
using BulletPro;
using TMPro;
using UnityEngine;

// This script is supported by the BulletPro package for Unity.
// Template author : Simon Albou <albou.simon@gmail.com>

// This script is actually a MonoBehaviour for coding advanced things with Bullets.
public class PredictionShot : BaseBulletBehaviour {

	// You can access this.bullet to get the parent bullet script.
	// After bullet's death, you can delay this script's death : use this.lifetimeAfterBulletDeath.

	// Use this for initialization (instead of Start)
	public override void OnBulletBirth ()
	{
		base.OnBulletBirth();

		GameObject target = GameObject.FindWithTag("Player");
		Vector3 targetPosition = target.transform.position;
		Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
		Vector3 targetVelocity = targetRb.linearVelocity;
		Vector3 shooterPosition = transform.position;
		float bulletSpeed = bullet.moduleMovement.baseSpeed;
		Vector3 hitPoint;

		Vector3 displacement = targetPosition - shooterPosition;
		float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
		//if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
		if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > bulletSpeed && Mathf.Sin(targetMoveAngle) / bulletSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
		{
			Debug.Log("Position prediction is not feasible.");
			hitPoint = targetPosition;
		}
		else
		{
			//also Sine Formula
			float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / bulletSpeed);
			hitPoint = targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
		}

		bullet.transform.up = (hitPoint - shooterPosition).normalized;
	}
	
	// Update is (still) called once per frame
	public override void Update ()
	{
		base.Update();

		// Your code here
	}

	// This gets called when the bullet dies
	public override void OnBulletDeath()
	{
		base.OnBulletDeath();

		// Your code here
	}

	// This gets called after the bullet has died, it can be delayed.
	public override void OnBehaviourDeath()
	{
		base.OnBehaviourDeath();

		// Your code here
	}

	// This gets called whenever the bullet collides with a BulletReceiver. The most common callback.
	public override void OnBulletCollision(BulletReceiver br, Vector3 collisionPoint)
	{
		base.OnBulletCollision(br, collisionPoint);

		// Your code here
	}

	// This gets called whenever the bullet collides with a BulletReceiver AND was not colliding during the previous frame.
	public override void OnBulletCollisionEnter(BulletReceiver br, Vector3 collisionPoint)
	{
		base.OnBulletCollisionEnter(br, collisionPoint);

		// Your code here
	}

	// This gets called whenever the bullet stops colliding with any BulletReceiver.
	public override void OnBulletCollisionExit()
	{
		base.OnBulletCollisionExit();

		// Your code here
	}

	// This gets called whenever the bullet shoots a pattern.
	public override void OnBulletShotAnotherBullet(int patternIndex)
	{
		base.OnBulletShotAnotherBullet(patternIndex);

		// Your code here
	}
}
