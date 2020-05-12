using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Mods
{
    public class BuildTester : MonoBehaviour
    {
        [InlineEditor(Expanded = true)]
        public ShipBuilder shipBuilder;


        public bool buildPlayerShip;
        

        [Button(ButtonSizes.Gigantic)]
        void TestBuilder()
        {
            var ship = shipBuilder.BuildShip(transform.position, transform.rotation);
            
            if (buildPlayerShip)
            {
                var playerShip = ship.gameObject.AddComponent<PlayerRtsShip>();
                var selectionObject = new GameObject("Selection Object");
                playerShip.Selection = selectionObject;
                selectionObject.transform.parent = playerShip.transform;
                PlayerShips.Instance.playerControlledShips.Add(playerShip);
                playerShip.IsSelected = true;
            }
        }
    }
}