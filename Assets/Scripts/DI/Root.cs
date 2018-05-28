using System.IO;
using Model;
using Platform;
using UnityEngine;

namespace DI
{
	public class Root : MonoBehaviour
	{
		public IFileSystem FileSystem { get; private set; }
		public IEvents Events { get; private set; }
	
		////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////

		private void Register()
		{
			FileSystem = new FileSystem(
				GetPlatform(),
				Application.persistentDataPath + "/");
			
			Events = new MainThreadEvents(FileSystem);
		}

		////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////

		private void Update()
		{
			Events.Process();
		}

		////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////
		
		private static IPlatform GetPlatform()
		{
#if UNITY_EDITOR
	#if UNITY_EDITOR_WIN
			return new PlatformWindowsEditor(Path.GetPathRoot(Application.persistentDataPath));
	#endif
#else
	#if UNITY_ANDROID
			return new PlatformAndroid();
	#endif
#endif
		}

		private void OnFileSystemChanged()
		{
		}
		
		////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////
		
		private static Root _instance;
		public static Root Instance
		{
			get
			{
				if (_instance == null)
				{
					var go = new GameObject("Root");
					DontDestroyOnLoad(go);
					_instance = go.AddComponent<Root>();
					_instance.Register();
				}
				return _instance;
			}
		}

		private Root() { }
	}
}
