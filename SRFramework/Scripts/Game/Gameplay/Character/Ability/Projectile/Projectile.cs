using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlimeRPG.Framework.Ability;


namespace SlimeRPG.Gameplay.Character.Ability
{
    // 이것도 ScriptableObject로부터 만들어지도록 수정할 예정.
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        public CharacterBase source;

        // targets 로 pierce 대응할수있도록 기능 추가 필요.
        [SerializeField]
        // trigger enter 쪽에서 부딪히는 타겟을 계속저장해서 중복으로 이펙트가 적용되지 않게 막는다.
        public List<CharacterBase> targets = new List<CharacterBase>();
        //public AbilitySystemCharacter Target;

        public GameplayEffectScriptableObject effect;
        // 발사 -> 폭발 -> ... 등 여러 단계를 표현하고 싶다면 아래 기능을 사용할것
        public AbilityBase secondaryAbility;

        private bool useRigidbody = true;
        [SerializeField]
        private float speed = 10.0f;

        [SerializeField]
        private float acceleration = 3.0f;

        [HideInInspector]
        public float lifeTime = 5f;

        private int pierceCount = 0;
        private float explosionRange = 0;

        public bool useGravity = true;


        private void Start()
        {
            if (TryGetComponent<Rigidbody>(out var rigidbody))
            {
                rigidbody.AddForce(gameObject.transform.forward * speed);
            }

            StartCoroutine(LifeCycle());
        }

        private IEnumerator LifeCycle()
        {
            float timeStamp = Time.time;

            while (true)
            {
                if (timeStamp + lifeTime <= Time.time)
                {
                    break;
                }

                yield return null;
            }

            Destroy(gameObject);
        }

        private void ProcessGameplayEffect(GameObject go)
        {
            if (secondaryAbility)
            {
                secondaryAbility.Setup(source);
                StartCoroutine(secondaryAbility.TryCastAbility());
            }
            gameObject.SetActive(false);
            //StopAllCoroutines();
            //Destroy(gameObject);

            if (go.tag != "Enemy" && go.tag != "Player")
                return;

            if (source.gameObject == go)
                return;

            if (go.TryGetComponent<CharacterBase>(out var target))
            {
                if (targets.Contains(target) == false)
                {
                    targets.Add(target);

                    target.attributeController.ApplyGameplayEffect(effect);
                }
            }

            if (this.secondaryAbility)
            {
                var ac = source.GetComponent<CharacterBase>();
                if (ac)
                {
                    // ac.CastAbility(this.secondaryAbility.secondaryAbility);
                }
            }

            StopAllCoroutines();
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ProcessGameplayEffect(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            ProcessGameplayEffect(other.gameObject);
        }
    }
}