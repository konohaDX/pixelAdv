using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMgr : MonoBehaviour
{

    [SerializeField] private string type;
    private Player player;
    private Enemy enemy;

    
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player").GetComponent<Player>();
        
        if(type == "Enemy")
        {
            enemy = transform.parent.GetComponent<Enemy>();
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collider)
    {     
        if(collider.tag != "AttackArea")
        {
            if (type == "Player")
            {
                if (player.IsAttackKeyFrame && !player.IsHitting)
                {
                    collider.GetComponent<Enemy>().IsDamaged();
                }
            }
            else if (type == "Enemy")
            {
                enemy.Attack();
                if (enemy.IsAttackKeyFrame)
                {
                    if (player.IsCounter)
                    {
                        Debug.Log("Player is counter~!!!");
                    }
                    else
                    {
                        player.IsDamaged();
                    }
                    
                }
            }
        }
        
        
    }
}
