using UnityEngine;

public class Expiration : MonoBehaviour
{
    float time;
    public float lifeTime = 5;
    private void OnEnable()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > lifeTime)
        {
            gameObject.SetActive(false);
        }
    }
}
