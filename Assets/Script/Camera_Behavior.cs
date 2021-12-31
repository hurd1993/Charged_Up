using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behavior : MonoBehaviour
{
    [SerializeField] float cameraX;
    [SerializeField] float cameraY;

    private bool playerIsJumping;

    public Transform playerCharacter;
    public float cameraZPosition = -10f;

    // Start is called before the first frame update
    void Start()
    {
        playerIsJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerIsJumping);
        //This is Normal Camera Follow

        FollowPlayer();
        


        //Have the camera look at the player's horizontal position without regard
        //to the player's vertical position
       //Vector3 playerPosition = playerCharacter.position;
       //
       //playerPosition.x = playerPosition.x + cameraX;
       //playerPosition.y = transform.position.y;
       //playerPosition.z = playerPosition.z;
       //
       //transform.LookAt(playerPosition);

    }

    void FixedUpdate()
    {

    }

    void FollowPlayer()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = playerCharacter.position.x + cameraX;
        cameraPosition.y = playerCharacter.position.y + cameraY;
        cameraPosition.z = cameraZPosition;

        transform.position = cameraPosition;
    }

    void FollowPlayerOnXAxis()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = playerCharacter.position.x + cameraX;
        cameraPosition.y = cameraPosition.y;
        cameraPosition.z = cameraZPosition;

        transform.position = cameraPosition;
    }

    public void CheckIfPlayerJumps(bool hasJumped)
    {
        playerIsJumping = hasJumped;
    }
}
