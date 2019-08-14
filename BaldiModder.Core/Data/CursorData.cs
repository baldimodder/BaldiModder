namespace BaldiModder.Data {
    public struct CursorData {

        public bool UseSystemCursor { get; set; }

        public string CursorImage { get; set; }

        public Pivot Pivot { get; set; }

        public bool UseHardwareCursorIfSupported { get; set; }

    }
}
