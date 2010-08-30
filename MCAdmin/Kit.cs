using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBT;
using System.IO;
using System.Windows.Forms;

namespace MCAdmin
{
    public class Kit
    {
        public Dictionary<int,int> items;
        public int reqlevel;
        public string name;
        public bool saved;

        public static string GetKitFile(string kitname)
        {
            return "kits/" + kitname + ".kit";
        }

        public void Save()
        {
            Tag mainTag = Tag.Create("kit");
            mainTag.Add("reqlevel", reqlevel);
            Tag itemsTag = Tag.Create("items");

            foreach(KeyValuePair<int,int> kv in items)
            {
                itemsTag.Add(kv.Key.ToString(), kv.Value);
            }

            mainTag.Add("items", itemsTag);
            mainTag.Save(GetKitFile(name));
            saved = true;
        }

        public Kit(string m_name)
        {
            name = m_name.ToLower();
            items = new Dictionary<int,int>();

            if (File.Exists(GetKitFile(name)))
            {
                Tag mainTag = Tag.Load(GetKitFile(name));
                reqlevel = (int)mainTag["reqlevel"].Value;

                foreach (Tag subTag in mainTag["items"])
                {
                    items.Add(Convert.ToInt32(subTag.Name), (int)subTag.Value);
                }

                saved = true;
            }
            else
            {
                reqlevel = 0;
                saved = false;
            }
        }




        public Kit(string m_name, string file)
        {
            name = m_name;
        }

        public override string ToString()
        {
            return name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Kit)) return false;
            Kit k2 = (Kit)obj;
            return (this.name == k2.name);
        }
    }
}
