using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaldiModder.Runtime {
    public abstract class LoadingScreenHandler : ILoadingScreenHandler {

        protected LoadingScreen loadingScreen;

        protected LoadingScreenHandler(LoadingScreen loadingScreen) {
            this.loadingScreen = loadingScreen;
        }

        public abstract void SetText(string text);
        public abstract void SetLoadingText(string text);

        public abstract void HandleException(Exception e);
        public abstract void FinishedLoading();
    }
}
