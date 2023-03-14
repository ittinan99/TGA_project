using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();    
    }

    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        Debug.DrawRay(ray.origin,ray.direction,Color.red,1f);
        
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            pv.RPC("RPC_Shoot", RpcTarget.All, hit.point,hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitposition,Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitposition, 0.3f);
        if(colliders.Length != 0)
        {
        var bulletimpact =  Instantiate(bulletimpactPrefab, hitposition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal,Vector3.up) * bulletimpactPrefab.transform.rotation);
            bulletimpact.transform.SetParent(colliders[0].transform);
            Destroy(bulletimpact, 10f);
        }

    }
}
