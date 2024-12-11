using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace APICalls
{

	public class WebHelpers : MonoBehaviour
	{
		// Delegates
		//Call back for APIs Response
		public delegate void CallbackGet(string aURL, bool aSuccess, object aData);
		//public delegate void CallbackPost(string aURL, bool aSuccess, object aData);



		// The return data types we support
		private List<System.Type> _supportedTypes = new List<System.Type>
		{
			typeof(string),
			typeof(Texture2D),
			typeof(byte[])
		};

		public static WebHelpers instance;
		
		private void Awake()
		{
			instance = this;

		}


		#region Public Methods
		/// <summary>
		/// Uses a HTTP GET request with the specified URL.
		/// </summary>
		/// <typeparam name="T">Can be any of the supported return types - string, byte[] or Texture2D</typeparam>
		/// <param name="aURL">Request URL</param>
		/// <param name="aCallback">Called when the request is complete</param>
		public void get<T>(string aURL, CallbackGet aCallback, bool isHeaderReuired = true)
		{
			// sanity - checkl for supported types
			var dataType = typeof(T);
			if (!_supportedTypes.Contains(dataType))
			{
				//Unsupported data type go back
				Debug.LogErrorFormat("WEBHELPERS: POST: {0}: Unsupported data type => {1}", aURL, dataType.ToString());

				return;
			}

			// create the request for valid data type request
			UnityWebRequest req = new UnityWebRequest(aURL);
			req.method = UnityWebRequest.kHttpVerbGET;
			if (isHeaderReuired)
			{
				//req.SetRequestHeader("token", MGMGlobals.UserToken);
			}


			//var uploadHandler = new UploadHandlerRaw(aContent);
			//uploadHandler.contentType = aContentType;
			//req.uploadHandler = uploadHandler;
			//req.disposeUploadHandlerOnDispose = true;

			// select the right handler based on supported types
			DownloadHandler dataHandler = null;
			// textures
			if (dataType == typeof(Texture2D))
			{
				dataHandler = new DownloadHandlerTexture();
			}
			// default
			else
			{
				dataHandler = new DownloadHandlerBuffer();
			}
			req.downloadHandler = dataHandler;
			req.disposeDownloadHandlerOnDispose = true;


			StartCoroutine(_getRequest<T>(req, aCallback));

			return;
		}


		public void post<T>(string aURL, byte[] aContent, string aContentType, CallbackGet aCallback)
		{
			// sanity - checkl for supported types
			var dataType = typeof(T);
			if (!_supportedTypes.Contains(dataType))
			{
				Debug.LogErrorFormat("WEBHELPERS: POST: {0}: Unsupported data type => {1}", aURL, dataType.ToString());
				return;
			}

			// create the request
			UnityWebRequest req = new UnityWebRequest(aURL);
			req.method = UnityWebRequest.kHttpVerbPOST;

#if   AUTHVALUE

				if (!string.IsNullOrEmpty(GlobalData.UserToken))
			{
				req.SetRequestHeader("token", GlobalData.UserToken);
			}
#endif
			
		
			


			var uploadHandler = new UploadHandlerRaw(aContent);
			uploadHandler.contentType = aContentType;
			req.uploadHandler = uploadHandler;
			req.disposeUploadHandlerOnDispose = true;


			// select the right handler based on supported types
			DownloadHandler dataHandler = null;
			// textures
			if (dataType == typeof(Texture2D))
			{
				dataHandler = new DownloadHandlerTexture();
			}
			// default
			else
			{
				dataHandler = new DownloadHandlerBuffer();
			}
			req.downloadHandler = dataHandler;
			req.disposeDownloadHandlerOnDispose = true;

			//Debug.LogFormat("WEBHELPERS: POST: {0}: Fetching as {1}", aURL, dataType.ToString());

			// Go Ninja Go!
			StartCoroutine(_postRequest<T>(req, aCallback));

			return;
		}


		/// <summary>
		/// Uses a HTTP POST request with the specified URL.
		/// </summary>
		/// <typeparam name="T">Can be any of the supported return types - string, byte[] or Texture2D</typeparam>
		/// <param name="aURL">Request URL</param>
		/// <param name="aContent">Content to upload as the body of the request</param>
		/// <param name="aContentType">Content type as per HTTP specs</param>
		/// <param name="aCallback">Called when the request is complete</param>
		public void post<T>(string aURL, WWWForm aContent, string aContentType, CallbackGet aCallback)
		{
			// sanity - checkl for supported types
			var dataType = typeof(T);
			if (!_supportedTypes.Contains(dataType))
			{
				//Debug.LogErrorFormat("WEBHELPERS: POST: {0}: Unsupported data type => {1}", aURL, dataType.ToString());
				return;
			}

			// create the request
			UnityWebRequest req = UnityWebRequest.Post(aURL, aContent);
			req.method = UnityWebRequest.kHttpVerbPOST;


			// upload handler sends our body
			//var uploadHandler = new UploadHandler(aContent);
			//uploadHandler.contentType = aContentType;
			//req.uploadHandler = uploadHandler;
			//req.disposeUploadHandlerOnDispose = true;
			// select the right handler based on supported types
			DownloadHandler dataHandler = null;
			// textures
			if (dataType == typeof(Texture2D))
			{
				dataHandler = new DownloadHandlerTexture();
			}
			// default
			else
			{
				dataHandler = new DownloadHandlerBuffer();
			}
			req.downloadHandler = dataHandler;
			req.disposeDownloadHandlerOnDispose = true;

			//TODO might need it
			//Debug.LogFormat("WEBHELPERS: POST: {0}: Fetching as {1}", aURL, dataType.ToString());

			// Go Ninja Go!
			StartCoroutine(_postRequest<T>(req, aCallback, aContent));

			return;
		}
		#endregion  Public Methods




		#region Private Coroutines
		IEnumerator _postRequest<T>(UnityWebRequest aRequest, CallbackGet aCallback)
		{
			// send off the request and wait
			yield return aRequest.SendWebRequest();

			// handle the results
			if (aRequest.isNetworkError || aRequest.isHttpError)
			{
				// something went wrong!
				if (aRequest.responseCode == 401)
				{
					//TODO: Take user to login as session Expired
					//MGMManager.Instance.AskUserLogin();
				}
				//Debug.LogErrorFormat("WEBHELPERS: POST: {0}: Failed => {1} (HTTP {2})", aRequest.url, aRequest.error, aRequest.responseCode);
				aCallback(aRequest.url, false, aRequest.error);
			}
			else
			{
				//Debug.LogFormat("WEBHELPERS: POST: {0}: Fetched => {1} bytes", aRequest.url, aRequest.downloadHandler.data.Length);

				// which data type was specified?
				var dataType = typeof(T);
				if (dataType == typeof(Texture2D))
				{
					var dataHandler = (DownloadHandlerTexture)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.texture);
				}
				else if (dataType == typeof(string))
				{
					var dataHandler = (DownloadHandlerBuffer)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.text);
				}
				else if (dataType == typeof(byte[]))
				{
					var dataHandler = (DownloadHandlerBuffer)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.data);
				}
			}

			// be polite and get rid of the request object to avoid leaks
			aRequest.Dispose();
		}


		IEnumerator _getRequest<T>(UnityWebRequest aRequest, CallbackGet aCallback)
		{
			// send off the request and wait
			yield return aRequest.SendWebRequest();

			// handle the results
			if (aRequest.isNetworkError || aRequest.isHttpError)
			{
				// something went wrong!

				if (aRequest.responseCode == 401)
				{
					//TODO: Take user to login as session Expired
					//MGMManager.Instance.AskUserLogin();
				}
				//Debug.LogErrorFormat("WEBHELPERS: GET: {0}: Failed => {1} (HTTP {2})", aRequest.url, aRequest.error, aRequest.responseCode);
				aCallback(aRequest.url, false, aRequest.error);
			}
			else
			{
				//Debug.LogFormat("WEBHELPERS: GET: {0}: Fetched => {1} bytes", aRequest.url, aRequest.downloadHandler.data.Length);

				// which data type was specified?
				var dataType = typeof(T);
				if (dataType == typeof(Texture2D))
				{
					var dataHandler = (DownloadHandlerTexture)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.texture);
				}
				else if (dataType == typeof(string))
				{
					var dataHandler = (DownloadHandlerBuffer)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.text);
				}
				else if (dataType == typeof(byte[]))
				{
					var dataHandler = (DownloadHandlerBuffer)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.data);
				}
			}

			// be polite and get rid of the request object to avoid leaks
			aRequest.Dispose();
		}



		IEnumerator _postRequest<T>(UnityWebRequest aRequest, CallbackGet aCallback, WWWForm aContent)
		{
			// send off the request and wait
			yield return aRequest.SendWebRequest();

			// handle the results
			if (aRequest.isNetworkError || aRequest.isHttpError)
			{
				// something went wrong!
				//TODO might need it
				//              Debug.LogErrorFormat("WEBHELPERS: POST: {0}: Failed => {1} (HTTP {2})", aRequest.url, aRequest.error, aRequest.responseCode);
				aCallback(aRequest.url, false, aRequest.error);
			}
			else
			{
				//TODO might need it
				// yaay!
				//              Debug.LogFormat("WEBHELPERS: POST: {0}: Fetched => {1} bytes", aRequest.url, aRequest.downloadHandler.data.Length);

				// which data type was specified?
				var dataType = typeof(T);
				if (dataType == typeof(Texture2D))
				{
					var dataHandler = (DownloadHandlerTexture)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.texture);
				}
				else if (dataType == typeof(string))
				{
					var dataHandler = (DownloadHandlerBuffer)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.text);
				}
				else if (dataType == typeof(byte[]))
				{
					var dataHandler = (DownloadHandlerBuffer)aRequest.downloadHandler;
					aCallback(aRequest.url, true, dataHandler.data);
				}
			}

			// be polite and get rid of the request object to avoid leaks
			aRequest.Dispose();
		}


		#endregion
	}
}

 
