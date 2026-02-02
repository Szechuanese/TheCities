using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate( Vector2.left * Time.deltaTime * 100);
    }
}
