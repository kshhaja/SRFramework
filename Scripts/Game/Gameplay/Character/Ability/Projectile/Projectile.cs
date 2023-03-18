using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlimeRPG.Framework.Ability;
using System;
using System.Linq;

namespace SlimeRPG.Gameplay.Character.Ability
{
    // 이것도 ScriptableObject로부터 만들어지도록 수정할 예정.
    public class Projectile : MonoBehaviour
    {
        public WeakReference<AbilitySystemComponent> weakInstigator;
        public WeakReference<AbilitySystemComponent> weakSource;
        public List<WeakReference<AbilitySystemComponent>> weakTargets = new List<WeakReference<AbilitySystemComponent>>();

        public List<GameplayEffectSpec> effects;

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

        protected AbilitySystemComponent Instigator
        {
            get
            {
                if (weakInstigator.TryGetTarget(out var instigator))
                    return instigator;

                return null;
            }
        }

        protected AbilitySystemComponent Source
        {
            get
            {
                if (weakSource.TryGetTarget(out var source))
                    return source;

                return null;
            }
        }

        protected List<AbilitySystemComponent> Targets
        {
            get
            {
                return weakTargets.Select(x =>
                {
                    if (x.TryGetTarget(out var target))
                        return target;
                    return null;
                }).Where(x => x != null).ToList();
            }
        }


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

            if (Source == null || Source.gameObject == go)
                return;

            if (go.TryGetComponent<AbilitySystemComponent>(out var target))
            {
                if (Targets.Contains(target) == false)
                    weakTargets.Add(new WeakReference<AbilitySystemComponent>(target));

                foreach (var effect in effects)
                    target.ApplyGameplayEffect(effect);
            }

            //if (secondaryAbilitySpec != null)
            //{
            //    var ac = source.GetComponent<CharacterBase>();
            //    if (ac)
            //    {
            //        // instigator.AbilitySystem.MakeOutgoingAbilitySpec(secondaryAbility, 1);
            //        // StartCoroutine(secondaryAbility.TryActivateAbility());
            //    }
            //}

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