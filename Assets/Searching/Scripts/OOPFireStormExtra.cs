using System.Collections;
using System.Collections.Generic;
using Searching;
using UnityEngine;

public class OOPFireStormExtra : Identity
{
    public override void Hit()
    {
        mapGenerator.player.inventory.AddItem("FireStorm");
        mapGenerator.player.inventory.AddItem("FireStorm");
        mapGenerator.fireStorms[positionX, positionY] = null;
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        Destroy(gameObject);
    }
}
