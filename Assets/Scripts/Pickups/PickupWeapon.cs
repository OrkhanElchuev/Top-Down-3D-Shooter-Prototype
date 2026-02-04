using UnityEditor;
using UnityEngine;

public class PickupWeapon : Interactable
{
    private PlayerWeaponManager weaponManager;
    [SerializeField] private WeaponDataSO weaponDataSO;
    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponModel[] models;

    private void Start()
    {
        weapon = new Weapon(weaponDataSO);
        SetupGameObject();
    }

    public override void Interaction()
    {
        weaponManager.PickupWeapon(weapon);
        ObjectPooling.instance.ReturnObject(gameObject);
    }

    [ContextMenu("Update Item Model")]    
    public void SetupGameObject()
    {
        gameObject.name = "Pickup Weapon - " + weaponDataSO.weaponType.ToString();
        SetupWeaponModel();
    }

    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        this.weapon = weapon;
        weaponDataSO = weapon.weaponDataSO;

        this.transform.position = transform.position + new Vector3(0, 0.75f, 0);
    }

    public void SetupWeaponModel()
    {
        foreach (WeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            if (model.weaponType == weaponDataSO.weaponType)
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());  
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (weaponManager == null)
            weaponManager = other.GetComponent<PlayerWeaponManager>();
    }
}
