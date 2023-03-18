using SlimeRPG.Gameplay.Character;
using SlimeRPG.Utility;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        // consider Serverside...
        [SerializeField] private GameObject playerGameObject;
        
        private Dictionary<int, PlayerCharacter> players = new Dictionary<int, PlayerCharacter>();

        protected override void Awake()
        {
            base.Awake();

            SpawnPlayer(0);
        }

        void SpawnPlayer(int index)
        {
            // 위치는 어떻게 찾을까...
            // 언리얼 로직과 유사하게 만들어보자..

            // step1. 월드에 배치된 PlayerCharacter가 있다면 ID부여하고 패스.
            var characters = Resources.FindObjectsOfTypeAll(typeof(PlayerCharacter));
            if (characters.Length > 0)
            {
                var c = characters[0] as PlayerCharacter;
                c.SetPlayerID(index);
                players.Add(index, c);
            }

            // step2. 월드에 배치된 PlayerStart를 찾아 PlayerCharacter를 만들어 배치한 후 PlayerStart 제거.

            // step3. 카메라 위치에 스폰? 이 케이스는 조금 더 생각을 해보자..
        }

        public PlayerCharacter GetPlayerCharacter(int index)
        {
            if (players.ContainsKey(index))
                return players[index];
            else
                return null;
        }
        
    }
}
