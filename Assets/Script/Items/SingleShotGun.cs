using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Drawing;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView pv;

    [SerializeField] private bool addBulletSpread = true;
    [SerializeField] Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSytem;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private float shootDelay = 0.5f;
    [SerializeField] LayerMask mask;

    private float LastShootTime;

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

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);  =========  HIT PLAYER

            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));


            hit.collider.gameObject.GetComponent<AttackTarget>()?.receiveAttack(((GunInfo)itemInfo).damage);
            pv.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 StartPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(StartPosition, hit.point, time);
            time += Time.deltaTime/trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        var impact =  Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Debug.Log("EnumTail");
        Destroy(trail.gameObject, trail.time);
        Destroy(impact.gameObject, 0.2f);
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitposition,Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitposition, 0.3f);
        if(colliders.Length != 0)
        {
           
            //trail.transform.position += Vector3.Lerp(this.transform.position, hitNormal, 10f);

            var bulletimpact =  Instantiate(bulletimpactPrefab, hitposition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal,Vector3.up) * bulletimpactPrefab.transform.rotation);

            bulletimpact.transform.SetParent(colliders[0].transform);
            Destroy(bulletimpact, 10f);
        }

    }
}
