using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour {

    CharacterController controller;
    private enum CURRENT_TERRAIN { BLUE_TILE, TEAL_TILE, WHITE_TILE, ORANGE_TILE };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    private FMOD.Studio.EventInstance footsteps;
    private FMOD.Studio.EventInstance hydraulics;

    public float speed;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        DetermineTerrain();
        DetermineSpeed();
    }
    private void DetermineSpeed()
    {
        speed = Mathf.Round(controller.velocity.magnitude * 1000f) / 1000f;
    }

    private void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 0.25f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Teal"))
            {
                currentTerrain = CURRENT_TERRAIN.TEAL_TILE;
                break;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("White"))
            {
                currentTerrain = CURRENT_TERRAIN.WHITE_TILE;
                break;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Blue"))
            {
                currentTerrain = CURRENT_TERRAIN.BLUE_TILE;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                currentTerrain = CURRENT_TERRAIN.ORANGE_TILE;
            }
        }
    }

    public void SelectAndPlayFootstep()
    {     
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.TEAL_TILE:
                PlayFootstep(1);
                break;

            case CURRENT_TERRAIN.BLUE_TILE:
                PlayFootstep(0);
                break;

            case CURRENT_TERRAIN.WHITE_TILE:
                PlayFootstep(2);
                break;

            case CURRENT_TERRAIN.ORANGE_TILE:
                PlayFootstep(3);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }

    public void PlayMachinery(){
        PlayHydraulics();
    }

    private void PlayHydraulics() {
        hydraulics = FMODUnity.RuntimeManager.CreateInstance("event:/Machinery");
        hydraulics.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        if (speed != 0)
        {
            hydraulics.start();
            hydraulics.release();
        }
    }

    private void PlayFootstep(int terrain)
    {
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps");
        footsteps.setParameterByName("Tiles", terrain);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        if (speed != 0)
        {
            footsteps.start();
            footsteps.release();
        }
    }
}