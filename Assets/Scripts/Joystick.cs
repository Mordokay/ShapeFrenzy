using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

        TouchManager tm;

    public bool isTouching;
		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

		public Vector3 m_StartPos;
		public bool m_UseX; // Toggle for using the x axis
		public bool m_UseY; // Toggle for using the Y axis
		public CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		public CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

    public Vector3 newPos;
		void OnEnable()
		{
			CreateVirtualAxes();
		}

        public void Start()
        {
            tm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TouchManager>();
        isTouching = false;
            m_StartPos = transform.position;
        }

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(-delta.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}


		public void OnDrag(PointerEventData data)
		{
            Vector3 newPos = Vector3.zero;

			if (m_UseX)
			{
				int delta = (int)(data.position.x - m_StartPos.x);
				//delta = Mathf.Clamp(delta, - MovementRange, MovementRange);
				newPos.x = delta;
			}

			if (m_UseY)
			{
				int delta = (int)(data.position.y - m_StartPos.y);
				//delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
				newPos.y = delta;
			}
            Vector3 allowedPos = Vector3.ClampMagnitude(newPos, MovementRange);

            transform.position = new Vector3(m_StartPos.x + allowedPos.x, m_StartPos.y + allowedPos.y, m_StartPos.z + allowedPos.z);
			UpdateVirtualAxes(transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			transform.position = m_StartPos;
			UpdateVirtualAxes(m_StartPos);
        }


		public void OnPointerDown(PointerEventData data) {}

		public void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
			}
		}

    public void setNewPos(Vector3 pos) {
        this.newPos = pos;
    }
    
    public void setIsTouching(bool value)
    {
        this.isTouching = value;
    }

    public void resetPos() {
        transform.position = m_StartPos;
        UpdateVirtualAxes(m_StartPos);
    }

    void Update() {
        if (tm.freeTouchMovement) {
                Vector3 newPosAux = Vector3.zero;

                if (m_UseX)
                {
                    float delta = newPos.x - m_StartPos.x;
                    newPosAux.x = delta;
                }

                if (m_UseY)
                {
                    float delta = newPos.y - m_StartPos.y;
                    newPosAux.y = delta;
                    
                }
                Vector3 allowedPos = Vector3.ClampMagnitude(newPosAux, MovementRange);

                transform.position = new Vector3(m_StartPos.x + allowedPos.x, m_StartPos.y + allowedPos.y, m_StartPos.z + allowedPos.z);
                UpdateVirtualAxes(transform.position);
        }
    }
}
