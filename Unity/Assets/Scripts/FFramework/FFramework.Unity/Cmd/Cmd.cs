using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace FFramework.Win32
{
#if UNITY_EDITOR_WIN
    public class Cmd
    {

        private Process proc = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public Cmd()
        {
            proc = new Process();
        }
        /// <summary>
        /// 执行CMD语句
        /// </summary>
        /// <param name="cmd">执行的CMD命令</param>
        public string RunCmd(string cmd)
        {
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.StandardInput.WriteLine(cmd);
            proc.StandardInput.WriteLine("exit");
            string outStr = proc.StandardOutput.ReadToEnd();
            proc.Close();
            return outStr;
        }
        /// <summary>
        /// 打开程序并执行命令
        /// </summary>
        /// <param name="programName">包括后缀的路径</param>
        /// <param name="cmd">要执行的命令</param>
        public void RunProgram(string programName, string cmd = "")
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = programName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            if (cmd.Length != 0)
            {
                proc.StandardInput.WriteLine(cmd);
            }
            proc.Close();
        }
    }
#endif
}
