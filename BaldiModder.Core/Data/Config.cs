using System;
using System.Collections.Generic;

namespace BaldiModder.Data {
    [Serializable]
    public struct Config {

        public string WarningText { get; set; }

        public TitleScreenMusicData TitleScreenMusic { get; set; }

        public Dictionary<string, string> SoundReplacements { get; set; }

        public float BaldiSpeedMultiplier { get; set; }

        public GameOverScreenData GameOver { get; set; }

        public BaldiAnimationOverride BaldiAnimation { get; set; }

        public PlaytimeAnimationOverride PlaytimeAnimation { get; set; }

        public BaldiTutorData BaldiTutor { get; set; }

        public CursorData Cursor { get; set; }

    }
}
