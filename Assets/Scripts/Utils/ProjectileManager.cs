using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviorSingleton<ProjectileManager>
{
    public GameObject projectilePrefab;
    public int projectilesToCreate = 10;
    private List<Projectile> projectileList;

    // Start is called before the first frame update
    void Start()
    {
        CreateProjectiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateProjectiles()
    {
        projectileList = new List<Projectile>(projectilesToCreate);
        for(int i = 0; i < projectilesToCreate; i++)
        {
            var gameObject = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity, transform);
            gameObject.name = $"{projectilePrefab.name}-{i}";
            projectileList.Add(gameObject.GetComponent<Projectile>());
            gameObject.SetActive(false);
        }
    }

    public Projectile GetItem()
    {
        if(projectileList.Count > 0)
        {
            var projectile = projectileList[0];
            projectileList.RemoveAt(0);
            return projectile;
        }
        else
        {
            return null;
        }
    }

    public void ReturnItem(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectileList.Add(projectile);
    }
}
