using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Gameplay.Character.Controller
{
    // TODO:
    // 인터페이스로 분리하고 상속받는 쪽이 Monobehaviour를 상속받을건지 선택하도록 바꿔야할듯.
    // 최종 목표는 Monobehaviour 및 Update 함수 등을 최소화하고 가능하면 이벤트 트리거 방식으로 처리하도록 한다.
    // ex> public abstract class MonoAbstractCharacterController : CharacterControlInterface
    public abstract class AbstractCharacterController : MonoBehaviour
    {
        protected CharacterBase character;
        
        private bool isSetup = false;

        public bool IsSetup => isSetup;

        public virtual void Setup(CharacterBase characterBase)
        {
            character = characterBase;
            isSetup = true;
        }

        public virtual void Refresh() 
        { 
        }
    }
}
