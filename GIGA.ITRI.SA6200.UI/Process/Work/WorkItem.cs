using GIGA.ITRI.SA6200.UI.Models.Recipe;

namespace GIGA.ITRI.SA6200.UI.Process.Work
{
    public class WorkItem
    {
        public WorkMode Mode { get; private set; }

        public MainRecipeModel Rcp { get; private set; }

        public WorkItem(MainRecipeModel rcp, WorkMode mode = WorkMode.AUTO)
        {
            this.Rcp = rcp;
            this.Mode = mode;
        }
    }

    public enum WorkMode
    {
        AUTO,
        IMPRINT,
        DEMOLD,
    }
}
