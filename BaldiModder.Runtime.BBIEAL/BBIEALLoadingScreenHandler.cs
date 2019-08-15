using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace BaldiModder.Runtime.BBIEAL {
    public class BBIEALLoadingScreenHandler : LoadingScreenHandler {

        public Text text;

        public BBIEALLoadingScreenHandler(LoadingScreen loadingScreen) : base(loadingScreen) {
            text = loadingScreen.GetComponentInChildren<Text>();
        }

        public override void SetText(string value) {
            text.text = value;
        }

        public override void SetLoadingText(string value) {
            if (String.IsNullOrEmpty(value) || !Master.VerboseMode) SetText("Loading...");
            else SetText($"Loading {value}...");
        }

        public override void HandleException(Exception e) {
            text.fontSize = 24;
            SetText($"An error has occured:\n{e.Message}\n\nPress R to retry. Press anything else to quit.");
        }

        public override void FinishedLoading() {
            try {
                Master.RuntimeAssembly = GetType().Assembly;

                SetText(AssetManager.Config.WarningText);

                GameObject controller = new GameObject("BaldiModder");
                controller.AddComponent<GameController>();

                AssetManager.UpdateCursor();
                Cursor.visible = true;

                loadingScreen.gameObject.AddComponent(Master.VersionData.GetType("WarningScreen"));
            } catch (Exception e) {
                HandleException(e);
            }
        }
    }
}
