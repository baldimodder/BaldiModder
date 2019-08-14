namespace BaldiModder.Data {
    public class EntityData {

        public bool ForcePivot { get; set; }

        //The reason I am using floats instead of Vector2s is because I don't want to include the "normalized" value in Vector2s.
        public float PivotX { get; set; }
        public float PivotY { get; set; }       

    }
}
