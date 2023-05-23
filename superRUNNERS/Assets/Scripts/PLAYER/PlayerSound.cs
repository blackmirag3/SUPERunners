using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public AudioSource footstep;
    public AudioSource slide;
    public AudioSource jump;
    public AudioSource land;

    private bool hasPlayedSlide = false;
    private bool hasJumped = false;
    private bool canFootstep = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleSlide();
        HandleJump();
    }

    public void HandleFootstep(float sine) //used in player cam script, head bobbing function
    {
        if (sine < -0.9f && canFootstep)
        {
            footstep.Play();
            canFootstep = false;
        }
        else if (sine > 0.9)
            canFootstep = true;
    }

    public void HandleSlide()
    {
        if (!hasPlayedSlide && playerMovement.isSliding)
        {
            slide.Play();
            hasPlayedSlide = true;
        }
        else if (hasPlayedSlide && !playerMovement.isSliding)
        {
            slide.Stop();
            hasPlayedSlide = false;
        }
    }
    
    //TODO: Fix player movement states
    public void HandleJump()
    {
        if (Input.GetKey(playerMovement.jumpKey))
        {
            if (!hasJumped)
                jump.Play();

            hasJumped = true;
        }
        else if (hasJumped && (playerMovement.isGrounded || playerMovement.isWallRunning))
        {
            land.Play();
            hasJumped = false;
        }
    }
    
}
