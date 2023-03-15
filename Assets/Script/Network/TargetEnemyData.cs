using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Newtonsoft.Json;

namespace TGA.Network
{
    [Serializable]
    public class TargetEnemyData : BaseNetworkData
{
        /// <summary>
        /// Collectable id list
        /// </summary>
        public List<Guid> TargetEnemyList { get; private set; }

        public TargetEnemyData(List<Guid> targetEnemyList) : base()
        {
            TargetEnemyList = targetEnemyList;
        }

        [JsonConstructor]
        public TargetEnemyData(Guid id, DateTime time, List<Guid> targetEnemyList) : base(id, time)
        {
            TargetEnemyList = targetEnemyList;
        }

        public override string GetInfo()
        {
            var info = base.GetInfo();

            foreach (var id in TargetEnemyList)
            {
                info += $"Guid = {id}\n";
            }

            return info;
        }
    }
}

