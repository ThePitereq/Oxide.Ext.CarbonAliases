using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Oxide.Ext.CarbonAliases;

public class LUIBuilder
{
    private LUI lui { get; set; }

    private string GetFieldName(LuiCompType type)
    {
	    return type switch
	    {
		    LuiCompType.Text => "UnityEngine.UI.Text",
		    LuiCompType.Image => "UnityEngine.UI.Image",
		    LuiCompType.RawImage => "UnityEngine.UI.RawImage",
		    LuiCompType.Button => "UnityEngine.UI.Button",
		    LuiCompType.Outline => "UnityEngine.UI.Outline",
		    LuiCompType.InputField => "UnityEngine.UI.InputField",
		    LuiCompType.NeedsCursor => "NeedsCursor",
		    LuiCompType.RectTransform => "RectTransform",
		    LuiCompType.Countdown => "Countdown",
		    LuiCompType.HorizontalLayoutGroup => "UnityEngine.UI.HorizontalLayoutGroup",
		    LuiCompType.VerticalLayoutGroup => "UnityEngine.UI.VerticalLayoutGroup",
		    LuiCompType.GridLayoutGroup => "UnityEngine.UI.GridLayoutGroup",
		    LuiCompType.ContentSizeFitter => "UnityEngine.UI.ContentSizeFitter",
		    LuiCompType.LayoutElement => "UnityEngine.UI.LayoutElement",
		    LuiCompType.Draggable => "Draggable",
		    LuiCompType.Slot  => "Slot",
		    LuiCompType.NeedsKeyboard => "NeedsKeyboard",
		    LuiCompType.ScrollView => "UnityEngine.UI.ScrollView",
		    _ => "UnityEngine.UI.Image"
	    };
    }
    
    private static readonly char[] charPreset = new char[32]; //Max 4 values with 8 chars as no ui x/y should be bigger than 1280

    public static string VectorToString(Vector2 vector) => $"{vector.x} {vector.y}";
    
    public LUIBuilder(LUI _lui)
    {
        lui = _lui;
        elements.Clear();
        foreach (var element in lui.elements)
        {
            var el = new LuiContainerCarbonAliases()
            {
                name = element.name,
                destroyUi = element.destroyUi,
                update = element.update,
                fadeOut = element.fadeOut,
                parent = element.parent
            };
            foreach (LuiCompBase component in element.luiComponents)
            {
	            Dictionary<string, object> compBuilder = new();
	            compBuilder.Add("type", GetFieldName(component.type));
	            if (!component.enabled)
	            {
		            compBuilder.Add("enabled", false);
	            }
	            if (component.fadeIn > 0)
	            {
		            compBuilder.Add("fadeIn", component.fadeIn);
	            }
	            if (component.placeholderParentId != null)
	            {
		            compBuilder.Add("placeholderParentId", component.placeholderParentId);
	            }
	            switch (component.type)
                {
                    case LuiCompType.Text:
	                    LuiTextComp text = component as LuiTextComp;
	                    
	                    if (text.text != null)
	                    {
		                    
		                    compBuilder.Add("text", text.text);
	                    }
	                    if (text.fontSize > 0)
	                    {
		                    
		                    compBuilder.Add("fontSize", text.fontSize);
	                    }
	                    if (text.font != null)
	                    {
		                    
		                    compBuilder.Add("font", text.font);
	                    }
	                    if (text.align != null)
	                    {
		                    
		                    compBuilder.Add("align", text.align);
	                    }
	                    if (text.color != null)
	                    {
		                    
		                    compBuilder.Add("color", text.color);
	                    }
	                    if (text.verticalOverflow != null)
	                    {
		                    
		                    compBuilder.Add("verticalOverflow", text.verticalOverflow);
	                    }
	                    break;
                    case LuiCompType.Image:
	                    LuiImageComp image = component as LuiImageComp;
	                    
	                    if (image.sprite != null)
	                    {
		                    
		                    compBuilder.Add("sprite", image.sprite);
	                    }
	                    if (image.material != null)
	                    {
		                    compBuilder.Add("material", image.material);
	                    }
	                    if (image.color != null)
	                    {
		                    compBuilder.Add("color", image.color);
	                    }
	                    if (image.imageType != null)
	                    {
		                    compBuilder.Add("imagetype", image.imageType);
	                    }
	                    if (image.fillCenter)
	                    {
		                    
		                    compBuilder.Add("fillCenter", true);
	                    }
	                    if (image.png != null)
	                    {
		                    compBuilder.Add("png", image.png);
	                    }
	                    if (image.slice != null)
	                    {
		                    
		                    compBuilder.Add("slice", image.slice);
	                    }
	                    if (image.itemid != 0)
	                    {
		                    compBuilder.Add("itemid", image.itemid);
	                    }
	                    if (image.skinid != 0)
	                    {
		                    compBuilder.Add("skinid", image.skinid);
	                    }
	                    break;
                    case LuiCompType.RawImage:
	                    LuiRawImageComp rawImage = component as LuiRawImageComp;
	                    
	                    if (rawImage.sprite != null)
	                    {
		                    compBuilder.Add("sprite", rawImage.sprite);
	                    }
	                    if (rawImage.color != null)
	                    {
		                    compBuilder.Add("color", rawImage.color);
	                    }
	                    if (rawImage.material != null)
	                    {
		                    compBuilder.Add("material", rawImage.material);
	                    }
	                    if (rawImage.url != null)
	                    {
		                    compBuilder.Add("url", rawImage.url);
	                    }
	                    if (rawImage.png != null)
	                    {
		                    compBuilder.Add("png", rawImage.png);
	                    }
	                    if (rawImage.steamid != null)
	                    {
		                    compBuilder.Add("steamid", rawImage.steamid);
	                    }
	                    break;
                    case LuiCompType.Button:
	                    LuiButtonComp button = component as LuiButtonComp;
	                    
	                    if (button.command != null)
	                    {
		                    compBuilder.Add("command", button.command);
	                    }
	                    if (button.close != null)
	                    {
		                    compBuilder.Add("close", button.close);
	                    }
	                    if (button.sprite != null)
	                    {
		                    compBuilder.Add("sprite", button.sprite);
	                    }
	                    if (button.material != null)
	                    {
		                    compBuilder.Add("material", button.material);
	                    }
	                    if (button.color != null)
	                    {
		                    compBuilder.Add("color", button.color);
	                    }
	                    if (button.imageType != null)
	                    {
		                    compBuilder.Add("imagetype", button.imageType);
	                    }
	                    if (button.normalColor != null)
	                    {
		                    compBuilder.Add("normalColor", button.normalColor);
	                    }
	                    if (button.highlightedColor != null)
	                    {
		                    compBuilder.Add("highlightedColor", button.highlightedColor);
	                    }
	                    if (button.pressedColor != null)
	                    {
		                    compBuilder.Add("pressedColor", button.pressedColor);
	                    }
	                    if (button.selectedColor != null)
	                    {
		                    compBuilder.Add("selectedColor", button.selectedColor);
	                    }
	                    if (button.disabledColor != null)
	                    {
		                    compBuilder.Add("disabledColor", button.disabledColor);
	                    }
	                    if (button.colorMultiplier != -1)
	                    {
		                    compBuilder.Add("colorMultiplier", button.colorMultiplier);
	                    }
	                    if (button.fadeDuration != -1)
	                    {
		                    compBuilder.Add("fadeDuration", button.fadeDuration);
	                    }
	                    break;
                    case LuiCompType.Outline:
	                    LuiOutlineComp outline = component as LuiOutlineComp;
	                    
	                    if (outline.color != null)
	                    {
		                    
		                    compBuilder.Add("color", outline.color);
	                    }
	                    if (outline.distance != default)
	                    {
		                    
		                    compBuilder.Add("distance", outline.distance);
	                    }
	                    if (outline.useGraphicAlpha)
	                    {
		                    
		                    compBuilder.Add("useGraphicAlpha", true);
	                    }
	                    break;
                    case LuiCompType.InputField:
	                    LuiInputComp input = component as LuiInputComp;
	                    
	                    if (input.fontSize > 0)
	                    {
		                    
		                    compBuilder.Add("fontSize", input.fontSize);
	                    }
	                    if (input.font != null)
	                    {
		                    
		                    compBuilder.Add("font", input.font);
	                    }
	                    if (input.align != null)
	                    {
		                    
		                    compBuilder.Add("align", input.align);
	                    }
	                    if (input.color != null)
	                    {
		                    
		                    compBuilder.Add("color", input.color);
	                    }
	                    if (input.characterLimit > 0)
	                    {
		                    
		                    compBuilder.Add("characterLimit", input.characterLimit);
	                    }
	                    if (input.command != null)
	                    {
		                    
		                    compBuilder.Add("command", input.command);
	                    }
	                    if (input.text != null)
	                    {
		                    
		                    compBuilder.Add("text", input.text);
	                    }
	                    if (input.readOnly)
	                    {
		                    
		                    compBuilder.Add("readOnly", true);
	                    }
	                    if (input.placeholderId != null)
	                    {
		                    
		                    compBuilder.Add("placeholderId", input.placeholderId);
	                    }
	                    if (input.lineType != null)
	                    {
		                    
		                    compBuilder.Add("lineType", input.lineType);
	                    }
	                    if (input.password)
	                    {
		                    
		                    compBuilder.Add("password", true);
	                    }
	                    if (input.needsKeyboard)
	                    {
		                    
		                    compBuilder.Add("needsKeyboard", true);
	                    }
	                    if (input.hudMenuInput)
	                    {
		                    
		                    compBuilder.Add("hudMenuInput", true);
	                    }
	                    if (input.autofocus)
	                    {
		                    
		                    compBuilder.Add("autofocus", true);
	                    }
	                    break;
                    case LuiCompType.RectTransform:
                        LuiRectTransformComp rect = component as LuiRectTransformComp;
                        
                        if (rect.anchor != LuiPosition.Full)
                        {
	                        
	                        compBuilder.Add("anchormin", VectorToString(rect.anchor.anchorMin));
	                        
	                        compBuilder.Add("anchormax", VectorToString(rect.anchor.anchorMax));
                        }
                         //Always adding offset, as RUST UI have weird one pixel offset by default, idk who came to this idea lol.
                        compBuilder.Add("offsetmin", VectorToString(rect.offset.offsetMin));
                        
                        compBuilder.Add("offsetmax", VectorToString(rect.offset.offsetMax));
                        if (rect.rotation != 0)
                        {
	                        
	                        compBuilder.Add("rotation", rect.rotation);
                        }
                        if (rect.setParent != null)
                        {
	                        
	                        compBuilder.Add("setParent", rect.setParent);
                        }
                        if (rect.setTransformIndex != -1)
                        {
	                        
	                        compBuilder.Add("setTransformIndex", rect.setTransformIndex);
                        }
                        break;
                    case LuiCompType.Countdown:
	                    LuiCountdownComp countdown = component as LuiCountdownComp;
	                    
	                    if (countdown.endTime != -1)
	                    {
		                    
		                    compBuilder.Add("endTime", countdown.endTime);
	                    }
	                    if (countdown.startTime != -1)
	                    {
		                    
		                    compBuilder.Add("startTime", countdown.startTime);
	                    }
	                    if (countdown.step > 0)
	                    {
		                    
		                    compBuilder.Add("step", countdown.step);
	                    }
	                    if (countdown.interval > 0)
	                    {
		                    
		                    compBuilder.Add("interval", countdown.interval);
	                    }
	                    if (countdown.timerFormat != null)
	                    {
		                    
		                    compBuilder.Add("timerFormat", countdown.timerFormat);
	                    }
	                    if (countdown.numberFormat != null)
	                    {
		                    
		                    compBuilder.Add("numberFormat", countdown.numberFormat);
	                    }
	                    if (!countdown.destroyIfDone)
	                    {
		                    
		                    compBuilder.Add("destroyIfDone", false);
	                    }
	                    if (countdown.command != null)
	                    {
		                    
		                    compBuilder.Add("command", countdown.command);
	                    }
	                    break;
                    case LuiCompType.HorizontalLayoutGroup:
	                    LuiHorizontalLayoutGroupComp horizontalLayoutGroup = component as LuiHorizontalLayoutGroupComp;
	                    if (horizontalLayoutGroup.spacing != 0)
	                    {
		                    
		                    compBuilder.Add("spacing", horizontalLayoutGroup.spacing);
	                    }
	                    if (horizontalLayoutGroup.childAlignment != null)
	                    {
		                    
		                    compBuilder.Add("childAlignment", horizontalLayoutGroup.childAlignment);
	                    }
	                    if (!horizontalLayoutGroup.childForceExpandWidth)
	                    {
		                    
		                    compBuilder.Add("childForceExpandWidth", false);
	                    }
	                    if (!horizontalLayoutGroup.childForceExpandHeight)
	                    {
		                    
		                    compBuilder.Add("childForceExpandHeight", false);
	                    }
	                    if (horizontalLayoutGroup.childControlWidth)
	                    {
		                    
		                    compBuilder.Add("childControlWidth", true);
	                    }
	                    if (horizontalLayoutGroup.childControlHeight)
	                    {
		                    
		                    compBuilder.Add("childControlHeight", true);
	                    }
	                    if (horizontalLayoutGroup.childScaleWidth)
	                    {
		                    
		                    compBuilder.Add("childScaleWidth", true);
	                    }
	                    if (horizontalLayoutGroup.childScaleHeight)
	                    {
		                    
		                    compBuilder.Add("childScaleHeight", true);
	                    }
	                    if (horizontalLayoutGroup.padding != null)
	                    {
		                    
		                    compBuilder.Add("padding", horizontalLayoutGroup.padding);
	                    }
	                    break;
                    case LuiCompType.VerticalLayoutGroup:
	                    LuiVerticalLayoutGroupComp verticalLayoutGroup = component as LuiVerticalLayoutGroupComp;
	                    if (verticalLayoutGroup.spacing != 0)
	                    {
		                    
		                    compBuilder.Add("spacing", verticalLayoutGroup.spacing);
	                    }
	                    if (verticalLayoutGroup.childAlignment != null)
	                    {
		                    
		                    compBuilder.Add("childAlignment", verticalLayoutGroup.childAlignment);
	                    }
	                    if (!verticalLayoutGroup.childForceExpandWidth)
	                    {
		                    
		                    compBuilder.Add("childForceExpandWidth", false);
	                    }
	                    if (!verticalLayoutGroup.childForceExpandHeight)
	                    {
		                    
		                    compBuilder.Add("childForceExpandHeight", false);
	                    }
	                    if (verticalLayoutGroup.childControlWidth)
	                    {
		                    
		                    compBuilder.Add("childControlWidth", true);
	                    }
	                    if (verticalLayoutGroup.childControlHeight)
	                    {
		                    
		                    compBuilder.Add("childControlHeight", true);
	                    }
	                    if (verticalLayoutGroup.childScaleWidth)
	                    {
		                    
		                    compBuilder.Add("childScaleWidth", true);
	                    }
	                    if (verticalLayoutGroup.childScaleHeight)
	                    {
		                    
		                    compBuilder.Add("childScaleHeight", true);
	                    }
	                    if (verticalLayoutGroup.padding != null)
	                    {
		                    
		                    compBuilder.Add("padding", verticalLayoutGroup.padding);
	                    }
	                    break;
                    case LuiCompType.GridLayoutGroup:
	                    LuiGridLayoutGroupComp gridLayoutGroup = component as LuiGridLayoutGroupComp;
	                    if (gridLayoutGroup.cellSize != new Vector2(100, 100))
	                    {
		                    
		                    compBuilder.Add("cellSize", gridLayoutGroup.cellSize);
	                    }
	                    if (gridLayoutGroup.spacing != default)
	                    {
		                    
		                    compBuilder.Add("spacing", gridLayoutGroup.spacing);
	                    }
	                    if (gridLayoutGroup.startCorner != null)
	                    {
		                    
		                    compBuilder.Add("startCorner", gridLayoutGroup.startCorner);
	                    }
	                    if (gridLayoutGroup.startAxis != null)
	                    {
		                    
		                    compBuilder.Add("startAxis", gridLayoutGroup.startAxis);
	                    }
	                    if (gridLayoutGroup.childAlignment != null)
	                    {
		                    
		                    compBuilder.Add("childAlignment", gridLayoutGroup.childAlignment);
	                    }
	                    if (gridLayoutGroup.constraint != null)
	                    {
		                    
		                    compBuilder.Add("constraint", gridLayoutGroup.constraint);
	                    }
	                    if (gridLayoutGroup.constraintCount != 0)
	                    {
		                    
		                    compBuilder.Add("constraintCount", gridLayoutGroup.constraintCount);
	                    }
	                    if (gridLayoutGroup.padding != null)
	                    {
		                    
		                    compBuilder.Add("padding", gridLayoutGroup.padding);
	                    }
	                    break;
                    case LuiCompType.ContentSizeFitter:
	                    LuiContentSizeFitterComp contentSizeFitter = component as LuiContentSizeFitterComp;
	                    if (contentSizeFitter.horizontalFit != null)
	                    {
		                    
		                    compBuilder.Add("horizontalFit", contentSizeFitter.horizontalFit);
	                    }
	                    if (contentSizeFitter.verticalFit != null)
	                    {
		                    
		                    compBuilder.Add("verticalFit", contentSizeFitter.verticalFit);
	                    }
	                    break;
                    case LuiCompType.LayoutElement:
	                    LuiLayoutElementComp layoutElement = component as LuiLayoutElementComp;
	                    if (layoutElement.preferredWidth != -1)
	                    {
		                    
		                    compBuilder.Add("preferredWidth", layoutElement.preferredWidth);
	                    }
	                    if (layoutElement.preferredHeight != -1)
	                    {
		                    
		                    compBuilder.Add("preferredHeight", layoutElement.preferredHeight);
	                    }
	                    if (layoutElement.minWidth != 0)
	                    {
		                    
		                    compBuilder.Add("minWidth", layoutElement.minWidth);
	                    }
	                    if (layoutElement.minHeight != 0)
	                    {
		                    
		                    compBuilder.Add("minHeight", layoutElement.minHeight);
	                    }
	                    if (layoutElement.flexibleWidth != 0)
	                    {
		                    
		                    compBuilder.Add("flexibleWidth", layoutElement.flexibleWidth);
	                    }
	                    if (layoutElement.flexibleHeight != 0)
	                    {
		                    
		                    compBuilder.Add("flexibleHeight", layoutElement.flexibleHeight);
	                    }
	                    if (layoutElement.ignoreLayout)
	                    {
		                    
		                    compBuilder.Add("ignoreLayout", true);
	                    }
	                    break;
                    case LuiCompType.Draggable:
	                    LuiDraggableComp draggable = component as LuiDraggableComp;
	                    
	                    if (draggable.limitToParent)
	                    {
		                    
		                    compBuilder.Add("limitToParent", true);
	                    }
	                    if (draggable.maxDistance > 0)
	                    {
		                    
		                    compBuilder.Add("maxDistance", draggable.maxDistance);
	                    }
	                    if (draggable.allowSwapping)
	                    {
		                    
		                    compBuilder.Add("allowSwapping", true);
	                    }
	                    if (!draggable.dropAnywhere)
	                    {
		                    
		                    compBuilder.Add("dropAnywhere", false);
	                    }
	                    if (draggable.dragAlpha != -1)
	                    {
		                    
		                    compBuilder.Add("dragAlpha", draggable.dragAlpha);
	                    }
	                    if (draggable.parentLimitIndex != -1)
	                    {
		                    
		                    compBuilder.Add("parentLimitIndex", draggable.parentLimitIndex);
	                    }
	                    if (draggable.filter != null)
	                    {
		                    
		                    compBuilder.Add("filter", draggable.filter);
	                    }
	                    if (draggable.parentPadding != default)
	                    {
		                    
		                    compBuilder.Add("parentPadding", draggable.parentPadding);
	                    }
	                    if (draggable.anchorOffset != default)
	                    {
		                    
		                    compBuilder.Add("anchorOffset", draggable.anchorOffset);
	                    }
	                    if (draggable.keepOnTop)
	                    {
		                    
		                    compBuilder.Add("anchorOffset", draggable.keepOnTop);
	                    }
	                    if (draggable.positionRPC != null)
	                    {
		                    
		                    compBuilder.Add("positionRPC", draggable.positionRPC);
	                    }
	                    if (draggable.moveToAnchor)
	                    {
		                    
		                    compBuilder.Add("moveToAnchor", draggable.moveToAnchor);
	                    }
	                    if (draggable.rebuildAnchor)
	                    {
		                    
		                    compBuilder.Add("rebuildAnchor", draggable.rebuildAnchor);
	                    }
	                    break;
                    case LuiCompType.Slot:
	                    LuiSlotComp slot = component as LuiSlotComp;
	                    
	                    if (slot.filter != null)
	                    {
		                    
		                    compBuilder.Add("filter", slot.filter);
	                    }
	                    break;
                    case LuiCompType.ScrollView:
	                    LuiScrollComp scroll = component as LuiScrollComp;
	                    
	                    bool changeAnchor = scroll.anchor != LuiPosition.Full;
	                    bool changeOffset = scroll.offset != LuiOffset.None;
	                    if (changeAnchor || changeOffset || scroll.pivot != new Vector2(0.5f, 0.5f))
	                    {
		                    
		                    Dictionary<string, string> transform = new();
		                    if (changeAnchor)
		                    {
			                    transform.Add("anchormin", VectorToString(scroll.anchor.anchorMin));
			                    
			                    transform.Add("anchormax", VectorToString(scroll.anchor.anchorMax));
		                    }
		                    if (changeOffset)
		                    {
			                    if (changeAnchor)
				                    
				                    transform.Add("offsetmin", VectorToString(scroll.offset.offsetMin));
			                    
			                    transform.Add("offsetmax", VectorToString(scroll.offset.offsetMax));
		                    }
		                    if (scroll.pivot != new Vector2(0.5f, 0.5f))
		                    {
			                    
			                    compBuilder.Add("pivot", scroll.pivot);
		                    }
		                    compBuilder.Add("contentTransform", transform);
	                    }
	                    if (scroll.horizontal)
	                    {
		                    
		                    compBuilder.Add("horizontal", true);
	                    }
	                    if (scroll.vertical)
	                    {
		                    
		                    compBuilder.Add("vertical", true);
	                    }
	                    if (scroll.movementType != null)
	                    {
		                    
		                    compBuilder.Add("movementType", scroll.movementType);
	                    }
	                    if (scroll.elasticity != -1)
	                    {
		                    
		                    compBuilder.Add("elasticity", scroll.elasticity);
	                    }
	                    if (scroll.inertia)
	                    {
		                    
		                    compBuilder.Add("inertia", true);
	                    }
	                    if (scroll.decelerationRate != -1)
	                    {
		                    
		                    compBuilder.Add("decelerationRate", scroll.decelerationRate);
	                    }
	                    if (scroll.scrollSensitivity != -1)
	                    {
		                    
		                    compBuilder.Add("scrollSensitivity", scroll.scrollSensitivity);
	                    }
	                    if (scroll.horizontal)
	                    {
		                    compBuilder.Add("horizontalScrollbar", WriteScrollBar(scroll.horizontalScrollbar));

	                    }
	                    if (scroll.vertical)
	                    {
		                    compBuilder.Add("verticalScrollbar", WriteScrollBar(scroll.verticalScrollbar));
	                    }
	                    if (scroll.horizontalNormalizedPosition != 0)
	                    {
		                    
		                    compBuilder.Add("horizontalNormalizedPosition", scroll.horizontalNormalizedPosition);
	                    }
	                    if (scroll.verticalNormalizedPosition != 0)
	                    {
		                    
		                    compBuilder.Add("verticalNormalizedPosition", scroll.verticalNormalizedPosition);
	                    }
	                    break;
                }
	            el.components.Add(compBuilder);
            }
            elements.Add(el);
        }
    }

    public byte[] GetMergedBytes()
    {
	    string stringJson = JsonConvert.SerializeObject(elements, Formatting.None, _cuiSettings).Replace("\\n", "\n");
	    return Encoding.UTF8.GetBytes(stringJson);
    }
    
    private Dictionary<string, object> WriteScrollBar(LuiScrollbar scroll)
    {
	    Dictionary<string, object> elements = new();
	    elements.Add("enabled", !scroll.disabled); //Adding so I don't need to check for first coma.
	    if (scroll.invert)
	    {
		    
		    elements.Add("invert", true);
	    }
	    if (scroll.autoHide)
	    {
		    
		    elements.Add("autoHide", true);
	    }
	    if (scroll.handleSprite != null)
	    {
		    
		    elements.Add("handleSprite", scroll.handleSprite);
	    }
	    if (scroll.size != 0)
	    {
		    
		    elements.Add("size", scroll.size);
	    }
	    if (scroll.handleColor != null)
	    {
		    
		    elements.Add("handleColor", scroll.handleColor);
	    }
	    if (scroll.highlightColor != null)
	    {
		    
		    elements.Add("highlightColor", scroll.highlightColor);
	    }
	    if (scroll.pressedColor != null)
	    {
		    
		    elements.Add("pressedColor", scroll.pressedColor);
	    }
	    if (scroll.trackSprite != null)
	    {
		    
		    elements.Add("trackSprite", scroll.trackSprite);
	    }
	    if (scroll.trackColor != null)
	    {
		    
		    elements.Add("trackColor", scroll.trackColor);
	    }

	    return elements;
    }

    public List<LuiContainerCarbonAliases> elements = new();

    public class LuiContainerCarbonAliases
    {
        public string name;
        public string parent;
        public List<Dictionary<string, object>> components = new();
        public string destroyUi;
        public float fadeOut;
        public bool update;
    }
    
    private static JsonSerializerSettings _cuiSettings = new()
    {
        DefaultValueHandling = DefaultValueHandling.Ignore
    };

    public string GetJsonString()
    {
        return JsonConvert.SerializeObject(elements, Formatting.None, _cuiSettings).Replace("\\n", "\n");
    }
}