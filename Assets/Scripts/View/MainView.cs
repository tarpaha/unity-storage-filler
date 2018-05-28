using System;
using System.Collections;
using System.IO;
using System.Threading;
using DI;
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
		
		private void Start()
		{		
			UpdateControls();
			SetControlsEnabled(true);
			Root.Instance.Events.FilesChanged += OnFilesChanged;
		}

		private void OnDestroy()
		{
			Root.Instance.Events.FilesChanged -= OnFilesChanged;
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
				Root.Instance.FileSystem.Create(GetRandomFileName(), size);
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
			UpdateControls();
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

		private void UpdateControls()
		{
			_total.text = Utils.HumanReadable(Root.Instance.FileSystem.Total);
			_free.text = Utils.HumanReadable(Root.Instance.FileSystem.FreeSpace);
		}

		private static string GetRandomFileName()
		{
			return Path.GetRandomFileName();
		}
	}
}
