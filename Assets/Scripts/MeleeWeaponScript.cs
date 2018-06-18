using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponScript : MonoBehaviour {

    public bool isEquipped;

    public bool canStick;
    public bool isStuck;

    public GameObject player;
    public GameObject playerCam;
    protected Animation AttackAnim;


    public float Durability;
    public float Damage;
    public float DurabilityLost;
    public float AnimationLength; // Set this to the lenght of the animation from start to finish(Until the weapon returns to default IDLE state).

    float ActualDamage; //The weapon loses damage based on its durability. Less Durability = Less Damage.

    // These are the rotation axis for when the player equips the weapon, the weapon will be locked as a child of the camera. 
    // Please make sure to manually set each of these in the inspector, for every prefab(Not placed), and save it into the prefab to be used later;
    public Vector3 relTransPos;
    public Quaternion relTransRot;

    // The same like before, but the positions.

    private float equipCD;
    private float damageCD; // Because the weapon damages due to two objects colliding, it fixes the damage being applied every frame.

    private float animCD;

    private float stickDice; // This is for the random number generation in the Attack() function.


    void Start () {
        gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
        AttackAnim = gameObject.GetComponent<Animation>();

        equipCD = 0f;
        //Set the bool to false, just to make sure it does not break anything.
        isEquipped = false;
        // Check if a player has been assigned to it, if not, then display a debug message and set the player.
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("The player has not been manually set! Searching for the player object, make sure it has the tag (Player)!");
        }
        if (playerCam == null)
        {
            playerCam = GameObject.FindGameObjectWithTag("MainCamera");
            Debug.Log("The player camera has not been manually set! Searching for the player camera object, make sure it has the tag (MainCamera)!");
        }

        WeaponTransformSnapping();

    }
	
	// Update is called once per frame
	void Update () {
        equipCD = equipCD - 1 * Time.deltaTime;
        damageCD = damageCD - 1 * Time.deltaTime;

        animCD = animCD - 1 * Time.deltaTime;

        if (DurabilityLost >= Durability || DurabilityLost >= Damage) // As I said, the ActualDamage equation should be changed to make this "DurabilityLost >= Damage" useless.
        {
            BreakWeapon();
        }

        if (Input.GetMouseButtonDown(0) && isEquipped)
        {
            Attack();
        }
        if (animCD <= 0f && isEquipped)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            canStick = !canStick;
        }
    }

    void WeaponTransformSnapping()
    {
        if (isEquipped)
        {
            gameObject.transform.localPosition = new Vector3(relTransPos.x, relTransPos.y, relTransPos.z);
            transform.localRotation = Quaternion.Euler(relTransRot.x, relTransRot.y, relTransRot.z);
        }

    }

    public void EquipWeapon() {
        isEquipped = true; // Set the bool to true, so the game detects it as being equipped.
        if (isEquipped && equipCD < 0f && !isStuck)
        {
            gameObject.transform.parent = playerCam.transform;  //Places the weapon under the player's camera.
            WeaponTransformSnapping();  // These two lines position the object to the player's hand to be ready to get animated.


            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = false; // Messes with the Capsule Collider to avoid the player being pushed when attacking.
            gameObject.GetComponent<CapsuleCollider>().isTrigger = true;

            equipCD = 0.5f;

            playerCam.GetComponent<InteractionScript>().CurrentWeapon = gameObject; // Makes this object be accessible through the "TestPickupScript" in Main Camera;
        }
        if (isStuck)
        {   // Maybe play a special animation where the player rips the weapon out of the object.
            isStuck = false;
            gameObject.transform.parent = playerCam.transform;
            WeaponTransformSnapping();

            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = false; // Messes with the Capsule Collider to avoid the player being pushed when attacking.
            gameObject.GetComponent<CapsuleCollider>().isTrigger = true;

            equipCD = 0.5f;

            playerCam.GetComponent<InteractionScript>().CurrentWeapon = gameObject;
        }
        if (!isEquipped) {
            Debug.Log("Something went wrong in" + gameObject.name);
            Debug.Log("A script is trying to set isEquipped value to false while this script is setting it to true");
        }
    }
    public void UnEquipWeapon() {
        if (equipCD < 0f)
        {
            equipCD = 0.5f;
            isEquipped = false;
            gameObject.transform.parent = null; // Removes the parent of the weapon.
            gameObject.GetComponent<Rigidbody>().isKinematic = false;

            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            gameObject.GetComponent<CapsuleCollider>().isTrigger = false;

            playerCam.GetComponent<InteractionScript>().CurrentWeapon = null;
        }
    }
    public void StuckWeapon() { // Does not work as intended right now.

        AttackAnim.Stop();

        canStick = false;
        isStuck = true;
        isEquipped = false;

        gameObject.transform.parent = null; // Removes the parent of the weapon.

        playerCam.GetComponent<InteractionScript>().CurrentWeapon = null;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider collision)

    {
        ActualDamage = Damage - DurabilityLost; // Should be modified for a better equation.
        float CalculatedDamage;
        //This should be replaced later, as the types of the opponents change, for better performance.
        //I suggest using Interfaces or Layers instead of tags, because you can have more one the same object.
        if (collision.gameObject.tag == "DummyTarget" && damageCD < 0f) {
            // Detect an enemy's script where the health is stored. For now I will use the Dummy's script, but if there are multiple enemy tipes, then scan through possible scripts.
            CalculatedDamage = ActualDamage; // This should be used to calculate the damage according to the enemy's armor, or resistance.
            collision.gameObject.GetComponent<DummyScript>().HP = collision.gameObject.GetComponent<DummyScript>().HP - CalculatedDamage;
            DurabilityLost = DurabilityLost + 1;
            damageCD = AnimationLength;
        }

        if (collision.gameObject.tag != "Player" && collision.gameObject.layer != 10 && canStick) {
            StuckWeapon(); // Check if the object is not colliding with the player while hitting an enemy.
            gameObject.transform.parent = collision.gameObject.transform;
        }
    }
    void BreakWeapon() {
        // Break the weapon. Either destroy the game object, or make it unusable by setting some kind of variable. Right now it destroys it.
        Destroy(gameObject);
    }

    void Attack() {
        stickDice = Random.Range(1, 100); // Generate a random number.
        animCD = AnimationLength; // Play the attack animation, the enemy gets damaged once the weapon collides with the enemy.
        AttackAnim.Play();
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        if (DurabilityLost > Durability / 1.5f && !canStick && stickDice >= 80)
        {
            canStick = true; // This is for a bit of randomness for the weapon to get stuck in an enemy.
        }
    }
}
