using NModbus;
using NModbus.Device;
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

        public MainWindow()
        {
            InitializeComponent();

            string portName = "COM2";
            int baudRate = 9600;
            int dataBits = 8;
            Parity parity = Parity.None;
            StopBits stopBits =  StopBits.One;

            serialPort = new SerialPort(portName);
            serialPort.BaudRate = baudRate;
            serialPort.DataBits = dataBits;
            serialPort.Parity = parity;
            serialPort.StopBits = stopBits;

            try
            {
                // 打开串口
                serialPort.Open();
                var factory = new ModbusFactory();
                // 创建Modbus主站
                modbusMaster = factory.CreateRtuMaster(serialPort);

                // 从Modbus从站读取数据
                byte slaveId = 1;           // 从站地址
                ushort startAddress = 0;    // 寄存器起始地址
                ushort numRegisters = 10;   // 要读取的寄存器数量

                ushort[] data = modbusMaster.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                // 输出读取的数据
                for (int i = 0; i < data.Length; i++)
                {
                    Console.WriteLine($"Register {startAddress + i}: {data[i]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:{ex.Message}");
            }
            finally
            {
                if (serialPort != null && serialPort.IsOpen) 
                {
                    serialPort.Close();
                }
            }
        }
    }
}
