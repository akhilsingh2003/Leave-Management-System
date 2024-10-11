using System.Reflection.Metadata;

namespace Assingment.AppCode
{
    public static class CommonFunction
    {
        public static List<string> GetStatusList()
        {
            List<string> items = new List<string>();
            items.Add(Constant.Active);
            items.Add(Constant.InActive);

            return items;
        }

        public static List<string> GetActionList()
        {
            List<string> items = new List<string>();
            items.Add(Constant.Approved);
            items.Add(Constant.Pending);

            return items;
        }
    }
}
