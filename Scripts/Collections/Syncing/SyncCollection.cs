using System;
using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Other/Sync collection")]
    public class SyncCollection : MonoBehaviour
    {
        [Required]
        public ItemCollectionBase toSyncFrom;

        [Required]
        public UIWindow toSyncFromWindow;

        [Required]
        public UIWindow toSyncToWindow;

        //[Required]
        //public RectTransform toSyncToContainer;

        [Required]
        public ItemCollectionBase toSyncTo;


        //        private Transform _fromParent;
        //        private int _fromChildIndex;

        protected virtual void Awake()
        {
            StartSyncing();

        }

        protected virtual void Start()
        {
        }


        protected void OnDestroy()
        {
            StopSyncing();
        }
        
        public void StartSyncing()
        {
            toSyncFromWindow.OnShow += CopyToOriginal;
            toSyncToWindow.OnShow += CopyToSynced;
            toSyncToWindow.OnHide += CopyToOriginal;
            toSyncFrom.OnSetItem += RebuildToLayout;
        }

        private void CopyToOriginal()
        {
            foreach (var item in toSyncFrom.items)
            {
                var c = item as UnityEngine.Component;
                if (c != null)
                {
                    c.transform.SetParent(toSyncFrom.container);
                    InventoryUtility.ResetTransform(c.transform);
                }
            }
        }

        private void CopyToSynced()
        {
            foreach (var item in toSyncFrom.items)
            {
                var c = item as UnityEngine.Component;
                if (c != null)
                {
                    c.transform.SetParent(toSyncTo.containerItemsParent);
                    InventoryUtility.ResetTransform(c.transform);
                }
            }
        }

        public void StopSyncing()
        {
            toSyncFromWindow.OnShow -= CopyToOriginal;
            toSyncToWindow.OnShow -= CopyToSynced;
            toSyncToWindow.OnHide -= CopyToOriginal;
            toSyncFrom.OnSetItem -= RebuildToLayout;
        }

        private void RebuildToLayout(uint slot, InventoryItemBase item)
        {
            if (toSyncTo.container is RectTransform)
            {
                LayoutRebuilder.MarkLayoutForRebuild(toSyncTo.container as RectTransform);
            }
        }
    }
}
