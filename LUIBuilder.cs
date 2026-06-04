using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Oxide.Ext.CarbonAliases;

public class LUIBuilder
{
    public List<LuiContainerCarbonAliases> elements = new();

    private static readonly JsonSerializerSettings _cuiSettings = new()
    {
        DefaultValueHandling = DefaultValueHandling.Ignore
    };

    private static readonly Dictionary<LuiCompType, string> _fieldNames = new()
    {
        [LuiCompType.Text]                 = "UnityEngine.UI.Text",
        [LuiCompType.Image]                = "UnityEngine.UI.Image",
        [LuiCompType.RawImage]             = "UnityEngine.UI.RawImage",
        [LuiCompType.Button]               = "UnityEngine.UI.Button",
        [LuiCompType.Outline]              = "UnityEngine.UI.Outline",
        [LuiCompType.InputField]           = "UnityEngine.UI.InputField",
        [LuiCompType.NeedsCursor]          = "NeedsCursor",
        [LuiCompType.RectTransform]        = "RectTransform",
        [LuiCompType.Countdown]            = "Countdown",
        [LuiCompType.HorizontalLayoutGroup]= "UnityEngine.UI.HorizontalLayoutGroup",
        [LuiCompType.VerticalLayoutGroup]  = "UnityEngine.UI.VerticalLayoutGroup",
        [LuiCompType.GridLayoutGroup]      = "UnityEngine.UI.GridLayoutGroup",
        [LuiCompType.ContentSizeFitter]    = "UnityEngine.UI.ContentSizeFitter",
        [LuiCompType.LayoutElement]        = "UnityEngine.UI.LayoutElement",
        [LuiCompType.Draggable]            = "Draggable",
        [LuiCompType.Slot]                 = "Slot",
        [LuiCompType.NeedsKeyboard]        = "NeedsKeyboard",
        [LuiCompType.ScrollView]           = "UnityEngine.UI.ScrollView",
    };

    private LUI lui { get; }

    public LUIBuilder(LUI _lui)
    {
        lui = _lui;
        elements.Clear();

        foreach (var element in lui.elements)
        {
            var el = new LuiContainerCarbonAliases
            {
                name       = element.name,
                destroyUi  = element.destroyUi,
                update     = element.update,
                fadeOut    = element.fadeOut,
                parent     = element.parent
            };

            foreach (LuiCompBase component in element.luiComponents)
                el.components.Add(BuildComponent(component));

            elements.Add(el);
        }
    }

    public static string VectorToString(Vector2 vector) => $"{vector.x} {vector.y}";

    public string GetJsonString() =>
        JsonConvert.SerializeObject(elements, Formatting.None, _cuiSettings).Replace("\\n", "\n");

    public byte[] GetMergedBytes() =>
        Encoding.UTF8.GetBytes(GetJsonString());

    private static string GetFieldName(LuiCompType type) =>
        _fieldNames.TryGetValue(type, out var name) ? name : "UnityEngine.UI.Image";

    private Dictionary<string, object> BuildComponent(LuiCompBase component)
    {
        var comp = new Dictionary<string, object>
        {
            ["type"] = GetFieldName(component.type)
        };

        if (!component.enabled)                        comp["enabled"]              = false;
        if (component.fadeIn > 0)                      comp["fadeIn"]               = component.fadeIn;
        if (component.placeholderParentId != null)     comp["placeholderParentId"]  = component.placeholderParentId;

        switch (component.type)
        {
            case LuiCompType.Text:
                BuildText(comp, (LuiTextComp)component);
                break;
            case LuiCompType.Image:
                BuildImage(comp, (LuiImageComp)component);
                break;
            case LuiCompType.RawImage:
                BuildRawImage(comp, (LuiRawImageComp)component);
                break;
            case LuiCompType.Button:
                BuildButton(comp, (LuiButtonComp)component);
                break;
            case LuiCompType.Outline:
                BuildOutline(comp, (LuiOutlineComp)component);
                break;
            case LuiCompType.InputField:
                BuildInputField(comp, (LuiInputComp)component);
                break;
            case LuiCompType.RectTransform:
                BuildRectTransform(comp, (LuiRectTransformComp)component);
                break;
            case LuiCompType.Countdown:
                BuildCountdown(comp, (LuiCountdownComp)component);
                break;
            case LuiCompType.HorizontalLayoutGroup:
                BuildLayoutGroup(comp, (LuiHorizontalLayoutGroupComp)component);
                break;
            case LuiCompType.VerticalLayoutGroup:
                BuildLayoutGroup(comp, (LuiVerticalLayoutGroupComp)component);
                break;
            case LuiCompType.GridLayoutGroup:
                BuildGridLayoutGroup(comp, (LuiGridLayoutGroupComp)component);
                break;
            case LuiCompType.ContentSizeFitter:
                BuildContentSizeFitter(comp, (LuiContentSizeFitterComp)component);
                break;
            case LuiCompType.LayoutElement:
                BuildLayoutElement(comp, (LuiLayoutElementComp)component);
                break;
            case LuiCompType.Draggable:
                BuildDraggable(comp, (LuiDraggableComp)component);
                break;
            case LuiCompType.Slot:
                BuildSlot(comp, (LuiSlotComp)component);
                break;
            case LuiCompType.ScrollView:
                BuildScrollView(comp, (LuiScrollComp)component);
                break;
        }

        return comp;
    }

    private static void BuildText(Dictionary<string, object> c, LuiTextComp t)
    {
        if (t.text != null)              c["text"]            = t.text;
        if (t.fontSize > 0)              c["fontSize"]        = t.fontSize;
        if (t.font != null)              c["font"]            = t.font;
        if (t.align != null)             c["align"]           = t.align;
        if (t.color != null)             c["color"]           = t.color;
        if (t.verticalOverflow != null)  c["verticalOverflow"]= t.verticalOverflow;
    }

    private static void BuildImage(Dictionary<string, object> c, LuiImageComp i)
    {
        if (i.sprite != null)       c["sprite"]    = i.sprite;
        if (i.material != null)     c["material"]  = i.material;
        if (i.color != null)        c["color"]     = i.color;
        if (i.imageType != null)    c["imagetype"] = i.imageType;
        if (i.fillCenter)           c["fillCenter"]= true;
        if (i.png != null)          c["png"]       = i.png;
        if (i.slice != null)        c["slice"]     = i.slice;
        if (i.itemid != 0)          c["itemid"]    = i.itemid;
        if (i.skinid != 0)          c["skinid"]    = i.skinid;
    }

    private static void BuildRawImage(Dictionary<string, object> c, LuiRawImageComp r)
    {
        if (r.sprite != null)    c["sprite"]   = r.sprite;
        if (r.color != null)     c["color"]    = r.color;
        if (r.material != null)  c["material"] = r.material;
        if (r.url != null)       c["url"]      = r.url;
        if (r.png != null)       c["png"]      = r.png;
        if (r.steamid != null)   c["steamid"]  = r.steamid;
    }

    private static void BuildButton(Dictionary<string, object> c, LuiButtonComp b)
    {
        if (b.command != null)              c["command"]           = b.command;
        if (b.close != null)                c["close"]             = b.close;
        if (b.sprite != null)               c["sprite"]            = b.sprite;
        if (b.material != null)             c["material"]          = b.material;
        if (b.color != null)                c["color"]             = b.color;
        if (b.imageType != null)            c["imagetype"]         = b.imageType;
        if (b.normalColor != null)          c["normalColor"]       = b.normalColor;
        if (b.highlightedColor != null)     c["highlightedColor"]  = b.highlightedColor;
        if (b.pressedColor != null)         c["pressedColor"]      = b.pressedColor;
        if (b.selectedColor != null)        c["selectedColor"]     = b.selectedColor;
        if (b.disabledColor != null)        c["disabledColor"]     = b.disabledColor;
        if (b.colorMultiplier != -1)        c["colorMultiplier"]   = b.colorMultiplier;
        if (b.fadeDuration != -1)           c["fadeDuration"]      = b.fadeDuration;
    }

    private static void BuildOutline(Dictionary<string, object> c, LuiOutlineComp o)
    {
        if (o.color != null)         c["color"]          = o.color;
        if (o.distance != default)   c["distance"]       = o.distance;
        if (o.useGraphicAlpha)       c["useGraphicAlpha"]= true;
    }

    private static void BuildInputField(Dictionary<string, object> c, LuiInputComp i)
    {
        if (i.fontSize > 0)          c["fontSize"]       = i.fontSize;
        if (i.font != null)          c["font"]           = i.font;
        if (i.align != null)         c["align"]          = i.align;
        if (i.color != null)         c["color"]          = i.color;
        if (i.characterLimit > 0)    c["characterLimit"] = i.characterLimit;
        if (i.command != null)       c["command"]        = i.command;
        if (i.lineType != null)      c["lineType"]       = i.lineType;
        if (i.text != null)          c["text"]           = i.text;
        if (i.readOnly)              c["readOnly"]       = true;
        if (i.placeholderId != null) c["placeholderId"]  = i.placeholderId;
        if (i.password)              c["password"]       = true;
        if (i.needsKeyboard)         c["needsKeyboard"]  = true;
        if (i.hudMenuInput)          c["hudMenuInput"]   = true;
        if (i.autofocus)             c["autofocus"]      = true;
    }

    private static void BuildRectTransform(Dictionary<string, object> c, LuiRectTransformComp r)
    {
        if (r.anchor != LuiPosition.Full)
        {
            c["anchormin"] = VectorToString(r.anchor.anchorMin);
            c["anchormax"] = VectorToString(r.anchor.anchorMax);
        }
        // Always adding offset — Rust UI has a weird 1px offset by default
        c["offsetmin"] = VectorToString(r.offset.offsetMin);
        c["offsetmax"] = VectorToString(r.offset.offsetMax);

        if (r.rotation != 0)            c["rotation"]          = r.rotation;
        if (r.setParent != null)        c["setParent"]         = r.setParent;
        if (r.setTransformIndex != -1)  c["setTransformIndex"] = r.setTransformIndex;
    }

    private static void BuildCountdown(Dictionary<string, object> c, LuiCountdownComp cd)
    {
        if (cd.endTime != -1)           c["endTime"]        = cd.endTime;
        if (cd.startTime != -1)         c["startTime"]      = cd.startTime;
        if (cd.step > 0)                c["step"]           = cd.step;
        if (cd.interval > 0)            c["interval"]       = cd.interval;
        if (cd.timerFormat != null)     c["timerFormat"]    = cd.timerFormat;
        if (cd.numberFormat != null)    c["numberFormat"]   = cd.numberFormat;
        if (!cd.destroyIfDone)          c["destroyIfDone"]  = false;
        if (cd.command != null)         c["command"]        = cd.command;
    }

    private static void BuildLayoutGroup(Dictionary<string, object> c, LuiHorizontalLayoutGroupComp lg)
    {
        if (lg.spacing != 0)                    c["spacing"]                = lg.spacing;
        if (lg.childAlignment != null)          c["childAlignment"]         = lg.childAlignment;
        if (!lg.childForceExpandWidth)          c["childForceExpandWidth"]  = false;
        if (!lg.childForceExpandHeight)         c["childForceExpandHeight"] = false;
        if (lg.childControlWidth)               c["childControlWidth"]      = true;
        if (lg.childControlHeight)              c["childControlHeight"]     = true;
        if (lg.childScaleWidth)                 c["childScaleWidth"]        = true;
        if (lg.childScaleHeight)                c["childScaleHeight"]       = true;
        if (lg.padding != null)                 c["padding"]                = lg.padding;
    }

    private static void BuildLayoutGroup(Dictionary<string, object> c, LuiVerticalLayoutGroupComp lg)
    {
        if (lg.spacing != 0)                    c["spacing"]                = lg.spacing;
        if (lg.childAlignment != null)          c["childAlignment"]         = lg.childAlignment;
        if (!lg.childForceExpandWidth)          c["childForceExpandWidth"]  = false;
        if (!lg.childForceExpandHeight)         c["childForceExpandHeight"] = false;
        if (lg.childControlWidth)               c["childControlWidth"]      = true;
        if (lg.childControlHeight)              c["childControlHeight"]     = true;
        if (lg.childScaleWidth)                 c["childScaleWidth"]        = true;
        if (lg.childScaleHeight)                c["childScaleHeight"]       = true;
        if (lg.padding != null)                 c["padding"]                = lg.padding;
    }

    private static void BuildGridLayoutGroup(Dictionary<string, object> c, LuiGridLayoutGroupComp g)
    {
        if (g.cellSize != new Vector2(100, 100)) c["cellSize"]        = g.cellSize;
        if (g.spacing != default)                c["spacing"]         = g.spacing;
        if (g.startCorner != null)               c["startCorner"]     = g.startCorner;
        if (g.startAxis != null)                 c["startAxis"]       = g.startAxis;
        if (g.childAlignment != null)            c["childAlignment"]  = g.childAlignment;
        if (g.constraint != null)                c["constraint"]      = g.constraint;
        if (g.constraintCount != 0)              c["constraintCount"] = g.constraintCount;
        if (g.padding != null)                   c["padding"]         = g.padding;
    }

    private static void BuildContentSizeFitter(Dictionary<string, object> c, LuiContentSizeFitterComp f)
    {
        if (f.horizontalFit != null) c["horizontalFit"] = f.horizontalFit;
        if (f.verticalFit != null)   c["verticalFit"]   = f.verticalFit;
    }

    private static void BuildLayoutElement(Dictionary<string, object> c, LuiLayoutElementComp le)
    {
        if (le.preferredWidth != -1)    c["preferredWidth"]  = le.preferredWidth;
        if (le.preferredHeight != -1)   c["preferredHeight"] = le.preferredHeight;
        if (le.minWidth != 0)           c["minWidth"]        = le.minWidth;
        if (le.minHeight != 0)          c["minHeight"]       = le.minHeight;
        if (le.flexibleWidth != 0)      c["flexibleWidth"]   = le.flexibleWidth;
        if (le.flexibleHeight != 0)     c["flexibleHeight"]  = le.flexibleHeight;
        if (le.ignoreLayout)            c["ignoreLayout"]    = true;
    }

    private static void BuildDraggable(Dictionary<string, object> c, LuiDraggableComp d)
    {
        if (d.limitToParent)            c["limitToParent"]      = true;
        if (d.maxDistance > 0)          c["maxDistance"]        = d.maxDistance;
        if (d.allowSwapping)            c["allowSwapping"]      = true;
        if (!d.dropAnywhere)            c["dropAnywhere"]       = false;
        if (d.dragAlpha != -1)          c["dragAlpha"]          = d.dragAlpha;
        if (d.parentLimitIndex != -1)   c["parentLimitIndex"]   = d.parentLimitIndex;
        if (d.filter != null)           c["filter"]             = d.filter;
        if (d.parentPadding != default) c["parentPadding"]      = d.parentPadding;
        if (d.anchorOffset != default)  c["anchorOffset"]       = d.anchorOffset;
        if (d.keepOnTop)                c["keepOnTop"]          = d.keepOnTop;
        if (d.positionRPC != null)      c["positionRPC"]        = d.positionRPC;
        if (d.moveToAnchor)             c["moveToAnchor"]       = d.moveToAnchor;
        if (d.rebuildAnchor)            c["rebuildAnchor"]      = d.rebuildAnchor;
    }

    private static void BuildSlot(Dictionary<string, object> c, LuiSlotComp s)
    {
        if (s.filter != null) c["filter"] = s.filter;
    }

    private static void BuildScrollView(Dictionary<string, object> c, LuiScrollComp s)
    {
        bool changeAnchor = s.anchor != LuiPosition.Full;
        bool changeOffset = s.offset != LuiOffset.None;

        if (changeAnchor || changeOffset || s.pivot != LUI.defaultPivot)
        {
            var transform = new Dictionary<string, string>();
            if (changeAnchor)
            {
                transform["anchormin"] = VectorToString(s.anchor.anchorMin);
                transform["anchormax"] = VectorToString(s.anchor.anchorMax);
            }
            if (changeOffset)
            {
                transform["offsetmin"] = VectorToString(s.offset.offsetMin);
                transform["offsetmax"] = VectorToString(s.offset.offsetMax);
            }
            if (s.pivot != LUI.defaultPivot)
                c["pivot"] = s.pivot;

            c["contentTransform"] = transform;
        }

        if (s.horizontal)               c["horizontal"]                  = true;
        if (s.vertical)                 c["vertical"]                    = true;
        if (s.movementType != null)     c["movementType"]                = s.movementType;
        if (s.elasticity != -1)         c["elasticity"]                  = s.elasticity;
        if (s.inertia)                  c["inertia"]                     = true;
        if (s.decelerationRate != -1)   c["decelerationRate"]            = s.decelerationRate;
        if (s.scrollSensitivity != -1)  c["scrollSensitivity"]           = s.scrollSensitivity;
        if (s.horizontal)               c["horizontalScrollbar"]         = WriteScrollBar(s.horizontalScrollbar);
        if (s.vertical)                 c["verticalScrollbar"]           = WriteScrollBar(s.verticalScrollbar);
        if (s.horizontalNormalizedPosition != 0) c["horizontalNormalizedPosition"] = s.horizontalNormalizedPosition;
        if (s.verticalNormalizedPosition != 0)   c["verticalNormalizedPosition"]   = s.verticalNormalizedPosition;
    }

    private static Dictionary<string, object> WriteScrollBar(LuiScrollbar s)
    {
        var c = new Dictionary<string, object>();
        if (s.disabled)              c["enabled"]        = false;
        if (s.invert)                c["invert"]         = true;
        if (s.autoHide)              c["autoHide"]       = true;
        if (s.handleSprite != null)  c["handleSprite"]   = s.handleSprite;
        if (s.size != 0)             c["size"]           = s.size;
        if (s.handleColor != null)   c["handleColor"]    = s.handleColor;
        if (s.highlightColor != null)c["highlightColor"] = s.highlightColor;
        if (s.pressedColor != null)  c["pressedColor"]   = s.pressedColor;
        if (s.trackSprite != null)   c["trackSprite"]    = s.trackSprite;
        if (s.trackColor != null)    c["trackColor"]     = s.trackColor;
        return c;
    }

    public class LuiContainerCarbonAliases
    {
        public string name;
        public string parent;
        public List<Dictionary<string, object>> components = new();
        public string destroyUi;
        public float fadeOut;
        public bool update;
    }
}