﻿using System;
using System.Collections;
using System.IO;
using System.Threading;
using DI;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class MainView : MonoBehaviour
	{
		[SerializeField]
		private Text _total;

		[SerializeField]
		private Text _free;

		[SerializeField]
		private GameObject _progress;
		
		[SerializeField]
		private Text _progressText;

		private IFileSystem _fileSystem;
		private IEvents _events;

		private void Awake()
		{
			_fileSystem = Root.Instance.FileSystem;
			_events = Root.Instance.Events;
		}

		private void Start()
		{		
			UpdateControls();
			SetControlsEnabled(true);
			_events.FileSystemOperationProgress += OnFileSystemOperationProgress;
			_events.FilesChanged += OnFilesChanged;
		}

		private void OnDestroy()
		{
			_events.FileSystemOperationProgress += OnFileSystemOperationProgress;
			_events.FilesChanged -= OnFilesChanged;
		}

		private void OnApplicationPause(bool pause)
		{
			if (!pause)
			{
				UpdateControls();
			}
		}

		private void OnFileSystemOperationProgress(float progress)
		{
			UpdateProgress(progress);
		}

		private void OnFilesChanged()
		{
			UpdateControls();
		}

		public void Create1M()
		{
			CreateFile(1 * (long)(1 << 20));
		}

		public void Create10M()
		{
			CreateFile(10 * (long)(1 << 20));
		}

		public void Create100M()
		{
			CreateFile(100 * (long)(1 << 20));
		}
		
		public void Create1G()
		{
			CreateFile(1 * (long)(1 << 30));
		}

		public void Create10G()
		{
			CreateFile(10 * (long)(1 << 30));
		}

		private void CreateFile(long size)
		{
			LongOperation(() =>
			{
				_fileSystem.Create(GetRandomFileName(), size);
			});
		}

		private void LongOperation(Action action)
		{
			StartCoroutine(LongOperationCoroutine(action));
		}

		private IEnumerator LongOperationCoroutine(Action action)
		{
			SetControlsEnabled(false);
			yield return RunThreadedCoroutine(action);
			SetControlsEnabled(true);
		}
		
		private static IEnumerator RunThreadedCoroutine(Action action)
		{
			var t = new Thread(() => action());
			t.Start();
			while (t.IsAlive)
			{
				yield return null;
			}
		}

		private void SetControlsEnabled(bool controlsEnabled)
		{
			_progress.SetActive(!controlsEnabled);
		}

		private void UpdateProgress(float progress)
		{
			_progressText.text = string.Format("Progress {0}%", (int) (100.0f * progress));
		}
		
		private void UpdateControls()
		{
			_total.text = "Total: " + Utils.HumanReadable(_fileSystem.Total);
			_free.text = "Free: " + Utils.HumanReadable(_fileSystem.FreeSpace);
		}

		private static string GetRandomFileName()
		{
			return Path.GetRandomFileName();
		}
	}
}
