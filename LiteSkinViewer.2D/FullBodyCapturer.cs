﻿using LiteSkinViewer.Shared.Interfaces;
using LiteSkinViewer2D.Extensions;
using LiteSkinViewer3D.Shared.Enums;
using LiteSkinViewer3D.Shared.Helpers;
using SkiaSharp;

namespace LiteSkinViewer2D;

public sealed class FullBodyCapturer : ICapturer {

    public static readonly FullBodyCapturer Default = new();

    //public SKBitmap Capture(SKBitmap image, SkinType? type = null) {
    //    using var image1 = new SKBitmap(136, 266);
    //    using var image2 = new SKBitmap(136 * Scale, 266 * Scale);

    //    var skintype = type ?? SkinHelper.DetectSkin(image);

    //    //head
    //    image1.ExtractSubsetWithFillImage(image, 4 + 8 * 4, 4, 8, 8, 8, 8, 8, 8);
    //    //body
    //    image1.ExtractSubsetWithFillImage(image, 4 + 8 * 4, 4 + 8 * 8, 20, 20, 8, 12, 8, 8);

    //    //right hand
    //    if (skintype is SkinType.Slim) {
    //        image1.ExtractSubsetWithFillImage(image, 4 + 1 * 8, 4 + 8 * 8, 44, 20, 3, 12, 8, 8);
    //    } else {
    //        image1.ExtractSubsetWithFillImage(image, 4, 4 + 8 * 8, 44, 20, 4, 12, 8, 8);
    //    }

    //    //left hand
    //    if (skintype == SkinType.Slim) {
    //        image1.ExtractSubsetWithFillImage(image, 4 + 12 * 8, 4 + 8 * 8, 36, 52, 3, 12, 8, 8);
    //    } else {
    //        if (skintype is SkinType.Legacy) {
    //            //旧版镜像
    //            for (int i = 3; i >= 0; i--) {
    //                for (int j = 0; j < 12; j++) {
    //                    var pix = image.GetPixel(i + 44, j + 20);
    //                    image1.FillRegion(4 + 12 * 8 + i * 8, 4 + 8 * 8 + j * 8, 8, 8, pix);
    //                }
    //            }
    //        } else {
    //            image1.ExtractSubsetWithFillImage(image, 4 + 12 * 8, 4 + 8 * 8, 36, 52, 4, 12, 8, 8);
    //        }
    //    }

    //    //right leg
    //    image1.ExtractSubsetWithFillImage(image, 4 + 4 * 8, 4 + 20 * 8, 4, 20, 4, 12, 8, 8);

    //    //left leg
    //    if (skintype is SkinType.Legacy) {
    //        //旧版镜像
    //        for (int i = 3; i >= 0; i--) {
    //            for (int j = 0; j < 12; j++) {
    //                var pix = image.GetPixel(i + 4, j + 20);
    //                image1.FillRegion(4 + 8 * 8 + i * 8, 4 + 20 * 8 + j * 8, 8, 8, pix);
    //            }
    //        }
    //    } else {
    //        image1.ExtractSubsetWithFillImage(image, 4 + 8 * 8, 4 + 20 * 8, 20, 52, 4, 12, 8, 8);
    //    }

    //    if (skintype is SkinType.Classic or SkinType.Slim) {
    //        //body over
    //        image1.ExtractSubsetWithFillImageMix(image, 4 * 8, 8 * 8 - 2, 20, 36, 8, 12, 9, 9);
    //    }
    //    //head top
    //    image1.ExtractSubsetWithFillImageMix(image, 4 * 9 - 4, 0, 40, 8, 8, 8, 9, 9);
    //    if (skintype is SkinType.Slim) {
    //        //top
    //        image1.ExtractSubsetWithFillImageMix(image, 1 * 8 + 1, 8 * 8 + 2, 44, 36, 3, 12, 9, 9);
    //        //top
    //        image1.ExtractSubsetWithFillImageMix(image, 12 * 8 + 4, 8 * 8 + 2, 52, 52, 3, 12, 9, 9);
    //    } else if (skintype is SkinType.Classic) {
    //        //top
    //        image1.ExtractSubsetWithFillImageMix(image, 0, 8 * 8 + 2, 44, 36, 4, 12, 9, 9);
    //        //top
    //        image1.ExtractSubsetWithFillImageMix(image, 12 * 8 + 4, 8 * 8 + 2, 52, 52, 4, 12, 9, 9);
    //    }
    //    if (skintype is SkinType.Classic or SkinType.Slim) {
    //        //top
    //        image1.ExtractSubsetWithFillImageMix(image, 4 * 8 + 2, 20 * 8 - 2, 4, 36, 4, 12, 9, 9);
    //        //top
    //        image1.ExtractSubsetWithFillImageMix(image, 8 * 8 + 2, 20 * 8 - 2, 4, 52, 4, 12, 9, 9);
    //    }

    //    for (int i = 0; i < 136 * Scale; i++) {
    //        for (int j = 0; j < 266 * Scale; j++) {
    //            image2.SetPixel(i, j, image1.GetPixel(i / Scale, j / Scale));
    //        }
    //    }

    //    return image2.Copy();
    //}

    public unsafe SKBitmap Capture(SKBitmap skin, int scale = 8) {
        const int RAW_W = 136, RAW_H = 266;
        var skintype = SkinHelper.DetectSkin(skin);
        var frame = new SKBitmap(RAW_W, RAW_H);

        skin.CopyBlock(frame, 8, 8, 8, 8, 4 + 8 * 4, 4, 8, 8);
        skin.CopyBlock(frame, 20, 20, 8, 12, 4 + 8 * 4, 4 + 8 * 8, 8, 8);

        if (skintype == SkinType.Slim) {
            skin.CopyBlock(frame, 44, 20, 3, 12, 4 + 1 * 8, 4 + 8 * 8, 8, 8);
            skin.CopyBlock(frame, 36, 52, 3, 12, 4 + 12 * 8, 4 + 8 * 8, 8, 8);
        } else {
            skin.CopyBlock(frame, 44, 20, 4, 12, 4, 4 + 8 * 8, 8, 8);
            if (skintype == SkinType.Legacy)
                skin.CopyBlockMirror(frame, 44, 20, 4, 12, 4 + 12 * 8, 4 + 8 * 8, 8);
            else
                skin.CopyBlock(frame, 36, 52, 4, 12, 4 + 12 * 8, 4 + 8 * 8, 8, 8);
        }

        skin.CopyBlock(frame, 4, 20, 4, 12, 4 + 4 * 8, 4 + 20 * 8, 8, 8);
        if (skintype == SkinType.Legacy)
            skin.CopyBlockMirror(frame, 4, 20, 4, 12, 4 + 8 * 8, 4 + 20 * 8, 8);
        else
            skin.CopyBlock(frame, 20, 52, 4, 12, 4 + 8 * 8, 4 + 20 * 8, 8, 8);

        if (skintype is SkinType.Classic or SkinType.Slim) {
            skin.CopyBlockBlend(frame, 20, 36, 8, 12, 4 * 8, 8 * 8 - 2, 9, 9);
            skin.CopyBlockBlend(frame, 40, 8, 8, 8, 4 * 9 - 4, 0, 9, 9);
            skin.CopyBlockBlend(frame, 4, 36, 4, 12, 4 * 8 + 2, 20 * 8 - 2, 9, 9);
            skin.CopyBlockBlend(frame, 4, 52, 4, 12, 8 * 8 + 2, 20 * 8 - 2, 9, 9);
        }

        if (skintype == SkinType.Slim) {
            skin.CopyBlockBlend(frame, 44, 36, 3, 12, 1 * 8 + 1, 8 * 8 + 2, 9, 9);
            skin.CopyBlockBlend(frame, 52, 52, 3, 12, 12 * 8 + 4, 8 * 8 + 2, 9, 9);
        } else if (skintype == SkinType.Classic) {
            skin.CopyBlockBlend(frame, 44, 36, 4, 12, 0, 8 * 8 + 2, 9, 9);
            skin.CopyBlockBlend(frame, 52, 52, 4, 12, 12 * 8 + 4, 8 * 8 + 2, 9, 9);
        }

        return frame.ResizeNearest(scale);
    }
}