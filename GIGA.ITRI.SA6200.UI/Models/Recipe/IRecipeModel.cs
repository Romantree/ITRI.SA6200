using System.Runtime.Serialization;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Recipe
{
    [DataContract]
    public abstract class IRecipeModel : DataModelBase
    {
        [DataMember]
        public RecipeType Type { get; set; }

        [DataMember]
        public int No { get => this.GetValue<int>(); set => this.SetValue(value); }

        [DataMember]
        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string LastWriteTime { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string CreationTime { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool IsSelcted { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public IRecipeModel(RecipeType type) => this.Type = type;

        public override string ToString() => $"{No},{Type},{Name}";
    }

    public enum RecipeType
    {
        MAIN,
    }
}
