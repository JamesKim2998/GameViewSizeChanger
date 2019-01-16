﻿using System;
using System.Collections;
using System.Collections.Generic;
using Kyusyukeigo.Helper;
using UnityEditor;
using UnityEngine;

namespace Syy.GameViewSizeChanger
{
    public class GameViewSizeChanger : EditorWindow
    {
        [MenuItem("Window/GameViewSizeChanger")]
        public static void Open()
        {
            GetWindow<GameViewSizeChanger>("GameViewSizeChanger");
        }

        private readonly GameViewSizeApplyer[] applyers = new GameViewSizeApplyer[]
        {
            new GameViewSizeApplyer() {Title="1080*1920", Aspect="9:16", Width=1080, Height=1920, },
            //iOS
            new GameViewSizeApplyer() {Title="iPhone4", Aspect="2:3", Width=640, Height=960, },
            new GameViewSizeApplyer() {Title="iPhone8", Aspect="9:16", Width=750, Height=1334, },
            new iPhoneXSizeApplyer() {Title="iPhoneX", Aspect="1:2", Width=1125, Height=2436, },
            new GameViewSizeApplyer() {Title="iPad", Aspect="3:4", Width=768, Height=1024, },
            // Android
            new GameViewSizeApplyer() {Title="GalaxyS8", Aspect="18.5：9", Width=1440, Height=2960, },

            //new SizeData() {Title="", Aspect="", Width=1, Height=1, },
        };

        Orientation orientation;
        int selectIndex = 0;

        void OnEnable()
        {
            int index = 0;
            foreach (var applyer in applyers)
            {
                applyer.orientation = orientation;
                applyer.OnChangeGameViewSize += OnChangeGameViewSize;
                if(applyer.IsSelect())
                {
                    selectIndex = index;
                }
                index++;
            }
        }

        void OnDisable()
        {
            foreach (var applyer in applyers)
            {
                applyer.OnChangeGameViewSize -= OnChangeGameViewSize;
            }
        }

        void OnGUI()
        {
            foreach (var applyer in applyers)
            {
                applyer.OnGUI();
            }

            using(var check = new EditorGUI.ChangeCheckScope())
            {
                orientation = (Orientation)EditorGUILayout.EnumPopup("Orientation", orientation);
                if(check.changed)
                {
                    foreach (var applyer in applyers)
                    {
                        applyer.orientation = orientation;
                    }
                }
            }

            var e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.UpArrow)
                {
                    selectIndex--;
                    if(selectIndex < 0)
                    {
                        selectIndex = applyers.Length -1;
                    }
                    applyers[selectIndex].Apply();
                    e.Use();
                }
                else if (e.keyCode == KeyCode.DownArrow)
                {
                    selectIndex++;
                    if (selectIndex > (applyers.Length - 1))
                    {
                        selectIndex = 0;
                    }
                    applyers[selectIndex].Apply();
                    e.Use();
                }
            }
        }

        void OnChangeGameViewSize()
        {
            Repaint();
            Focus();
            int index = 0;
            foreach (var applyer in applyers)
            {
                if (applyer.IsSelect())
                {
                    selectIndex = index;
                } else {
                    applyer.NoticeChangedOtherSize();
                }
                index++;
            }
        }
    }
}
