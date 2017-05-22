﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using static ColonistBarKF.CBKF;
using static ColonistBarKF.ColorsPSI;
using static ColonistBarKF.PSI.PSI;

namespace ColonistBarKF.PSI
{
    public static class PSIDrawer
    {
        public static void DrawIconOnColonist(Vector3 bodyPos, ref int num, Icons icon, Color color, float opacity)
        {
            if (WorldRendererUtility.WorldRenderedNow)
                return;

            Material material = PSIMaterials[icon];
            if (material == null)
            {
                Debug.LogError("Material = null.");
                return;
            }

            DrawIcon_posOffset(bodyPos, _iconPosVectorsPSI[num], material, color, opacity);
            num++;
        }

        public static void DrawIconOnBar(Rect psiRect, ref int num, Icons icon, Color color, float rectAlpha)
        {
            // only two columns visible
            if (num + 1 > ColBarSettings.IconsInColumn * 2)
                return;

            Material material = PSIMaterials[icon];

            if (material == null)
                return;

            DrawIcon_onBar(psiRect, _iconPosRectsBar[num], material, color, rectAlpha);
            num++;
        }


        public static void DrawIcon_posOffset(Vector3 bodyPos, Vector3 posOffset, Material material, Color color, float opacity)
        {
            color.a = opacity;
            material.color = color;
            Color guiColor = GUI.color;
            GUI.color = color;
            Vector2 vectorAtBody;

            float wordscale = WorldScale;
            if (PsiSettings.IconsScreenScale)
            {
                wordscale = 45f;
                vectorAtBody = bodyPos.MapToUIPosition();
                vectorAtBody.x += posOffset.x * 45f;
                vectorAtBody.y -= posOffset.z * 45f;
            }
            else
                vectorAtBody = (bodyPos + posOffset).MapToUIPosition();


            float num2 = wordscale * (PsiSettings.IconSizeMult * 0.5f);
            //On Colonist
            Rect position = new Rect(vectorAtBody.x, vectorAtBody.y, num2 * PsiSettings.IconSize, num2 * PsiSettings.IconSize);
            position.x -= position.width * 0.5f;
            position.y -= position.height * 0.5f;

            GUI.DrawTexture(position, material.mainTexture, ScaleMode.ScaleToFit, true);
            GUI.color = guiColor;
        }

        private static void DrawIcon_onBar(Rect rect, Vector3 posOffset, Material material, Color color, float rectAlpha)
        {
            color.a *= rectAlpha;
            Color GuiColor = GUI.color;
            GuiColor.a = rectAlpha;
            GUI.color = GuiColor;

            material.color = color;

            Rect iconRect = new Rect(rect);

            iconRect.width /= ColBarSettings.IconsInColumn;
            iconRect.height = iconRect.width;
            iconRect.x = rect.xMin;
            iconRect.y = rect.yMax;

            switch (ColBarSettings.ColBarPsiIconPos)
            {
                case Position.Alignment.Left:
                    iconRect.x = rect.xMax - iconRect.width;
                    iconRect.y = rect.yMax - iconRect.width;
                    //if (ColBarSettings.UseExternalMoodBar && ColBarSettings.MoodBarPos == Alignment.Left)
                    //    iconRect.x -= rect.width / 4;
                    break;
                case Position.Alignment.Right:
                    iconRect.x = rect.xMin;
                    iconRect.y = rect.yMax - iconRect.width;
                    //if (ColBarSettings.UseExternalMoodBar && ColBarSettings.MoodBarPos == Alignment.Right)
                    //    iconRect.x += rect.width / 4;
                    break;
                case Position.Alignment.Top:
                    iconRect.y = rect.yMax - iconRect.height;
                    // if (ColBarSettings.UseExternalMoodBar && ColBarSettings.MoodBarPos == Alignment.Top)
                    //     iconRect.y -= rect.height / 4;
                    break;
                case Position.Alignment.Bottom:
                    iconRect.y = rect.yMin;
                    //  if (ColBarSettings.UseExternalMoodBar && ColBarSettings.MoodBarPos == Alignment.Bottom)
                    //      iconRect.y += rect.height / 4;
                    break;

            }

            //    iconRect.x += (-0.5f * CBKF.ColBarSettings.IconDistanceX - 0.5f  * CBKF.ColBarSettings.IconOffsetX) * iconRect.width;
            //    iconRect.y -= (-0.5f * CBKF.ColBarSettings.IconDistanceY + 0.5f  * CBKF.ColBarSettings.IconOffsetY) * iconRect.height;

            iconRect.x += ColBarSettings.IconOffsetX * posOffset.x * iconRect.width;
            iconRect.y -= ColBarSettings.IconOffsetY * posOffset.z * iconRect.height;
            //On Colonist
            //iconRect.x -= iconRect.width * 0.5f;
            //iconRect.y -= iconRect.height * 0.5f;


            GUI.DrawTexture(iconRect, ColonistBarTextures.BGTexIconPSI);
            GUI.color = color;

            iconRect.x += iconRect.width * 0.1f;
            iconRect.y += iconRect.height * 0.1f;
            iconRect.width *= 0.8f;
            iconRect.height *= 0.8f;

            GUI.DrawTexture(iconRect, material.mainTexture, ScaleMode.ScaleToFit, true);
            GUI.color = GuiColor;

        }


        public static void DrawIcon_FadeRedAlertToNeutral(Vector3 bodyPos, ref int num, Icons icon, float v, float opacity)
        {
            v = v * 0.9f; // max settings according to neutral icon
            DrawIconOnColonist(bodyPos, ref num, icon, new Color(0.9f, v, v), opacity);
        }

        public static void DrawIcon_FadeRedAlertToNeutral(Rect rect, ref int num, Icons icon, float v, float rectAlpha)
        {
            v = v * 0.9f; // max settings according to neutral icon
            DrawIconOnBar(rect, ref num, icon, new Color(0.9f, v, v, 1f), rectAlpha);
        }

        public static void DrawIcon_FadeFloatWithTwoColors(Vector3 bodyPos, ref int num, Icons icon, float v, Color c1, Color c2, float opacity)
        {
            DrawIconOnColonist(bodyPos, ref num, icon, Color.Lerp(c1, c2, v), opacity);
        }

        public static void DrawIcon_FadeFloatWithTwoColors(Rect rect, ref int num, Icons icon, float v, Color c1, Color c2, float rectAlpha)
        {
            DrawIconOnBar(rect, ref num, icon, Color.Lerp(c1, c2, v), rectAlpha);
        }

        public static void DrawIcon_FadeFloatWithThreeColors(Vector3 bodyPos, ref int num, Icons icon, float v, Color c1, Color c2, Color c3, float opacity)
        {
            DrawIconOnColonist(bodyPos, ref num, icon, v < 0.5 ? Color.Lerp(c1, c2, v * 2f) : Color.Lerp(c2, c3, (float)((v - 0.5) * 2.0)), opacity);
        }

        public static void DrawIcon_FadeFloatWithThreeColors(Rect rect, ref int num, Icons icon, float v, Color c1, Color c2, Color c3, float rectAlpha)
        {
            DrawIconOnBar(rect, ref num, icon, v < 0.5 ? Color.Lerp(c1, c2, v * 2f) : Color.Lerp(c2, c3, (float)((v - 0.5) * 2.0)), rectAlpha);
        }

        public static void DrawIcon_FadeFloatWithFourColorsHB(Vector3 bodyPos, ref int num, Icons icon, float v, Color c1, Color c2, Color c3, Color c4, float opacity)
        {
            if (v > 0.8f)
            {
                DrawIconOnColonist(bodyPos, ref num, icon, Color.Lerp(c2, c1, (v - 0.8f) * 5), opacity);
            }
            else if (v > 0.6f)
            {
                DrawIconOnColonist(bodyPos, ref num, icon, Color.Lerp(c3, c2, (v - 0.6f) * 5), opacity);
            }
            else if (v > 0.4f)
            {
                DrawIconOnColonist(bodyPos, ref num, icon, Color.Lerp(c4, c3, (v - 0.4f) * 5), opacity);
            }
            else
            {
                DrawIconOnColonist(bodyPos, ref num, icon, c4, opacity);
            }
        }

        public static void DrawIcon_FadeFloatWithFourColorsHB(Rect rect, ref int num, Icons icon, float v, Color c1, Color c2, Color c3, Color c4, float rectAlpha)
        {
            if (v > 0.8f)
            {
                DrawIconOnBar(rect, ref num, icon, Color.Lerp(c2, c1, (v - 0.8f) * 5), rectAlpha);
            }
            else if (v > 0.6f)
            {
                DrawIconOnBar(rect, ref num, icon, Color.Lerp(c3, c2, (v - 0.6f) * 5), rectAlpha);
            }
            else if (v > 0.4f)
            {
                DrawIconOnBar(rect, ref num, icon, Color.Lerp(c4, c3, (v - 0.4f) * 5), rectAlpha);
            }
            else
            {
                DrawIconOnBar(rect, ref num, icon, c4, rectAlpha);
            }
        }

        public static void DrawIcon_FadeFloatFiveColors(Vector3 bodyPos, ref int num, Icons icon, float v, Color c1, Color c2, Color c3, Color c4, Color c5, float opacity)
        {
            if (v < 0.2f)
            {
                DrawIconOnColonist(bodyPos, ref num, icon, Color.Lerp(c1, c2, v * 5), opacity);
            }
            else if (v < 0.4f)
            {
                DrawIconOnColonist(bodyPos, ref num, icon, Color.Lerp(c2, c3, (v - 0.2f) * 5), opacity);
            }
            else if (v < 0.6f)
            {
                DrawIconOnColonist(bodyPos, ref num, icon, Color.Lerp(c3, c4, (v - 0.4f) * 5), opacity);
            }
            else if (v < 0.8f)
            {
                DrawIconOnColonist(bodyPos, ref num, icon, Color.Lerp(c4, c5, (v - 0.6f) * 5), opacity);
            }
            else
            {
                DrawIconOnColonist(bodyPos, ref num, icon, c5, opacity);
            }
        }

        public static void DrawIcon_FadeFloatFiveColors(Rect rect, ref int num, Icons icon, float v, Color c1, Color c2, Color c3, Color c4, Color c5, float rectAlpha)
        {
            if (v < 0.2f)
            {
                DrawIconOnBar(rect, ref num, icon, Color.Lerp(c1, c2, v * 5), rectAlpha);
            }
            else if (v < 0.4f)
            {
                DrawIconOnBar(rect, ref num, icon, Color.Lerp(c2, c3, (v - 0.2f) * 5), rectAlpha);
            }
            else if (v < 0.6f)
            {
                DrawIconOnBar(rect, ref num, icon, Color.Lerp(c3, c4, (v - 0.4f) * 5), rectAlpha);
            }
            else if (v < 0.8f)
            {
                DrawIconOnBar(rect, ref num, icon, Color.Lerp(c4, c5, (v - 0.6f) * 5), rectAlpha);
            }
            else
            {
                DrawIconOnBar(rect, ref num, icon, c5, rectAlpha);
            }
        }

    }
}