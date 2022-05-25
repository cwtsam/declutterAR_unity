﻿using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class NativeLibAdapter : MonoBehaviour {

    const bool IS_GREEN = true;//tracks red if false

#if UNITY_EDITOR
    [DllImport("macPlugin")]
    private static extern int RecieveImage(byte[] bytes, int width, int height, bool isGreen);
    [DllImport("macPlugin")]
    private static extern void SetBackground(byte[] bytes, int width, int height, bool mirror, bool rotate);
    [DllImport("macPlugin")]
    private static extern void SetViewTextureFromUnity(IntPtr texture, int w, int h);
    [DllImport("macPlugin")]
    private static extern IntPtr GetRenderEventFunc();
#elif UNITY_IOS
    [DllImport("__Internal")]
    private static extern void RecieveImage(byte[] bytes, int width, int height, bool isGreen);
    [DllImport("__Internal")]
    private static extern void SetBackground(byte[] bytes, int width, int height, bool mirror, bool rotate);
    [DllImport("__Internal")]
    private static extern void SetViewTextureFromUnity(IntPtr texture, int w, int h);
    [DllImport("__Internal")]
    private static extern IntPtr GetRenderEventFunc();
#else
    [DllImport("OpenCVPlugin")]
    private static extern int RecieveImage(byte[] bytes, int width, int height, bool isGreen);
    [DllImport("OpenCVPlugin")]
    private static extern void SetBackground(byte[] bytes, int width, int height, bool mirror, bool rotate);
    [DllImport("OpenCVPlugin")]
    private static extern void SetViewTextureFromUnity(IntPtr texture, int w, int h);
    [DllImport("OpenCVPlugin")]
    private static extern IntPtr GetRenderEventFunc();
#endif

    public void PassViewTextureToPlugin(Texture2D tex) {
        // Pass texture pointer to the plugin
        SetViewTextureFromUnity(tex.GetNativeTexturePtr(), tex.width, tex.height);
    }

    public void StartOnRenderEvent() {
        StartCoroutine(CallPluginAtEndOfFrames());
    }

    IEnumerator CallPluginAtEndOfFrames() {
        yield return new WaitForSeconds(1f);
        while (true) {
            // Wait until all frame rendering is done
            yield return new WaitForEndOfFrame();
            GL.IssuePluginEvent(GetRenderEventFunc(), 1);
            GC.Collect();
        }
    }

    public void SendBackgroundImage(Texture2D tex, bool mirror, bool rotate) {
        SetBackground(tex.GetRawTextureData(), tex.width, tex.height, mirror, rotate);
    }

    public void SendImage(Texture2D tex) {
       RecieveImage(tex.GetRawTextureData(), tex.width, tex.height, IS_GREEN);
    }
}