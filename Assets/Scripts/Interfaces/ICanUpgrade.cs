using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanUpgrade
{
    void ApplyUpgrade(IUpgradeEffect upgradeEffect);
}
