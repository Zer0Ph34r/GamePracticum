using UnityEngine;

public class ParticleDestroyerScript : MonoBehaviour {

    ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
