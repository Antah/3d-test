using System;
using System.Collections;
using UniRx;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{
    public Transform bulletsParent;
    public Transform bulletStart;
    public GameObject bullet;
    public GameObject bullet2;
    public Rigidbody rb;
    public Transform cameraPosition;

    public float speed = 10.0f;
    public float turnSpeed = 100f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public bool grounded = false;
    public float distanceToGround;

    public float shootDelayGun = 0.2f;
    public ReactiveProperty<float> elapsedGun;
    public ReactiveProperty<float> gunBoostTimer, speedBoostTimer;

    public float damageModifier = 1f;
    public float speedModifier = 1f;

    public ReactiveProperty<PickupCollider> carriedObject;
    public ReactiveProperty<int> score;

    void Awake()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        elapsedGun = new ReactiveProperty<float>(0f);
        gunBoostTimer = new ReactiveProperty<float>(0f);
        speedBoostTimer = new ReactiveProperty<float>(0f);
        carriedObject = new ReactiveProperty<PickupCollider>(null);
        score = new ReactiveProperty<int>(0);

        speedBoostTimer.Where(x => x > 0f).Subscribe(_ => { speedModifier = 1.5f; });
        gunBoostTimer.Where(x => x > 0f).Subscribe(_ => { damageModifier = 1.5f; });
        speedBoostTimer.Where(x => x <= 0f).Subscribe(_ => { speedModifier = 1f; });
        gunBoostTimer.Where(x => x <= 0f).Subscribe(_ => { damageModifier = 1f; });

        distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
    }
    

    private bool IsGrounded(){
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }

    void ShootGun()
    {
        GameObject BulletClone = Instantiate(bullet, new Vector3(bulletStart.position.x, bulletStart.position.y, bulletStart.position.z), transform.rotation);
        BulletClone.transform.parent = bulletsParent;
        BulletClone.SetActive(true);
        BulletClone.GetComponent<Bullet>().Init(damageModifier);
        BulletClone.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(0, 0, 0);
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //Rotate character according to camera roatation
            //right is shorthand for (1,0,0) or the x value forward is short for (0,0,1) or the z value 
            Vector3 dir = (cameraPosition.right * Input.GetAxis("Horizontal")) + (cameraPosition.forward * Input.GetAxis("Vertical"));
            dir.y = 0;//Keeps character upright against slight fluctuations
            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
            targetVelocity = transform.forward * speed * speedModifier;
        }

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        if (IsGrounded())
        {
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && Input.GetButton("Jump"))
            {
                rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                this.gameObject.layer = 10;
            }
        }
        else
        {
            rb.AddForce(velocityChange / 6, ForceMode.VelocityChange);
        }
            
        // We apply gravity manually for more tuning control
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
    }

    private void ScanForItem(out RaycastHit hit)
    {
        int layerMask = ~LayerMask.GetMask("Player", "PlayerJumping");
        //int layerMask = LayerMask.GetMask("PickupObject");
        Ray forwardRay = new Ray(cameraPosition.transform.position, cameraPosition.transform.forward);
        if (Physics.Raycast(forwardRay, out hit, 10, layerMask))
        {
            //Debug.Log(hit.collider.gameObject.name);
        }
    }

    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * speedModifier * gravity);
    }

    public void SpeedBoost()
    {
        speedBoostTimer.Value += 10f;
    }

    public void GunBoost()
    {
        gunBoostTimer.Value += 10f;
    }

    public void AddScore(int score)
    {
        this.score.Value += score;
    }

    private void Update()
    {
        elapsedGun.Value += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            if (elapsedGun.Value >= shootDelayGun)
            {
                elapsedGun.Value = 0f;
                ShootGun();
            }
        }

        if (Input.GetMouseButtonDown(1) && carriedObject.Value != null)
        {
                carriedObject.Value.Throw(transform.forward * 600 + transform.up * 300, damageModifier);
                carriedObject.Value.item.transform.parent = null;
                carriedObject.Value = null;
                return;
        }

        if (speedBoostTimer.Value > 0f)
            speedBoostTimer.Value -= Time.deltaTime;
        if (gunBoostTimer.Value > 0f)
            gunBoostTimer.Value -= Time.deltaTime;

        RaycastHit hit = new RaycastHit();
        ScanForItem(out hit);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (carriedObject.Value != null)
            {
                carriedObject.Value.Drop(transform.forward * 50 + transform.up * 50, damageModifier);
                carriedObject.Value.item.transform.parent = null;
                carriedObject.Value = null;
            }
            else if (hit.collider != null)
            {
                if (hit.collider.GetComponent<PickupCollider>() != null)
                {
                    carriedObject.Value = hit.collider.GetComponent<PickupCollider>();
                    carriedObject.Value.Pickup(bulletStart);
                }
            }
        }
    }
}