using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{

    private SpriteRenderer objectRenderer;
    private Color ogColour;
    private Animator anim;
    private Renderer[] renderers;
    

    void Start()
    {
        objectRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        renderers = GetComponentsInChildren<Renderer>();
        if (gameObject.name.Equals("GhostRed")) {
            ogColour = Color.red;
        } else if (gameObject.name.Equals("GhostBlue")) {
            ogColour = Color.blue;
        } else if (gameObject.name.Equals("GhostPink")) {
            ogColour = Color.magenta;
        } else if (gameObject.name.Equals("GhostYellow")) {
            ogColour = Color.yellow;
        }
}

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateName = anim.GetCurrentAnimatorStateInfo(0);
        if (stateName.IsName("GhostRecovering"))
        {
            RemoveColour();
        }
        else if (stateName.IsTag("GhostMove"))
        {
            RestoreColour();
        }
    }

    void RemoveColour()
    {
        if (!(objectRenderer.material.color == Color.white))
        {
            objectRenderer.material.color = Color.white;

        }
    }

    void RestoreColour()
    {
        if (!(objectRenderer.material.color == ogColour))
        {
            objectRenderer.material.color = ogColour;
        }
    }
}
