using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject Prefab_Agent;
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject obj = Instantiate(Prefab_Agent);
            obj.GetComponent<RollerAgent>().Target = Target;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
