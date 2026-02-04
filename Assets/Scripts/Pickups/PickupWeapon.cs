using UnityEditor;
using UnityEngine;

public class PickupWeapon : Interactable
{
    private PlayerWeaponManager weaponManager;
    [SerializeField] private WeaponDataSO weaponDataSO;
    [SerializeField] private WeaponModel[] models;

    private void Start()
    {
        UpdateGameObject();
    }

    public override void Interaction()
    {
        weaponManager.PickupWeapon(weaponDataSO);
    }

    [ContextMenu("Update Item Model")]    
    public void UpdateGameObject()
    {
        gameObject.name = "Pickup Weapon - " + weaponDataSO.weaponType.ToString();
        UpdateItemModel();
    }

    public void UpdateItemModel()
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
