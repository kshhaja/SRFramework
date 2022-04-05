using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Gameplay.Character.Controller
{
    // SlimeRPG.UI.EquipmentSlot 이거랑 겹친다. UI를 없애자.
    // poe 따라가기.
    public enum EquipmentSlot
    {
        // weapon
        mainHand,
        offHand,

        // armour
        gloves,
        boots,
        bodyArmour,
        helmet,
        belt,

        // acc
        neck,
        leftRing,
        rightRing,

        // flask
        flask1,
        flask2,
        flask3,
        flask4,
        flask5,
    }


    // 인벤토리와 분리 시키는 이유는 장비장착 가능한npc나 플레이어가 아니라면 필요없기 때문.
    public class EquipmentController : AbstractCharacterController
    {
        // private Action<EquipmentItem> onEquip;
        private Action<object> onUnequip;

        Dictionary<EquipmentSlot, object> equipments = new Dictionary<EquipmentSlot, object>();


        public override void Setup(CharacterBase characterBase)
        {
            base.Setup(characterBase);

            // 빈 슬롯들로 채워준다.
            foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
                equipments.Add(slot, null);
        }

        public bool IsEquipable(object item)
        {
            // var t = item.type
            // t로 장착할 수 있는 슬롯 찾기.

            return true;
        }

        public EquipmentSlot FindEquipmentSlot(object item)
        {
            // 한손무기는 메인 -> 오프 순서로 찾고 둘다 안된다면 메인을 넘겨준다.
            return EquipmentSlot.mainHand;
        }

        public object EquipmentFrom(EquipmentSlot slot)
        {
            return equipments[slot];
        }

        // 지금은 이렇게 처리하지만, 서버가 준비되면 transaction으로 장비교환한 결과를 가져오게되겠지..
        public void Equip(object item)
        {
            // Unequip(item.slot);

            // item.EquippedBy(character);
            
            // equipments[item.slot, item];

            // onEquiped?.Invoke(item);
        }

        public void Unequip(EquipmentSlot slot)
        {
            var equipment = equipments[slot];
            if (equipments != null)
                onUnequip?.Invoke(equipment);

            equipments[slot] = null;
        }
    }
}