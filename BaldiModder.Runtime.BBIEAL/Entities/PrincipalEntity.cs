using System;
using System.Collections.Generic;
using System.Reflection;
using BaldiModder.Runtime.BBIEAL.Data;
using BaldiModder.Runtime.Entities;
using UnityEngine;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("Principal")]
    [EntityDefinition("Principal")]
    public class PrincipalEntity : AnimatedEntity {

        public bool SeesRuleBreak { get => (bool)GetFieldValue("SeesRuleBreak"); }
        public bool BullySeen { get => (bool)GetFieldValue("BullySeen"); }

        public float CoolDown { get => (float)GetFieldValue("CoolDown"); set => SetFieldValue("CoolDown", value); }
        public float TimeSeenRuleBreak { get => (float)GetFieldValue("TimeSeenRuleBreak"); set => SetFieldValue("TimeSeenRuleBreak", value); }

        public bool Angry { get => (bool)GetFieldValue("Angry"); set => SetFieldValue("Angry", value); }
        public bool InOffice { get => (bool)GetFieldValue("InOffice"); }

        public int Detentions { get => (int)GetFieldValue("Detentions"); }

        [ConfigurableEntityField] public List<PrincipalPunishmentTime> PrincipalPunishmentTimes { get; set; } = new List<PrincipalPunishmentTime>();

        private void Start() {
            //Lock Time
            FieldInfo lockTime = GetField("LockTime");

            int[] times = (int[])lockTime.GetValue(GetComponent(runAlongSide.Type));
            Array.Resize(ref times, PrincipalPunishmentTimes.Count);

            for (int i = 0; i < times.Length; i++) {
                times[i] = PrincipalPunishmentTimes[i].Time;
            }

            lockTime.SetValue(GetComponent(runAlongSide.Type), times);

            //Audio Clips
            FieldInfo lockTimeAudio = GetField("LockTimeAudio");

            AudioClip[] principalTimeClips = new AudioClip[PrincipalPunishmentTimes.Count];

            for (int i = 0; i < principalTimeClips.Length; i++) {
                principalTimeClips[i] = AssetManager.GetAudioClip(PrincipalPunishmentTimes[i].SoundName);
            }

            lockTimeAudio.SetValue(GetComponent(runAlongSide.Type), principalTimeClips);
        }
    }
}
