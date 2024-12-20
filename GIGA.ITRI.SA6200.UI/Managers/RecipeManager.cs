using GIGA.ITRI.SA6200.UI.Models.Recipe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TS.FW;
using TS.FW.Serialization;

namespace GIGA.ITRI.SA6200.UI.Managers
{
    public class RecipeManager
    {
        private const string ROOT = @"..\Recipe";

        public void InitPath()
        {
            if (Directory.Exists(ROOT) == false) Directory.CreateDirectory(ROOT);

            foreach (RecipeType item in Enum.GetValues(typeof(RecipeType)))
            {
                var path = Path.Combine(ROOT, item.ToString());
                if (Directory.Exists(path)) continue;

                Directory.CreateDirectory(path);
            }
        }

        public void LoadDatabase()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public List<IRecipeModel> ToRecipeList(RecipeType type)
        {
            var list = new List<IRecipeModel>();

            try
            {
                foreach (var file in this.ToFileList(type))
                {
                    if (int.TryParse(this.ToFileName(file), out int no) == false) continue;

                    var item = this.GetRecipe(type, no);
                    if (item == null) continue;

                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }

            return list;
        }

        public T Reload<T>(IRecipeModel model) where T : IRecipeModel
        {
            return (T)this.GetRecipe(model.Type, model.No);
        }

        public Response NewRecipe(RecipeType type, string name)
        {
            try
            {
                var no = this.ToRecipeNo(type);
                IRecipeModel model = null;

                switch (type)
                {
                    case RecipeType.MAIN:
                        {
                            model = new MainRecipeModel()
                            {
                                No = no,
                                Name = name
                            };
                        }
                        break;
                }

                if (model == null) return new Response(false, $"This is an unregistered recipe type.{type}");

                return this.Save(model);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public Response Save(IRecipeModel model)
        {
            try
            {
                var file = this.ToFile(model.Type, model.No);
                if (File.Exists(file)) File.Delete(file);

                return Serialization.JsonSerializerFile(model, file);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public Response SaveAs(string name, IRecipeModel model)
        {
            try
            {
                var no = this.ToRecipeNo(model.Type);
                model.No = no;
                model.Name = name;

                return this.Save(model);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public Response Delete(IRecipeModel model)
        {
            try
            {
                var file = this.ToFile(model.Type, model.No);
                if (File.Exists(file)) File.Delete(file);

                return new Response();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        private void Clear(RecipeType type)
        {
            try
            {
                foreach (var file in this.ToFileList(type)) File.Delete(file);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private IRecipeModel GetRecipe(RecipeType type, int no)
        {
            try
            {
                var file = this.ToFile(type, no);
                if (File.Exists(file) == false) return null;

                var res = Serialization.JsonDeserializerFile<IRecipeModel>(file);
                if (res == false) return null;

                var info = new FileInfo(file);

                res.Result.CreationTime = info.CreationTime.ToString("yyyy-MM-dd HH:mm:ss ");
                res.Result.LastWriteTime = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

                return res.Result;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }

            return null;
        }

        private bool Exists(RecipeType type, int no) => File.Exists(this.ToFile(type, no));

        private string ToFile(RecipeType type, int no) => Path.Combine(ROOT, type.ToString(), $"{no}.Json");

        private string ToFileName(string path) => Path.GetFileNameWithoutExtension(path);

        private List<string> ToFileList(RecipeType type) => Directory.EnumerateFiles(Path.Combine(ROOT, type.ToString()), "*.json").ToList();

        private int ToRecipeNo(RecipeType type)
        {
            var list = this.ToFileList(type);
            if (list.Count <= 0) return 0;

            return list.Select(t => int.TryParse(this.ToFileName(t), out int no) ? no : -1).Max(t => t) + 1;
        }
    }
}
