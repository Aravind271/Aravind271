using System.Collections.Generic;
using UnityEngine;
using LynxCity.Core;
using LynxCity.Interaction;

namespace LynxCity.Karaoke
{
    public class KaraokeBooth : MonoBehaviour, IInteractable
    {
        [System.Serializable] public class Song { public string title; public string language; public AudioClip licensedAudio; }
        public List<Song> songs = new()
        {
            new Song{ title="Tokyo After Rain", language="English" },
            new Song{ title="Neon Heartbeat", language="English" },
            new Song{ title="夜明けのキャンパス (Campus at Dawn)", language="Japanese" },
            new Song{ title="星空の帰り道 (Way Home Under Stars)", language="Japanese" },
            new Song{ title="City Lights, Quiet Nights", language="English" },
            new Song{ title="青い自転車 (Blue Bicycle)", language="Japanese" }
        };
        int index;
        public string Prompt => "E — Karaoke (¥500)";
        public void Interact(GameInteractor interactor)
        {
            if (GameState.Instance == null) return;
            if (!GameState.Instance.Spend(500)) { GameState.Instance.InteractionPrompt = "Not enough yen."; return; }
            var song = songs[index++ % songs.Count];
            GameState.Instance.InteractionPrompt = $"Karaoke: {song.title} [{song.language}] — timing minigame hook ready";
            if (song.licensedAudio)
            {
                var src = GetComponent<AudioSource>();
                if (!src) src = gameObject.AddComponent<AudioSource>();
                src.clip = song.licensedAudio; src.Play();
            }
        }
    }
}
