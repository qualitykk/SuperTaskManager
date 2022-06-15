using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STaskManagerLibrary
{
    public interface ITreeItem
    {
        public string Name { get; }
    }

    public partial class Category : ITreeItem
    {
        public string Name => Title;
    }

    public partial class STask : ITreeItem
    {

    }
}
