using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class SelectScene : Button
{
    public new class UxmlFactory : UxmlFactory<SelectScene, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits 
    {
        UxmlStringAttributeDescription m_Status = new UxmlStringAttributeDescription { name = "status" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var _element = ve as SelectScene;

            _element.text = m_Status.GetValueFromBag(bag, cc);
            //_element.Status =
        }
    }

    public string Status { get; set; }
}
