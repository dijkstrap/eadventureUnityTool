﻿using System;
using UnityEngine;
using System.Collections;

public class ScenesWindowAppearance : LayoutWindow, DialogReceiverInterface
{
    private enum AssetType
    {
        BACKGROUND,
        FOREGROUND,
        MUSIC
    };

    private Texture2D addTexture = null;
    private Texture2D duplicateImg = null;
    private Texture2D clearImg = null;

    private Texture2D backgroundPreview = null;
    private static float windowWidth, windowHeight;
    private static Rect previewRect, appearanceTableRect, propertiesTable, rightPanelRect;

    private static GUISkin defaultSkin;
    private static GUISkin noBackgroundSkin;

    private Vector2 scrollPosition;

    private string backgroundPath = "";
    private string foregroundMaskPath = "";
    private string musicPath = "";

    public ScenesWindowAppearance(Rect aStartPos, GUIContent aContent, GUIStyle aStyle, params GUILayoutOption[] aOptions)
        : base(aStartPos, aContent, aStyle, aOptions)
    {
        clearImg = (Texture2D)Resources.Load("EAdventureData/img/icons/deleteContent", typeof(Texture2D));
        addTexture = (Texture2D)Resources.Load("EAdventureData/img/icons/addNode", typeof(Texture2D));
        duplicateImg = (Texture2D)Resources.Load("EAdventureData/img/icons/duplicateNode", typeof(Texture2D));

        windowWidth = aStartPos.width;
        windowHeight = aStartPos.height;

        if(GameRources.GetInstance().selectedSceneIndex >= 0)
            backgroundPath =
                Controller.getInstance().getSelectedChapterDataControl().getScenesList().getScenes()[
                    GameRources.GetInstance().selectedSceneIndex].getPreviewBackground();
        //foregroundMaskPath = Controller.getInstance().getSelectedChapterDataControl().getScenesList().getScenes()[GameRources.GetInstance().selectedSceneIndex].
        //musicPath = "";
        if(backgroundPath != null && !backgroundPath.Equals(""))
            backgroundPreview = (Texture2D)Resources.Load(backgroundPath.Substring(0, backgroundPath.LastIndexOf(".")), typeof(Texture2D));

        noBackgroundSkin = (GUISkin)Resources.Load("Editor/EditorNoBackgroundSkin", typeof(GUISkin));

        appearanceTableRect = new Rect(0f, 0.1f * windowHeight, 0.9f * windowWidth, 0.2f * windowHeight);
        rightPanelRect = new Rect(0.9f * windowWidth, 0.1f * windowHeight, 0.08f * windowWidth, 0.2f * windowHeight);
        propertiesTable = new Rect(0f, 0.3f * windowHeight, 0.95f * windowWidth, 0.2f * windowHeight);
        previewRect = new Rect(0f, 0.5f * windowHeight, windowWidth, windowHeight * 0.45f);
    }


    public override void Draw(int aID)
    {
        GUILayout.BeginArea(appearanceTableRect);
        GUILayout.BeginHorizontal();
        GUILayout.Box("Appearance block", GUILayout.Width(windowWidth * 0.44f));
        GUILayout.Box("Conditions", GUILayout.Width(windowWidth * 0.44f));
        GUILayout.EndHorizontal();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        // Appearance table
        for (int i = 0; i < Controller.getInstance().getSelectedChapterDataControl().getScenesList().getScenes()[GameRources.GetInstance().selectedSceneIndex].getActiveAreasList().getActiveAreasList().Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Controller.getInstance().getSelectedChapterDataControl().getScenesList().getScenes()[
                GameRources.GetInstance().selectedSceneIndex].getActiveAreasList().getActiveAreasList()[i].getId(), GUILayout.Width(windowWidth * 0.44f));
            GUILayout.Label(Controller.getInstance().getSelectedChapterDataControl().getScenesList().getScenes()[GameRources.GetInstance().selectedSceneIndex].getActiveAreasList().getActiveAreasList()[i].getConditions
                ().size().ToString(), GUILayout.Width(windowWidth * 0.44f));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        
        /*
        * Right panel
        */
        GUILayout.BeginArea(rightPanelRect);
        GUI.skin = noBackgroundSkin;
        if (GUILayout.Button(addTexture, GUILayout.MaxWidth(0.08f * windowWidth)))
        {
            Debug.Log("ADD");
        }
        if (GUILayout.Button(duplicateImg, GUILayout.MaxWidth(0.08f * windowWidth)))
        {
            Debug.Log("Duplicate");
        }
        if (GUILayout.Button(clearImg, GUILayout.MaxWidth(0.08f * windowWidth)))
        {
            Debug.Log("Clear");
        }
        GUI.skin = defaultSkin;
        GUILayout.EndArea();

        GUILayout.Space(30);

        GUILayout.BeginArea(propertiesTable);
        // Background chooser
        GUILayout.Label("Background image of the scene");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(clearImg, GUILayout.Width(0.1f * windowWidth)))
        {
            backgroundPath = "";
        }
        GUILayout.Box(backgroundPath, GUILayout.Width(0.7f * windowWidth));
        if (GUILayout.Button("Select", GUILayout.Width(0.19f * windowWidth)))
        {
            ShowAssetChooser(AssetType.BACKGROUND);
        }
        GUILayout.EndHorizontal();

        // Foreground chooser
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(clearImg, GUILayout.Width(0.1f * windowWidth)))
        {
            foregroundMaskPath = "";
        }
        GUILayout.Box(foregroundMaskPath, GUILayout.Width(0.7f * windowWidth));
        if (GUILayout.Button("Select", GUILayout.Width(0.19f * windowWidth)))
        {
            ShowAssetChooser(AssetType.FOREGROUND);
        }
        GUILayout.EndHorizontal();

        // Music chooser
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(clearImg, GUILayout.Width(0.1f * windowWidth)))
        {
            musicPath = "";
        }
        GUILayout.Box(musicPath, GUILayout.Width(0.7f * windowWidth));
        if (GUILayout.Button("Select", GUILayout.Width(0.19f * windowWidth)))
        {
            ShowAssetChooser(AssetType.MUSIC);
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();

        if (backgroundPath != "")
        {
            GUI.DrawTexture(previewRect, backgroundPreview, ScaleMode.ScaleToFit);
        }
    }

    void ShowAssetChooser(AssetType type)
    {
        switch (type)
        {
            case AssetType.BACKGROUND:
                ImageFileOpenDialog backgroundDialog =
                (ImageFileOpenDialog)ScriptableObject.CreateInstance(typeof(ImageFileOpenDialog));
                backgroundDialog.Init(this, BaseFileOpenDialog.FileType.SCENE_BACKGROUND);
                break;
            case AssetType.FOREGROUND:
                ImageFileOpenDialog foregroundDialgo =
                (ImageFileOpenDialog)ScriptableObject.CreateInstance(typeof(ImageFileOpenDialog));
                foregroundDialgo.Init(this, BaseFileOpenDialog.FileType.SCENE_FOREGROUND);
                break;
            case AssetType.MUSIC:
                MusicFileOpenDialog musicDialog =
                (MusicFileOpenDialog)ScriptableObject.CreateInstance(typeof(MusicFileOpenDialog));
                musicDialog.Init(this, BaseFileOpenDialog.FileType.SCENE_MUSIC);
                break;
        }

    }

    public void OnDialogOk(string message, object workingObject = null, object workingObjectSecond = null)
    {
        switch ((BaseFileOpenDialog.FileType)workingObject)
        {
            case BaseFileOpenDialog.FileType.SCENE_BACKGROUND:
                backgroundPath = message;
                break;
            case BaseFileOpenDialog.FileType.SCENE_FOREGROUND:
                foregroundMaskPath = message;
                break;
            case BaseFileOpenDialog.FileType.SCENE_MUSIC:
                musicPath = message;
                break;
            default:
                break;
        }
    }

    public void OnDialogCanceled(object workingObject = null)
    {
        Debug.Log("Wiadomość nie OK" );
    }
}
