using System;
using APICalls;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputSystem
{
	public class SwipeControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler , IPointerDownHandler
	{
		public event Action<Vector3> OnDragStart;

		public event Action<PointerEventData> OnDragging;

		public event Action OnDragEnd;

		public event Action<SwipeData> OnSwipe;

		public event Action<Vector3> OnClick;

		private Vector2 fingerDownPosition;
		private Vector2 fingerUpPosition;

		[SerializeField]
		private bool detectSwipeOnlyAfterRelease = false;

		[SerializeField]
		private float minDistanceForSwipe = 30f;

		[SerializeField]
		private bool sendDataOnDrag = false;

		private float coolDownTimer;

		[SerializeField]
		private bool useNormalDrag = true;



		public void OnPointerDown(PointerEventData eventData)
		{
			OnClick?.Invoke(eventData.position);
		}


		public void OnBeginDrag(PointerEventData eventData)
		{
			if (OnDragStart == null) return; // Guard Clause

			//LogSystem.LogEvent("PointerEnter" + gameObject.name);
			fingerUpPosition = eventData.position;
			fingerDownPosition = eventData.position;
			//LogSystem.LogEvent("BEGIN DRAG");
			OnDragStart(eventData.position);


		}

		public void OnDrag(PointerEventData eventData)
		{
			if (OnDragging == null) return; // Guard Clause


			//LogSystem.LogEvent("PointerEnter {0}"  ,eventData. );

			if (useNormalDrag)
			{
				OnDragging(eventData);
			}
			else
			{
			
				fingerDownPosition = eventData.position;


				if (!IsVerticalSwipe() || !SwipeDistanceCheckVert())
				{
					OnDragging(eventData);
					if (sendDataOnDrag) SentEndData(eventData.position);
				}
				else
				{
					if (VerticalMovementDistance() > 200f)
					{
						if (!sendDataOnDrag)
							SentEndData(eventData.position);
					}
				}
			}



		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (OnDragEnd == null) return; //Guard Clause

				if (!sendDataOnDrag)
				SentEndData(eventData.position);
				//LogSystem.LogEvent("END DRAG");

			 	OnDragEnd();

		}

		private void SentEndData(Vector3 pos) 
		{
		
				//coolDownTimer = Time.time + 2f;


				fingerDownPosition = pos;
				float magnitudeY = VerticalMovementDistance();
				float magnitudeX = HorizontalMovementDistance();

			float xChange = fingerDownPosition.x - fingerUpPosition.x;

				SwipeDirection direction = DetectSwipe();
				if (direction != SwipeDirection.None)
				{
					
					SendSwipe(direction, magnitudeY,magnitudeX, xChange);
				}
		


		}

		private SwipeDirection DetectSwipe()
		{
			SwipeDirection direction = SwipeDirection.None;
			if (SwipeDistanceCheckMet())
			{
			
				if (IsVerticalSwipe())
				{
					 direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;

				}
				else
				{
					 direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;

				}
				fingerUpPosition = fingerDownPosition;
			}
			return direction; 
		}

		private bool IsVerticalSwipe()
		{
			return VerticalMovementDistance() > HorizontalMovementDistance();
		}

		private bool SwipeDistanceCheckMet()
		{
			return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
		}

		private bool SwipeDistanceCheckVert() 
		{
			return VerticalMovementDistance() > minDistanceForSwipe;
		}

		private float VerticalMovementDistance()
		{
			return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
		}

		private float HorizontalMovementDistance()
		{
			return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
		}

		private void SendSwipe(SwipeDirection direction, float magnitudeY, float magnitudeX, float xChnage)
		{
			SwipeData swipeData = new SwipeData()
			{
				Direction = direction,
				StartPosition = fingerDownPosition,
				EndPosition = fingerUpPosition,
				MagnitudeY = magnitudeY,
				MagnitudeX = magnitudeX,
				DiffInX = xChnage

		};

			OnSwipe?.Invoke(swipeData);
		}

	
	}


	public struct SwipeData
	{
		public Vector2 StartPosition;
		public Vector2 EndPosition;
		public SwipeDirection Direction;
		public float MagnitudeY;
		public float MagnitudeX;
		public float DiffInX;
	}

	public enum SwipeDirection
	{
		None,
		Up,
		Down,
		Left,
		Right
	}


}
