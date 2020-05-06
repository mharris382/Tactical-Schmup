using System;
using System.Collections.Generic;
using System.Linq;
using Ships.Defenses;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Mods
{
    [CreateAssetMenu(menuName = "Data/New Ship")]
    public class ShipBuilder : ScriptableObject
    {
        
        [TitleGroup("Ship Prefab"), HideLabel, PreviewField(100, ObjectFieldAlignment.Left)]
        public ShipModManager shipTemplate;

        
        [Required,PropertyOrder(-10), AssetsOnly,ValidateInput("validateUi")]
        public Canvas uiHealthCanvasPrefab;


        [Title("Defense Mods")] 
        
        [GUIColor("hullColor")]  public int hullHP = 100;
        [GUIColor("shieldColr")]  public DefenseConfig shieldConfig;
        [GUIColor("armorColor")]  public DefenseConfig armorConfig;




        #region [EDITOR HELPERS]

#if UNITY_EDITOR
        private Color hullColor => Color.Lerp(Color.red, Color.white, 0.65f);
        private Color armorColor => Color.Lerp(Color.green, Color.white, 0.65f);
        private Color shieldColr => Color.Lerp(Color.cyan, Color.white, 0.65f);
        bool validateUi(Canvas go, ref string msg)
        {
            if (go == null) return true;
            var hullHealth = GetUiCircles(go, out var shield, out var armor);
            msg = "";
            
            if (armor == null)msg += "No Armor Circle Found.\n";
            if (shield == null) msg += "No Shield Circle Found.\n";
            if (hullHealth == null) msg += "No Hull Circle Found.";
            
            return hullHealth != null && shield != null && armor != null;
        }
#endif

        #endregion

        private static UiHealthCircle GetUiCircles(Canvas go, out UiHealthCircle shield, out UiHealthCircle armor)
        {
            var circles = go.GetComponentsInChildren<UiHealthCircle>();
            var hullHealth = circles.FirstOrDefault(t => t.target == UiHealthCircle.HealthTarget.Hull);
            shield = circles.FirstOrDefault(t => t.target == UiHealthCircle.HealthTarget.Shield);
            armor = circles.FirstOrDefault(t => t.target == UiHealthCircle.HealthTarget.Armor);
            return hullHealth;
        }
        

        public ShipModManager BuildShip(Vector3 position, Quaternion rotation)
        {
            var shipInstance = Instantiate(shipTemplate, position, rotation);
            shipInstance.isTemplate = false;
            
            InjectDefenses(shipInstance);
    

            return shipInstance;
        }

        private void InjectDefenses(ShipModManager shipInstance)
        {
            //Setup Health UI
            var uiParent = shipInstance.hudParent;
            var ui = Instantiate(uiHealthCanvasPrefab, uiParent);
            UiHealthCircle uiHull = GetUiCircles(ui, out UiHealthCircle uiShield, out UiHealthCircle uiArmor);
            ui.transform.localPosition =Vector3.zero;
            ui.transform.localScale = Vector3.one;
            ui.transform.localRotation = Quaternion.identity;
            //Setup Armor
            var defenseLayers = new List<DefenseLayer>();
            if (armorConfig != null)
            {
                var armorLayer = CreateDefenseLayerInstance(shipInstance.gameObject, armorConfig);
                armorLayer.name += " Armor";
                uiArmor.healthTarget = armorLayer.gameObject;
                defenseLayers.Add(armorLayer);
            }
            else
            {
                uiArmor.gameObject.SetActive(false);
            }

            //Setup Shields
            if (shieldConfig != null)
            {
                var shieldLayer = CreateDefenseLayerInstance(shipInstance.gameObject, shieldConfig);
                shieldLayer.name += "Shield";
                uiShield.healthTarget = shieldLayer.gameObject;
                defenseLayers.Add(shieldLayer);
            }
            else
            {
                uiShield.gameObject.SetActive(false);
            }

            //Setup Hull
            var damageHandler = shipInstance.gameObject.AddComponent<ShipDamageHandler>();
            damageHandler.maxHullHP = this.hullHP;
            damageHandler.defenseLayers = defenseLayers;
        }
        
        public DefenseLayer CreateDefenseLayerInstance(GameObject shipInstance, DefenseConfig config)
        {
            var go = new GameObject("Defense Layer");
            go.transform.parent = shipInstance.transform;
            var dl = go.AddComponent<DefenseLayer>();
            dl._defenseConfig = config;
            return dl;
        }
    }
}