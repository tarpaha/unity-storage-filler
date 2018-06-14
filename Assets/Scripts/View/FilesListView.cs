using System.Collections.Generic;
using System.Linq;
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

		private IFileSystem _fileSystem;
		private IEvents _events;

		private void Awake()
		{
			_fileSystem = Root.Instance.FileSystem;
			_events = Root.Instance.Events;
		}

		private void Start()
		{
			PrepareList();
			UpdateList();
			_events.FilesChanged += OnFilesChanged;
		}

		private void OnDestroy()
		{
			_events.FilesChanged -= OnFilesChanged;
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
			
			var files = GetFiles().OrderBy(file => file.Date);
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

		private IEnumerable<IFileRecord> GetFiles()
		{
			return _fileSystem.Dir();
		}				
	}
}
