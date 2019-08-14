﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace BaldiModder.Data {
    public struct Pivot {

        internal Vector2 AsVector2 {
            get {
                return new Vector2(X, Y);
            }
        }

        public float X { get; set; }
        public float Y { get; set; }

        public Pivot(Vector2 vector2) : this(vector2.x, vector2.y) { }

        public Pivot(float x, float y) {
            X = x;
            Y = y;
        }

    }
}
