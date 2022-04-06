using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlimeRPG.Framework.Ability;


namespace SlimeRPG.Gameplay.Character.Ability
{
    // 이것도 ScriptableObject로부터 만들어지도록 수정할 예정.
    public class Projectile : MonoBehaviour
    {
        public CharacterBase instigator;
        [SerializeField]
        public AbilitySystemCharacter source;

        [SerializeField]
        public List<AbilitySystemCharacter> targets = new List<AbilitySystemCharacter>();

        public GameplayEffectSpec effect;
        // 발사 -> 폭발 -> ... 등 여러 단계를 표현하고 싶다면 아래 기능을 사용할것
        public AbilityBase secondaryAbility;

        [SerializeField]
        private float speed = 10.0f;

        //[SerializeField]
        //private float acceleration = 3.0f;

        [HideInInspector]
        public float lifeTime = 5f;

        public ParticleSystem particle;

        // custom datas
        private int pierceCount = 0;
        private float explosionRange = 0;
        private bool useGravity = true;

        private int internalPierceCount = 0;


        private void Start()
        {
            if (TryGetComponent<Rigidbody>(out var rigidbody))
            {
                rigidbody.AddForce(gameObject.transform.forward * speed);
                rigidbody.useGravity = useGravity;
            }

            if (particle)
                Instantiate(particle.gameObject, transform);

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
            if (go.tag != "Enemy" && go.tag != "Player")
                return;

            if (source.gameObject == go)
                return;

            if (go.TryGetComponent<AbilitySystemCharacter>(out var target))
            {
                if (targets.Contains(target) == false)
                    targets.Add(target);

                target.ApplyGameplayEffect(effect);
            }

            if (secondaryAbility)
            {
                var ac = source.GetComponent<CharacterBase>();
                if (ac)
                {
                    secondaryAbility.Setup(instigator);
                    StartCoroutine(secondaryAbility.TryActivateAbility());
                }
            }

            internalPierceCount++;
            if (internalPierceCount > pierceCount)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
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