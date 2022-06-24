using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace HairDye
{
    public class WindowGrid
    {
        float baseX;
        float baseY;
        float cellWidth;
        float cellHeight;
        public float cellCountHor;
        public float cellCountVert;

        public WindowGrid(int w, int h)
        {
            cellCountHor = w;
            cellCountVert = h;
        }

        public void set(Rect baseRect)
        {
            baseX = baseRect.x;
            baseY = baseRect.y;
            cellWidth = baseRect.width / cellCountHor;
            cellHeight = baseRect.height / cellCountVert;
        }

        Rect tempRect = new Rect(0, 0, 0, 0);
        public Rect rect(float x, float y, float w, float h)
        {
            tempRect.x = baseX + cellWidth * x;
            tempRect.y = baseY + cellHeight * y;
            tempRect.width = cellWidth * w;
            tempRect.height = cellHeight * h;
            return tempRect;
        }
    }
    
    public class WindowChooseHairColor : Window
    {
        ThingWithComps dye;
        Pawn pawn;
        ThingCompHairDye comp;
        RenderTexture portrait;
        bool needRefresh = true;
        WindowGrid grid;
        static WidgetColorSelector colorEditA = new WidgetColorSelector();
        static WidgetColorSelector colorEditB = new WidgetColorSelector();
        bool gradientHairIsAvailable;

        PawnHairData hairData = new PawnHairData();
        PawnHairData originalHairData = new PawnHairData();

        public WindowChooseHairColor(ThingCompHairDye comp, ThingWithComps dye, Pawn pawn)
        {
            this.dye = dye;
            this.pawn = pawn;
            this.comp = comp;

            hairData.Read(pawn);
            originalHairData.Read(pawn);

            gradientHairIsAvailable = GradientHairApi.GradientHairIsAvailable();

            grid = new WindowGrid(4, gradientHairIsAvailable ? 7 : 6 );
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(grid.cellCountHor * 100, grid.cellCountVert * 100);
            }
        }

        public RenderTexture PawnPortrait(Pawn pawn, Rect portraitRect)
        {
            if (!needRefresh) return portrait;
            needRefresh = false;

            hairData.Write(pawn);
            pawn.Drawer.renderer.graphics.ResolveAllGraphics();

            portrait = new RenderTexture((int) portraitRect.width, (int) portraitRect.height, 24);

            Find.PawnCacheRenderer.RenderPawn(pawn, portrait, default(Vector3), 1f, 0f, Rot4.South, pawn.health.hediffSet.HasHead, true, true, true, true, default(Vector3));

            originalHairData.Write(pawn);
            pawn.Drawer.renderer.graphics.ResolveAllGraphics();

            return portrait;
        }


        public override void DoWindowContents(Rect rect)
        {
            grid.set(rect);

            Rect portraitRect = grid.rect(0, 0, 4, 4);
            GUI.BeginGroup(rect);
            GUI.DrawTexture(portraitRect, PawnPortrait(pawn, portraitRect));
            GUI.EndGroup();

            if (colorEditA.SelectColor(grid.rect(0, 4, 4, 1), "HairDyeHairColor".Translate(), ref hairData.colorA))
            {
                needRefresh = true;
            }

            if (gradientHairIsAvailable)
            {
                if (colorEditA.SelectColor(grid.rect(0, 5, 4, 1), "HairDyeGradientColor".Translate(), ref hairData.colorB, ref hairData.gradientHairEnabled))
                {
                    needRefresh = true;
                }
            }


            if (Widgets.ButtonText(grid.rect(0, grid.cellCountVert - 1, 2, 1).ContractedBy(20), "Close".Translate()))
            {
                Close();
            }
            if (Widgets.ButtonText(grid.rect(2, grid.cellCountVert-1, 2, 1).ContractedBy(20), "HairDyeApply".Translate()))
            {
                comp.hairData = hairData;

                Job job = new Job(JobDefOf.HairDyeChangeHairColor, dye, pawn);
                job.count = 1;

                pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                Close();
            }
        }

    }
}
