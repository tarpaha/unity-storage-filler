using System;
using DI;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class FileView : MonoBehaviour
    {
        [SerializeField]
        private Text _name;
        [SerializeField]
        private Text _size;

        private IFileRecord _fileRecord;

        public void Init(IFileRecord fileRecord)
        {
            _fileRecord = fileRecord;
            UpdateControls();
        }

        private void UpdateControls()
        {
            _name.text = _fileRecord.Name;
            _size.text = Utils.HumanReadable(_fileRecord.Size);
        }

        public void Delete()
        {
            Root.Instance.FileSystem.Delete(_fileRecord.Name);
        }
    }
}
