using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyModbus;

namespace MODBUS_TCP通信小工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static ModbusClient mc;
        public void LogMessage(string message)
        {
            richTextBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd————hh:mm:ss") + "\r\n" + message + "\r\n");
        }

        //TCP连接
        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                mc = new ModbusClient();
                mc.IPAddress = tb_IP.Text;//设置IP
                mc.Port = int.Parse(tb_port.Text);//设置端口
                mc.Connect(mc.IPAddress, mc.Port);//创建连接
                if(mc.Connected)//判断是否连接成功
                {
                    LogMessage("连接成功！");
                }
                else
                {
                    LogMessage("连接失败！");
                }
            }
            catch(Exception ex) 
            {
                LogMessage("连接失败："+ ex.ToString());
            }
            
        }

        //断开连接
        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            try
            {
                mc.Disconnect();
                if (!mc.Connected)
                {
                    LogMessage("连接已断开！");
                }
            }
            catch
            {

            }
        }

        //读寄存器数据
        private void btn_read_Click(object sender, EventArgs e)
        {
            if (mc.Connected)
            {
                try
                {
                    int addr = int.Parse(tb_addr.Text);
                    int num = int.Parse(tb_num.Text);
                    int[] reads = mc.ReadHoldingRegisters(addr, num);
                    for (int i = 1; i < reads.Length; i++)
                    {
                        LogMessage("读取的寄存器数据：" + reads[i].ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("读取失败：" + ex.ToString());
                }
            }
            else
            {
                LogMessage("错误！请检测是否连接！");
            }
        }

        //写入寄存器数据
        private void btn_write_Click(object sender, EventArgs e)
        {
            if (mc.Connected)
            {
                try
                {
                    int addr = int.Parse(tb_addr.Text);
                    int write = int.Parse(tb_write.Text);
                    mc.WriteSingleRegister(addr, write);
                    LogMessage("写入成功：" + write.ToString());
                }
                catch (Exception ex)
                {
                    LogMessage("写入失败：" + ex.ToString());
                }
            }
            else
            {
                LogMessage("错误！请检测是否连接！");
            }
            
        }

        //读取线圈数据
        private void btn_read2_Click(object sender, EventArgs e)
        {
            if (mc.Connected)
            {
                try
                {
                    int start = int.Parse(tb_start.Text);
                    int length = int.Parse(tb_length.Text);
                    bool[] reads = mc.ReadCoils(start, length);
                    for (int i = 0; i < reads.Length; i++)
                    {
                        LogMessage("读取的线圈数据：" + reads[i].ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("读取失败：" + ex.ToString());
                }
            }
            else
            {
                LogMessage("错误！请检测是否连接！");
            }
            
        }

        //写入线圈数据
        private void btn_write2_Click(object sender, EventArgs e)
        {
            if (mc.Connected)
            {
                try
                {
                    int start = int.Parse(tb_start.Text);
                    string writes = richTextBox2.Text;
                    string[] write_ary = writes.Split(',');
                    bool[] write_array = new bool[write_ary.Length];
                    for (int i = 0; i < write_ary.Length; i++)
                    {
                        write_array[i] = bool.Parse(write_ary[i]);
                    }
                    mc.WriteMultipleCoils(start, write_array);
                    LogMessage("写入完成！");
                }
                catch (Exception ex)
                {
                    LogMessage("写入失败：" + ex.ToString());
                }
            }
            else
            {
                LogMessage("错误！请检测是否连接！");
            }
        }

        //清空日志
        private void btn_clear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
