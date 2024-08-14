using System.Data;
using System.Runtime.InteropServices;

namespace TemporaryVolumeSettingTool
{
    internal class Program
    {
        static void Main(string[] args)
        {           
            if (args.Length == 3)
            {                
                Console.WriteLine("名为\"{0}\"的临时音量设置开始运行", args[2]);
                SystemVolume.SetMasterVolume((float)int.Parse(args[1]));
                Console.WriteLine("已将音量设置为设定值({0}),点击回车将音量设置回默认值({1})并退出程序...", args[1], args[0]);
                void SetVolumeToBack()
                {
                    SystemVolume.SetMasterVolume((float)int.Parse(args[0]));
                }
                AppDomain.CurrentDomain.ProcessExit += new EventHandler((object sender,EventArgs e) =>
                {
                    SetVolumeToBack();
                }!);
                Console.ReadLine();
                SetVolumeToBack();
            }else if(args.Length == 0)
            {
                Console.WriteLine("未检测到参数，运行脚本创建模式。");
                int inputValue=-1;
                int targetValue = -1;
                string fileName;
                string name;
                Console.Write("请输入默认音量(留空则自动获取当前系统音量): ");
                static int Ifnull()
                {
                    string str = Console.ReadLine()!;
                    if (str != "")
                    {
                        return int.Parse(str);
                    }
                    else
                    {
                        return (int)SystemVolume.GetMasterVolume();
                    }
                }
                inputValue = Ifnull();
                Console.WriteLine("默认音量已记录: {0}",inputValue.ToString());
                Console.Write("请输入需要临时更改的音量: ");
               targetValue=int.Parse( Console.ReadLine()!);
                //Console.WriteLine("临时更改的目标音量已记录: {0}",targetValue.ToString());
                Console.Write("请输入生成的脚本的备注名: ");
                name= Console.ReadLine()!;
                fileName = "./" + name + ".cmd";
                StreamWriter writer = new(fileName);
                writer.Write("tvst {0} {1} {2}", inputValue, targetValue,name);
                writer.Close();
                Console.WriteLine("脚本已生成，路径为: {0}", Path.GetFullPath(fileName));
            }
            else
            {
                Console.WriteLine("参数错误!");
            }
        }
    }
}

