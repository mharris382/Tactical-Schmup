using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Mods
{
    public class BuildTester : MonoBehaviour
    {
        [InlineEditor(Expanded = true)]
        public ShipBuilder shipBuilder;





        [Button(ButtonSizes.Gigantic)]
        void TestBuilder()
        {
            var res = shipBuilder.BuildShip(transform.position, transform.rotation);
        }
    }
}