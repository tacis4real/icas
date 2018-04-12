using AwesomeMvc;

namespace ICAS.Areas.Helpers.Awesome
{
    public class ColumnModCfg
    {
        private readonly ColumnModInfo info = new ColumnModInfo();

        /// <summary>
        /// use to disable hiding of the column from the pick columns dropdown mod
        /// </summary>
        /// <returns></returns>
        public ColumnModCfg Nohide()
        {
            info.Nohide = true;
            return this;
        }

        /// <summary>
        /// set inline format for the column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">editor helper</param>
        /// <param name="valueProperty">grid model property to get value from, when not set will use Column.Name</param>
        /// <returns></returns>
        public ColumnModCfg Inline<T>(IAweHelper<T> helper, string valueProperty = "")
        {
            helper.Svalue("#Value");
            helper.Prefix("#Prefix");
            info.Format = helper.ToString();
            info.ValProp = valueProperty;
            info.ModelProp = helper.Awe.Prop;

            return this;
        }

        /// <summary>
        /// set inline format for the column
        /// </summary>
        /// <param name="format">editor string format, #Value and #Prefix will be replaced, prefix is used for unique ids </param>
        /// <param name="valueProperty">grid model property to get value from, when not set will use Column.Name</param>
        /// <param name="modelProperty">viewmodel property to match in the edit/create action, when not set will use valueProperty</param>
        /// <returns></returns>
        public ColumnModCfg Inline(string format, string valueProperty = "", string modelProperty = "")
        {
            info.Format = format;
            info.ValProp = valueProperty;
            info.ModelProp = modelProperty;

            return this;
        }

        public ColumnModCfg InlineId(string valueProperty = "")
        {
            info.Format = "<input type='hidden' name='Id' value='#Value'>" + "#Value";
            info.ValProp = valueProperty;
            info.ModelProp = "Id";

            return this;
        }

        internal ColumnModInfo GetInfo()
        {
            return info;
        }
    }
}