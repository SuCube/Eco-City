//Building.cs

using UnityEngine;

public class Building : MonoBehaviour
{
    public enum Status
    {
        IsEmpty,
        IsUnderConstruction,
        IsBuilt,
        IsUpgrading,
        IsUnderDestruction,
        IsDestructed
    }

    public int Id { get; set; }
    //public int NumberOfPeople { get; set; }
    //public float Health { get; set; } //ћб не пригодитс€
    public float ConstructionProgress { get; set; }
    public Status BuildingStatus { get; set; }
    public int MaxUpgrades { get; set; }

    public Building(int Id)
    {
        this.Id = Id;
    }


    // You can add common methods for buildings here
}