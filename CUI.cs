using Facepunch.Extend;
using Oxide.Core.Libraries;
using Oxide.Ext.CarbonAliases;
using Oxide.Game.Rust.Cui;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Oxide.Ext.CarbonAliases.CUI;
using static Oxide.Ext.CarbonAliases.CUI.Handler;

namespace Oxide.Ext.CarbonAliases
{
    public class CUI : Library, IDisposable
    {
        public Handler Manager { get; private set; }
        
        public LUI v2 { get; }

        internal int _currentId = 0; 

        public enum ClientPanels
        {
            Overall,
            Overlay,
            OverlayNonScaled,
            Hud,
            HudMenu,
            Under,
            UnderNonScaled,
            Inventory,
            TechTree,
            Crafting,
            Contacts,
            Clans,
            Map
        }

        public string GetClientPanel(ClientPanels panel)
        {
            return panel switch
            {
                ClientPanels.Overall => "Overall",
                ClientPanels.OverlayNonScaled => "OverlayNonScaled",
                ClientPanels.Hud => "Hud",
                ClientPanels.HudMenu => "Hud.Menu",
                ClientPanels.Under => "Under",
                ClientPanels.UnderNonScaled => "UnderNonScaled",
                ClientPanels.Inventory => "Inventory",
                ClientPanels.TechTree => "TechTree",
                ClientPanels.Crafting => "Crafting",
                ClientPanels.Contacts => "Contacts",
                ClientPanels.Clans => "Clans",
                ClientPanels.Map => "Map",
                _ => "Overlay",
            };
        }

        public CUI(Handler manager)
        {
            Manager = manager;
            v2 = new LUI(this);
        }

        #region Update

        public Handler.UpdatePool UpdatePool() => new UpdatePool();

        internal string AppendId()
        {
            _currentId++;
            return $"CarbonAliasesID_{_currentId}";
        }

        internal static string ProcessColor(string color)
        {
            if (color.StartsWith("#")) return CUI.HexToRustColor(color);

            return color;
        }

        public static string HexToRustColor(string hexColor, float? alpha = null, bool includeAlpha = true)
        {
            if (!ColorUtility.TryParseHtmlString(hexColor, out var color))
            {
                return $"1 1 1{(includeAlpha ? $" {alpha.GetValueOrDefault(1)}" : "")}";
            }

            return $"{color.r} {color.g} {color.b}{(includeAlpha ? $" {alpha ?? color.a}" : "")}";
        }

        #endregion



        #region Methods

        public CuiElementContainer CreateContainer(string panel, string color = "0 0 0 0", float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, ClientPanels parent = ClientPanels.Overlay, string destroyUi = null)
        {
            CuiElementContainer container = new CuiElementContainer();
            CuiElement element = new CuiElement
            {
                Name = panel,
                Parent = GetClientPanel(parent),
                Components =
                {
                    new CuiImageComponent
                    {
                        Color = ProcessColor(color),
                        FadeIn = fadeIn
                    },
                    new CuiRectTransformComponent
                    {
                        AnchorMin = $"{xMin} {yMin}",
                        AnchorMax = $"{xMax} {yMax}",
                        OffsetMin = $"{OxMin} {OyMin}",
                        OffsetMax = $"{OxMax} {OyMax}"
                    }
                },
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
            };
            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());
            container.Add(element);
            return container;
        }

        public Pair<string, CuiElement> CreatePanel(CuiElementContainer container, string parent, string color, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, bool blur = false, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (string.IsNullOrEmpty(id))
                id = AppendId();
            CuiElement element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            CuiImageComponent imageComponent = new CuiImageComponent();
            if (material != null) imageComponent.Material = material;
            imageComponent.Color = color;
            imageComponent.FadeIn = fadeIn;
            if (blur) imageComponent.Material = "assets/content/ui/uibackgroundblur.mat";
            element.Components.Add(imageComponent);
            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());
            if (outlineColor != null)
            {
                CuiOutlineComponent outline = new CuiOutlineComponent();
                outline.Color = ProcessColor(outlineColor);
                outline.Distance = outlineDistance;
                outline.UseGraphicAlpha = outlineUseGraphicAlpha;
                element.Components.Add(outline);
            }
            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }

        public Pair<string, CuiElement> CreateText(CuiElementContainer container, string parent, string color, string text, int size, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, VerticalWrapMode verticalOverflow = VerticalWrapMode.Overflow, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            var label = new CuiTextComponent();
            label.Text = string.IsNullOrEmpty(text) ? string.Empty : text;
            label.FontSize = size;
            label.Font = GetFont(font);
            label.Align = align;
            label.Color = ProcessColor(color);
            label.FadeIn = fadeIn;
            label.VerticalOverflow = verticalOverflow;
            element.Components.Add(label);

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());

            if (outlineColor != null)
            {
                CuiOutlineComponent outline = new CuiOutlineComponent();
                outline.Color = ProcessColor(outlineColor);
                outline.Distance = outlineDistance;
                outline.UseGraphicAlpha = outlineUseGraphicAlpha;
                element.Components.Add(outline);
            }

            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }
        public Pair<string, CuiElement, CuiElement> CreateProtectedButton(CuiElementContainer container, string parent, string color, string textColor, string text, int size, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false) => CreateButton(container, parent, color, textColor, text, size, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, update);

        public Pair<string, CuiElement, CuiElement> CreateButton(CuiElementContainer container, string parent, string color, string textColor, string text, int size, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            var button = new CuiButtonComponent();
            if (material != null) button.Material = material;
            button.FadeIn = fadeIn;
            button.Color = ProcessColor(color);
            button.Command = command;
            element.Components.Add(button);

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());



            if (!update) container?.Add(element);

            var textElement = (CuiElement)null;

            if (!string.IsNullOrEmpty(text))
            {
                textElement = new CuiElement
                {
                    Parent = id,
                    Name = AppendId(),
                    Components =
                    {
                        new CuiRectTransformComponent
                        {

                            AnchorMin = "0.02 0",
                            AnchorMax = "0.98 1"
                        }
                    },
                    FadeOut = fadeOut,
                    DestroyUi = destroyUi,
                    Update = update
                }; ;

                var ptext = new CuiTextComponent();
                ptext.Text = text;
                ptext.FontSize = size;
                ptext.Align = align;
                ptext.Color = ProcessColor(textColor);
                ptext.Font = GetFont(font);
                textElement.Components.Add(ptext);

                container.Add(textElement);
            }

            if (outlineColor != null)
            {
                CuiOutlineComponent outline = new CuiOutlineComponent();
                outline.Color = ProcessColor(outlineColor);
                outline.Distance = outlineDistance;
                outline.UseGraphicAlpha = outlineUseGraphicAlpha;
                element.Components.Add(outline);
            }

            return new Pair<string, CuiElement, CuiElement>(id, element, textElement);
        }
        public Pair<string, CuiElement> CreateInputField(CuiElementContainer container, string parent, string color, string text, int size, int characterLimit, bool readOnly, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, bool autoFocus = false, bool hudMenuInput = false, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string id = null, string destroyUi = null, bool update = false) => CreateInputField(container, parent, color, text, size, characterLimit, readOnly, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, autoFocus, hudMenuInput, InputField.LineType.SingleLine, fadeIn, fadeOut, needsCursor, needsKeyboard, id, destroyUi, update);

        public Pair<string, CuiElement> CreateInputField(CuiElementContainer container, string parent, string color, string text, int size, int characterLimit, bool readOnly, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, bool autoFocus = false, bool hudMenuInput = false, InputField.LineType lineType = InputField.LineType.SingleLine, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            var inputField = new CuiInputFieldComponent();
            inputField.Color = ProcessColor(color);
            inputField.Text = string.IsNullOrEmpty(text) ? string.Empty : text;
            inputField.FontSize = size;
            inputField.Font = GetFont(font);
            inputField.Align = align;
            inputField.CharsLimit = characterLimit;
            inputField.ReadOnly = readOnly;
            inputField.Command = command;
            inputField.Autofocus = autoFocus;
            inputField.HudMenuInput = hudMenuInput;
            inputField.LineType = lineType;
            element.Components.Add(inputField);

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());

            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }
        public Pair<string, CuiElement> CreateProtectedInputField(CuiElementContainer container, string parent, string color, string text, int size, int characterLimit, bool readOnly, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, bool autoFocus = false, bool hudMenuInput = false, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string id = null, string destroyUi = null, bool update = false)
        {
            return CreateInputField(container, parent, color, text, size, characterLimit, readOnly, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, autoFocus, hudMenuInput, InputField.LineType.SingleLine, fadeIn, fadeOut, needsCursor, needsKeyboard, id, destroyUi, update);
        }

        public Pair<string, CuiElement> CreateProtectedInputField(CuiElementContainer container, string parent, string color, string text, int size, int characterLimit, bool readOnly, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, bool autoFocus = false, bool hudMenuInput = false, InputField.LineType lineType = InputField.LineType.SingleLine, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string id = null, string destroyUi = null, bool update = false)
        {
            return CreateInputField(container, parent, color, text, size, characterLimit, readOnly, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, autoFocus, hudMenuInput, lineType, fadeIn, fadeOut, needsCursor, needsKeyboard, id, destroyUi, update);
        }

        public Pair<string, CuiElement> CreateImage(CuiElementContainer container, string parent, uint png, string color, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            var rawImage = new CuiRawImageComponent();
            if (material != null) rawImage.Material = material;
            rawImage.Png = png.ToString();
            rawImage.FadeIn = fadeIn;
            rawImage.Color = ProcessColor(color);
            element.Components.Add(rawImage);

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());

            if (outlineColor != null)
            {
                CuiOutlineComponent outline = new CuiOutlineComponent();
                outline.Color = ProcessColor(outlineColor);
                outline.Distance = outlineDistance;
                outline.UseGraphicAlpha = outlineUseGraphicAlpha;
                element.Components.Add(outline);
            }

            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }

        public Pair<string, CuiElement> CreateImage(CuiElementContainer container, string parent, string url, string color, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            var rawImage = new CuiRawImageComponent();
            if (material != null) rawImage.Material = material;
            rawImage.Url = url;
            rawImage.FadeIn = fadeIn;
            rawImage.Color = ProcessColor(color);
            element.Components.Add(rawImage);

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());

            if (outlineColor != null)
            {
                CuiOutlineComponent outline = new CuiOutlineComponent();
                outline.Color = ProcessColor(outlineColor);
                outline.Distance = outlineDistance;
                outline.UseGraphicAlpha = outlineUseGraphicAlpha;
                element.Components.Add(outline);
            }

            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }

        public Pair<string, CuiElement> CreateSprite(CuiElementContainer container, string parent, string sprite, string color, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            var rawImage = new CuiRawImageComponent();
            if (material != null) rawImage.Material = material;
            rawImage.Sprite = sprite;
            rawImage.FadeIn = fadeIn;
            rawImage.Color = ProcessColor(color);
            element.Components.Add(rawImage);

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());

            if (outlineColor != null)
            {
                CuiOutlineComponent outline = new CuiOutlineComponent();
                outline.Color = ProcessColor(outlineColor);
                outline.Distance = outlineDistance;
                outline.UseGraphicAlpha = outlineUseGraphicAlpha;
                element.Components.Add(outline);
            }

            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }

        public Pair<string, CuiElement> CreateSimpleImage(CuiElementContainer container, string parent, string png, string sprite, string color, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            var simpleImage = new CuiImageComponent();
            simpleImage.Png = png;
            simpleImage.Sprite = sprite;
            simpleImage.FadeIn = fadeIn;
            simpleImage.Color = ProcessColor(color);
            if (material != null) simpleImage.Material = material;
            element.Components.Add(simpleImage);

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());

            if (outlineColor != null)
            {
                CuiOutlineComponent outline = new CuiOutlineComponent();
                outline.Color = ProcessColor(outlineColor);
                outline.Distance = outlineDistance;
                outline.UseGraphicAlpha = outlineUseGraphicAlpha;
                element.Components.Add(outline);
            }

            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }

        public Pair<string, CuiElement> CreateItemImage(CuiElementContainer container, string parent, int itemID, ulong skinID, string color, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update,
            };

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            var rawImage = new CuiImageComponent();
            if (material != null) rawImage.Material = material;
            rawImage.ItemId = itemID;
            rawImage.SkinId = skinID;
            rawImage.FadeIn = fadeIn;
            rawImage.Color = ProcessColor(color);
            element.Components.Add(rawImage);

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());

            if (outlineColor != null)
            {
                CuiOutlineComponent outline = new CuiOutlineComponent();
                outline.Color = ProcessColor(outlineColor);
                outline.Distance = outlineDistance;
                outline.UseGraphicAlpha = outlineUseGraphicAlpha;
                element.Components.Add(outline);
            }

            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }

        public Pair<string, CuiElement> CreateClientImage(CuiElementContainer container, string parent, string url, string color, string material, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string id = null, string destroyUi = null, bool update = false)
        {
            return CreateImage(container, parent, url, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, update);
        }

        public Pair<string, CuiElement> CreateScrollView(CuiElementContainer container, string parent,bool vertical, bool horizontal, ScrollRect.MovementType movementType, float elasticity, bool inertia, float decelerationRate, float scrollSensitivity, out CuiRectTransform contentTransformComponent, out CuiScrollbar horizontalScrollBar, out CuiScrollbar verticalScrollBar, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string id = null, string destroyUi = null, bool update = false)
        {
            if (id == null) id = AppendId();
            var element = new CuiElement
            {
                Parent = parent,
                Name = id,
                FadeOut = fadeOut,
                DestroyUi = destroyUi,
                Update = update,
            };

            var scrollview = new CuiScrollViewComponent();
            scrollview.Vertical = vertical;
            scrollview.Horizontal = horizontal;
            scrollview.MovementType = movementType;
            scrollview.Elasticity = elasticity;
            scrollview.Inertia = inertia;
            scrollview.DecelerationRate = decelerationRate;
            scrollview.ScrollSensitivity = scrollSensitivity;
            scrollview.ContentTransform = new CuiRectTransform();
            contentTransformComponent = scrollview.ContentTransform;
            scrollview.HorizontalScrollbar = new CuiScrollbar();
            horizontalScrollBar = scrollview.HorizontalScrollbar;
            scrollview.VerticalScrollbar = new CuiScrollbar();
            verticalScrollBar = scrollview.VerticalScrollbar;

            element.Components.Add(scrollview);

            if (!update || (update && (xMin != 0 || xMax != 1 || yMin != 0 || yMax != 1)))
            {
                var rect = new CuiRectTransformComponent();
                rect.AnchorMin = $"{xMin} {yMin}";
                rect.AnchorMax = $"{xMax} {yMax}";
                rect.OffsetMin = $"{OxMin} {OyMin}";
                rect.OffsetMax = $"{OxMax} {OyMax}";
                element.Components.Add(rect);
            }

            if (needsCursor) element.Components.Add(new CuiNeedsCursorComponent());
            if (needsKeyboard) element.Components.Add(new CuiNeedsKeyboardComponent());

            if (!update) container?.Add(element);
            return new Pair<string, CuiElement>(id, element);
        }

        public static string RustToHexColor(string rustColor, float? alpha = null, bool includeAlpha = true)
        {
            var colors = rustColor.Split(' ');
            var color = new Color(colors[0].ToFloat(), colors[1].ToFloat(), colors[2].ToFloat(), includeAlpha ? alpha ?? (colors.Length > 2 ? colors[3].ToFloat() : 1f) : 1);
            var result = includeAlpha ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color);
            Array.Clear(colors, 0, colors.Length);
            return $"#{result}";
        }

        public string GetFont(FontTypes type)
        {
            return type switch
            {
                FontTypes.RobotoCondensedBold => "robotocondensed-bold.ttf",
                FontTypes.RobotoCondensedRegular => "robotocondensed-regular.ttf",
                FontTypes.PermanentMarker => "permanentmarker.ttf",
                FontTypes.DroidSansMono => "droidsansmono.ttf",
                FontTypes.NotoSansArabicBold => "NotoSansArabic-Bold.ttf",
                _ => "robotocondensed-regular.ttf"
            };
        }

        #endregion

        #region Send

        public void Send(CuiElementContainer container, BasePlayer player)
        {
            CuiHelper.AddUi(player, container);
        }
        public void Destroy(string name, BasePlayer player)
        {
            CuiHelper.DestroyUi(player, name);
        }

        #endregion

        public struct Pair<T1, T2>
        {
            public T1 Id;
            public T2 Element;

            public Pair(T1 id, T2 element)
            {
                Id = id;
                Element = element;
            }

            public static implicit operator string(Pair<T1, T2> value)
            {
                return value.Id.ToString();
            }
        }
        public struct Pair<T1, T2, T3>
        {
            public T1 Id;
            public T2 Element1;
            public T3 Element2;

            public Pair(T1 id, T2 element1, T3 element2)
            {
                Id = id;
                Element1 = element1;
                Element2 = element2;
            }

            public static implicit operator string(Pair<T1, T2, T3> value)
            {
                return value.Id.ToString();
            }
        }

        public void Dispose()
        {
        }

        public class Handler
        {
            internal string Identifier { get; set; }

            public Handler()
            {
                Identifier = "CarbonAliasesID";
            }

            #region Properties

            internal int _currentId { get; set; }

            #endregion

            #region Pooling

            internal string AppendId()
            {
                _currentId++;
                return $"{Identifier}_{_currentId}";
            }

            #endregion


            #region Classes


            public enum FontTypes
            {
                RobotoCondensedBold, RobotoCondensedRegular,
                PermanentMarker, DroidSansMono, NotoSansArabicBold
            }

            #endregion

            #region Network

            public void Send(CuiElementContainer container, BasePlayer player)
            {
                CuiHelper.AddUi(player, container);
            }
            public void SendUpdate(Pair<string, CuiElement> pair, BasePlayer player)
            {
                pair.SendUpdate(player);
            }
            public void Destroy(string name, BasePlayer player)
            {
                CuiHelper.DestroyUi(player, name);
            }

            #endregion

            public class UpdatePool : CuiElementContainer, IDisposable
            {
                internal bool _hasDisposed;

                public void Add(Pair<string, CuiElement> pair)
                {
                    if (pair.Element != null)
                    {
                        if (!pair.Element.Update)
                        {
                            return;
                        }
                        else Add(pair.Element);
                    }
                }
                public void Add(Pair<string, CuiElement, CuiElement> pair)
                {
                    if (pair.Element1 != null)
                    {
                        if (!pair.Element1.Update)
                        {
                            return;
                        }
                        else Add(pair.Element1);
                    }

                    if (pair.Element2 != null)
                    {
                        if (!pair.Element2.Update)
                        {
                            return;
                        }
                        else Add(pair.Element2);
                    }
                }

                public void Send(BasePlayer player)
                {
                    CuiHelper.AddUi(player, this);

                    Dispose();
                }

                public void Dispose()
                {
                    if (_hasDisposed) return;

                    Clear();

                    _hasDisposed = true;
                }
            }
        }
    }
}

public static class CUIStatics
{

    internal static string ProcessColor(string color)
    {
        if (color.StartsWith("#")) return CUI.HexToRustColor(color);

        return color;
    }

    public static Pair<string, CuiElement> UpdatePanel(this CUI cui, string id, string color, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, bool blur = false, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreatePanel(null, null, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, blur, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateText(this CUI cui, string id, string color, string text, int size, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, VerticalWrapMode verticalOverflow = VerticalWrapMode.Overflow, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateText(null, null, color, text, size, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, align, font, verticalOverflow, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement, CuiElement> UpdateButton(this CUI cui, string id, string color, string textColor, string text, int size, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateButton(null, null, color, textColor, text, size, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement, CuiElement> UpdateProtectedButton(this CUI cui, string id, string color, string textColor, string text, int size, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateProtectedButton(null, null, color, textColor, text, size, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateInputField(this CUI cui, string id, string color, string text, int size, int characterLimit, bool readOnly, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, bool autoFocus = false, bool hudMenuInput = false, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string destroyUi = null)
    {
        return cui.CreateInputField(null, null, color, text, size, characterLimit, readOnly, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, autoFocus, hudMenuInput, InputField.LineType.SingleLine, fadeIn, fadeOut, needsCursor, needsKeyboard, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateInputField(this CUI cui, string id, string color, string text, int size, int characterLimit, bool readOnly, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, bool autoFocus = false, bool hudMenuInput = false, InputField.LineType lineType = InputField.LineType.SingleLine, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string destroyUi = null)
    {
        return cui.CreateInputField(null, null, color, text, size, characterLimit, readOnly, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, autoFocus, hudMenuInput, lineType, fadeIn, fadeOut, needsCursor, needsKeyboard, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateProtectedInputField(this CUI cui, string id, string color, string text, int size, int characterLimit, bool readOnly, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, bool autoFocus = false, bool hudMenuInput = false, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string destroyUi = null)
    {
        return cui.CreateProtectedInputField(null, null, color, text, size, characterLimit, readOnly, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, autoFocus, hudMenuInput, InputField.LineType.SingleLine, fadeIn, fadeOut, needsCursor, needsKeyboard, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateProtectedInputField(this CUI cui, string id, string color, string text, int size, int characterLimit, bool readOnly, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, string command = null, TextAnchor align = TextAnchor.MiddleCenter, Handler.FontTypes font = Handler.FontTypes.RobotoCondensedRegular, bool autoFocus = false, bool hudMenuInput = false, InputField.LineType lineType = InputField.LineType.SingleLine, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string destroyUi = null)
    {
        return cui.CreateProtectedInputField(null, null, color, text, size, characterLimit, readOnly, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, command, align, font, autoFocus, hudMenuInput, lineType, fadeIn, fadeOut, needsCursor, needsKeyboard, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateImage(this CUI cui, string id, uint png, string color, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateImage(null, null, png, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateImage(this CUI cui, string id, string url, string color, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateImage(null, null, url, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true); ;
    }
    public static Pair<string, CuiElement> UpdateImage(this CUI cui, string id, string url, float scale, string color, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateImage(null, null, url, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateSprite(this CUI cui, string id, string sprite, string color, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateSprite(null, null, sprite, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateItemImage(this CUI cui, string id, int itemID, ulong skinID, string color, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateItemImage(null, null, itemID, skinID, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateClientImage(this CUI cui, string id, string url, string color, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateClientImage(null, null, url, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateSimpleImage(this CUI cui, string id, string png, string sprite, string color, string material = null, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string outlineColor = null, string outlineDistance = null, bool outlineUseGraphicAlpha = false, string destroyUi = null)
    {
        return cui.CreateSimpleImage(null, null, png, sprite, color, material, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, outlineColor, outlineDistance, outlineUseGraphicAlpha, id, destroyUi, true);
    }
    public static Pair<string, CuiElement> UpdateScrollView(this CUI cui, string id, bool vertical, bool horizontal, ScrollRect.MovementType movementType, float elasticity, bool inertia, float decelerationRate, float scrollSensitivity, out CuiRectTransform contentTransformComponent, out CuiScrollbar horizontalScrollBar, out CuiScrollbar verticalScrollBar, float xMin = 0f, float xMax = 1f, float yMin = 0f, float yMax = 1f, float OxMin = 0f, float OxMax = 0f, float OyMin = 0f, float OyMax = 0f, float fadeIn = 0f, float fadeOut = 0f, bool needsCursor = false, bool needsKeyboard = false, string destroyUi = null)
    {
        return cui.CreateScrollView(null, null, vertical, horizontal, movementType, elasticity, inertia, decelerationRate, scrollSensitivity, out contentTransformComponent, out horizontalScrollBar, out verticalScrollBar, xMin, xMax, yMin, yMax, OxMin, OxMax, OyMin, OyMax, fadeIn, fadeOut, needsCursor, needsKeyboard, id, destroyUi, true);
    }

    public static void SendUpdate(this Pair<string, CuiElement> pair, BasePlayer player)
    {
        var elements = Facepunch.Pool.Get<List<CuiElement>>();
        elements.Add(pair.Element);

        CuiHelper.AddUi(player, elements);

        Facepunch.Pool.FreeUnmanaged(ref elements);
    }
}
