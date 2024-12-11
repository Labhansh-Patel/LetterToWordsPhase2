using UnityEngine;

namespace Platform.Scripts.Animations
{

	public class UIScale_Bounce : MonoBehaviour
	{

		public float timeZoomIn = 0.5f;

		public float timeZoomOut = 0.2f;

		public float UpScaleFactor = 1.2f;

		private Vector3 UpScaleVector;

		public bool OverrideFinalScale;

		public float ovverrideScale;

		private void OnEnable()
		{
			Vector3 scale = Vector3.zero;

			transform.localScale = scale;

			UpScaleVector = new Vector3(UpScaleFactor, UpScaleFactor, UpScaleFactor);

			ScaleUp();


		}

		private void ScaleUp()
		{
			LeanTween.scale(this.gameObject, UpScaleVector, timeZoomIn).setOnComplete(ScaleToOne);
		}

		private void ScaleToOne()
		{
			if (OverrideFinalScale)
			{
				LeanTween.scale(this.gameObject, new Vector3(ovverrideScale,ovverrideScale,ovverrideScale), timeZoomOut);
			}
			else
			{
				LeanTween.scale(this.gameObject, Vector3.one, timeZoomOut);
			}

			if (gameObject.activeInHierarchy)
			{
				ScaleUp();
			}
			
		}

		


	}
}
