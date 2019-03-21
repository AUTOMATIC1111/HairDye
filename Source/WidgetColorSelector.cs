using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace HairDye
{
    public class WidgetColorSelector
    {
        public WidgetColorSelector() {
        }

        protected static bool CloseEnough(float a, float b)
        {
            return a > b - 0.0001f && a < b + 0.0001f;
        }
        public bool SelectColor(Rect rect, string caption, ref Color color)
        {
            bool e = true;
            return SelectColor(rect, false, caption, ref color, ref e);
        }
        public bool SelectColor(Rect rect, string caption, ref Color color, ref bool enable)
        {
            return SelectColor(rect, true, caption, ref color, ref enable);
        }

        public bool SelectColor(Rect rect, bool enableSelection, string caption, ref Color color, ref bool enable)
        {
            bool res = false;

            GUI.BeginGroup(rect);

            Text.Font = GameFont.Medium;
            if (enableSelection)
            {
                bool toggle = enable;
                Widgets.CheckboxLabeled(new Rect(18, 2, rect.width - 18 * 2, 33), caption, ref enable);
                if (toggle != enable)
                {
                    res = true;
                }
            }
            else
            {
                Widgets.Label(new Rect(18, 2, rect.width - 18 * 2, 33), caption);
            }
            Text.Font = GameFont.Small;

            if (enable)
            {
                Rect rectColor = new Rect(17, 36, 46, 46);
                GUI.color = Color.white;
                GUI.DrawTexture(rectColor, BaseContent.WhiteTex);
                GUI.color = color;
                GUI.DrawTexture(GenUI.ContractedBy(rectColor, 1f), BaseContent.WhiteTex);

                GUI.color = Color.red;
                float r = color.r;
                float vr = GUI.HorizontalSlider(new Rect(72, 38, 276, 9), color.r, 0f, 1f);

                GUI.color = Color.green;
                float g = color.g;
                float vg = GUI.HorizontalSlider(new Rect(72, 54, 276, 9), color.g, 0f, 1f);

                GUI.color = Color.blue;
                float b = color.b;
                float vb = GUI.HorizontalSlider(new Rect(72, 70, 276, 9), color.b, 0f, 1f);

                if (!CloseEnough(vr, r) || !CloseEnough(vg, g) || !CloseEnough(vb, b))
                {
                    color = new Color(vr, vg, vb);
                    res = true;
                }
            }


            /*
            for (int i = 0; i < swatches.Length; i++) {
                float x = 256 + 25 * (i % 4);
                float y = 12 + 25 * (i / 4);
                Rect rectSwatch = new Rect(x, y, 20, 20);

                GUI.color = Color.white;
                GUI.DrawTexture(rectSwatch, BaseContent.WhiteTex);
                GUI.color = swatches[i];
                GUI.DrawTexture(GenUI.ContractedBy(rectSwatch, 1f), BaseContent.WhiteTex);

                if (Widgets.ButtonInvisible(rectSwatch, true))
                {
                    if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
                    {
                        swatches[i] = color;
                    }
                    else
                    {
                        color = swatches[i];
                        res = true;
                    }
                }
            }*/

            GUI.color = Color.white;

            GUI.EndGroup();

            return res;
        }
    }
}
