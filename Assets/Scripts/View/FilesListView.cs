using System.Collections.Generic;
using DI;
using Model;
using UnityEngine;

namespace View
{
	public class FilesListView : MonoBehaviour
	{
		[SerializeField]
		private FileView _fileView;
		private Transform _elementsParent;

		private void Start()
		{
			PrepareList();
			UpdateList();
			Root.Instance.Events.FilesChanged += OnFilesChanged;
		}

		private void OnDestroy()
		{
			Root.Instance.Events.FilesChanged -= OnFilesChanged;
		}

		private void PrepareList()
		{		
			_elementsParent = _fileView.transform.parent;
			_fileView.transform.SetParent(null);
			_fileView.gameObject.SetActive(false);
			ClearList();
		}

		public void UpdateList()
		{
			ClearList();
			
			var files = GetFiles();
			foreach (var file in files)
			{
				var element = CreateFileView(file);
				element.transform.SetParent(_elementsParent);
			}
		}

		private void ClearList()
		{
			foreach (Transform element in _elementsParent)
			{
				Destroy(element.gameObject);
			}
		}

		private void OnFilesChanged()		
		{
			UpdateList();
		}
		
		private FileView CreateFileView(IFileRecord file)
		{
			var view = Instantiate(_fileView);
			view.gameObject.SetActive(true);
			view.Init(file);
			return view;
		}

		private static IEnumerable<IFileRecord> GetFiles()
		{
			return Root.Instance.FileSystem.Dir();
		}				
	}
}
