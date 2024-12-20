using GIGA.ITRI.SA6200.UI.Models.Product;
using System;
using System.Collections.Generic;
using System.IO;
using TS.FW;
using TS.FW.Serialization;

namespace GIGA.ITRI.SA6200.UI.Managers
{
    public class ProductManager
    {
        // 기능 요청 해야 될때 사용
        private const bool USED = false;
        private const string ROOT = @"..\History";

        static ProductManager()
        {
            if (USED == false) return;

            if (Directory.Exists(ROOT) == false) Directory.CreateDirectory(ROOT);
        }

        private ProductHistoryModel _cur;

        public List<ProductHistoryModel> GetProductHistory(DateTime start, DateTime end)
        {
            var list = new List<ProductHistoryModel>();
            var total = (int)Math.Ceiling((end - start).TotalDays + 1);

            for (int i = 0; i < total; i++)
            {
                var time = start.AddDays(i);
                var path = Path.Combine(ROOT, time.ToString("yyyyMMdd"));
                if (Directory.Exists(path) == false) continue;

                foreach (var file in Directory.EnumerateFiles(path, "*.json"))
                {
                    if (File.Exists(file) == false) continue;

                    var item = this.ToConvert(file);
                    if (item == null) continue;

                    list.Add(item);
                }
            }

            return list;
        }

        public void Start(string recipeName)
        {
            if (USED == false) return;

            try
            {
                _cur = new ProductHistoryModel() { RecipeName = recipeName };
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void Collect()
        {
            if (USED == false) return;

            try
            {
                if (_cur == null) return;

                _cur.Collect();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void Stop()
        {
            if (USED == false) return;

            try
            {
                if (_cur == null) return;

                _cur.EndTime = DateTime.Now;

                var root = Path.Combine(ROOT, _cur.StartTime.ToString("yyyyMMdd"));
                if (Directory.Exists(root)) Directory.CreateDirectory(root);

                var paht = Path.Combine(root, $"{_cur.StartTime.ToString("HHmmss")}.Json");
                _cur.JsonSerializerFile(paht);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private ProductHistoryModel ToConvert(string file)
        {
            try
            {
                var res = Serialization.JsonDeserializerFile<ProductHistoryModel>(file);
                if (res == false) Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);

                return res.Result;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return null;
            }
        }
    }
}
