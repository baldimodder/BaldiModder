using System;

namespace BaldiModder.Runtime {
    public interface ILoadingScreenHandler {

        void SetText(string text);
        void SetLoadingText(string text);

        void HandleException(Exception e);
        void FinishedLoading();

    }
}
