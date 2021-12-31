using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    // -1 for Negative Charge/ 0 for No Charge / 1 for Positive Charge
    [SerializeField] int magneticCharge;
    private new SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        VisualizeCharge(magneticCharge);
    }

    private void VisualizeCharge(int charge)
    {
        switch(charge)
        {
            case -1:
                renderer.color = Color.red;
                break;
            case 0:
                renderer.color = Color.white;
                break;
            case 1:
                renderer.color = Color.blue;
                break;
        }
    }

    public int GetCharge()
    {
        return magneticCharge;
    }
}
