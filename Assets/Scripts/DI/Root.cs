using Model;
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
				Platform.Platform.Get(),
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
