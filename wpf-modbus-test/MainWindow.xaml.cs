using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_modbus_test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        private IModbusMaster modbusMaster;
        private string rbxRWMsg;

        private byte slaveId = 1;           // 从站地址
        private ushort startAddress = 183;    // 寄存器起始地址
        private ushort numRegisters = 7;   // 要读取的寄存器数量

        public MainWindow()
        {
            InitializeComponent();

            string portName = "COM3";
            int baudRate = 9600;
            int dataBits = 8;
            Parity parity = Parity.None;
            StopBits stopBits = StopBits.One;

            serialPort = new SerialPort(portName);
            serialPort.BaudRate = baudRate;
            serialPort.DataBits = dataBits;
            serialPort.Parity = parity;
            serialPort.StopBits = stopBits;

            try
            {
                serialPort.Open();
                modbusMaster = ModbusSerialMaster.CreateRtu(serialPort);


                // 从Modbus从站读取数据

                //ushort[] data = modbusMaster.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                //SetMsg(data);
                // 输出读取的数据
                //for (int i = 0; i < data.Length; i++)
                //{
                //    var addr = startAddress + i;
                //    var item = data[i];
                    
                //    Console.WriteLine($"Register {startAddress + i}: {data[i]}");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:{ex.Message}");
            }
            finally 
            {
                //if (serialPort != null && serialPort.IsOpen)
                //{
                //    serialPort.Close();
                //}
            }
        }

        /// <summary>
        /// 读取输出线圈
        /// </summary>
        /// <returns></returns>
        private bool[] ReadCoils()
        {
            return modbusMaster.ReadCoils(slaveId, startAddress, numRegisters);
        }

        /// <summary>
        /// 读取输入线圈
        /// </summary>
        /// <returns></returns>
        private bool[] ReadInputs()
        {
            return modbusMaster.ReadInputs(slaveId, startAddress, numRegisters);
        }

        /// <summary>
        /// 读取保持型寄存器
        /// </summary>
        /// <returns></returns>
        private ushort[] ReadHoldingRegisters()
        {
            return modbusMaster.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
        }

        /// <summary>
        /// 读取输入寄存器
        /// </summary>
        /// <returns></returns>
        private ushort[] ReadInputRegisters()
        {
            return modbusMaster.ReadInputRegisters(slaveId, startAddress, numRegisters);
        }

        public void SetMsg<T>(List<T> result)
        {
            string msg = string.Empty;

            result.ForEach(m => msg += $"{m}");

            rbxRWMsg = msg.Trim();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rbxRWMsg = "";

            SetMsg(ReadHoldingRegisters().ToList());

            MessageBox.Show(rbxRWMsg);
        }
    }
}
