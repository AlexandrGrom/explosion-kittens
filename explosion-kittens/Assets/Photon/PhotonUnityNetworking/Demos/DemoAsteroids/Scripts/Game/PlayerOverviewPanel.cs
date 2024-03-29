﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerNumbering.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player Overview Panel
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.Asteroids
{
    public class PlayerOverviewPanel : MonoBehaviourPunCallbacks
    {
        public GameObject PlayerOverviewEntryPrefab;

        private Dictionary<int, GameObject> playerListEntries;

        #region UNITY

        public void Awake()
        {
            playerListEntries = new Dictionary<int, GameObject>();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerOverviewEntryPrefab);
                entry.transform.SetParent(gameObject.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<Text>().color = AsteroidsGame.GetColor(p.GetPlayerNumber());
                
                
                entry.GetComponent<Text>().text =
                    $"{p.NickName}\nScore: {p.GetScore()}\nLives: {AsteroidsGame.PLAYER_MAX_LIVES}";

                playerListEntries.Add(p.ActorNumber, entry);
            }
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                entry.GetComponent<Text>().text =
                    $"{targetPlayer.NickName}\nScore: {targetPlayer.GetScore()}\nLives: {targetPlayer.CustomProperties[AsteroidsGame.PLAYER_LIVES]}";
            }
        }

        #endregion
    }
}